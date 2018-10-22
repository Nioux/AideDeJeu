using AideDeJeu.Tools;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.GTK.Version_GTK))]
namespace AideDeJeu.GTK
{
    public class Version_GTK : INativeAPI
    {
        public string GetVersion()
        {
            return "";
            //var context = global::Android.App.Application.Context;

            //PackageManager manager = context.PackageManager;
            //PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            //return info.VersionName;
        }

        public int GetBuild()
        {
            return 0;
            //var context = global::Android.App.Application.Context;
            //PackageManager manager = context.PackageManager;
            //PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            //return info.VersionCode;
        }

        public async Task<string> GetDatabasePathAsync(string databaseName)
        {
            var assembly = typeof(Version_GTK).GetTypeInfo().Assembly;
            var path = AppDomain.CurrentDomain.BaseDirectory; // System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            return Path.Combine(path, $"{databaseName}.db");
        }
    }
}