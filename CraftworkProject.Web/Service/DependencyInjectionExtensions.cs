﻿using System.IO;
using AutoMapper.Configuration;
using CraftworkProject.Services.Implementations;
using CraftworkProject.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CraftworkProject.Web.Service
{
    public static class ServiceExtensions
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration config)
        {
            var appOptions = new AppOptions();
            var googleOptions = new GoogleOptions();
            config.GetSection(AppOptions.SectionName).Bind(appOptions);
            config.GetSection(GoogleOptions.SectionName).Bind(googleOptions);
            
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = appOptions.CompanyEmail + "Auth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.SlidingExpiration = true;
            });
            
            services.AddAuthentication().AddGoogle(opt =>
            {
                opt.ClientId = googleOptions.ClientId;
                opt.ClientSecret = googleOptions.ClientSecret;
            });
            
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });
        }

        public static void AddAppServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            var mailOptions = new MailingOptions();
            var twilioOptions = new TwilioOptions();
            var loggingOptions = new LoggingOptions();
            config.GetSection(MailingOptions.SectionName).Bind(mailOptions);
            config.GetSection(TwilioOptions.SectionName).Bind(twilioOptions);
            config.GetSection(LoggingOptions.SectionName).Bind(loggingOptions);

            services.AddScoped<IDataManager, DataManager>();
            services.AddScoped<IEmailService>(x => new EmailService(
                mailOptions.Sender, mailOptions.SmtpServer, mailOptions.SmtpPort, mailOptions.Username, mailOptions.Password
            ));
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISmsService>(x => new SmsService(
                twilioOptions.Sender, twilioOptions.AccountSid, twilioOptions.AuthToken
            ));
            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddFile(Path.Combine(env.WebRootPath, loggingOptions.CommonLogFilePath));
                opt.AddFile(Path.Combine(env.WebRootPath, loggingOptions.ErrorLogFilePath), LogLevel.Error);
            });
            services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
        }
    }
}