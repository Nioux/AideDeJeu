using AideDeJeu.Models;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using AideDeJeuLib.Spells;
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

            var spellvm = new SpellDetailViewModel(item);
            spellvm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new SpellDetailPage(spellvm));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}