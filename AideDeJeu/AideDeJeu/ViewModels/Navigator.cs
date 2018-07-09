using AideDeJeu.Views;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public interface INavigator
    {
        Task GotoAboutPageAsync();
        Task GotoItemDetailPageAsync(Item item);
    }
    public class Navigator : INavigator
    {
        INavigation Navigation;

        public Navigator(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task GotoAboutPageAsync()
        {
            await Navigation.PushAsync(new Views.AboutPage());
        }

        public async Task GotoItemDetailPageAsync(Item item)
        {
            if (item == null)
                return;

            var vm = new ItemDetailViewModel(item);
            vm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new ItemDetailPage(vm));
        }

        public async Task GotoItemsPageAsync(Item item)
        {
            //if (item == null)
            //    return;

            //var vm = new ItemDetailViewModel(item);
            //vm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new ItemsPage());
        }

    }
}
