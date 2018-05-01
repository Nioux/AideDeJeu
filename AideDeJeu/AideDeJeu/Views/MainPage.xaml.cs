using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
    {
        public MainPage ()
		{
			InitializeComponent ();
        }

        async void OnSpellsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SpellsPage());
        }

        async void OnMonstersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonstersPage());
        }

        async void OnAboutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}