using AideDeJeu.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.WPF.Version_WPF))]
namespace AideDeJeu.WPF
{
    public class Version_WPF : INativeAPI
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
            var assembly = typeof(Version_WPF).GetTypeInfo().Assembly;
            var path = AppDomain.CurrentDomain.BaseDirectory; // System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            return Path.Combine(path, $"{databaseName}.db");
            //try
            //{
            //    if (await CheckDatabaseVersionAsync(databaseName))
            //    {
            //        await CopyOldToNewFileAsync(databaseName, "db");
            //        await CopyOldToNewFileAsync(databaseName, "ver");
            //    }
            //}
            //catch
            //{

            //}
            //return GetNewFilePath(databaseName, "db");
        }

        public Stream GetOldFileStream(string fileName, string extension)
        {
            var assembly = typeof(Version_WPF).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream($"AideDeJeu.WPF.{fileName}.{extension}");
        }
        public string GetNewFilePath(string fileName, string extension)
        {
            var documentsDirectoryPath = "."; // Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;
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

        public async Task SaveStreamAsync(string filename, Stream stream)
        {
            using (var outStream = CreateStream(filename))
            {
                await stream.CopyToAsync(outStream);
            }
        }
        public Stream CreateStream(string filename)
        {
            var documentsDirectoryPath = "."; // Windows.Storage.ApplicationData.Current.LocalCacheFolder.Path;
            var filepath = Path.Combine(documentsDirectoryPath, filename);
            return new FileStream(filepath, FileMode.Create);
        }

        public async Task LaunchFileAsync(string title, string message, string filePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    //StorageFile attachment = await StorageFile.GetFileFromPathAsync(filePath);
                    //var success = await Windows.System.Launcher.LaunchFileAsync(attachment);
                }
            }
            finally
            {
            }
        }

    }
}