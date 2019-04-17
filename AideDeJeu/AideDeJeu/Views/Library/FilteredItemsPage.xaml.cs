using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilteredItemsPage : MasterDetailPage
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
        public FilteredItemsPage (ItemsViewModel itemsViewModel)
		{
			InitializeComponent();

            BindingContext = _ItemsViewModel = itemsViewModel;
        }
        public FilteredItemsPage()
        {
            InitializeComponent();

            BindingContext = Main;
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.IsPresented = !this.IsPresented;
        }
    }
}