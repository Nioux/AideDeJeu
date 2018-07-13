﻿using AideDeJeu.Views;
using AideDeJeuLib;
using System.Collections.Generic;
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
            await Navigation.PushAsync(new ItemDetailPage(vm));
        }

        public async Task GotoItemsPageAsync(ItemsViewModel itemsVM)
        {
            if (itemsVM == null)
                return;

            await Navigation.PushAsync(new ItemsPage(itemsVM));
        }

    }
}
