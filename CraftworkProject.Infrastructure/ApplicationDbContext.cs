using System;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CraftworkProject.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>()
            {
                Id = Guid.Parse("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid>()
            {
                Id = Guid.Parse("3800721a-cf25-427e-b5e1-9c26710df0d5"),
                Name = "customer",
                NormalizedName = "CUSTOMER"
            });

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser()
            {
                Id = Guid.Parse("5a1e1cfc-ee1d-4afb-aad3-a6d932066727"),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "dzmitriy20magic@gmail.com",
                NormalizedEmail = "DZMITRIY20MAGIC@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "admin"),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>()
            {
                RoleId = Guid.Parse("1a1059b8-61e5-4ea8-b2dd-7d44793910f4"),
                UserId = Guid.Parse("5a1e1cfc-ee1d-4afb-aad3-a6d932066727")
            });
        }
    }
}