
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;


namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole,
        IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, 
        IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var listValueComparer = new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList());

            builder.Entity<Product>()
                .Property(e => e.Images)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v))
                .Metadata.SetValueComparer(listValueComparer);

            builder.Entity<Product>()
                .Property(e => e.Colors)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v))
                .Metadata.SetValueComparer(listValueComparer);

            builder.Entity<Product>()
              .Property(e => e.Sizes)
              .HasConversion(
                  v => JsonConvert.SerializeObject(v),
                  v => JsonConvert.DeserializeObject<List<string>>(v))
              .Metadata.SetValueComparer(listValueComparer);

            builder.Entity<AppUser>()
                  .HasMany(u => u.UserRoles)
                  .WithOne(u => u.User)
                  .HasForeignKey(ur => ur.UserId)
                  .IsRequired();

             builder.Entity<AppRole>()
                  .HasMany(u => u.UserRoles)
                  .WithOne(u => u.Role)
                  .HasForeignKey(ur => ur.RoleId)
                  .IsRequired();

            builder.Entity<AppUser>()
              .HasOne(u => u.Cart)
              .WithOne(u => u.AppUser)
              .HasForeignKey<Cart>(u => u.AppUserId);

            builder.Entity<AppUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.AppUser)
                .HasForeignKey(o => o.AppUserId);


            builder.Entity<Product>()
                .HasIndex(p => p.Slug).IsUnique();

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)");

            builder.Entity<Category>()
                .Property(c => c.Name);

            builder.Entity<Cart>()
                .Property(c => c.SubTotal).HasColumnType("decimal(18,4)");

            builder.Entity<Order>()
                .Property(o => o.SubTotal)
                .HasColumnType("decimal(18,4)"); 

              builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,4)"); 
              
        }

    }
}
