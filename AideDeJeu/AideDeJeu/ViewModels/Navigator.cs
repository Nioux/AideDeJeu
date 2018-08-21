using AideDeJeu.Views;
using AideDeJeuLib;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public interface INavigator
    {
        Task GotoAboutPageAsync();
        Task GotoItemDetailPageAsync(Item item);
    }
    public class Navigator : BaseViewModel, INavigator
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

        public async Task GotoFilteredItemsPageAsync(ItemsViewModel itemsVM)
        {
            if (itemsVM == null)
                return;

            await Navigation.PushAsync(new FilteredItemsPage(itemsVM));
        }

        private ICommand _NavigateToLinkCommand = null;
        public ICommand NavigateToLinkCommand
        {
            get
            {
                return _NavigateToLinkCommand ?? (_NavigateToLinkCommand = new Command<string>(async(s) => await NavigateToLinkAsync(s)));
            }
        }

        public async Task NavigateToLinkAsync(string s)
        {
            if (s != null)
            {
                var regex = new Regex("/(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
                var match = regex.Match(s);
                var file = match.Groups["file"].Value;
                var anchor = match.Groups["anchor"].Value;
                var with = match.Groups["with"].Value;
                Main.IsBusy = true;
                Main.IsLoading = true;
                var item = await Main.GetItemFromDataAsync(file);
                Main.IsBusy = false;
                Main.IsLoading = false;
                if (item != null)
                {
                    if (item is Items)
                    {
                        var items = item as Items;
                        if (!string.IsNullOrEmpty(anchor))
                        {
                            var subitem = items.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                            if (subitem != null)
                            {
                                await GotoItemDetailPageAsync(subitem);
                            }
                        }
                        else
                        {
                            var filterViewModel = items.GetNewFilterViewModel();
                            var itemsViewModel = new ItemsViewModel() { AllItems = items, Filter = filterViewModel };
                            itemsViewModel.LoadItemsCommand.Execute(null);
                            if(!string.IsNullOrEmpty(with))
                            {
                                var swith = with.Split('_');
                                for (int i = 0; i < swith.Length / 2; i++)
                                {
                                    var key = swith[i * 2 + 0];
                                    var val = swith[i * 2 + 1];
                                    filterViewModel.FilterWith(key, val);
                                }
                            }
                            if (filterViewModel == null)
                            {
                                await GotoItemsPageAsync(itemsViewModel);
                            }
                            else
                            {
                                await GotoFilteredItemsPageAsync(itemsViewModel);
                            }
                        }
                    }
                    else
                    {
                        await GotoItemDetailPageAsync(item);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Lien invalide", s, "OK");
                }
            }
        }

    }
}
