using AideDeJeu.Tools;
using Android.Content.PM;
using System;
using System.IO;

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

        public string GetDatabasePath(string databaseName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
        }
    }
}