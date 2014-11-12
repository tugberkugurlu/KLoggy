using KLoggy.Domain;
using KLoggy.Web.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;

namespace KLoggy.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHostingEnvironment _env;

        public Startup(ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            if(loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }

            if (env == null)
            {
                throw new ArgumentNullException("env");
            }

            _loggerFactory = loggerFactory;
            _env = env;

            _configuration = new Configuration()
                .AddIniFile(@"App_Data\config.ini")
                .AddIniFile(@"App_Data\git.ini");
        }

        // This method gets called by the runtime.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                // Configure MVC options here. such as filters, etc.
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                // Configure Razor View Engine options here. such as LanguageViewLocationExpander
            });

            services.Configure<AppOptions>(options =>
            {
                options.ServeCdnContent = Convert.ToBoolean(_configuration.Get("App:ServeCdnContent"));
                options.CdnServerBaseUrl = _configuration.Get("App:CdnServerBaseUrl");
                options.GenerateLowercaseUrls = Convert.ToBoolean(_configuration.Get("App:GenerateLowercaseUrls"));
                options.EnableBundlingAndMinification = Convert.ToBoolean(_configuration.Get("App:EnableBundlingAndMinification"));
                options.LatestCommitSha = _configuration.Get("git:sha");
            });

            services.AddMvc(_configuration);

            services.AddScoped<IUrlHelper, CustomUrlHelper>();
            services.AddScoped<IProfileLinkManager, InMemoryProfileLinkManager>();
            services.AddSingleton<FrontEndAssetManager, FrontEndAssetManager>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app)
        {
            // Add the following to the request pipeline only in development environment.
            if (string.Equals(_env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action}");
                routes.MapRoute("defaultRoute", "{controller=Home}/{action=Index}");

                routes.MapRoute(
                    "controllerRoute",
                    "{controller}",
                    new { controller = "Home" });
            });
        }
    }
}