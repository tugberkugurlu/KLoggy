using Microsoft.AspNet.Builder;
using System;

namespace KLoggy.Web 
{
    public class Startup 
    {
        public void Configure(IBuilder builder)
        {
            Console.WriteLine(string.Concat("Hello from '", typeof(Startup).FullName, "'"));
        }
    }
}