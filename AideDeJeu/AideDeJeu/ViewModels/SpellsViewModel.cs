using AideDeJeuLib;
using AideDeJeuLib.Spells;
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
    public class SpellsViewModel : ItemsViewModel
    {
        ItemSourceType ItemSourceType;
        public SpellsViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
        }
        private IEnumerable<Spell> _AllSpells = null;
        private IEnumerable<Spell> AllSpells
        {
            get
            {
                if(_AllSpells == null)
                {
                    string resourceName = null;
                    switch(ItemSourceType)
                    {
                        case ItemSourceType.SpellVF:
                            resourceName = "AideDeJeu.Data.spells_vf.json";
                            break;
                        case ItemSourceType.SpellVO:
                            resourceName = "AideDeJeu.Data.spells_vo.json";
                            break;
                        case ItemSourceType.SpellHD:
                            resourceName = "AideDeJeu.Data.spells_hd.json";
                            break;
                    }
                    _AllSpells = Tools.Helpers.GetResourceObject<IEnumerable<Spell>>(resourceName);
                }
                return _AllSpells;
            }
        }

        public IEnumerable<Spell> GetSpells(string classe, string niveauMin, string niveauMax, string ecole, string rituel, string source)
        {
            return AllSpells
                    .Where(spell =>
                                (int.Parse(spell.Level) >= int.Parse(niveauMin)) &&
                                (int.Parse(spell.Level) <= int.Parse(niveauMax)) &&
                                spell.Type.ToLower().Contains(ecole.ToLower()) &&
                                spell.Source.Contains(source) &&
                                spell.Source.Contains(classe) &&
                                spell.Type.Contains(rituel)
                                )
                        .OrderBy(spell => spell.NamePHB)
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
                IEnumerable<Spell> items = null;
                SpellFilterViewModel filters = filterViewModel as SpellFilterViewModel;
                items = GetSpells(classe: filters.Classes[filters.Classe].Key, niveauMin: filters.Niveaux[filters.NiveauMin].Key, niveauMax: filters.Niveaux[filters.NiveauMax].Key, ecole: filters.Ecoles[filters.Ecole].Key, rituel: filters.Rituels[filters.Rituel].Key, source: filters.Sources[filters.Source].Key);
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
            await Main.Navigator.GotoSpellDetailPageAsync(item as Spell);
        }

    }
}