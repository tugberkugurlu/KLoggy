using KLoggy.Domain;
using KLoggy.Web.Infrastructure;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace KLoggy.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new Configuration()
                .AddIniFile(@"App_Data\config.ini")
                .AddIniFile(@"App_Data\git.ini");
        }

        public void Configure(IApplicationBuilder app)
        {   
            app.UseFileServer();
            app.UseErrorPage();
            
            app.UseServices(services =>
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