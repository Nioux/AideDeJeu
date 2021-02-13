using AideDeJeu.Pdf;
using AideDeJeu.Repositories;
using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using AideDeJeu.ViewModels.PlayerCharacter;
using AideDeJeu.Views;
using AideDeJeuLib;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AideDeJeu
{
    public partial class App : Application
	{

		public App (string search = null)
		{
			InitializeComponent();
            /*
            App.Current.UserAppTheme = Application.Current.RequestedTheme;
            //App.Current.UserAppTheme = OSAppTheme.Dark;*/

            App.Current.UserAppTheme = BaseViewModel.OSAppTheme;
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                App.Current.UserAppTheme = Application.Current.RequestedTheme;
            };


            DependencyService.Register<MainViewModel>();
            DependencyService.Register<PdfService>();
            DependencyService.Register<BookmarksRepository>();
            DependencyService.Register<StoreViewModel>();
            DependencyService.Register<PlayerCharacterEditorViewModel>();
            var vm = DependencyService.Get<MainViewModel>();
            var titlered = (Color)Resources["HDRed"];
            var bgtan = (Color)Resources["HDWhite"];


            MainPage = new MainShell();
            //var mainNavigationPage = new MainNavigationPage();
            //vm.Navigator = new Navigator(mainNavigationPage.Navigation);
            vm.Navigator = new Navigator(Shell.Current.Navigation);

            Routing.RegisterRoute("item", typeof(Views.Library.ItemPage));
            //MainPage = mainNavigationPage;
            //mainNavigationPage.Navigation.PushAsync(new MainPage());
            if (search != null)
            {
                Shell.Current.Navigation.PushAsync(new Views.Library.DeepSearchPage(), true);
            }

        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
