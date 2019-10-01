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
    [IntentFilter(new[] { Android.Content.Intent.ActionAssist }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    //[Activity(Label = "Aide de Jeu", Icon = "@drawable/black_book", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Name = "com.nioux.aidedejeu.MainActivity", Label = "Beta Haches & Dés", Icon = "@drawable/battle_axe", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // Application Level Assistance
            //Application.RegisterOnProvideAssistDataListener(new AndroidListener());

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            Xamarin.Essentials.Platform.Init(this, bundle);
            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.ExperimentalFeatures.Enable(Xamarin.Essentials.ExperimentalFeatures.ShareFileRequest);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            //global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            //global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //global::Xamarin.Forms.FormsMaterial.Init(this, bundle);
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            DisplayCrashReport();

            //// Verify the action and get the query
            //if (Android.Content.Intent.ActionSearch == Intent.Action)
            //{
            //    var query = Intent.GetStringExtra(SearchManager.Query);
            //}



            LoadApplication(new App());
        }

        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
        }

        protected override void OnPostResume()
        {
            base.OnPostResume();
            if (Intent.Extras != null)
            {
                string search = Intent.Extras.GetString("search");
                if (!string.IsNullOrEmpty(search))
                {
                    Xamarin.Forms.Shell.Current.Navigation.PushAsync(new Views.Library.DeepSearchPage(search), true);
                }
                Intent.RemoveExtra("search");
            }
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

        public override void OnProvideAssistContent(Android.App.Assist.AssistContent outContent)
        {
            base.OnProvideAssistContent(outContent);

            outContent.WebUri = Android.Net.Uri.Parse("https://heros-et-dragons.fr/");
            // Provide some JSON 
            string structuredJson = new Org.Json.JSONObject()
                .Put("@type", "Book")
                .Put("author", "Lewis Carroll")
                .Put("name", "Alice in Wonderland")
                .Put("description",
                    "This is an 1865 novel about a girl named Alice, " +
                    "who falls through a rabbit hole and " +
                    "enters a fantasy world."
                ).ToString();
                //.Put("@type", "MusicRecording")
                //.Put("@id", "https://example.com/music/recording")
                //.Put("name", "Album Title")
                //.ToString();

            outContent.StructuredData = structuredJson;
        }

        public override void OnProvideAssistData(Bundle data)
        {
            base.OnProvideAssistData(data);
        }

    }

    [IntentFilter(new[] { Android.Content.Intent.ActionAssist }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    //[Activity(Label = "Aide de Jeu", Icon = "@drawable/black_book", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Name = "com.nioux.aidedejeu.SearchActivity")]
    public class SearchActivity : Android.App.Activity // global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Application Level Assistance
            //Application.RegisterOnProvideAssistDataListener(new AndroidListener());

            // Android.Content.Intent.ActionSearch.Equals
            // Verify the action and get the query
            if ("com.google.android.gms.actions.SEARCH_ACTION".Equals(Intent.Action))
            {
                var query = Intent.GetStringExtra(SearchManager.Query);
                System.Diagnostics.Debug.WriteLine(query);

                var intent = new Android.Content.Intent(Android.App.Application.Context, typeof(MainActivity));
                intent.AddFlags(Android.Content.ActivityFlags.NewTask);
                intent.PutExtra("search", query);
                intent.AddFlags(Android.Content.ActivityFlags.ReorderToFront);
                Android.App.Application.Context.StartActivity(intent);
            }
            this.FinishActivity(0);
        }
    }
    //public class AndroidListener : Java.Lang.Object, Application.IOnProvideAssistDataListener
    //{
    //    public void OnProvideAssistData(Android.App.Activity activity, Bundle data) 
    //    {
    //        System.Diagnostics.Debug.WriteLine("OnProvideAssistData");
    //    }
    //}
}

