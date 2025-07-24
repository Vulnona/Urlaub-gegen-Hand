using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UGHApi.DATA
{
    public class UghContextFactory : IDesignTimeDbContextFactory<Ugh_Context>
    {
        public Ugh_Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            
            var builder = new DbContextOptionsBuilder<Ugh_Context>();
            
            // For design-time, we need to use localhost instead of 'db' container name
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            // Design-Time Fix: Replace docker container name with localhost for EF tools
            if (connectionString.Contains("Server=db;")) {
                connectionString = connectionString.Replace("Server=db;", "Server=localhost;");
                connectionString = connectionString.Replace("root", "root").Replace("password", "password");
                Console.WriteLine($"Design-Time Connection String: {connectionString}");
            }
            
            // Replace 'db' container name with localhost and use root user for design-time tools
            if (connectionString != null && connectionString.Contains("Server=db"))
            {
                connectionString = connectionString
                    .Replace("Server=db", "Server=localhost")
                    .Replace("User ID=user", "User ID=root");
                Console.WriteLine($"[Design-Time] Using connection string: {connectionString.Replace("Password=password", "Password=***")}");
            }
            
            // Use a fixed MySQL version instead of AutoDetect to avoid connection during build
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 21));
            builder.UseMySql(connectionString, serverVersion, b => b.MigrationsAssembly("UGHApi"));
            return new Ugh_Context(builder.Options);
        }
    }
}
