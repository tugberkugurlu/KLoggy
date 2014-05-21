using Microsoft.AspNet.Builder;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System;

namespace KLoggy.Web 
{
    public class Startup 
    {
        public void Configure(IBuilder app)
        {
            app.UseServices(services =>
            {
            });
            
            Console.WriteLine(string.Concat("Hello from '", typeof(Startup).FullName, "'"));
        }
    }
}