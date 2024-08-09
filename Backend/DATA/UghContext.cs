using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UGHApi.Models;
using UGHModels;
using Backend.Models;
using System;
using System.Linq;

public class UghContext : DbContext
{
    public UghContext(DbContextOptions<UghContext> options) : base(options) { }

    public DbSet<User> users { get; set; }
    public DbSet<UserProfile> userprofiles { get; set; }
    public DbSet<Membership> memberships { get; set; }
    public DbSet<Skill> skills { get; set; }
    public DbSet<Continent> continents { get; set; }
    public DbSet<UGHApi.Models.Region> regions { get; set; }
    public DbSet<Country> countries { get; set; }
    public DbSet<EmailVerificator> emailverificators { get; set; }
    public DbSet<UserRole> userroles { get; set; }
    public DbSet<UserRoleMapping> userrolesmapping { get; set; }
    public DbSet<Coupon> coupons { get; set; }
    public DbSet<Redemption> redemptions { get; set; }
    public DbSet<Offer> offers { get; set; }
    public DbSet<Accommodation> accomodations { get; set; }
    public DbSet<SuitableAccommodation> accommodationsuitables { get; set; }
    public DbSet<Ratings> ratings { get; set; }
    public DbSet<Review> reviews { get; set; }
    public DbSet<ReviewOfferUser> reviewofferusers { get; set; }
    public DbSet<ReviewLoginUser> reviewloginusers { get; set; }
    public DbSet<ReviewPost> reviewposts { get; set; }
    public DbSet<RatingUserLogin> ratinguserlogins { get; set; }
    public DbSet<RatingHostLogin> ratinghostlogins { get; set; }
    public DbSet<State> states { get; set; }
    public DbSet<City> cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
