using AideDeJeu.ViewModels;
using AideDeJeu.Views;
using AideDeJeuLib;
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
            var vm = DependencyService.Get<MainViewModel>();
            var tabbeddPage = new AideDeJeu.Views.MainTabbedPage();
            //var mainPage = new ItemDetailPage(new ItemDetailViewModel(new HomeItem()) { Title = "Haches & Dés" });
            var titlered = (Color)Resources["titlered"];
            var bgtan = (Color)Resources["bgtan"];
            //var navigationPage = new MainNavigationPage(mainPage) { BarBackgroundColor = titlered, BarTextColor = bgtan };
            //tabbeddPage.MainNavigationPage = navigationPage;
            var navigationPage = tabbeddPage.MainNavigationPage;
            vm.Navigator = new Navigator(navigationPage.Navigation);
            MainPage = tabbeddPage;
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
