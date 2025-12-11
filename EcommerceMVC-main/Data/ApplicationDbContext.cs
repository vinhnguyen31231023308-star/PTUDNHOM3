using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace EcommerceMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<UsageTag> UsageTags { get; set; }
        public DbSet<ProductUsage> ProductUsages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<VolumeOption> VolumeOptions { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }

        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AdminAccount> AdminAccounts { get; set; }

        // Các bảng thông tin tài khoản
        public DbSet<UserProfile> UserProfiles { get; set; } = default!;
        public DbSet<Address> Addresses { get; set; } = default!;
        public DbSet<Payment> PaymentMethods { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình thêm ở đây

            modelBuilder.Entity<ProductUsage>()
                .HasKey(pu => new { pu.ProductId, pu.UsageTagId });
        }
    }
}
