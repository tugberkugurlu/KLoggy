using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace KLoggy.Web 
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
    
    public class AssetsOptions
    {
        public string LatestCommitSha { get; set; }
    }
    
    [ViewComponent(Name = "AssetsLinker")]
    public class AssetsLinker : ViewComponent 
    {
        private readonly IOptionsAccessor<AssetsOptions> _optionsAccessor;
        
        public AssetsLinker(IOptionsAccessor<AssetsOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }
        
        public IViewComponentResult Invoke(string fileName)
        {
            return Content(string.Format("{0}-{1}", fileName, _optionsAccessor.Options.LatestCommitSha));
        }
    }
    
    public class Startup 
    {
        public void Configure(IApplicationBuilder app)
        {
            var configuration = new Configuration()
                .AddJsonFile(@"App_Data\config.json")
                .AddIniFile("git.ini");
            
            string latestCommitSha;
            configuration.TryGet("git:sha", out latestCommitSha);
            
            app.UseFileServer();
            app.UseErrorPage();
            
            app.UseServices(services =>
            {   
                services.AddMvc();
                
                services.SetupOptions<MvcOptions>(options =>
                {
                    // Configure MVC options here. such as filters, etc.
                });
                
                services.SetupOptions<RazorViewEngineOptions>(options =>
                {
                    // Configure Razor View Engine options here. such as LanguageViewLocationExpander
                });
                
                services.SetupOptions<AssetsOptions>(options =>
                {
                    options.LatestCommitSha = latestCommitSha;
                });
            });
            
            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action}");

                routes.MapRoute(
                    "controllerActionRoute",
                    "{controller}/{action}",
                    new { controller = "Home", action = "Index" },
                    constraints: null,
                    dataTokens: new { NameSpace = "default" });

                routes.MapRoute(
                    "controllerRoute",
                    "{controller}",
                    new { controller = "Home" });
            });
            
            Console.WriteLine("commit: {0}", latestCommitSha);
        }
    }
}