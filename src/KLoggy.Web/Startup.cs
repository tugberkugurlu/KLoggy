using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.DependencyInjection;
using KLoggy.Web.Infrastructure;
using System.Linq;
using System;

namespace KLoggy.Web 
{    
    public class Startup 
    {
        public void Configure(IApplicationBuilder app)
        {
            var configuration = new Configuration();
            configuration.AddIniFile(@"App_Data\config.ini");
            configuration.AddIniFile(@"App_Data\git.ini");
            
            app.UseFileServer();
            app.UseErrorPage();
            
            app.UseServices(services =>
            {
                services.SetupOptions<MvcOptions>(options =>
                {
                    // Configure MVC options here. such as filters, etc.
                });
                
                services.SetupOptions<RazorViewEngineOptions>(options =>
                {
                    // Configure Razor View Engine options here. such as LanguageViewLocationExpander
                });
                
                services.SetupOptions<AppOptions>(options =>
                {
                    options.ServeCdnContent = Convert.ToBoolean(configuration.Get("App:ServeCdnContent"));
                    options.CdnServerBaseUrl = configuration.Get("App:CdnServerBaseUrl");
                    options.GenerateLowercaseUrls = Convert.ToBoolean(configuration.Get("App:GenerateLowercaseUrls"));
                    options.EnableBundlingAndMinification = Convert.ToBoolean(configuration.Get("App:EnableBundlingAndMinification"));
                    options.LatestCommitSha = configuration.Get("git:sha");
                });
                
                // Add MVC services to the services container
                services.AddMvc(configuration);
                
                services.AddScoped<IUrlHelper, CustomUrlHelper>();
                services.AddScoped<IBadgesManager, BadgesManager>();
                services.AddSingleton<FrontEndAssetManager, FrontEndAssetManager>();
            });
            
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