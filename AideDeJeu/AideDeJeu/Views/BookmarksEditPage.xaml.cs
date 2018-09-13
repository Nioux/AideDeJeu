using AideDeJeu.ViewModels;
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
	public partial class BookmarksEditPage : ContentPage
	{
		public BookmarksEditPage ()
		{
			InitializeComponent ();

            BindingContext = DependencyService.Get<BookmarksViewModel>();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}