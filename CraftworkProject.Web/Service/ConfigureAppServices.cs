using System.IO;
using AutoMapper;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Infrastructure;
using CraftworkProject.Infrastructure.Models;
using CraftworkProject.Infrastructure.Repositories;
using CraftworkProject.Services.Implementations;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CraftworkProject.Web.Service
{
    public static class ConfigureAppServices
    {
        public static void Configure(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(Config.ConnectionString), 
                ServiceLifetime.Transient);
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<PurchaseDetail>, PurchaseDetailRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();
            services.AddScoped<IRepository<Review>, ReviewRepository>();
            
            services.AddScoped<IDataManager, DataManager>();
            services.AddScoped<IUserManager>(x =>
                new ApplicationUserManager(
                    x.GetRequiredService<UserManager<EFUser>>(), 
                    x.GetRequiredService<RoleManager<EFUserRole>>(), 
                    x.GetRequiredService<SignInManager<EFUser>>(), 
                    x.GetRequiredService<IRepository<Review>>(),
                    x.GetRequiredService<IMapper>()
                    )
            );
            services.AddScoped<IUserManagerHelper>(x => 
                new UserManagerHelper(
                    x.GetRequiredService<UserManager<EFUser>>(),
                    x.GetRequiredService<SignInManager<EFUser>>()
                )
            );
            services.AddScoped<IEmailService>(x => new EmailService(
                MailConfig.Sender, MailConfig.SmtpServer, MailConfig.SmtpPort, MailConfig.Username, MailConfig.Password
            ));
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISmsService>(x => new SmsService(
                TwilioConfig.Sender, TwilioConfig.AccountSid, TwilioConfig.AuthToken
            ));
            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddFile(Path.Combine(env.WebRootPath, "logs", "all.log"));
                opt.AddFile(Path.Combine(env.WebRootPath, "logs", "error.log"), LogLevel.Error);
            });
            services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            services.AddSingleton(typeof(IMapper), mapperConfig.CreateMapper());

            services.AddSignalR();
            services.AddSignalRCore();
        }
    }
}