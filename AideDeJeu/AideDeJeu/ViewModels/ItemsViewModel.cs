using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        CancellationTokenSource cancellationTokenSource;

        public ItemsViewModel()
        {
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
                if(_Filter != null)
                {
                    _Filter.LoadItemsCommand = LoadItemsCommand;
                }
            }
        }

        public IEnumerable<Item> _Items = new List<Item>();
        public IEnumerable<Item> Items
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
                        Main.Navigator.NavigateToLinkAsync("/" + (_SelectedItem as LinkItem).Link);
                    }
                    else
                    { 
                        Main.Navigator.GotoItemDetailPageAsync(_SelectedItem);
                    }
                }
            }
        }

        private Items _AllItems;
        public Items AllItems
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
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                if (Filter != null)
                {
                    var items = await Filter.FilterItems(AllItems, cancellationToken: cancellationToken);
                    Items = items.ToList();
                }
                else
                {
                    Items = AllItems.ToList();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                Main.IsLoading = false;
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadItemsCommandAsync()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            await LoadItemsAsync(cancellationTokenSource.Token);
        }
    }
}
