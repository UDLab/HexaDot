﻿using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Serialization;
using Autofac;
using Hangfire;

using HexaDot.Features.Security;
using HexaDot.Data.Models;
using HexaDot.Features.DataAccess;
using HexaDot.Web.Configuration;
using HexaDot.Data.ModelBinding;

//using HexaDot.Hangfire;
//using HexaDot.Data.ModelBinding;


namespace HexaDot.Web
{

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Setup configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("version.json")
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
                //builder.AddApplicationInsightsSettings(developerMode: false);
            }
            else if (env.IsStaging() || env.IsProduction())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: false);
            }

            Configuration = builder.Build();

            Configuration["version"] = new ApplicationEnvironment().ApplicationVersion; // version in project.json

        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Add CORS support.
            // Must be first to avoid OPTIONS issues when calling from Angular/Browser
            services.AddCors(options =>
            {
                options.AddPolicy("HexaDot", HexaDotCorsPolicyFactory.BuildHexaDotOpenCorsPolicy());
            });

            // Add Application Insights data collection services to the services container.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add Entity Framework services to the services container.
            services.AddDbContext<HexaDotContext>(options => options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            Options.LoadConfigurationOptions(services, Configuration);

            // Add Identity services to the services container.
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Home/AccessDenied");
            })
            .AddEntityFrameworkStores<HexaDotContext>()
            .AddDefaultTokenProviders();

            // Add Authorization rules for the app
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SysAdmin", b => b.RequireClaim(ClaimTypes.UserType, "SysAdmin", "InsCoordinator"));
                options.AddPolicy("InsCoordinator", b => b.RequireClaim(ClaimTypes.UserType, "InsCoordinator"));
            });

            services.AddLocalization();

            //Currently HexaDot only supports en-US culture. This forces datetime and number formats to the en-US culture regardless of local culture
            var usCulture = new CultureInfo("en-US");
            var supportedCultures = new[] { usCulture };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(usCulture, usCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Add MVC services to the services container.
            // config add to get passed Angular failing on Options request when logging in.
            services.AddMvc(config =>
            {
                config.ModelBinderProviders.Insert(0, new AdjustToTimezoneModelBinderProvider());
            })
                .AddJsonOptions(options =>
                options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //Hangfire
            //services.AddHangfire(configuration => configuration.UseSqlServerStorage(Configuration["Data:HangfireConnection:ConnectionString"]));

            // configure IoC support
            var container = HexaDot.Web.Configuration.Services.CreateIoCContainer(services, Configuration);
            return container.Resolve<IServiceProvider>();
        }

        // Configure is called after ConfigureServices is called.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, SampleDataGenerator sampleData, HexaDotContext context, IConfiguration configuration)
        {
            // Put first to avoid issues with OPTIONS when calling from Angular/Browser.  
            app.UseCors("HexaDot");

            // todo: in RC update we can read from a logging.json config file
            loggerFactory.AddConsole((category, level) =>
            {
                if (category.StartsWith("Microsoft."))
                {
                    return level >= LogLevel.Information;
                }
                return true;
            });

            if (env.IsDevelopment())
            {
                // this will go to the VS output window
                loggerFactory.AddDebug((category, level) =>
                {
                    if (category.StartsWith("Microsoft."))
                    {
                        return level >= LogLevel.Information;
                    }
                    return true;
                });
            }

            // Add Application Insights to the request pipeline to track HTTP request telemetry data.
            app.UseApplicationInsightsRequestTelemetry();

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if (env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            // Track data about exceptions from the application. Should be configured after all error handling middleware in the request pipeline.
            app.UseApplicationInsightsExceptionTelemetry();

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            app.UseRequestLocalization();

            Authentication.ConfigureAuthentication(app, Configuration);

            //call Migrate here to force the creation of the HexaDot database so Hangfire can create its schema under it
            if (!env.IsProduction())
            {
                context.Database.Migrate();
            }

            //Hangfire
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangireDashboardAuthorizationFilter() } });
            //app.UseHangfireServer();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute", template: "{area:exists}/{controller}/{action=Index}/{id?}");
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Add sample data and test admin accounts if specified in Config.Json.
            // for production applications, this should either be set to false or deleted.
            if (Configuration["SampleData:InsertSampleData"] == "true")
            {
                sampleData.InsertTestData();
            }

            if (Configuration["SampleData:InsertTestUsers"] == "true")
            {
                await sampleData.CreateAdminUser();
            }

        }
    }
}