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
    public enum ItemType
    {
        Spell,
        Monster,
    }

    public enum ItemSource
    {
        VF,
        VO,
        HD
    }

    [Flags]
    public enum ItemSourceType
    {
        Spell = 0x01,
        Monster = 0x10,
        //VF = 0x0100,
        VO = 0x1100,
        HD = 0x1000,
        //SpellVF = Spell | VF,
        SpellVO = Spell | VO,
        SpellHD = Spell | HD,
        //MonsterVF = Monster | VF,
        MonsterVO = Monster | VO,
        MonsterHD = Monster | HD,
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

        public Dictionary<ItemSourceType, Lazy<ItemsViewModel>> AllItemsViewModel = new Dictionary<ItemSourceType, Lazy<ItemsViewModel>>()
        {
            //{ ItemSourceType.SpellVF, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellVF)) },
            { ItemSourceType.SpellVO, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellVO)) },
            { ItemSourceType.SpellHD, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellHD)) },
            //{ ItemSourceType.MonsterVF, new Lazy<ItemsViewModel>(() => new MonstersViewModel(ItemSourceType.MonsterVF)) },
            { ItemSourceType.MonsterVO, new Lazy<ItemsViewModel>(() => new MonstersViewModel(ItemSourceType.MonsterVO)) },
            { ItemSourceType.MonsterHD, new Lazy<ItemsViewModel>(() => new MonstersViewModel(ItemSourceType.MonsterHD)) },
        };

        public ItemsViewModel GetItemsViewModel(ItemSourceType itemSourceType)
        {
            return AllItemsViewModel[itemSourceType].Value;
        }

        public Dictionary<ItemSourceType, Lazy<FilterViewModel>> AllFiltersViewModel = new Dictionary<ItemSourceType, Lazy<FilterViewModel>>()
        {
            //{ ItemSourceType.SpellVF, new Lazy<FilterViewModel>(() => new VFSpellFilterViewModel()) },
            { ItemSourceType.SpellVO, new Lazy<FilterViewModel>(() => new VOSpellFilterViewModel()) },
            { ItemSourceType.SpellHD, new Lazy<FilterViewModel>(() => new HDSpellFilterViewModel()) },
            //{ ItemSourceType.MonsterVF, new Lazy<FilterViewModel>(() => new VFMonsterFilterViewModel()) },
            { ItemSourceType.MonsterVO, new Lazy<FilterViewModel>(() => new VOMonsterFilterViewModel()) },
            { ItemSourceType.MonsterHD, new Lazy<FilterViewModel>(() => new HDMonsterFilterViewModel()) },
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
                    var spell = spells.Where(i => i.Id == anchor).FirstOrDefault();
                    if (spell != null)
                    {
                        await Navigator.GotoSpellDetailPageAsync(spell);
                    }
                }
                else if (file == "spells_vo")
                {
                    var spells = await GetItemsViewModel(ItemSourceType.SpellVO).GetAllItemsAsync();
                    var spell = spells.Where(i => i.Id == anchor).FirstOrDefault();
                    if (spell != null)
                    {
                        await Navigator.GotoSpellDetailPageAsync(spell);
                    }
                }
                else if (file == "monsters_hd")
                {
                    var monsters = await GetItemsViewModel(ItemSourceType.MonsterHD).GetAllItemsAsync();
                    var monster = monsters.Where(i => i.Id == anchor).FirstOrDefault();
                    if (monster != null)
                    {
                        await Navigator.GotoMonsterDetailPageAsync(monster);
                    }
                }
                else if (file == "monsters_vo")
                {
                    var monsters = await GetItemsViewModel(ItemSourceType.MonsterVO).GetAllItemsAsync();
                    var monster = monsters.Where(i => i.Id == anchor).FirstOrDefault();
                    if (monster != null)
                    {
                        await Navigator.GotoMonsterDetailPageAsync(monster);
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