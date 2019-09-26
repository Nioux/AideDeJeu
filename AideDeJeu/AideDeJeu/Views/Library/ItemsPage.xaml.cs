using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Library
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
    {
        public MainViewModel Main
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

            //mdMarkdown.NavigateToLink = async (s) => await itemsViewModel.Main.Navigator.NavigateToLinkAsync(s);
        }

        public ItemsPage(string id)
        {
            InitializeComponent();

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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Main.Speech.ExecuteCancelSpeakCommand();
        }
    }
}