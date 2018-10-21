using AideDeJeu.Tools;
using Android.Content.PM;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Droid.Version_Android))]
namespace AideDeJeu.Droid
{
    public class Version_Android : INativeAPI
    {
        public string GetVersion()
        {
            var context = global::Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public int GetBuild()
        {
            var context = global::Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode;
        }

        public async Task<string> GetDatabasePathAsync(string databaseName)
        {
            if (await CheckDatabaseVersionAsync(databaseName))
            {
                await CopyOldToNewFileAsync(databaseName, "db");
                await CopyOldToNewFileAsync(databaseName, "ver");
            }
            return GetNewFilePath(databaseName, "db");
        }

        public Stream GetOldFileStream(string fileName, string extension)
        {
            return Android.App.Application.Context.Assets.Open($"{fileName}.{extension}");
        }
        public string GetNewFilePath(string fileName, string extension)
        {
            var documentsDirectoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(documentsDirectoryPath, $"{fileName}.{extension}");
        }
        public async Task CopyOldToNewFileAsync(string fileName, string extension)
        {
            using (var inStream = GetOldFileStream(fileName, extension))
            {
                using (var outStream = new FileStream(GetNewFilePath(fileName, extension), FileMode.Create))
                {
                    await inStream.CopyToAsync(outStream);
                }
            }
        }

        public async Task<bool> CheckDatabaseVersionAsync(string databaseName)
        {
            var path = GetNewFilePath(databaseName, "ver");
            if (!File.Exists(path))
            {
                return true;
            }
            int newVersion = 0;
            int oldVersion = -1;
            using (var newStream = GetOldFileStream(databaseName, "ver"))
            {
                using (var sr = new StreamReader(newStream))
                {
                    var str = await sr.ReadToEndAsync();
                    int.TryParse(str, out newVersion);
                }
            }
            using (var oldStream = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(oldStream))
                {
                    var str = await sr.ReadToEndAsync();
                    int.TryParse(str, out oldVersion);
                }
            }
            return newVersion > oldVersion;
        }
    }
}