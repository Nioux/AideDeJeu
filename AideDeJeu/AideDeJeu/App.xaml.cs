using System;
using AideDeJeu.ViewModels;
using AideDeJeu.Views;
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
            var mainPage = new MainPage();
            vm.Navigator = new Navigator(mainPage.Detail.Navigation);
            MainPage = mainPage;
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
