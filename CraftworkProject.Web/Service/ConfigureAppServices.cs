using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure;
using CraftworkProject.Infrastructure.Models;
using CraftworkProject.Infrastructure.Repositories;
using CraftworkProject.Services.Implementations;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftworkProject.Web.Service
{
    public static class ConfigureAppServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(Config.ConnectionString));
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<PurchaseDetail>, PurchaseDetailRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            
            services.AddTransient<IDataManager, DataManager>();
            services.AddTransient<IUserManager>(x =>
                new ApplicationUserManager(
                    x.GetRequiredService<UserManager<EFUser>>(), 
                    x.GetRequiredService<RoleManager<EFUserRole>>(), 
                    x.GetRequiredService<SignInManager<EFUser>>(), 
                    x.GetRequiredService<IMapper>()
                    )
            );
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(typeof(IMapper), mapperConfig.CreateMapper());
        }
    }
}