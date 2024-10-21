using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using UGH.infrastructure.Repositories;
using UGH.Application.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using UGH.Application.Reviews;
using UGH.Domain.Interfaces;
using System.Reflection;
using Serilog.Events;
using System.Text;
using Serilog;
using MediatR;
using UGHApi.Repositories;
using Mapster;
using UGHApi.Shared;

namespace UGHApi
{
    public class Program
    {
        private readonly IConfiguration _configuration;

        public Program(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureLogging(builder);
            ConfigureWebHost(builder);
            ConfigureServices(builder);
            var app = builder.Build();
            ConfigureMiddleware(app);
            ConfigureEndpoints(app);
            app.Run();
        }

        private static void ConfigureLogging(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel
                .Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("/app/logs/logfile.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Host.UseSerilog(Log.Logger);
        }

        private static void ConfigureWebHost(WebApplicationBuilder builder)
        {
            // Configure Kestrel to listen on a specific port
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8080); // Set the desired port
            });
            builder.Configuration.AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true
            );
            builder.Configuration.AddUserSecrets<Program>(optional: true, reloadOnChange: true);
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            MapsterConfig.RegisterMappings();
            var connectionString = config.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<Ugh_Context>(
                options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
            builder.Services.AddControllers();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddHostedService<MembershipStatusUpdaterService>();
            builder.Services.AddMediatR(
                typeof(AddReviewCommandHandler).Assembly,
                typeof(VerifyEmailCommandHandler).Assembly
            );
            builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection("AwsOptions"));
            builder.Services.AddMediatR(
                Assembly.GetExecutingAssembly(),
                typeof(AddReviewCommandHandler).Assembly
            );
            //builder.Services.AddHostedService<PythonScriptRunner>();
            //builder.Services.AddTransient<DatabaseIntegrityChecker>();
            ConfigureAuthentication(builder);
            ConfigureCors(builder);
            ConfigureMailSettings(builder);
            ConfigureSwagger(builder);
            RegisterServices(builder.Services);
            //SeedDefaultRoles(builder.Services);
            //CreateAutoAdminUser(builder.Services.BuildServiceProvider().GetService<userService>());
        }

        private static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                });
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(config["Jwt:Key"])
                        )
                    };
                });
        }

        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    "MyPolicy",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://ugh.csdevhub.com:3000", "http://localhost:3000")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                );
            });
        }

        private static void ConfigureMailSettings(WebApplicationBuilder builder)
        {
            builder.Services.Configure<MailSettings>(
                builder.Configuration.GetSection("MailSettings")
            );
            builder.Services.Configure<TemplateSettings>(
                builder.Configuration.GetSection("TemplateSettings")
            );
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UHG Api", Version = "v1" });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] { }
                    }
                };
                c.AddSecurityRequirement(securityRequirement);
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<EmailService>();
            services.AddTransient<UserService>();
            services.AddScoped<PasswordService>();
            services.AddScoped<S3Uploader>();
            //services.AddScoped<MembershipService>();
            services.AddMemoryCache();
            services.AddScoped<TokenService>();
            //services.AddTransient<CouponService>();
            services.AddTransient<UGH.Infrastructure.Services.AdminVerificationMailService>();
            services.AddScoped<
                IReviewRepository,
                UGH.Infrastructure.Repositories.ReviewRepository
            >();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IUserRepository, UGH.Infrastructure.Repositories.UserRepository>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<
                Repositories.Interfaces.IUserMembershipRepository,
                UserMembershipRepository
            >();
            services.AddScoped<
                Repositories.Interfaces.IMembershipRepository,
                MembershipRepository
            >();

            services.AddScoped<IOfferRepository, UGH.Infrastructure.Repositories.OfferRepository>();
            services.AddHttpContextAccessor();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
            DatabaseWaiter.WaitForDatabaseConnection(connectionString);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseAuthorization();
        }

        private static void ConfigureEndpoints(WebApplication app)
        {
            app.MapControllers();
        }

        private static void SeedDefaultRoles(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Ugh_Context>();

            if (!dbContext.userroles.Any())
            {
                dbContext.userroles.AddRange(
                    new List<UGH.Domain.Entities.UserRole>
                    {
                        new UGH.Domain.Entities.UserRole { RoleName = "Admin" },
                        new UGH.Domain.Entities.UserRole { RoleName = "User" }
                    }
                );
                dbContext.SaveChanges();
            }
        }
        //private static async void CreateAutoAdminUser(UserService userService)
        //{
        //    var user = await userService.GetUserByEmailAsync("admin@example.com");
        //    if (user == null)
        //    {
        //        var adminUser = new RegisterRequest
        //        {
        //            FirstName = "Admin",
        //            LastName = "User",
        //            DateOfBirth = "1990-01-01",
        //            Gender = "Male",
        //            Street = "Admin Street",
        //            HouseNumber = "123",
        //            PostCode = "12345",
        //            City = "Admin City",
        //            Country = "Admin Country",
        //            State = "Admin State",
        //            Email_Address = "admin@example.com",
        //            Password = "admin@123"
        //        };
        //        userService.CreateAdmin(adminUser);
        //    }
        //}
    }
}
