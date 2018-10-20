using AideDeJeu.Tools;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.UWP.Version_UWP))]
namespace AideDeJeu.UWP
{
    public class Version_UWP : INativeAPI
    {
        public string GetVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}", version.Major, version.Minor);
        }

        public int GetBuild()
        {
            return 0;
        }

        public async Task<string> GetDatabasePathAsync(string databaseName)
        {
            var documentsDirectoryPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var path = Path.Combine(documentsDirectoryPath, databaseName);

            if (!File.Exists(path))
            {
                var assembly = typeof(Version_UWP).GetTypeInfo().Assembly;
                using (var inStream = assembly.GetManifestResourceStream("AideDeJeu.UWP." + databaseName))
                {
                    using (var outStream = new FileStream(path, FileMode.Create))
                    {
                        inStream.CopyTo(outStream);
                    }
                }
            }
            return path;
        }
    }
}