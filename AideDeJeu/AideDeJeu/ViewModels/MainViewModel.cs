using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private Dictionary<string, Item> _AllItems = new Dictionary<string, Item>();
        public async Task<Item> GetItemFromDataAsync(string source)
        {
            if (!_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                //return Tools.MarkdownExtensions.ToItem(md);
                if (md != null)
                {
                    _AllItems[source] = Tools.MarkdownExtensions.ToItem(md);
                }
                else
                {
                    return null;
                }
            }
            return _AllItems[source];
        }

        //public async Task<ItemsViewModel> GetItemsViewModelAsync(string source)
        //{
        //    var itemsViewModel = new ItemsViewModel();
        //    itemsViewModel.AllItems = await GetAllItemsAsync(source);
        //    return itemsViewModel;
        //}

        public Command LoadItemsCommand { get; private set; }
        public Command AboutCommand { get; private set; }

        public Navigator Navigator { get; set; }

        public MainViewModel()
        {
            AboutCommand = new Command(async () => await Main.Navigator.GotoAboutPageAsync());
        }

        public async Task NavigateToLink(string s)
        {
            if (s != null)
            {
                var regex = new Regex("/(?<file>.*)\\.md(#(?<anchor>.*))?");
                var match = regex.Match(s);
                var file = match.Groups["file"].Value;
                var anchor = match.Groups["anchor"].Value;
                var item = await GetItemFromDataAsync(file);
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
                                await Navigator.GotoItemDetailPageAsync(subitem);
                            }
                        }
                        else
                        {
                            var itemsViewModel = new ItemsViewModel() { AllItems = items };
                            itemsViewModel.LoadItemsCommand.Execute(null);
                            if (items.GetNewFilterViewModel() == null)
                            {
                                await Navigator.GotoItemsPageAsync(itemsViewModel);
                            }
                            else
                            {
                                await Navigator.GotoFilteredItemsPageAsync(itemsViewModel);
                            }
                        }
                    }
                    else
                    {
                        await Navigator.GotoItemDetailPageAsync(item);
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