using Craftwork_Project.Domain.Models;
using Craftwork_Project.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Craftwork_Project.Domain
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<IdentityUser>().HasData(new IdentityUser()
            {
                Id = "5a1e1cfc-ee1d-4afb-aad3-a6d932066727",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "dzmitriy20magic@gmail.com",
                NormalizedEmail = "DZMITRIY20MAGIC@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "admin"),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = "1a1059b8-61e5-4ea8-b2dd-7d44793910f4",
                UserId = "5a1e1cfc-ee1d-4afb-aad3-a6d932066727"
            });
        }
    }
}