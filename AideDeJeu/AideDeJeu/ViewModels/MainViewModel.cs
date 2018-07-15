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
            var allItems = await GetAllItemsAsync(source);
            var itemsViewModel = new ItemsViewModel();
            itemsViewModel.AllItems = allItems;
            await itemsViewModel.InitAsync();
            return itemsViewModel;
        }

        //public Dictionary<ItemSourceType, Func<FilterViewModel>> AllFiltersViewModel = new Dictionary<ItemSourceType, Func<FilterViewModel>>()
        //{
        //    { ItemSourceType.SpellVO, () => new VOSpellFilterViewModel() },
        //    { ItemSourceType.SpellHD, () => new HDSpellFilterViewModel() },
        //    { ItemSourceType.MonsterVO, () => new VOMonsterFilterViewModel() },
        //    { ItemSourceType.MonsterHD, () => new HDMonsterFilterViewModel() },
        //    { ItemSourceType.ConditionHD, () => new SearchFilterViewModel() },
        //    { ItemSourceType.ConditionVO, () => new SearchFilterViewModel() },
        //};

        //public FilterViewModel GetFilterViewModel(ItemSourceType itemSourceType)
        //{
        //    return AllFiltersViewModel[itemSourceType].Invoke();
        //}

        //public IEnumerable<Item> _Items = new List<Item>();
        //public IEnumerable<Item> Items
        //{
        //    get
        //    {
        //        return _Items;
        //    }
        //    set
        //    {
        //        SetProperty(ref _Items, value);
        //    }
        //}

        public Command LoadItemsCommand { get; private set; }
        //public Command<Item> GotoItemCommand { get; private set; }

        //public Command SwitchToSpellsHD { get; private set; }
        //public Command SwitchToMonstersHD { get; private set; }
        //public Command SwitchToSpellsVO { get; private set; }
        //public Command SwitchToMonstersVO { get; private set; }
        public Command AboutCommand { get; private set; }

        public Navigator Navigator { get; set; }

        public MainViewModel()
        {
            //GotoItemCommand = new Command<Item>(async (item) =>
            //{
            //    await Navigator.GotoItemDetailPageAsync(item);
            //});
            AboutCommand = new Command(async () => await Main.Navigator.GotoAboutPageAsync());
        }

        ItemSourceType MDFileToItemSourceType(string file)
        {
            if (file == "spells_hd")
            {
                return ItemSourceType.SpellHD;
            }
            else if (file == "spells_vo")
            {
                return ItemSourceType.SpellVO;
            }
            else if (file == "monsters_hd")
            {
                return ItemSourceType.MonsterHD;
            }
            else if (file == "monsters_vo")
            {
                return ItemSourceType.MonsterVO;
            }
            else if (file == "conditions_hd")
            {
                return ItemSourceType.ConditionHD;
            }
            else if (file == "conditions_vo")
            {
                return ItemSourceType.ConditionVO;
            }
            return ItemSourceType.SpellHD;
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
                    var itemSourceType = MDFileToItemSourceType(file);
                    var spells = await GetAllItemsAsync(file);
                    var spell = spells.Where(i => Tools.Helpers.IdFromName(i.Name) == anchor).FirstOrDefault();
                    if (spell != null)
                    {
                        await Navigator.GotoItemDetailPageAsync(spell);
                    }
                }
                else
                {
                    var regex = new Regex("/(?<file>.*)\\.md");
                    var match = regex.Match(s);
                    var file = match.Groups["file"].Value;
                    //var itemSourceType = MDFileToItemSourceType(file);
                    var items = await GetItemsViewModelAsync(file);
                    items.LoadItemsCommand.Execute(null);
                    await Navigator.GotoItemsPageAsync(items);
                }
            }
        }

    }
}