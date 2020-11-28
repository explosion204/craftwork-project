using System;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftworkProject.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, DbContextOptions options)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(options.ConnectionString), 
                ServiceLifetime.Transient);
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<PurchaseDetail>, PurchaseDetailRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Review>, ReviewRepository>();
        }
        
        public static void ConfigureIdentity(this IServiceCollection services, Action<IdentityOptions> options)
        {
            services.AddIdentity<EFUser, EFUserRole>(options)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<EFUser, EFUserRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<EFUserRole, ApplicationDbContext, Guid>>();
        }

        public static void AddUserManager(this IServiceCollection services)
        {
            services.AddScoped<IUserManager>(x =>
                new ApplicationUserManager(
                    x.GetRequiredService<UserManager<EFUser>>(), 
                    x.GetRequiredService<RoleManager<EFUserRole>>(), 
                    x.GetRequiredService<SignInManager<EFUser>>(), 
                    x.GetRequiredService<IRepository<Review>>(),
                    x.GetRequiredService<IMapper>()
                )
            );
        }

        public static void AddUserManagerHelper(this IServiceCollection services)
        {
            services.AddScoped<IUserManagerHelper>(x => 
                new UserManagerHelper(
                    x.GetRequiredService<UserManager<EFUser>>(),
                    x.GetRequiredService<SignInManager<EFUser>>()
                )
            );
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(typeof(IMapper), mapperConfig.CreateMapper());
        }
    }
}