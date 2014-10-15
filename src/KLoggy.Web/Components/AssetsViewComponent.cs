using Microsoft.AspNet.Mvc;
using KLoggy.Web.Infrastructure;
using Microsoft.Framework.OptionsModel;
using System;
using Microsoft.AspNet.Mvc.Rendering;
using System.Text;

namespace KLoggy.Web.Components
{
    public class AssetsViewComponent : ViewComponent
    {
        private readonly IOptions<AppOptions> _appOptionsAccessor;
        private readonly FrontEndAssetManager _assetManager;
        private readonly IUrlHelper _urlHelper;
        
        public AssetsViewComponent(FrontEndAssetManager assetManager, IUrlHelper urlHelper, IOptions<AppOptions> appOptionsAccessor)
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
        
        public HtmlString Invoke(string assetType)
        {
            HtmlString result;
            if (assetType.Equals("scripts", StringComparison.OrdinalIgnoreCase))
            {
                AssetInfo assetInfo = _assetManager.GetScripts();
                result = GetHtmlForJs(assetInfo, "~/assets/js/{0}");
            }
            else if(assetType.Equals("styles", StringComparison.OrdinalIgnoreCase))
            {
                AssetInfo assetInfo = _assetManager.GetStyles();
                result = GetHtmlForCss(assetInfo, "~/assets/css/{0}");
            }
            else 
            {
                throw new InvalidOperationException(string.Format("Asset type '{0}' is not recognized."));
            }

            return result;
        }

        private HtmlString GetHtmlForCss(AssetInfo assetInfo, string fileFormat)
        {
            StringBuilder builder = new StringBuilder();
            if (_appOptionsAccessor.Options.EnableBundlingAndMinification)
            {
                var filePath = _urlHelper.Content(
                    string.Format(
                        string.Concat(fileFormat, ".min-{1}.css"),
                        assetInfo.MinifiedFileName,
                        _appOptionsAccessor.Options.LatestCommitSha));

                TagBuilder tagBuilder = new TagBuilder("link");
                tagBuilder.Attributes.Add("rel", "stylesheet");
                tagBuilder.Attributes.Add("href", filePath);
                builder.AppendLine(tagBuilder.ToString());
            }
            else
            {
                foreach (var assetFileInfo in assetInfo.Files)
                {
                    string filePath = _urlHelper.Content(string.Format(fileFormat, assetFileInfo.FileName));
                    TagBuilder tagBuilder = new TagBuilder("link");
                    tagBuilder.Attributes.Add("rel", "stylesheet");
                    tagBuilder.Attributes.Add("href", filePath);
                    builder.AppendLine(tagBuilder.ToString());
                }
            }

            return new HtmlString(builder.ToString());
        }

        private HtmlString GetHtmlForJs(AssetInfo assetInfo, string fileFormat)
        {
            StringBuilder builder = new StringBuilder();
            if (_appOptionsAccessor.Options.EnableBundlingAndMinification)
            {
                var filePath = _urlHelper.Content(
                    string.Format(
                        string.Concat(fileFormat, ".min-{1}.js"),
                        assetInfo.MinifiedFileName,
                        _appOptionsAccessor.Options.LatestCommitSha));

                TagBuilder tagBuilder = new TagBuilder("script");
                tagBuilder.Attributes.Add("src", filePath);
                builder.AppendLine(tagBuilder.ToString());
            }
            else
            {
                foreach (var assetFileInfo in assetInfo.Files)
                {
                    string filePath = _urlHelper.Content(string.Format(fileFormat, assetFileInfo.FileName));
                    TagBuilder tagBuilder = new TagBuilder("script");
                    tagBuilder.Attributes.Add("src", filePath);
                    builder.AppendLine(tagBuilder.ToString());
                }
            }

            return new HtmlString(builder.ToString());
        }
    }
}