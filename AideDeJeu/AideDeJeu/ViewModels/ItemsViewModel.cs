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
            //this.ItemSourceType = itemSourceType;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync().ConfigureAwait(false));
            //Filter = Main.GetFilterViewModel(ItemSourceType);
            //Filter.LoadItemsCommand = LoadItemsCommand;
            SearchCommand = new Command<string>((text) =>
            {
                Filter.SearchText = text;
            });
        }
        public Command<string> SearchCommand { get; private set; }
        public ICommand LoadItemsCommand { get; protected set; }
        public async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Main.Navigator.GotoItemDetailPageAsync(item);
        }

        //private ItemSourceType _ItemSourceType = ItemSourceType.SpellHD;
        //public ItemSourceType ItemSourceType
        //{
        //    get
        //    {
        //        return _ItemSourceType;
        //    }
        //    set
        //    {
        //        SetProperty(ref _ItemSourceType, value);
        //        OnPropertyChanged(nameof(Items));
        //    }
        //}

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
                    Main.GotoItemCommand.Execute(_SelectedItem);
                }
            }
        }

        public IEnumerable<Item> AllItems;
        public async Task InitAsync()
        {
            //AllItems = await Main.GetAllItemsAsync(ItemSourceType);
            Title = (AllItems as Item)?.Name;
            Filter = (AllItems as Items).GetNewFilterViewModel(); //Main.GetFilterViewModel(ItemSourceType);
            Filter.LoadItemsCommand = LoadItemsCommand;
        }

        async Task LoadItemsAsync(CancellationToken cancellationToken = default)
        {
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                //var filterViewModel = Filter;
                //var allItems = await Main.GetAllItemsAsync(ItemSourceType);
                var items = await Filter.FilterItems(AllItems, cancellationToken: cancellationToken);
                Items = items.ToList();
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
