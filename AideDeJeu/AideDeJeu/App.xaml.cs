using AideDeJeu.ViewModels;
using AideDeJeu.Views;
using AideDeJeuLib;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AideDeJeu
{
    public partial class App : Application
	{

		public App ()
		{
			InitializeComponent();

            DependencyService.Register<MainViewModel>();
            DependencyService.Register<BookmarksViewModel>();
            DependencyService.Register<StoreViewModel>();
            var vm = DependencyService.Get<MainViewModel>();
            var titlered = (Color)Resources["HDRed"];
            var bgtan = (Color)Resources["HDWhite"];


            var mainNavigationPage = new MainNavigationPage();
            
            //MainPage = new PlayerCharacterEditorPage();
            //MainPage = new MainPage();

            //var tabbeddPage = new AideDeJeu.Views.MainTabbedPage();



            //var navigationPage = tabbeddPage.MainNavigationPage;
            vm.Navigator = new Navigator(mainNavigationPage.Navigation);
            MainPage = mainNavigationPage;
            mainNavigationPage.Navigation.PushAsync(new MainPage());

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
