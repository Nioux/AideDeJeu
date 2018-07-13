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

        public ItemsViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync().ConfigureAwait(false));
            Filter = Main.GetFilterViewModel(ItemSourceType);
            Filter.LoadItemsCommand = LoadItemsCommand;
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
        //protected ItemSourceType ItemSourceType;


        //private IEnumerable<Item> _AllItems = null;
        //public async Task<IEnumerable<Item>> GetAllItemsAsync()
        //{
        //    if (_AllItems == null)
        //    {
        //        string resourceName = null;
        //        switch (ItemSourceType)
        //        {
        //            case ItemSourceType.MonsterVO:
        //                {
        //                    resourceName = "AideDeJeu.Data.monsters_vo.md";
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToMonsters<MonsterVO>(md);
        //                }
        //                break;
        //            case ItemSourceType.MonsterHD:
        //                {
        //                    resourceName = "AideDeJeu.Data.monsters_hd.md";
        //                    //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/monsters_hd.md");
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToMonsters<MonsterHD>(md);
        //                }
        //                break;
        //            case ItemSourceType.SpellVO:
        //                {
        //                    resourceName = "AideDeJeu.Data.spells_vo.md";
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToSpells<SpellVO>(md);
        //                }
        //                break;
        //            case ItemSourceType.SpellHD:
        //                {
        //                    resourceName = "AideDeJeu.Data.spells_hd.md";
        //                    //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToSpells<SpellHD>(md);
        //                }
        //                break;
        //            case ItemSourceType.ConditionVO:
        //                {
        //                    resourceName = "AideDeJeu.Data.conditions_vo.md";
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToConditions<AideDeJeuLib.Condition>(md);
        //                }
        //                break;
        //            case ItemSourceType.ConditionHD:
        //                {
        //                    resourceName = "AideDeJeu.Data.conditions_hd.md";
        //                    //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
        //                    var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
        //                    _AllItems = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
        //                    //_AllItems = Tools.MarkdownExtensions.MarkdownToConditions<AideDeJeuLib.Condition>(md);
        //                }
        //                break;
        //        }
        //    }
        //    return _AllItems;
        //}

        private ItemSourceType _ItemSourceType = ItemSourceType.SpellHD;
        public ItemSourceType ItemSourceType
        {
            get
            {
                return _ItemSourceType;
            }
            set
            {
                SetProperty(ref _ItemSourceType, value);
                //LoadItemsCommand.Execute(null);
                OnPropertyChanged(nameof(Items));
            }
        }

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


        async Task LoadItemsAsync(CancellationToken cancellationToken = default)
        {
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                var filterViewModel = Filter;
                var allItems = await Main.GetAllItemsAsync(ItemSourceType);
                var items = await filterViewModel.FilterItems(allItems, cancellationToken: cancellationToken);
                Items = items.ToList();
                Title = (allItems as Item)?.Name;
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
