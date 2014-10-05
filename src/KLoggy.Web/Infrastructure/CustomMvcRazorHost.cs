using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.Runtime;
using System.Collections.Generic;
using Microsoft.AspNet.Razor.Generator.Compiler;
using System.Collections.ObjectModel;
using System.Linq;

namespace KLoggy.Web.Infrastructure
{
    public class CustomMvcRazorHost : MvcRazorHost
    {
#if ASPNET50 || ASPNETCORE50
        public CustomMvcRazorHost(IApplicationEnvironment appEnvironment)
            : base(appEnvironment)
        {
        }
#elif NET45
        public MvcRazorHost(string root) : base(root)
        {
        }
#endif
    
        public override IReadOnlyList<Chunk> DefaultInheritedChunks
        {
            get 
            {
                var defaultInheritedChunks = base.DefaultInheritedChunks.ToList();
                defaultInheritedChunks.Add(new InjectChunk("KLoggy.Web.Infrastructure.IAssetsHelper", "Assets"));
                return new ReadOnlyCollection<Chunk>(defaultInheritedChunks);
            }
        }
    }
}