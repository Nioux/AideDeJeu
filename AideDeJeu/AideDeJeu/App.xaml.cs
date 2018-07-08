using System;
using AideDeJeu.ViewModels;
using AideDeJeu.Views;
using AideDeJeuLib.Models;
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
            var vm = DependencyService.Get<MainViewModel>();
            var mainPage = new ItemDetailPage(new ItemDetailViewModel(new HomeItem()));// new MainPage();
            var navigationPage = new NavigationPage(mainPage);
            vm.Navigator = new Navigator(navigationPage.Navigation); //mainPage.Detail.Navigation);
            MainPage = navigationPage;
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
