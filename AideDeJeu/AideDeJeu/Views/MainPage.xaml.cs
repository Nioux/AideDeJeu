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

        void OnSpellsClicked()
        {
            Navigation.PushAsync(new SpellsPage());
        }

        void OnMonstersClicked()
        {
            Navigation.PushAsync(new MonstersPage());
        }

        void OnAboutClicked()
        {
            Navigation.PushAsync(new AboutPage());
        }
    }
}