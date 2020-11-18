using CraftworkProject.Services.Interfaces;
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
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        } 
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("Project", new Config());
            Configuration.Bind("Mailing", new MailConfig());
            Configuration.Bind("Twilio", new TwilioConfig());
            ConfigureAppServices.Configure(services, _env);
            ConfigureAuthServices.Configure(services);
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
            }
            
            app.ConfigureExceptionHandler(logger);

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