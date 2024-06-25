using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sofra.Api.Helpers;
using Sofra.Api.Models;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Claims;

namespace Sofra.Api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var cascadeFKs = modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(t => t.GetForeignKeys())
                    .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<Meal>()
       .HasMany(m => m.MealPhotos)
       .WithOne(mp => mp.Meal)
       .HasForeignKey(mp => mp.MealId)
       .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Kitchen>(entity =>
                     entity.HasCheckConstraint("CK_MaxDeliveryDistance", "MaxDeliveryDistance >= 1.0 AND MaxDeliveryDistance <= 10")
            );

            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Latitude)
                    .IsRequired()
                    .HasPrecision(9, 6)
                    .HasColumnType("decimal(9, 6)")
                    .HasDefaultValue(0)
                    .HasComment("Latitude must be between 22.0 and 31.5 degrees.");

                entity.Property(e => e.Longitude)
                    .IsRequired()
                    .HasPrecision(9, 6)
                    .HasColumnType("decimal(9, 6)")
                    .HasDefaultValue(0)
                    .HasComment("Longitude must be between 25.0 and 35.0 degrees.");

                entity.HasCheckConstraint("CK_Location_Latitude", "Latitude >= 22.0 AND Latitude <= 31.5");
                entity.HasCheckConstraint("CK_Location_Longitude", "Longitude >= 25.0 AND Longitude <= 35.0");
            });

            modelBuilder.Entity<Meal>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            foreach (var entityEntry in entries)
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    if (entityEntry.Property(x => x.IsDeleted).IsModified
                       && entityEntry.Property(x => x.IsDeleted).CurrentValue == true)
                    {
                        entityEntry.Property(x => x.DeletedById).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.DeletedOn).CurrentValue = DateTime.UtcNow;
                    }
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }



        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Kitchen> Kitchens { get; set; }
        public DbSet<KitchenCategory> KitchenCategories { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealPhoto> MealPhotos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
