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
	public partial class DeepSearchPage : ContentPage
	{
        MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        public DeepSearchPage ()
		{
			InitializeComponent ();

            BindingContext = new DeepSearchViewModel();
		}

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var searchedItem = e.Item as MainViewModel.SearchedItem;
            await Main.Navigator.GotoItemDetailPageAsync(searchedItem.Item);
        }
    }
}