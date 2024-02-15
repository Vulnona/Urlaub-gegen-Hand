using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using UGHApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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
			
			builder.Logging.ClearProviders();
			builder.Logging.AddConsole();
			builder.Logging.SetMinimumLevel(LogLevel.Debug);
			
			
			builder.WebHost.ConfigureKestrel(serverOptions =>
			{
				serverOptions.ListenAnyIP(8080); // PORT
			});
			
			var appsettingsPath = "/app/binaries"; 
			
			IConfigurationRoot config =new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
			
            var connectionString=config.GetConnectionString("DefaultConnection");
			
			var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();       
            // Logging des Connection Strings
            //logger.LogInformation($"Geladener Connection String: {connectionString}");

			builder.Services.AddDbContext<UghContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UghContext>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UHG Api", Version = "v1" });

                // Define the security scheme
                        var securityScheme = new OpenApiSecurityScheme
                        {
                            Name = "JWT Authentication",
                            Description = "Enter JWT Bearer token **_only_**",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer", // must be lowercase
                            BearerFormat = "JWT",
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        };

                c.AddSecurityDefinition("Bearer", securityScheme);

                // Make sure swagger UI requires a Bearer token specified
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

            // Register the EmailService as a Singleton
            builder.Services.AddSingleton<EmailService>();
            builder.Services.AddTransient<UserService>();
            builder.Services.AddScoped<PasswordService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<TokenService>();

            var app = builder.Build();
			
			app.UseMiddleware<ErrorHandlingMiddleware>();
			
			DatabaseWaiter.WaitForDatabaseConnection(connectionString); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
           
            app.Run();
        }
    }
}
