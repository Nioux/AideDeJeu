using AideDeJeu.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
    {
        MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        public ItemsViewModel _ItemsViewModel;
        public ItemsViewModel ItemsViewModel
        {
            get
            {
                return _ItemsViewModel;
            }
        }
        public ItemsPage (ItemsViewModel itemsViewModel)
		{
			InitializeComponent ();

            BindingContext = _ItemsViewModel = itemsViewModel;
        }
        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = Main;
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}