using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels.Library
{
    public class ItemViewModel : BaseViewModel
    {
        Item _Item = null;
        public Item Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);
            }
        }

        CancellationTokenSource cancellationTokenSource;

        public ItemViewModel(Item item = null)
        {
            Title = item?.Name;
            Item = item;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync().ConfigureAwait(false));
            SearchCommand = new Command<string>((text) =>
            {
                if (Filter != null)
                {
                    Filter.SearchText = text;
                }
            });
        }
        public Command<string> SearchCommand { get; private set; }
        public ICommand LoadItemsCommand { get; protected set; }

        private FilterViewModel _Filter;
        public FilterViewModel Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                SetProperty(ref _Filter, value);
                if (_Filter != null)
                {
                    _Filter.LoadItemsCommand = LoadItemsCommand;
                }
            }
        }

        public Item _Items = new Item();
        public Item Items
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

        public IEnumerable<Item> _Children = new List<Item>();
        public IEnumerable<Item> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                SetProperty(ref _Children, value);
            }
        }

        private Item _SelectedItem;
        public Item SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                SetProperty(ref _SelectedItem, value);
                if (_SelectedItem != null)
                {
                    if (_SelectedItem is LinkItem)
                    {
                        Main.Navigator.NavigateToLinkAsync("/" + (_SelectedItem as LinkItem).Link).ConfigureAwait(true);
                    }
                    else
                    {
                        Main.Navigator.GotoItemDetailPageAsync(_SelectedItem).ConfigureAwait(true);
                    }
                }
            }
        }

        private Item _AllItems;
        public Item AllItems
        {
            get
            {
                return _AllItems;
            }
            set
            {
                _AllItems = value;
                if (_AllItems != null)
                {
                    Title = _AllItems.Name;
                    Filter = _AllItems.GetNewFilterViewModel();
                }
            }
        }

        async Task LoadItemsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (Filter != null)
                {
                    var start = DateTime.Now;
                    var items = await Task.Run(async () => await Filter.GetFilteredItemsAsync(cancellationToken: cancellationToken));
                    var end = DateTime.Now;
                    Debug.WriteLine((end - start).TotalMilliseconds);
                    Items = new Item(items.ToList());
                    Children = items;
                }
                else
                {
                    Items = AllItems;
                    //Children = await AllItems.GetChildrenAsync();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task ExecuteLoadItemsCommandAsync()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            await LoadItemsAsync(cancellationTokenSource.Token);
        }


        public async Task LoadPageAsync(string path)
        {
            var regex = new Regex("/?(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
            var match = regex.Match(Uri.UnescapeDataString(path));
            var file = match.Groups["file"].Value;
            var anchor = match.Groups["anchor"].Value;
            var with = match.Groups["with"].Value;
            Item item = null;
            try
            {
                Main.IsBusy = true;
                Main.IsLoading = true;
                item = await Task.Run(async () => await Main.Store.GetItemFromDataAsync(file, anchor));

                if (item != null)
                {
                    var filterViewModel = item.GetNewFilterViewModel();
                    Item = item;
                    AllItems = item;
                    Filter = filterViewModel;
                    await ExecuteLoadItemsCommandAsync();
                    if (!string.IsNullOrEmpty(with))
                    {
                        var swith = with.Split('_');
                        for (int i = 0; i < swith.Length / 2; i++)
                        {
                            var key = swith[i * 2 + 0];
                            var val = swith[i * 2 + 1];
                            filterViewModel.FilterWith(key, val);
                        }
                    }
                }
                else
                {
                    //await App.Current.MainPage.DisplayAlert("Lien invalide", s, "OK");
                }
            }
            finally
            {
                Main.IsBusy = false;
                Main.IsLoading = false;
            }
        }

    }
}
