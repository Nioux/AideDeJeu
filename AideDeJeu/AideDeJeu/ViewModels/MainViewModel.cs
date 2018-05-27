using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        VF = 0x0100,
        VO = 0x1100,
        HD = 0x1000,
        SpellVF = Spell | VF,
        SpellVO = Spell | VO,
        SpellHD = Spell | HD,
        MonsterVF = Monster | VF,
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
                //CurrentViewModel.SearchText = "";
                SetProperty(ref _ItemSourceType, value);
                //CurrentViewModel.SearchText = "";
                LoadItemsCommand.Execute(null);
                OnPropertyChanged(nameof(Items));
            }
        }

        //private ItemSource _ItemsSource = ItemSource.VF;
        //public ItemSource ItemsSource
        //{
        //    get
        //    {
        //        return _ItemsSource;
        //    }
        //    set
        //    {
        //        //CurrentViewModel.SearchText = "";
        //        SetProperty(ref _ItemsSource, value);
        //        //CurrentViewModel.SearchText = "";
        //        //OnPropertyChanged(nameof(CurrentViewModel));
        //        LoadItemsCommand.Execute(null);
        //    }
        //}

        public Dictionary<ItemSourceType, Lazy<ItemsViewModel>> AllItemsViewModel = new Dictionary<ItemSourceType, Lazy<ItemsViewModel>>()
        {
            { ItemSourceType.SpellVF, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellVF)) },
            { ItemSourceType.SpellVO, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellVO)) },
            { ItemSourceType.SpellHD, new Lazy<ItemsViewModel>(() => new SpellsViewModel(ItemSourceType.SpellHD)) },
            { ItemSourceType.MonsterVF, new Lazy<ItemsViewModel>(() => new MonstersViewModel()) },
            { ItemSourceType.MonsterVO, new Lazy<ItemsViewModel>(() => new MonstersViewModel()) },
            { ItemSourceType.MonsterHD, new Lazy<ItemsViewModel>(() => new MonstersViewModel()) },
        };

        public ItemsViewModel GetItemsViewModel(ItemSourceType itemSourceType)
        {
            return AllItemsViewModel[itemSourceType].Value;
        }

        public Dictionary<ItemSourceType, Lazy<FilterViewModel>> AllFiltersViewModel = new Dictionary<ItemSourceType, Lazy<FilterViewModel>>()
        {
            { ItemSourceType.SpellVF, new Lazy<FilterViewModel>(() => new VFSpellFilterViewModel()) },
            { ItemSourceType.SpellVO, new Lazy<FilterViewModel>(() => new VOSpellFilterViewModel()) },
            { ItemSourceType.SpellHD, new Lazy<FilterViewModel>(() => new HDSpellFilterViewModel()) },
        };

        public FilterViewModel GetFilterViewModel(ItemSourceType itemSourceType)
        {
            return AllFiltersViewModel[itemSourceType].Value;
        }

        //public ItemsViewModel SpellsVF
        //{
        //    get
        //    {
        //        return AllItemsViewModel[ItemSourceType.SpellVF].Value;
        //    }
        //}
        //public ItemsViewModel CurrentViewModel
        //{
        //    get
        //    {
        //        if (ItemsType == ItemType.Spell)
        //        {
        //            return Spells;
        //        }
        //        if (ItemsType == ItemType.Monster)
        //        {
        //            return Monsters;
        //        }
        //        return null;
        //    }
        //}
        public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();

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

        public Command SwitchToSpells { get; private set; }
        public Command SwitchToMonsters { get; private set; }
        public Command SwitchToVF { get; private set; }
        public Command SwitchToVO { get; private set; }
        public Command SwitchToHD { get; private set; }
        public Command AboutCommand { get; private set; }
        public Command<string> SearchCommand { get; private set; }

        public Navigator Navigator { get; set; }
        public MainViewModel()
        {
            //Spells = new SpellsViewModel(navigator, Items);
            //Monsters = new MonstersViewModel(navigator, Items);
            LoadItemsCommand = new Command(() => GetItemsViewModel(ItemSourceType).ExecuteLoadItemsCommand(GetFilterViewModel(ItemSourceType)));
            GotoItemCommand = new Command<Item>(async (item) => await GetItemsViewModel(ItemSourceType).ExecuteGotoItemCommandAsync(item));
            SwitchToSpells = new Command(() => ItemSourceType = (ItemSourceType & ~ ItemSourceType.Monster) | ItemSourceType.Spell);
            SwitchToMonsters = new Command(() => ItemSourceType = (ItemSourceType & ~ItemSourceType.Spell) | ItemSourceType.Monster);
            SwitchToVF = new Command(() => ItemSourceType = (ItemSourceType & ~ItemSourceType.VO & ~ItemSourceType.HD) | ItemSourceType.VF);
            SwitchToVO = new Command(() => ItemSourceType = (ItemSourceType & ~ItemSourceType.VF & ~ItemSourceType.HD) | ItemSourceType.VO);
            SwitchToHD = new Command(() => ItemSourceType = (ItemSourceType & ~ItemSourceType.VF & ~ItemSourceType.VO) | ItemSourceType.HD);
            //AboutCommand = new Command(async() => await navigator.GotoAboutPageAsync());
            //SearchCommand = new Command<string>((text) => GetItemsViewModel(ItemSourceType).SearchText = text);
        }
    }
}