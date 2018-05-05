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

        public MainPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new MainViewModel(Navigation);
        }

        protected override bool OnBackButtonPressed()
        {
            IsPresented = !IsPresented;
            return true;
        }


        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem is Spell)
            {
                var item = args.SelectedItem as Spell;
                if (item == null)
                    return;

                var vm = new SpellDetailViewModel(item);
                vm.LoadItemCommand.Execute(null);
                await Navigation.PushAsync(new SpellDetailPage(vm));
            }
            else if (args.SelectedItem is Monster)
            {
                var item = args.SelectedItem as Monster;
                if (item == null)
                    return;

                var vm = new MonsterDetailViewModel(item);
                vm.LoadItemCommand.Execute(null);
                await Navigation.PushAsync(new MonsterDetailPage(vm));
            }

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}