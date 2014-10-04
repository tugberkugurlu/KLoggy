using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using KLoggy.Web.Infrastructure;
using Microsoft.Framework.OptionsModel;
using System.Collections.Generic;
using System;

namespace KLoggy.Web.Components
{
    [ViewComponent(Name = "Assets")]
    public class AssetsViewComponent : ViewComponent
    {
        private readonly IOptionsAccessor<AppOptions> _appOptionsAccessor;
        private readonly FrontEndAssetManager _assetManager;
        private readonly IUrlHelper _urlHelper;
        
        public AssetsViewComponent(FrontEndAssetManager assetManager, IUrlHelper urlHelper, IOptionsAccessor<AppOptions> appOptionsAccessor)
        {
            if (assetManager == null)
            {
                throw new ArgumentNullException("assetManager");
            }
            
            if (urlHelper == null)
            {
                throw new ArgumentNullException("urlHelper");
            }
            
            if (appOptionsAccessor == null)
            {
                throw new ArgumentNullException("appOptionsAccessor");
            }
            
            _assetManager = assetManager;
            _urlHelper = urlHelper;
            _appOptionsAccessor = appOptionsAccessor;
        }
        
        public IViewComponentResult Invoke(string assetType)
        {
            string viewName;
            IList<string> filePaths = new List<string>();
            
            if(assetType.Equals("scripts", StringComparison.OrdinalIgnoreCase))
            {
                viewName = "Scripts";
                AssetInfo assetInfo = _assetManager.GetScripts();
                foreach(var assetFileInfo in assetInfo.Files)
                {
                    // TODO: Look here if we need to serve minified + bundled file
                    filePaths.Add(_urlHelper.Content(string.Format("~/assets/js/{0}", assetFileInfo.FileName)));
                }
            }
            else if(assetType.Equals("styles", StringComparison.OrdinalIgnoreCase))
            {
                viewName = "Styles";
                AssetInfo assetInfo = _assetManager.GetStyles();
                foreach(var assetFileInfo in assetInfo.Files)
                {
                    // TODO: Look here if we need to serve minified + bundled file
                    filePaths.Add(_urlHelper.Content(string.Format("~/assets/css/{0}", assetFileInfo.FileName)));
                }
            }
            else 
            {
                throw new InvalidOperationException(string.Format("Asset type '{0}' is not recognized."));
            }
                                  
            return View(viewName, filePaths);
        }
    }
}