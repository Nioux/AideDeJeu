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
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //string dbPath = Path.Combine(path, databaseName);
            //return dbPath;
            var documentsDirectoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsDirectoryPath, databaseName);

            // This is where we copy in our pre-created database
            if (!File.Exists(path))
            {
                using (var inStream = Android.App.Application.Context.Assets.Open(databaseName))
                {
                    using (var outStream = new FileStream(path, FileMode.Create))
                    {
                        inStream.CopyTo(outStream);
                    }
                }
                //using (var binaryReader = new BinaryReader(Android.App.Application.Context.Assets.Open(databaseName)))
                //{
                //    using (var binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
                //    {
                //        byte[] buffer = new byte[2048];
                //        int length = 0;
                //        while ((length = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                //        {
                //            binaryWriter.Write(buffer, 0, length);
                //        }
                //    }
                //}
            }
            return path;
        }
    }
}