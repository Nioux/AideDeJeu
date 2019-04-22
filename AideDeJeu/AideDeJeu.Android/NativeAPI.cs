using AideDeJeu.Tools;
using Android.Content;
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
            try
            {
                if (await CheckDatabaseVersionAsync(databaseName))
                {
                    await CopyOldToNewFileAsync(databaseName, "db");
                    await CopyOldToNewFileAsync(databaseName, "ver");
                }
            }
            catch
            {

            }
            return GetNewFilePath(databaseName, "db");
        }

        public Stream GetOldFileStream(string fileName, string extension)
        {
            return Android.App.Application.Context.Assets.Open($"{fileName}.{extension}");
        }
        public string GetNewFilePath(string fileName, string extension)
        {
            //var documentsDirectoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var documentsDirectoryPath = Android.App.Application.Context.CacheDir.AbsolutePath;
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
            var documentsDirectoryPath = Android.App.Application.Context.CacheDir.AbsolutePath;
            var filepath = Path.Combine(documentsDirectoryPath, filename);
            return new FileStream(filepath, FileMode.Create);
        }


        //public void OpenFileByName(string fileName)
        //{
        //    //try
        //    //{
        //    var documentsPath = Android.App.Application.Context.CacheDir.AbsolutePath; ;// System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //        var filePath = Path.Combine(documentsPath, fileName);

        //        var bytes = File.ReadAllBytes(filePath);

        //        //Copy the private file's data to the EXTERNAL PUBLIC location
        //        string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
        //        var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads + "/" + fileName;
        //        File.WriteAllBytes(externalPath, bytes);

        //        Java.IO.File file = new Java.IO.File(externalPath);
        //        file.SetReadable(true);

        //        string application = "";
        //        string extension = Path.GetExtension(filePath);

        //        // get mimeTye
        //        switch (extension.ToLower())
        //        {
        //            case ".txt":
        //                application = "text/plain";
        //                break;
        //            case ".doc":
        //            case ".docx":
        //                application = "application/msword";
        //                break;
        //            case ".pdf":
        //                application = "application/pdf";
        //                break;
        //            case ".xls":
        //            case ".xlsx":
        //                application = "application/vnd.ms-excel";
        //                break;
        //            case ".jpg":
        //            case ".jpeg":
        //            case ".png":
        //                application = "image/jpeg";
        //                break;
        //            default:
        //                application = "*/*";
        //                break;
        //        }

        //        //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
        //        Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
        //        Intent intent = new Intent(Intent.ActionView);
        //        intent.SetDataAndType(uri, application);
        //        intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

        //        Xamarin.Forms.Forms.Context.StartActivity(intent);


        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}
        //}

        //static Task PlatformRequestAsync(Xamarin.Essentials.ShareFileRequest request)
        //{
        //    var contentUri = Platform.GetShareableFileUri(request.File.FullPath);

        //    var intent = new Intent(Intent.ActionSend);
        //    intent.SetType(request.File.ContentType);
        //    intent.SetFlags(ActivityFlags.GrantReadUriPermission);
        //    intent.PutExtra(Intent.ExtraStream, contentUri);

        //    if (!string.IsNullOrEmpty(request.Title))
        //    {
        //        intent.PutExtra(Intent.ExtraTitle, request.Title);
        //    }

        //    var chooserIntent = Intent.CreateChooser(intent, request.Title ?? string.Empty);
        //    chooserIntent.SetFlags(ActivityFlags.ClearTop);
        //    chooserIntent.SetFlags(ActivityFlags.NewTask);
        //    Platform.AppContext.StartActivity(chooserIntent);

        //    return Task.CompletedTask;
        //}

    }
}
