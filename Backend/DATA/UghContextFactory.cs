using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class UghContextFactory:IDesignTimeDbContextFactory<UghContext>{
    public UghContext CreateDbContext(string[] args){
        IConfigurationRoot configuration =new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var builder=new DbContextOptionsBuilder<UghContext>();
        var connectionString =configuration.GetConnectionString("DefaultConnection");
        builder.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString),b=>b.MigrationsAssembly ("UGHApi"));       
        return new UghContext(builder.Options);
    }
}