using Microsoft.AspNet.Mvc.Rendering;

namespace KLoggy.Web.Infrastructure
{
    public interface IAssetsHelper
    {
        HtmlString GetThing();
    }
    
    public class DefaultAssetsHelper : IAssetsHelper
    {
        public HtmlString GetThing()
        {
            return new HtmlString("<strong>Foo</strong>");
        }
    }
}