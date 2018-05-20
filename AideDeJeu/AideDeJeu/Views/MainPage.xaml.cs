using AideDeJeu.ViewModels;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
    {
        MainViewModel viewModel;
        INavigator Navigator;

        public MainPage ()
		{
			InitializeComponent ();
            Navigator = new Navigator((Detail as NavigationPage).Navigation);
            BindingContext = viewModel = new MainViewModel(Navigator);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}