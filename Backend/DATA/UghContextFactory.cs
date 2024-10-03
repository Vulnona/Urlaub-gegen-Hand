using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class UghContextFactory:IDesignTimeDbContextFactory<Ugh_Context>{
    public Ugh_Context CreateDbContext(string[] args){
        IConfigurationRoot configuration =new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var builder=new DbContextOptionsBuilder<Ugh_Context>();
        var connectionString =configuration.GetConnectionString("DefaultConnection");
        builder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString),b=>b.MigrationsAssembly ("UGHApi"));       
        return new Ugh_Context(builder.Options);
    }
}