using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UGH.Domain.Entities;
using UGHApi.Entities;

namespace UGHApi.DATA
{
    public class Ugh_Context : DbContext
    {
        public Ugh_Context(DbContextOptions<Ugh_Context> options)
            : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Address> addresses { get; set; } // NEW: Geographic address system
        public DbSet<ShopItem> shopitems { get; set; }
        public DbSet<Transaction> transaction { get; set; }
        public DbSet<UserProfile> userprofiles { get; set; }
        public DbSet<Membership> memberships { get; set; }
        public DbSet<UserMembership> usermembership { get; set; }
        public DbSet<Skill> skills { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<EmailVerificator> emailverificators { get; set; }
        public DbSet<PasswordResetToken> passwordresettokens { get; set; }
        public DbSet<Coupon> coupons { get; set; }
        public DbSet<Redemption> redemptions { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<Picture> pictures { get; set; }
        public DbSet<OfferTypeRequest> offertyperequests { get; set; }
        public DbSet<OfferTypeLodging> offertypelodgings { get; set; }
        public DbSet<OfferApplication> offerapplication { get; set; }
        public DbSet<Accommodation> accomodations { get; set; }
        public DbSet<SuitableAccommodation> accommodationsuitables { get; set; }

        public DbSet<Backend.Models.DeletedUserBackup> DeletedUserBackups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"[EF DEBUG] Using connection string: {connectionString}");
            try {
                System.IO.File.AppendAllText("/app/connectionstring.log", $"[EF DEBUG] {connectionString}\n");
            } catch {}
            try {
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            } catch (Exception ex) {
                Console.WriteLine($"[EF ERROR] Failed to connect: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new Backend.DATA.Configurations.DeletedUserBackupConfiguration());
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configure Review-User relationships
            builder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Reviewed)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.ReviewedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
