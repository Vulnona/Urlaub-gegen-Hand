using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using UGHApi.Models;
using UGHApi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
        }

        private static void ConfigureWebHost(WebApplicationBuilder builder)
        {
            // Configure Kestrel to listen on a specific port
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8080); // Set the desired port
            });

            // Load the configuration from appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Load the configuration from secrets.json
            builder.Configuration.AddUserSecrets<Program>(optional: true, reloadOnChange: true);
        }


        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            var connectionString = config.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<UghContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddHostedService<PythonScriptRunner>();
            builder.Services.AddTransient<DatabaseIntegrityChecker>();
            ConfigureAuthentication(builder);
            ConfigureCors(builder);
            ConfigureMailSettings(builder);
            ConfigureSwagger(builder);
            RegisterServices(builder.Services);

            //SeedDefaultRoles(builder.Services);
            //CreateAutoAdminUser(builder.Services.BuildServiceProvider().GetService<userservice>());
        }
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    // Other configuration code...

        //    using (var serviceScope = app.ApplicationServices.CreateScope())
        //    {
        //        var context = serviceScope.ServiceProvider.GetRequiredService<UghContext>();
        //        context.SeedDataIfEmpty();
        //    }
        //}
        private static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Forbidden/";
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });
        }
        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
            });
        }
        private static void ConfigureMailSettings(WebApplicationBuilder builder)
        {
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<TemplateSettings>(builder.Configuration.GetSection("TemplateSettings"));
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
                        new string[] {}
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
            services.AddMemoryCache();
            services.AddScoped<TokenService>();
            services.AddTransient<CouponService>();
            services.AddTransient<AdminVerificationMailService>();
            services.AddHostedService<ReviewUserHostedService>();
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
            var dbContext = scope.ServiceProvider.GetRequiredService<UghContext>();

            if (!dbContext.userroles.Any())
            {
                dbContext.userroles.AddRange(new List<UserRole>
                {
                    new UserRole { RoleName = "Admin" },
                    new UserRole { RoleName = "User" }
                });
                dbContext.SaveChanges();
            }
        }

        private static async void CreateAutoAdminUser(UserService userservice)
        {
            var user = await userservice.GetUserByEmailAsync("admin@example.com");
            if (user == null)
            {
                var adminUser = new RegisterRequest
                {
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = "1990-01-01",
                    Gender = "Male",
                    Street = "Admin Street",
                    HouseNumber = "123",
                    PostCode = "12345",
                    City = "Admin City",
                    Country = "Admin Country",
                    State = "Admin State",
                    Email_Address = "admin@example.com",
                    Password = "admin@123"
                };
                userservice.CreateAdmin(adminUser);
            }
        }
    }
}
