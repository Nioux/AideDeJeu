using AideDeJeu.Models;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage //TabbedPage
    {
        SpellsViewModel viewModel;

        public MainPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new SpellsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Spell;
            if (item == null)
                return;

            await Navigation.PushAsync(new SpellDetailPage(new SpellDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}