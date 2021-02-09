using AideDeJeu.Views;
using AideDeJeuLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels.Library
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
            try
            {
                Main.IsLoading = true;
                //await Task.Run(async () => await Store.PreloadAllItemsAsync());
                Items = await Task.Run(async () => await DeepSearchAllItemsAsync(searchText));
            }
            finally
            {
                Main.IsLoading = false;
            }
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

        public static string GetPreview(string markdown, string searchText)
        {
            int position = markdown.IndexOf(searchText);
            int startPosition = Math.Max(0, position - 30);
            int endPosition = Math.Min(markdown.Length, position + searchText.Length + 30);
            return $"\"{markdown.Substring(startPosition, endPosition - startPosition - 1).Replace("\n","")}\"";
        }

        public async Task<IEnumerable<SearchedItem>> DeepSearchAllItemsAsync(string searchText)
        {
            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var primary = await context.Items.
                        Where(item => EF.Functions.Like(item.Name, $"%{searchText}%")).
                        Select(item => new SearchedItem() { Item = item, Preview = item.ParentName != null ? $"> {item.ParentName}" : "" }).
                        ToListAsync();
                    var secondary = await context.Items.
                        Where(item => EF.Functions.Like(item.Markdown, $"%{searchText}%")).
                        Select(item => new SearchedItem()
                        {
                            Item = item,
                            Preview = (item.ParentName != null ? $"> {item.ParentName} > " : "") + GetPreview(item.Markdown, searchText)
                        }).ToListAsync();
                    //var primary = await context.Items.
                    //    Where(item => item.Name.Contains(searchText)).
                    //    Select(item => new SearchedItem() { Item = item, Preview = item.Name }).
                    //    ToListAsync();
                    //var secondary = await context.Items.
                    //    Where(item => item.Markdown.Contains(searchText)).
                    //    Select(item => new SearchedItem()
                    //    {
                    //        Item = item,
                    //        Preview = GetPreview(item.Markdown, searchText)
                    //    }).ToListAsync();
                    primary.AddRange(secondary);
                    return primary.ToList();
                }



                //List<SearchedItem> primaryItems = new List<SearchedItem>();
                //List<SearchedItem> secondaryItems = new List<SearchedItem>();
                //var cleanSearchText = Tools.Helpers.RemoveDiacritics(searchText ?? string.Empty).ToLower();
                //foreach (var item in Store._AllItems)
                //{
                //    var name = item.Value.Name;
                //    var cleanName = Tools.Helpers.RemoveDiacritics(name).ToLower();
                //    if (cleanName.Contains(cleanSearchText))
                //    {
                //        primaryItems.Add(new SearchedItem() { Item = item.Value, Preview = name });
                //    }
                //    else
                //    {
                //        var markdown = item.Value.Markdown;
                //        var cleanMarkdown = Tools.Helpers.RemoveDiacritics(markdown).ToLower();
                //        if (cleanMarkdown.Contains(cleanSearchText))
                //        {
                //            int position = cleanMarkdown.IndexOf(cleanSearchText);
                //            int startPosition = Math.Max(0, position - 30);
                //            int endPosition = Math.Min(markdown.Length, position + searchText.Length + 30);
                //            var preview = markdown.Substring(startPosition, endPosition - startPosition - 1);
                //            secondaryItems.Add(new SearchedItem() { Item = item.Value, Preview = preview });
                //        }
                //    }
                //}
                //primaryItems.AddRange(secondaryItems);
                //return primaryItems;
            }
            catch
            {
                return new List<SearchedItem>();
            }
            finally
            {
                StoreViewModel.SemaphoreLibrary.Release();
            }
        }


    }
}
