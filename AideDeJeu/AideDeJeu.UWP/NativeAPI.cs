using AideDeJeu.Tools;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Droid.Version_Android))]
namespace AideDeJeu.Droid
{
    public class Version_Android : INativeAPI
    {
        public string GetVersion()
        {
            return "";
        }

        public int GetBuild()
        {
            return 0;
        }

        public string GetDatabasePath(string databaseName)
        {
            return Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, databaseName);
        }
    }
}