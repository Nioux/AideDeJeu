using AideDeJeu.ViewModels;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BookmarksPage : ContentPage
	{
		public BookmarksPage ()
		{
			InitializeComponent ();

            BindingContext = DependencyService.Get<BookmarksViewModel>();
		}

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as LinkItem;
            var Main = DependencyService.Get<MainViewModel>();
            await Main.Navigator.NavigateToLinkAsync(item.Link);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookmarksEditPage());
        }
    }
}