using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;
using KLoggy.Web;

namespace KLoggy.Web.Infrastructure
{
    /// <summary>
    /// Following are some of the scenarios exercised here:
    /// 1. Based on configuration, generate Content urls pointing to local or a CDN server
    /// 2. Based on configuration, generate lower case urls
    /// </summary>
    /// <remarks>
    /// ref: https://github.com/aspnet/Mvc/blob/dev/test/WebSites/UrlHelperWebSite/CustomUrlHelper.cs
    /// </remarks>
    public class CustomUrlHelper : UrlHelper
    {
        private readonly IOptions<AppOptions> _appOptions;
        private readonly HttpContext _httpContext;

        public CustomUrlHelper(IContextAccessor<ActionContext> contextAccessor, 
                               IActionSelector actionSelector,
                               IOptions<AppOptions> appOptions) : base(contextAccessor, actionSelector)
        {
            _appOptions = appOptions;
            _httpContext = contextAccessor.Value.HttpContext;
        }

        /// <summary>
        /// Depending on config data, generates an absolute url pointing to a CDN server
        /// or falls back to the default behavior
        /// </summary>
        /// <param name="contentPath"></param>
        /// <returns></returns>
        public override string Content(string contentPath)
        {
            if (_appOptions.Options.ServeCdnContent
                && contentPath.StartsWith("~/", StringComparison.Ordinal))
            {
                var segment = new PathString(contentPath.Substring(1));

                return ConvertToLowercaseUrl(_appOptions.Options.CdnServerBaseUrl + segment);
            }

            return ConvertToLowercaseUrl(base.Content(contentPath));
        }

        public override string RouteUrl(string routeName, object values, string protocol, string host, string fragment)
        {
            return ConvertToLowercaseUrl(base.RouteUrl(routeName, values, protocol, host, fragment));
        }

        public override string Action(string action, string controller, object values, string protocol, string host, string fragment)
        {
            return ConvertToLowercaseUrl(base.Action(action, controller, values, protocol, host, fragment));
        }

        private string ConvertToLowercaseUrl(string url)
        {
            if (!string.IsNullOrEmpty(url)
                && _appOptions.Options.GenerateLowercaseUrls)
            {
                return url.ToLowerInvariant();
            }

            return url;
        }
    }
}