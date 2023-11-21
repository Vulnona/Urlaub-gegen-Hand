using System.Drawing;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHModels;



    public class UghContext : DbContext
{
    //static readonly string connectionString = "Server=db; User ID=root; Password=password; Database=db";
    public UghContext (DbContextOptions<UghContext> options) :base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles{get;set;}
    public DbSet<Membership> Memberships{get;set;}
    public DbSet<Skill> Skills{get;set;}
    public DbSet<Continent> Continents {get;set;}
    public DbSet<Backend.Models.Region> Regions{get;set;}
    public DbSet<Country> Countries{get;set;}
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration =new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString =configuration.GetConnectionString("DefaultConnection"); 
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
