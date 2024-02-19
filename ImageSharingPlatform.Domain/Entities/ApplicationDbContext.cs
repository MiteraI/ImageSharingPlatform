using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Domain.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ImageCategory> ImageCategories { get; set; }
        public DbSet<ImageRequest> ImageRequests { get; set; }
        public DbSet<SharedImage> SharedImages { get; set; }
        public DbSet<RequestDetail> RequestDetails { get; set; }
        public DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }
        public DbSet<OwnedSubscription> OwnedSubscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(user =>
            {
                user.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(
                    "UserRole",
                    l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                    r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                    j => j.HasKey("UserId", "RoleId")
                );

                user.ToTable("user");
            });

            modelBuilder.Entity<Role>(role => role.ToTable("role"));

            modelBuilder.Entity<ImageCategory>(ic =>
            {
                ic.ToTable("image_category");
            });

            modelBuilder.Entity<SharedImage>(si =>
            {
                si.Property(si => si.ImageName).HasColumnName("image_name");
                si.Property(si => si.ImageUrl).HasColumnName("image_url");
                si.Property(si => si.IsPremium).HasColumnName("is_premium");

                si.HasOne(si => si.Artist).WithMany().HasForeignKey(si => si.ArtistId);

                si.HasOne(si => si.ImageCategory).WithMany().HasForeignKey(si => si.ImageCategoryId).OnDelete(DeleteBehavior.SetNull);

                si.ToTable("shared_image");
            });

            modelBuilder.Entity<Review>(review =>
            {
                review.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);

                review.ToTable("review");
            });

            modelBuilder.Entity<ImageRequest>(ir =>
            {
                ir.HasOne(ir => ir.RequesterUser).WithMany().HasForeignKey(ir => ir.RequesterUserId).OnDelete(DeleteBehavior.SetNull);

                ir.HasOne(ir => ir.Artist).WithMany().HasForeignKey(ir => ir.ArtistId).OnDelete(DeleteBehavior.Restrict);

                ir.ToTable("image_request");
            });

            modelBuilder.Entity<SubscriptionPackage>(sp =>
            {
				sp.ToTable("subscription_package");

                sp.HasOne(sp => sp.Artist).WithOne().HasForeignKey<SubscriptionPackage>(sp => sp.ArtistId);
			});

            modelBuilder.Entity<OwnedSubscription>(os =>
            {
				os.ToTable("owned_subscription");
			});
        }
    }
}
