using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AideDeJeu.Droid
{
    //[Activity(Label = "Aide de Jeu", Icon = "@drawable/black_book", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "Beta Haches & Dés", Icon = "@drawable/battle_axe", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Xamarin.Essentials.Platform.Init(this, bundle);
            Xamarin.Essentials.ExperimentalFeatures.Enable(Xamarin.Essentials.ExperimentalFeatures.ShareFileRequest);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            //global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            //global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //global::Xamarin.Forms.FormsMaterial.Init(this, bundle);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            DisplayCrashReport();

            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                //Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = System.IO.Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                System.IO.File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var errorFilePath = System.IO.Path.Combine(libraryPath, errorFilename);

            if (!System.IO.File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = System.IO.File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    System.IO.File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
                    // User pressed Close.
                })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        }

    }
}

