using AideDeJeu.Tools;
using System;
using System.IO;
using Foundation;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Droid.Version_Android))]
namespace AideDeJeu.Droid
{
    public class Version_Android : INativeAPI
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
            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName);
            //return databasePath;
        }
    }
}