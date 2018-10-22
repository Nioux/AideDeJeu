using AideDeJeu.Tools;
using System;
using System.IO;
using Foundation;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.iOS.Version_iOS))]
namespace AideDeJeu.iOS
{
    public class Version_iOS : INativeAPI
    {
        public string GetVersion()
        {

            return NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleShortVersionString")].ToString();
        }

        public int GetBuild()
        {
            var buildVersion = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
            int build = 0;
            var res = int.TryParse(buildVersion, out build);
            return res ? build : 0;
        }

        public async Task<string> GetDatabasePathAsync(string databaseName)
        {
            return NSBundle.MainBundle.PathForResource(databaseName, "db");
        }
    }
}