using System.Collections.Generic;

namespace KLoggy.Web.Infrastructure
{
    public class AssetInfo
    {
        public string AssetsDir { get; set; }
        public string MinifiedFileName { get; set; }
        public IEnumerable<AssetFileInfo> Files { get; set; }
    }   
}