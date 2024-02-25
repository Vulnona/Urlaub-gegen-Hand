using System.Drawing;
using UGHApi.Models;
using Microsoft.EntityFrameworkCore;
using UGHModels;
using Backend.Models;



public class UghContext : DbContext
{
    public UghContext (DbContextOptions<UghContext> options) :base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles{get;set;}
    public DbSet<Membership> Memberships{get;set;}
    public DbSet<Skill> Skills{get;set;}
    public DbSet<Continent> Continents {get;set;}
    public DbSet<UGHApi.Models.Region> Regions{get;set;}
    public DbSet<Country> Countries{get;set;}
    public DbSet<EmailVerificator> EmailVerificators{get;set;}
    public DbSet<UserRole> UserRoles { get;set;}
    public DbSet<UserRoleMapping> UserRolesMapping { get;set;}
    public DbSet<Coupon> Coupons { get;set;}    
    public DbSet<Redemption>Redemptions { get;set;}
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
