using System;
using CraftworkProject.Infrastructure;
using CraftworkProject.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CraftworkProject.Web.Service
{
    public static class ConfigureAuthServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddIdentity<EFUser, EFUserRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<EFUser, EFUserRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<EFUserRole, ApplicationDbContext, Guid>>();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = Config.CompanyEmail + "Auth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });
            
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new AdminAreaAuth("Admin", "AdminArea"));
            })
            .AddSessionStateTempDataProvider();

            services.AddSession();
        }
    }
}