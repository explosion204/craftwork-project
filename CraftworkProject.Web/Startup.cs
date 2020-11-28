using CraftworkProject.Infrastructure;
using CraftworkProject.Web.Hubs;
using CraftworkProject.Web.Service;
using CraftworkProject.Web.Service.ErrorHandling;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CraftworkProject.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        } 
        
        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<AppOptions>(Configuration.GetSection(AppOptions.SectionName));
            // services.Configure<MailOptions>(Configuration.GetSection(MailOptions.SectionName));
            // services.Configure<TwilioOptions>(Configuration.GetSection(TwilioOptions.SectionName));
            // services.Configure<GoogleOptions>(Configuration.GetSection(GoogleOptions.SectionName));
            
            var appOptions = new AppOptions();
            Configuration.GetSection(AppOptions.SectionName).Bind(appOptions);

            services.AddRepositories(new DbContextOptions { ConnectionString = appOptions.ConnectionString });
            services.ConfigureIdentity(x =>
            {
                x.User.RequireUniqueEmail = true;
                x.Password.RequiredLength = 8;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireDigit = false;
            });
            services.AddAuth(Configuration);
            services.AddAppServices(_env, Configuration);
            services.AddUserManager();
            services.AddUserManagerHelper();
            services.AddMapper();

            
            services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new AdminAreaAuth("Admin", "AdminArea"));
            })
            .AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddSignalR();
            services.AddSignalRCore();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            // dev env
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/error/{0}");
                app.ConfigureExceptionHandler(logger);
            }

            // setting up static files
            app.UseStaticFiles();
            app.UseSession();
            
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
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}