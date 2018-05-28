using AideDeJeu.Tools;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class MonstersViewModel : ItemsViewModel
    {
        ItemSourceType ItemSourceType;
        public MonstersViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
        }


        private IEnumerable<Monster> _AllMonsters = null;
        private IEnumerable<Monster> AllMonsters
        {
            get
            {
                if (_AllMonsters == null)
                {
                    string resourceName = null;
                    switch (ItemSourceType)
                    {
                        case ItemSourceType.MonsterVF:
                            resourceName = "AideDeJeu.Data.monsters_vf.json";
                            break;
                        case ItemSourceType.MonsterVO:
                            resourceName = "AideDeJeu.Data.monsters_vo.json";
                            break;
                        case ItemSourceType.MonsterHD:
                            resourceName = "AideDeJeu.Data.monsters_hd.json";
                            break;
                    }
                    _AllMonsters = Tools.Helpers.GetResourceObject<IEnumerable<Monster>>(resourceName);
                }
                return _AllMonsters;
            }
        }


        public IEnumerable<Monster> GetMonsters(string category, string type, string minPower, string maxPower, string size, string legendary, string source)
        {
            var powerComparer = new PowerComparer();
            return AllMonsters.Where(monster =>
                            monster.Type.Contains(type) &&
                            (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                            monster.Source.Contains(source) &&
                            powerComparer.Compare(monster.Challenge, minPower) >= 0 &&
                            powerComparer.Compare(monster.Challenge, maxPower) <= 0
                            )
                        .OrderBy(monster => monster.NamePHB)
                        .ToList();
        }


        public override void ExecuteLoadItemsCommand(FilterViewModel filterViewModel)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Main.Items.Clear();
                //var filter = filterViewModel as MonsterFilterViewModel;
                //var items = GetMonsters(category: filter.Categories[filter.Category].Key, type: filter.Types[filter.Type].Key, minPower: filter.Powers[filter.MinPower].Key, maxPower: filter.Powers[filter.MaxPower].Key, size: filter.Sizes[filter.Size].Key, legendary: filter.Legendaries[filter.Legendary].Key, source: filter.Sources[filter.Source].Key);
                var items = filterViewModel.FilterItems(AllMonsters);

                foreach (var item in items)
                {
                    Main.Items.Add(item);
                }
                //FilterItems();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Main.Navigator.GotoMonsterDetailPageAsync(item as Monster);
        }

    }
}