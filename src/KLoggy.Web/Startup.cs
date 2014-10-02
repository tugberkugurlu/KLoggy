using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace KLoggy.Web 
{
    public class Startup 
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseServices(services =>
            {
                Configuration configuration = new Configuration();
                configuration.AddIniFile("git.ini");
                foreach(var source in configuration)
                {
                    Console.WriteLine("==={0}===", ((BaseConfigurationSource)source).GetType());
                    foreach(var kvp in ((BaseConfigurationSource)source).Data)
                    {
                        Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    }
                }
            });
            
            Console.WriteLine(string.Concat("Hello from '", typeof(Startup).FullName, "'"));
        }
    }
}