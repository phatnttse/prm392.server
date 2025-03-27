using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;


namespace PRM392.Repositories.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public ApplicationDbContext()
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Roles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Claims)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(r => r.Users)
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationRole>()
                .HasMany(r => r.Claims)
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion(new EnumToStringConverter<OrderStatus>());

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentStatus)
                .HasConversion(new EnumToStringConverter<PaymentStatus>());

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .HasConversion(new EnumToStringConverter<PaymentMethod>());
        }


        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddAuditInfo();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddAuditInfo()
        {

            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity &&
                           (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                var now = DateTimeOffset.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                }

                entity.UpdatedAt = now;
            }
        }
    }
}
