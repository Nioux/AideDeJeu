using AideDeJeu.Tools;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Cmd.Version_CMD))]
namespace AideDeJeu.Cmd
{
    public class Version_CMD : INativeAPI
    {
        public string GetVersion()
        {
            //Package package = Package.Current;
            //PackageId packageId = package.Id;
            //PackageVersion version = packageId.Version;

            //return string.Format("{0}.{1}", version.Major, version.Minor);
            return "";
        }

        public int GetBuild()
        {
            return 0;
        }

        public async Task<string> GetDatabasePathAsync(string databaseName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), $@"..\..\..\..\..\Data\{databaseName}_{AideDeJeu.Config.Domain}.db");
        }

        public Task SaveStreamAsync(string filename, Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public Stream CreateStream(string filename)
        {
            throw new System.NotImplementedException();
        }

        public Task LaunchFileAsync(string title, string message, string filePath)
        {
            throw new System.NotImplementedException();
        }
    }
}