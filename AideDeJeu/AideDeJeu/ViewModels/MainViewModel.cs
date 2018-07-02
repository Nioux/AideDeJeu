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
    [Flags]
    public enum ItemSourceType
    {
        SpellVO, 
        SpellHD, 
        MonsterVO, 
        MonsterHD,
        ConditionVO,
        ConditionHD,
    }

    public class MainViewModel : BaseViewModel
    {
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
                LoadItemsCommand.Execute(null);
                OnPropertyChanged(nameof(Items));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<KeyValuePair<ItemSourceType, string>> ItemsSources { get; set; } = new List<KeyValuePair<ItemSourceType, string>>()
        {
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.SpellHD, "Sorts (H&D)"),
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.SpellVO, "Spells (VO)"),
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.MonsterHD, "Créatures (H&D)"),
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.MonsterVO, "Monsters (VO)"),
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.ConditionHD, "Etats spéciaux (H&D)"),
            new KeyValuePair<ItemSourceType, string>(ItemSourceType.ConditionVO, "Conditions (VO)"),
        };

        private int _ItemsSourcesIndex = 0;
        public int ItemsSourcesIndex
        {
            get
            {
                return _ItemsSourcesIndex;
            }
            set
            {
                SetProperty(ref _ItemsSourcesIndex, value);
                ItemSourceType = ItemsSources[value].Key;
            }
        }

        public Dictionary<ItemSourceType, Lazy<ItemsViewModel>> AllItemsViewModel = new Dictionary<ItemSourceType, Lazy<ItemsViewModel>>()
        {
            { ItemSourceType.SpellVO, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.SpellVO)) },
            { ItemSourceType.SpellHD, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.SpellHD)) },
            { ItemSourceType.MonsterVO, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.MonsterVO)) },
            { ItemSourceType.MonsterHD, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.MonsterHD)) },
            { ItemSourceType.ConditionHD, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.ConditionHD)) },
            { ItemSourceType.ConditionVO, new Lazy<ItemsViewModel>(() => new ItemsViewModel(ItemSourceType.ConditionVO)) },
        };

        public ItemsViewModel GetItemsViewModel(ItemSourceType itemSourceType)
        {
            return AllItemsViewModel[itemSourceType].Value;
        }

        public Dictionary<ItemSourceType, Lazy<FilterViewModel>> AllFiltersViewModel = new Dictionary<ItemSourceType, Lazy<FilterViewModel>>()
        {
            { ItemSourceType.SpellVO, new Lazy<FilterViewModel>(() => new VOSpellFilterViewModel()) },
            { ItemSourceType.SpellHD, new Lazy<FilterViewModel>(() => new HDSpellFilterViewModel()) },
            { ItemSourceType.MonsterVO, new Lazy<FilterViewModel>(() => new VOMonsterFilterViewModel()) },
            { ItemSourceType.MonsterHD, new Lazy<FilterViewModel>(() => new HDMonsterFilterViewModel()) },
            { ItemSourceType.ConditionHD, new Lazy<FilterViewModel>(() => new SearchFilterViewModel()) },
            { ItemSourceType.ConditionVO, new Lazy<FilterViewModel>(() => new SearchFilterViewModel()) },
        };

        public FilterViewModel GetFilterViewModel(ItemSourceType itemSourceType)
        {
            return AllFiltersViewModel[itemSourceType].Value;
        }

        // Yan : pas besoin d'ObservableCollection, on ne modifie jamais la liste item par item
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
                    GotoItemCommand.Execute(_SelectedItem);
                }
            }
        }

        public Command LoadItemsCommand { get; private set; }
        public Command<Item> GotoItemCommand { get; private set; }

        public Command SwitchToSpellsHD { get; private set; }
        public Command SwitchToMonstersHD { get; private set; }
        public Command SwitchToSpellsVO { get; private set; }
        public Command SwitchToMonstersVO { get; private set; }
        public Command AboutCommand { get; private set; }
        public Command<string> SearchCommand { get; private set; }

        public Navigator Navigator { get; set; }

        public MainViewModel()
        {
            LoadItemsCommand = new Command(async () =>
                {
                    await GetItemsViewModel(ItemSourceType).ExecuteLoadItemsCommandAsync();
                });
            GotoItemCommand = new Command<Item>(async (item) =>
            {
                await GetItemsViewModel(ItemSourceType).ExecuteGotoItemCommandAsync(item);
            });
            SwitchToSpellsHD = new Command(() => ItemSourceType = ItemSourceType.SpellHD);
            SwitchToMonstersHD = new Command(() => ItemSourceType = ItemSourceType.MonsterHD);
            SwitchToSpellsVO = new Command(() => ItemSourceType = ItemSourceType.SpellVO);
            SwitchToMonstersVO = new Command(() => ItemSourceType = ItemSourceType.MonsterVO);
            AboutCommand = new Command(async () => await Main.Navigator.GotoAboutPageAsync());
            SearchCommand = new Command<string>(async (text) =>
                {
                    GetFilterViewModel(ItemSourceType).SearchText = text;
                    await GetItemsViewModel(ItemSourceType).ExecuteLoadItemsCommandAsync();
                });
        }

        public async Task NavigateToLink(string s)
        {
            if (s != null)
            { 
                var regex = new Regex("/(?<file>.*)\\.md#(?<anchor>.*)");
                var match = regex.Match(s);
                var file = match.Groups["file"].Value;
                var anchor = match.Groups["anchor"].Value;
                if (file == "spells_hd")
                {
                    var spells = await GetItemsViewModel(ItemSourceType.SpellHD).GetAllItemsAsync();
                    var spell = spells.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (spell != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(spell);
                    }
                }
                else if (file == "spells_vo")
                {
                    var spells = await GetItemsViewModel(ItemSourceType.SpellVO).GetAllItemsAsync();
                    var spell = spells.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (spell != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(spell);
                    }
                }
                else if (file == "monsters_hd")
                {
                    var monsters = await GetItemsViewModel(ItemSourceType.MonsterHD).GetAllItemsAsync();
                    var monster = monsters.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (monster != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(monster);
                    }
                }
                else if (file == "monsters_vo")
                {
                    var monsters = await GetItemsViewModel(ItemSourceType.MonsterVO).GetAllItemsAsync();
                    var monster = monsters.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (monster != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(monster);
                    }
                }
                else
                {
                    //Device.OpenUri(new Uri(s));
                }
            }
        }

    }
}