using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHModels;



    public class UghContext : DbContext
{
    static readonly string connectionString = "Server=localhost; User ID=root; Password=Hummel; Database=ughdata";
    public UghContext (DbContextOptions<UghContext> options) :base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles{get;set;}
    public DbSet<Membership> Memberships{get;set;}
    public DbSet<Skill> Skills{get;set;}
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
