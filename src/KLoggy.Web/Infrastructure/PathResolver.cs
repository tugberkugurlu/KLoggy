#if ASPNET50 || ASPNETCORE50 || NET45
using System.IO;

#if ASPNET50 || ASPNETCORE50
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;
#endif

// ref: https://github.com/aspnet/Configuration/blob/dev/src/Microsoft.Framework.ConfigurationModel.Shared/PathResolver.cs
namespace KLoggy.Web.Infrastructure
{
    internal static class PathResolver
    {
        private static string ApplicationBaseDirectory
        {
            get
            {
#if ASPNET50 || ASPNETCORE50
                var locator = CallContextServiceLocator.Locator;

                var appEnv = (IApplicationEnvironment)locator.ServiceProvider.GetService(typeof(IApplicationEnvironment));
                return appEnv.ApplicationBasePath;

#elif NET45
                return AppDomain.CurrentDomain.BaseDirectory;
#endif
            }
        }

        public static string ResolveAppRelativePath(string path)
        {
            return Path.Combine(ApplicationBaseDirectory, path);
        }
    }
}
#endif