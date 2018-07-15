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

        private Dictionary<string, Items> _AllItems = new Dictionary<string, Items>();
        public async Task<Items> GetAllItemsAsync(string source)
        {
            if (!_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                _AllItems[source] = Tools.MarkdownExtensions.ToItem(md) as Items;
            }
            return _AllItems[source];
        }

        public async Task<ItemsViewModel> GetItemsViewModelAsync(string source)
        {
            var itemsViewModel = new ItemsViewModel();
            itemsViewModel.AllItems = await GetAllItemsAsync(source);
            return itemsViewModel;
        }

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
                if (s.Contains("#"))
                {
                    var regex = new Regex("/(?<file>.*)\\.md#(?<anchor>.*)");
                    var match = regex.Match(s);
                    var file = match.Groups["file"].Value;
                    var anchor = match.Groups["anchor"].Value;
                    var items = await GetAllItemsAsync(file);
                    var item = items.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (item != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(item);
                    }
                }
                else
                {
                    var regex = new Regex("/(?<file>.*)\\.md");
                    var match = regex.Match(s);
                    var file = match.Groups["file"].Value;
                    var items = await GetItemsViewModelAsync(file);
                    items.LoadItemsCommand.Execute(null);
                    await Navigator.GotoItemsPageAsync(items);
                }
            }
        }
    }
}