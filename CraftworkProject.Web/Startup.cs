using CraftworkProject.Web.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CraftworkProject.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("Project", new Config());
            Configuration.Bind("Mailing", new MailConfig());
            ConfigureAppServices.Configure(services);
            ConfigureAuthServices.Configure(services);
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
            });
            
        }
    }
}