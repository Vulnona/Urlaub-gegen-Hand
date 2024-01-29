using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace UGHApi
{
    public class Program
    {
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
            logger.LogInformation($"Geladener Connection String: {connectionString}");

			builder.Services.AddDbContext<UghContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<UghContext>();

            // Register the EmailService as a Singleton
            builder.Services.AddSingleton<EmailService>();

            var app = builder.Build();
			
			app.UseMiddleware<ErrorHandlingMiddleware>();
			
			DatabaseWaiter.WaitForDatabaseConnection(connectionString); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
