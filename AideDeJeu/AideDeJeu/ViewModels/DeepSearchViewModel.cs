using AideDeJeu.Views;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class DeepSearchViewModel : BaseViewModel
    {
        private Command<string> _SearchCommand = null;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new Command<string>(async (searchText) => await ExecuteSearchCommandAsync(searchText)));
            }
        }

        public async Task ExecuteSearchCommandAsync(string searchText)
        {
            Main.IsLoading = true;
            await Task.Run(async () => await Store.PreloadAllItemsAsync());
            Items = await Task.Run(async () => await DeepSearchAllItemsAsync(searchText));
            Main.IsLoading = false;
        }

        public IEnumerable<SearchedItem> _Items = null;
        public IEnumerable<SearchedItem> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                SetProperty(ref _Items, value);
            }
        }

        public class SearchedItem
        {
            public string Preview { get; set; }
            public Item Item { get; set; }
        }

        public async Task<IEnumerable<SearchedItem>> DeepSearchAllItemsAsync(string searchText)
        {
            List<SearchedItem> primaryItems = new List<SearchedItem>();
            List<SearchedItem> secondaryItems = new List<SearchedItem>();
            var cleanSearchText = Tools.Helpers.RemoveDiacritics(searchText).ToLower();
            foreach (var allItem in Store._AllItems)
            {
                foreach (var item in allItem.Value.Anchors)
                {
                    var name = item.Value.Name;
                    var cleanName = Tools.Helpers.RemoveDiacritics(name).ToLower();
                    if (cleanName.Contains(cleanSearchText))
                    {
                        primaryItems.Add(new SearchedItem() { Item = item.Value, Preview = name });
                    }
                    else
                    {
                        var markdown = item.Value.Markdown;
                        var cleanMarkdown = Tools.Helpers.RemoveDiacritics(markdown).ToLower();
                        if (cleanMarkdown.Contains(cleanSearchText))
                        {
                            int position = cleanMarkdown.IndexOf(cleanSearchText);
                            int startPosition = Math.Max(0, position - 30);
                            int endPosition = Math.Min(markdown.Length, position + searchText.Length + 30);
                            var preview = markdown.Substring(startPosition, endPosition - startPosition - 1);
                            secondaryItems.Add(new SearchedItem() { Item = item.Value, Preview = preview });
                        }
                    }
                }
            }
            primaryItems.AddRange(secondaryItems);
            return primaryItems;
        }


    }
}
