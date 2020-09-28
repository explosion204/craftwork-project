using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Craftwork_Project.Domain;
using Craftwork_Project.Domain.Repositories.EntityFramework;
using Craftwork_Project.Domain.Repositories.Interfaces;
using Craftwork_Project.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Craftwork_Project
{
    public class Startup
    {
        public IConfiguration Configuration { get;  }

        public Startup(IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            // binding Config class to appsettings.json
            Configuration.Bind("Project", new Config());

            // adding model repositories as services
            services.AddTransient<ICategoryRepository, EFCategoryRepository>();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IPurchaseDetailRepository, EFPurchaseDetailRepository>();
            
            // adding repository aggregator as service
            services.AddTransient<DataManager>();

            // setting up db context
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(Config.ConnectionString));

            // setting up identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // setting up auth cookie
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
            
            // add MVC
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new AdminAreaAuth("Admin", "AdminArea"));
            })
            .AddSessionStateTempDataProvider();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // dev env
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // setting up static files
            app.UseStaticFiles();
            
            // setting up routing
            app.UseRouting();
            
            // setting up auth
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // routes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}");
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=categories}/{action=index}/{id?}");
            });
            
        }
    }
}