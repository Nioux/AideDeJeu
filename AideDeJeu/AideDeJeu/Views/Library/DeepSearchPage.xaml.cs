using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static AideDeJeu.ViewModels.Library.DeepSearchViewModel;

namespace AideDeJeu.Views.Library
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

        public DeepSearchPage(string search = null)
        {
            InitializeComponent();

            BindingContext = new DeepSearchViewModel();

            if (search != null)
            {
                (BindingContext as DeepSearchViewModel).ExecuteSearchCommandAsync(search);
            }
        }

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var searchedItem = e.Item as SearchedItem;
            await Main.Navigator.GotoItemDetailPageAsync(searchedItem.Item);
        }
    }
}