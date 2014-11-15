namespace KLoggy.Web.Infrastructure
{
    public class AssetFileInfo 
    {
        // TODO: Have this with the custom json serializer
        // public AssetFileInfo(string baseDir, string fileName)
        // {
        //     if (baseDir == null)
        //     {
        //         throw new ArgumentNullException("baseDir");
        //     }
        //     
        //     if (fileName == null)
        //     {
        //         throw new ArgumentNullException("fileName");
        //     }
        //     
        //     BaseDir = baseDir;
        //     FileName = fileName;
        // }
        
        public string BaseDir { get; set; }
        public string FileName { get; set; }
    }
}