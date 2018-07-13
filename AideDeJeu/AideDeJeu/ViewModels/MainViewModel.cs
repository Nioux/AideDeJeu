﻿using AideDeJeuLib;
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

        private Dictionary<ItemSourceType, IEnumerable<Item>> _AllItems = new Dictionary<ItemSourceType, IEnumerable<Item>>();
        public async Task<IEnumerable<Item>> GetAllItemsAsync(ItemSourceType itemSourceType)
        {
            if (!_AllItems.ContainsKey(itemSourceType))
            {
                string resourceName = null;
                switch (itemSourceType)
                {
                    case ItemSourceType.MonsterVO:
                        {
                            resourceName = "monsters_vo";
                        }
                        break;
                    case ItemSourceType.MonsterHD:
                        {
                            resourceName = "monsters_hd";
                        }
                        break;
                    case ItemSourceType.SpellVO:
                        {
                            resourceName = "spells_vo";
                        }
                        break;
                    case ItemSourceType.SpellHD:
                        {
                            resourceName = "spells_hd";
                        }
                        break;
                    case ItemSourceType.ConditionVO:
                        {
                            resourceName = "conditions_vo";
                        }
                        break;
                    case ItemSourceType.ConditionHD:
                        {
                            resourceName = "conditions_hd";
                        }
                        break;
                }
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{resourceName}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{resourceName}.md");
                _AllItems[itemSourceType] = Tools.MarkdownExtensions.ToItem(md) as IEnumerable<Item>;
            }
            return _AllItems[itemSourceType];
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

        public Dictionary<ItemSourceType, Func<ItemsViewModel>> AllItemsViewModel = new Dictionary<ItemSourceType, Func<ItemsViewModel>>()
        {
            { ItemSourceType.SpellVO, () => new ItemsViewModel(ItemSourceType.SpellVO) },
            { ItemSourceType.SpellHD, () => new ItemsViewModel(ItemSourceType.SpellHD) },
            { ItemSourceType.MonsterVO, () => new ItemsViewModel(ItemSourceType.MonsterVO) },
            { ItemSourceType.MonsterHD, () => new ItemsViewModel(ItemSourceType.MonsterHD) },
            { ItemSourceType.ConditionHD, () => new ItemsViewModel(ItemSourceType.ConditionHD) },
            { ItemSourceType.ConditionVO, () => new ItemsViewModel(ItemSourceType.ConditionVO) },
        };

        public ItemsViewModel GetItemsViewModel(ItemSourceType itemSourceType)
        {
            return AllItemsViewModel[itemSourceType].Invoke();
        }

        public Dictionary<ItemSourceType, Func<FilterViewModel>> AllFiltersViewModel = new Dictionary<ItemSourceType, Func<FilterViewModel>>()
        {
            { ItemSourceType.SpellVO, () => new VOSpellFilterViewModel() },
            { ItemSourceType.SpellHD, () => new HDSpellFilterViewModel() },
            { ItemSourceType.MonsterVO, () => new VOMonsterFilterViewModel() },
            { ItemSourceType.MonsterHD, () => new HDMonsterFilterViewModel() },
            { ItemSourceType.ConditionHD, () => new SearchFilterViewModel() },
            { ItemSourceType.ConditionVO, () => new SearchFilterViewModel() },
        };

        public FilterViewModel GetFilterViewModel(ItemSourceType itemSourceType)
        {
            return AllFiltersViewModel[itemSourceType].Invoke();
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

        public Command LoadItemsCommand { get; private set; }
        public Command<Item> GotoItemCommand { get; private set; }

        public Command SwitchToSpellsHD { get; private set; }
        public Command SwitchToMonstersHD { get; private set; }
        public Command SwitchToSpellsVO { get; private set; }
        public Command SwitchToMonstersVO { get; private set; }
        public Command AboutCommand { get; private set; }

        public Navigator Navigator { get; set; }

        public MainViewModel()
        {
            GotoItemCommand = new Command<Item>(async (item) =>
            {
                await Navigator.GotoItemDetailPageAsync(item);
            });
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
                    var spells = await GetAllItemsAsync(itemSourceType);
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
                    var itemSourceType = MDFileToItemSourceType(file);
                    var items = GetItemsViewModel(itemSourceType);
                    items.LoadItemsCommand.Execute(null);
                    await Navigator.GotoItemsPageAsync(items);
                }
            }
        }

    }
}