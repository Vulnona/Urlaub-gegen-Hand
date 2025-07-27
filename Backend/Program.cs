using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using UGH.Application.Authentication;
using UGH.Domain.Interfaces;
using UGHApi.Repositories;
using UGH.Infrastructure.Services;
using UGH.Domain.Services; // NEW: Geocoding services
using UGHApi.DATA; // NEW: Database context
using UGHApi.Models;
using UGHApi.Services.AWS;
using UGHApi.Services.HtmlTemplate;
using UGHApi.Services.Stripe;
using UGHApi.Services.UserProvider;
using UGHApi.Services;
using UGHApi.Shared;

namespace UGHApi
{
    public class Program
    {
        private readonly IConfiguration _configuration;
        public static TimeZoneInfo AppTimeZone { get; private set; } = TimeZoneInfo.Utc;

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
            // Set up timezone first for consistent logging
            try
            {
                AppTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    AppTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                }
                catch (TimeZoneNotFoundException)
                {
                    AppTimeZone = TimeZoneInfo.Utc;
                    Console.WriteLine("Warning: Could not set German timezone for logging, using UTC");
                }
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("/app/logs/logfile.log", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
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
            // Timezone is already configured in ConfigureLogging
            var config = builder.Configuration;
            MapsterConfig.RegisterMappings();
            var connectionString = config.GetConnectionString("DefaultConnection");
            Console.WriteLine($"[EF DEBUG] Registering DbContext with connection string: {connectionString}");

static ServerVersion GetDesignTimeServerVersion(string connectionString)
{
    try 
    {
        // Fix connection string for design-time (localhost instead of docker container)
        if (connectionString.Contains("Server=db;")) {
            connectionString = connectionString.Replace("Server=db;", "Server=localhost;");
        }
        return ServerVersion.AutoDetect(connectionString);
    }
    catch 
    {
        // Fallback for design-time when database is not available
        return ServerVersion.Create(Version.Parse("8.0.0"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
    }
}
            builder.Services.AddHttpClient();
            
            // For design-time tools, use localhost and fixed MySQL version to avoid connection issues
            // Check if we're running in a container or in development mode
            var inContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            builder.Services.AddDbContext<Ugh_Context>(options =>
                options.UseMySql(connectionString, serverVersion)
            );
            builder.Services.AddControllers();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(
                typeof(VerifyEmailCommandHandler).Assembly
            );
            builder.Services.Configure<AwsOptions>(builder.Configuration.GetSection("AwsOptions"));
            ConfigureAuthentication(builder);
            ConfigureCors(builder);
            ConfigureMailSettings(builder);
            ConfigureSwagger(builder);
            RegisterServices(builder.Services);
            //CreateAutoAdminUser(builder.Services.BuildServiceProvider().GetService<UserService>());

            builder.Services.Configure<StripeWebhookPolicyOptions>(config.GetSection("StripeWebhookPolicy"));

            builder.Services.AddRateLimiter(options =>
            {
                var policyConfig = config.GetSection("StripeWebhookPolicy").Get<StripeWebhookPolicyOptions>();

                options.AddPolicy("StripeWebhookPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                        factory: key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = policyConfig.PermitLimit,
                            Window = TimeSpan.FromMinutes(policyConfig.WindowMinutes),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = policyConfig.QueueLimit
                        }));
            });
        }

        private static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                    ),
                };
            });
        }

        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin => true) // Allow any origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
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
                        Type = ReferenceType.SecurityScheme,
                    },
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
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
                        new string[] { }
                    },
                };
                c.AddSecurityRequirement(securityRequirement);
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<EmailService>();
            services.AddTransient<UserService>();
            services.AddScoped<PasswordService>();
            services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();
            services.AddScoped<S3Service>();
            services.AddScoped<HtmlTemplateService>();
            services.AddScoped<IUrlBuilderService, UrlBuilderService>();
            //services.AddScoped<MembershipService>();
            services.AddMemoryCache();
            services.AddScoped<TokenService>();
            services.AddTransient<ICouponRepository, CouponRepository>();
            services.AddScoped<UGHApi.Repositories.ReviewRepository>();
            services.AddTransient<IStripeService, StripeService>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UGH.Infrastructure.Repositories.UserRepository>();
            services.AddScoped<IShopItemRepository, ShopItemRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUserMembershipRepository, UserMembershipRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();

            services.AddScoped<UGHApi.Repositories.OfferRepository>();
            
            // NEW: Geographic services for OpenStreetMap integration
            services.AddHttpClient<NominatimGeocodingService>();
            services.AddScoped<IGeocodingService, NominatimGeocodingService>();
            services.AddScoped<IAddressService, AddressService>();
            
            services.AddHttpContextAccessor();
        }

        private static void ConfigureMiddleware(WebApplication app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<UGHApi.Middleware.UserExistenceMiddleware>();
            var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

            DatabaseWaiter.WaitForDatabaseConnection(connectionString);
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            
            // Only use HTTPS redirection in production!
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }
            
            app.UseCors(); // Use default policy
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static void ConfigureEndpoints(WebApplication app)
        {
            app.MapControllers();
            
            // Health Check Endpoint
            app.MapGet("/api/health", () => Results.Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                germanTime = GetGermanTime(),
                version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
                environment = app.Environment.EnvironmentName
            }))
            .WithName("HealthCheck")
            .WithOpenApi();
        }
        
        /// <summary>
        /// Get current German time (MEZ/MESZ)
        /// </summary>
        public static DateTime GetGermanTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AppTimeZone);
        }
    }
}
