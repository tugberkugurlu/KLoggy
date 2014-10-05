using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace KLoggy.Web.Infrastructure
{
    public class FrontEndAssetManager
    {
        private const string ScriptsFilePath = "client/js/files.json";
        private const string StylesFilePath = "client/less/files.json";
        
        private static readonly string _scriptsAppRelativePath;
        private static readonly string _stylesAppRelativePath;
        private static readonly JsonSerializerSettings _serializerSettings;
        
        static FrontEndAssetManager()
        {
            _scriptsAppRelativePath = PathResolver.ResolveAppRelativePath(ScriptsFilePath);
            _stylesAppRelativePath = PathResolver.ResolveAppRelativePath(StylesFilePath);
            _serializerSettings = new JsonSerializerSettings 
            { 
                ContractResolver = new CamelCasePropertyNamesContractResolver() 
            };
        }
        
        public AssetInfo GetScripts()
        {
            return GetAssetInfo(_scriptsAppRelativePath);
        }
        
        public AssetInfo GetStyles()
        {
            return GetAssetInfo(_stylesAppRelativePath);
        }
        
        private static AssetInfo GetAssetInfo(string filePath)
        {
            using (Stream inputStream = new FileStream(filePath, FileMode.Open))
            using (StreamReader reader = new StreamReader(inputStream))
            {
                string jsonValue = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<AssetInfo>(jsonValue, _serializerSettings);
            }
        }
    }
}