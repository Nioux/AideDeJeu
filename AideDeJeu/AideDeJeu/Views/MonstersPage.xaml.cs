using AideDeJeu.Models;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonstersPage : MasterDetailPage //TabbedPage
    {
        MonstersViewModel viewModel;

        public MonstersPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new MonstersViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Monster;
            if (item == null)
                return;

            var spellvm = new MonsterDetailViewModel(item);
            spellvm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new MonsterDetailPage(spellvm));

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