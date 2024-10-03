using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;

public class Ugh_Context : DbContext
{
    public Ugh_Context(DbContextOptions<Ugh_Context> options)
        : base(options) { }

    public DbSet<User> users { get; set; }
    public DbSet<UserProfile> userprofiles { get; set; }
    public DbSet<Membership> memberships { get; set; }
    public DbSet<UserMembership> usermembership { get; set; }
    public DbSet<Skill> skills { get; set; }
    public DbSet<Continent> continents { get; set; }
    public DbSet<Region> regions { get; set; }
    public DbSet<Country> countries { get; set; }
    public DbSet<EmailVerificator> emailverificators { get; set; }
    public DbSet<UserRole> userroles { get; set; }
    public DbSet<UserRoleMapping> userrolesmapping { get; set; }
    public DbSet<Coupon> coupons { get; set; }
    public DbSet<Redemption> redemptions { get; set; }
    public DbSet<Offer> offers { get; set; }
    public DbSet<OfferApplication> offerapplication { get; set; }
    public DbSet<Accommodation> accomodations { get; set; }
    public DbSet<SuitableAccommodation> accommodationsuitables { get; set; }
    public DbSet<Review> reviews { get; set; }
    public DbSet<State> states { get; set; }
    public DbSet<City> cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            b => b.MigrationsAssembly("UGH.Infrastructure")
        );
    }
}
