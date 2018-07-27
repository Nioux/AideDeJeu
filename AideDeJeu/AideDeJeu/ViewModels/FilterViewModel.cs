using AideDeJeu.Tools;
using AideDeJeuLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AideDeJeu.ViewModels
{
    public abstract class FilterViewModel : BaseViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public abstract Task<IEnumerable<Item>> FilterItems(IEnumerable<Item> items, CancellationToken cancellationToken = default);
        public abstract IEnumerable<Filter> Filters { get; }
        private string _SearchText = "";
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                SetProperty(ref _SearchText, value);
                LoadItemsCommand.Execute(null);
            }
        }

        protected void RegisterFilters()
        {
            foreach (var filter in Filters)
            {
                filter.PropertyChanged += Filter_PropertyChanged;
            }
        }

        protected void Filter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Index")
            {
                LoadItemsCommand.Execute(null);
            }
        }


    }

    public enum FilterKeys
    {
        Class,
        MinLevel,
        MaxLevel,
        School,
        Ritual,
        Source,
        Category,
        Type,
        MinPower,
        MaxPower,
        Size,
        Legendary,
        MinPrice,
        MaxPrice,
        MinWeight,
        MaxWeight,
    }

    public class Filter : BaseViewModel
    {
        public FilterKeys Key { get; set; }
        public string Name { get; set; }
        public List<KeyValuePair<string, string>> KeyValues { get; set; }
        public int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                if (value >= 0)
                {
                    if (_Index != value)
                    {
                        SetProperty(ref _Index, value);
                    }
                }
            }
        }

        public string SelectedKey
        {
            get
            {
                if (Index >= 0 && Index < KeyValues.Count)
                {
                    return KeyValues[Index].Key;
                }
                return null;
            }
        }
    }

    public class SearchFilterViewModel : FilterViewModel
    {
        private IEnumerable<Filter> _Filters = null;
        public override IEnumerable<Filter> Filters
        {
            get
            {
                if (_Filters == null)
                {
                    _Filters = new List<Filter>()
                    {
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }


        public override async Task<IEnumerable<Item>> FilterItems(IEnumerable<Item> items, CancellationToken token = default)
        {
            return await Task.Run(() =>
            {
                return items.Where(item =>
                {
                    var spell = item;
                    return 
                        (
                            (Helpers.RemoveDiacritics(spell.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(spell.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        );
                }).AsEnumerable();
            }, token);

        }

    }


    #region Spells
    public abstract class SpellFilterViewModel : FilterViewModel
    {
        private IEnumerable<Filter> _Filters = null;
        public override IEnumerable<Filter> Filters
        {
            get
            {
                if (_Filters == null)
                {
                    _Filters = new List<Filter>()
                    {
                        new Filter() { Key = FilterKeys.Class, Name = "Classe", KeyValues = Classes, _Index = 0 },
                        new Filter() { Key = FilterKeys.MinLevel, Name = "Niveau Minimum", KeyValues = Niveaux, _Index = 0 },
                        new Filter() { Key = FilterKeys.MaxLevel, Name = "Niveau Maximum", KeyValues = Niveaux, _Index = 9 },
                        new Filter() { Key = FilterKeys.School, Name = "École", KeyValues = Ecoles, _Index = 0 },
                        new Filter() { Key = FilterKeys.Ritual, Name = "Rituel", KeyValues = Rituels, _Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, _Index = 0 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public override async Task<IEnumerable<Item>> FilterItems(IEnumerable<Item> items, CancellationToken token = default)
        {
            return await Task.Run(() =>
            {
                var classe = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Class).SelectedKey ?? "";
                var niveauMin = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinLevel).SelectedKey ?? "0";
                var niveauMax = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxLevel).SelectedKey ?? "9";
                var ecole = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.School).SelectedKey ?? "";
                var rituel = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Ritual).SelectedKey ?? "";
                var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey ?? "";
                token.ThrowIfCancellationRequested();
                return items.Where(item =>
                {
                    var spell = item as Spell;
                    return (int.Parse(spell.Level) >= int.Parse(niveauMin)) &&
                        (int.Parse(spell.Level) <= int.Parse(niveauMax)) &&
                        spell.Type.ToLower().Contains(ecole.ToLower()) &&
                        spell.Source.Contains(source) &&
                        spell.Classes.Contains(classe) &&
                        spell.Rituel.Contains(rituel) &&
                        (
                            (Helpers.RemoveDiacritics(spell.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(spell.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        );
                }).OrderBy(spell => spell.Name)
                            .AsEnumerable();
            }, token);

        }

        public abstract List<KeyValuePair<string, string>> Classes { get; }

        public abstract List<KeyValuePair<string, string>> Niveaux { get; }

        public abstract List<KeyValuePair<string, string>> Ecoles { get; }

        public abstract List<KeyValuePair<string, string>> Rituels { get; }

        public abstract List<KeyValuePair<string, string>> Sources { get; }

    }

    public class VFSpellFilterViewModel : SpellFilterViewModel
    {

        public override List<KeyValuePair<string, string>> Classes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("Barde", "Barde" ),
            new KeyValuePair<string, string>("Clerc", "Clerc" ),
            new KeyValuePair<string, string>("Druide", "Druide" ),
            new KeyValuePair<string, string>("Ensorceleur", "Ensorceleur" ),
            new KeyValuePair<string, string>("Magicien", "Magicien" ),
            new KeyValuePair<string, string>("Paladin", "Paladin" ),
            new KeyValuePair<string, string>("Rôdeur", "Rôdeur" ),
            new KeyValuePair<string, string>("Sorcier", "Sorcier" ),
        };

        public override List<KeyValuePair<string, string>> Niveaux { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("0", "Sorts mineurs"),
            new KeyValuePair<string, string>("1", "Niveau 1"),
            new KeyValuePair<string, string>("2", "Niveau 2"),
            new KeyValuePair<string, string>("3", "Niveau 3"),
            new KeyValuePair<string, string>("4", "Niveau 4"),
            new KeyValuePair<string, string>("5", "Niveau 5"),
            new KeyValuePair<string, string>("6", "Niveau 6"),
            new KeyValuePair<string, string>("7", "Niveau 7"),
            new KeyValuePair<string, string>("8", "Niveau 8"),
            new KeyValuePair<string, string>("9", "Niveau 9"),
        };

        public override List<KeyValuePair<string, string>> Ecoles { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("abjuration", "Abjuration"),
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantement", "Enchantement"),
            new KeyValuePair<string, string>("évocation", "Évocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("invocation", "Invocation"),
            new KeyValuePair<string, string>("cromancie", "Nécromancie"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public override List<KeyValuePair<string, string>> Rituels { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous"),
            new KeyValuePair<string, string>("(rituel)", "Rituel"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
        };

    }

    public class VOSpellFilterViewModel : SpellFilterViewModel
    {
        public override List<KeyValuePair<string, string>> Classes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All" ),
            new KeyValuePair<string, string>("Bard", "Bard" ),
            new KeyValuePair<string, string>("Cleric", "Cleric" ),
            new KeyValuePair<string, string>("Druid", "Druid" ),
            new KeyValuePair<string, string>("Sorcerer", "Sorcerer" ),
            new KeyValuePair<string, string>("Paladin", "Paladin" ),
            new KeyValuePair<string, string>("Ranger", "Ranger" ),
            new KeyValuePair<string, string>("Warlock", "Warlock" ),
            new KeyValuePair<string, string>("Wizard", "Wizard" ),
        };

        public override List<KeyValuePair<string, string>> Niveaux { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("0", "Sorts mineurs"),
            new KeyValuePair<string, string>("1", "Level 1"),
            new KeyValuePair<string, string>("2", "Level 2"),
            new KeyValuePair<string, string>("3", "Level 3"),
            new KeyValuePair<string, string>("4", "Level 4"),
            new KeyValuePair<string, string>("5", "Level 5"),
            new KeyValuePair<string, string>("6", "Level 6"),
            new KeyValuePair<string, string>("7", "Level 7"),
            new KeyValuePair<string, string>("8", "Level 8"),
            new KeyValuePair<string, string>("9", "Level 9"),
        };

        public override List<KeyValuePair<string, string>> Ecoles { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("abjuration", "Abjuration"),
            new KeyValuePair<string, string>("conjuration", "Conjuration"),
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantment", "Enchantment"),
            new KeyValuePair<string, string>("evocation", "Evocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("necromancy", "Necromancy"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public override List<KeyValuePair<string, string>> Rituels { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("(ritual)", "Ritual"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
        };
    }

    public class HDSpellFilterViewModel : SpellFilterViewModel
    {
        public override List<KeyValuePair<string, string>> Classes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("Barde", "Barde" ),
            new KeyValuePair<string, string>("Clerc", "Clerc" ),
            new KeyValuePair<string, string>("Druide", "Druide" ),
            new KeyValuePair<string, string>("Ensorceleur", "Ensorceleur" ),
            new KeyValuePair<string, string>("Magicien", "Magicien" ),
            new KeyValuePair<string, string>("Ombrelame", "Ombrelame" ),
            new KeyValuePair<string, string>("Paladin", "Paladin" ),
            new KeyValuePair<string, string>("Rôdeur", "Rôdeur" ),
            new KeyValuePair<string, string>("Sorcier", "Sorcier" ),
        };

        public override List<KeyValuePair<string, string>> Niveaux { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("0", "Sorts mineurs"),
            new KeyValuePair<string, string>("1", "Niveau 1"),
            new KeyValuePair<string, string>("2", "Niveau 2"),
            new KeyValuePair<string, string>("3", "Niveau 3"),
            new KeyValuePair<string, string>("4", "Niveau 4"),
            new KeyValuePair<string, string>("5", "Niveau 5"),
            new KeyValuePair<string, string>("6", "Niveau 6"),
            new KeyValuePair<string, string>("7", "Niveau 7"),
            new KeyValuePair<string, string>("8", "Niveau 8"),
            new KeyValuePair<string, string>("9", "Niveau 9"),
        };

        public override List<KeyValuePair<string, string>> Ecoles { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("abjuration", "Abjuration"),
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantement", "Enchantement"),
            new KeyValuePair<string, string>("évocation", "Évocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("invocation", "Invocation"),
            new KeyValuePair<string, string>("cromancie", "Nécromancie"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public override List<KeyValuePair<string, string>> Rituels { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous"),
            new KeyValuePair<string, string>("(rituel)", "Rituel"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            new KeyValuePair<string, string>("(HD)", "H&D"),
        };
    }
    #endregion Spells

    #region Monsters
    public abstract class MonsterFilterViewModel : FilterViewModel
    {
        private IEnumerable<Filter> _Filters = null;
        public override IEnumerable<Filter> Filters
        {
            get
            {
                if (_Filters == null)
                {
                    _Filters = new List<Filter>()
                    {
                        //new Filter() { Key = FilterKeys.Category, Name = "Catégories", KeyValues = Categories, _Index = 0 },
                        new Filter() { Key = FilterKeys.Type, Name = "Type", KeyValues = Types, _Index = 0 },
                        new Filter() { Key = FilterKeys.MinPower, Name = "FP Minimum", KeyValues = Powers, _Index = 0 },
                        new Filter() { Key = FilterKeys.MaxPower, Name = "FP Maximum", KeyValues = Powers, _Index = 28 },
                        new Filter() { Key = FilterKeys.Size, Name = "Taille", KeyValues = Sizes, _Index = 0 },
                        //new Filter() { Key = FilterKeys.Legendary, Name = "Légendaire", KeyValues = Legendaries, _Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, _Index = 0 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public override async Task<IEnumerable<Item>> FilterItems(IEnumerable<Item> items, CancellationToken token = default)
        {
            return await Task.Run(() =>
            {
                var powerComparer = new PowerComparer();

                //var category = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Category).SelectedKey ?? "";
                var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
                token.ThrowIfCancellationRequested();

                var minPower = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinPower).SelectedKey ?? "0 (0 PX)";
                token.ThrowIfCancellationRequested();

                var maxPower = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxPower).SelectedKey ?? "30 (155000 PX)";
                token.ThrowIfCancellationRequested();

                var size = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Size).SelectedKey ?? "";
                token.ThrowIfCancellationRequested();
                //var legendary = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Legendary).SelectedKey ?? "";

                var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey ?? "";
                token.ThrowIfCancellationRequested();

                return items.Where(item =>
                {
                    var monster = item as Monster;
                    return 
                        monster != null &&
                        monster.Type.Contains(type) &&
                        (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                        monster.Source.Contains(source) &&
                        powerComparer.Compare(monster.Challenge, minPower) >= 0 &&
                        powerComparer.Compare(monster.Challenge, maxPower) <= 0 &&
                        (
                            (Helpers.RemoveDiacritics(monster.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(monster.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        );
                })
                    .OrderBy(monster => monster.Name)
                            .AsEnumerable();
            }, token);

        }

        public abstract List<KeyValuePair<string, string>> Categories { get; }

        public abstract List<KeyValuePair<string, string>> Types { get; }

        public abstract List<KeyValuePair<string, string>> Powers { get; }

        public abstract List<KeyValuePair<string, string>> Sizes { get; }

        public abstract List<KeyValuePair<string, string>> Legendaries { get; }

        public abstract List<KeyValuePair<string, string>> Sources { get; }

    }

    public class VFMonsterFilterViewModel : MonsterFilterViewModel
    {
        public override List<KeyValuePair<string, string>> Categories { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("M", "Monstres" ),
            new KeyValuePair<string, string>("A", "Animaux" ),
            new KeyValuePair<string, string>("P", "PNJ" ),
        };

        public override List<KeyValuePair<string, string>> Types { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous" ),
            new KeyValuePair<string, string>("Humanoïde", "Humanoïde"),
            new KeyValuePair<string, string>("Aberration", "Aberration"),
            new KeyValuePair<string, string>("Bête", "Bête"),
            new KeyValuePair<string, string>("Céleste", "Céleste"),
            new KeyValuePair<string, string>("Créature artificielle", "Créature artificielle"),
            new KeyValuePair<string, string>("Créature monstrueuse", "Créature monstrueuse"),
            new KeyValuePair<string, string>("Dragon", "Dragon"),
            new KeyValuePair<string, string>("Élémentaire", "Élémentaire"),
            new KeyValuePair<string, string>("Fée", "Fée"),
            new KeyValuePair<string, string>("Fiélon", "Fiélon"),
            new KeyValuePair<string, string>("Géant", "Géant"),
            new KeyValuePair<string, string>("Mort-vivant", "Mort-vivant"),
            new KeyValuePair<string, string>("Plante", "Plante"),
            new KeyValuePair<string, string>("Vase", "Vase"),
        };

        public override List<KeyValuePair<string, string>> Powers { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(" 0 (0 PX)", "0" ),
            new KeyValuePair<string, string>(" 1/8 (25 PX)", "1/8" ),
            new KeyValuePair<string, string>(" 1/4 (50 PX)", "1/4" ),
            new KeyValuePair<string, string>(" 1/2 (100 PX)", "1/2" ),
            new KeyValuePair<string, string>(" 1 (200 PX)", "1" ),
            new KeyValuePair<string, string>(" 2 (450 PX)", "2" ),
            new KeyValuePair<string, string>(" 3 (700 PX)", "3" ),
            new KeyValuePair<string, string>(" 4 (1100 PX)", "4" ),
            new KeyValuePair<string, string>(" 5 (1800 PX)", "5" ),
            new KeyValuePair<string, string>(" 6 (2300 PX)", "6" ),
            new KeyValuePair<string, string>(" 7 (2900 PX)", "7" ),
            new KeyValuePair<string, string>(" 8 (3900 PX)", "8" ),
            new KeyValuePair<string, string>(" 9 (5000 PX)", "9" ),
            new KeyValuePair<string, string>(" 10 (5900 PX)", "10" ),
            new KeyValuePair<string, string>(" 11 (7200 PX)", "11" ),
            new KeyValuePair<string, string>(" 12 (8400 PX)", "12" ),
            new KeyValuePair<string, string>(" 13 (10000 PX)", "13" ),
            new KeyValuePair<string, string>(" 14 (11500 PX)", "14" ),
            new KeyValuePair<string, string>(" 15 (13000 PX)", "15" ),
            new KeyValuePair<string, string>(" 16 (15000 PX)", "16" ),
            new KeyValuePair<string, string>(" 17 (18000 PX)", "17" ),
            new KeyValuePair<string, string>(" 18 (20000 PX)", "18" ),
            new KeyValuePair<string, string>(" 19 (22000 PX)", "19" ),
            new KeyValuePair<string, string>(" 20 (25000 PX)", "20" ),
            new KeyValuePair<string, string>(" 21 (33000 PX)", "21" ),
            new KeyValuePair<string, string>(" 22 (41000 PX)", "22" ),
            new KeyValuePair<string, string>(" 23 (50000 PX)", "23" ),
            new KeyValuePair<string, string>(" 24 (62000 PX)", "24" ),
            new KeyValuePair<string, string>(" 30 (155000 PX)", "30" ),
        };

        public override List<KeyValuePair<string, string>> Sizes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("TP", "Très petite"),
            new KeyValuePair<string, string>("P", "Petite"),
            new KeyValuePair<string, string>("M", "Moyenne"),
            new KeyValuePair<string, string>("G", "Grande"),
            new KeyValuePair<string, string>("TG", "Très grande"),
            new KeyValuePair<string, string>("Gig", "Gigantesque"),
        };

        public override List<KeyValuePair<string, string>> Legendaries { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("si", "Si"),
            new KeyValuePair<string, string>("no", "Non"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            //new KeyValuePair<string, string>("Monster Manual", "MM"),
            //new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            //new KeyValuePair<string, string>("supno", "AL, AideDD"),
        };
    }

    public class VOMonsterFilterViewModel : MonsterFilterViewModel
    {
        public override List<KeyValuePair<string, string>> Categories { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("M", "Monstres" ),
            new KeyValuePair<string, string>("A", "Animaux" ),
            new KeyValuePair<string, string>("P", "PNJ" ),
        };

        public override List<KeyValuePair<string, string>> Types { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All" ),
            new KeyValuePair<string, string>("humanoid", "Humanoid"),
            new KeyValuePair<string, string>("aberration", "Aberration"),
            new KeyValuePair<string, string>("beast", "Beast"),
            new KeyValuePair<string, string>("celestial", "Celestial"),
            new KeyValuePair<string, string>("construct", "Construct"),
            new KeyValuePair<string, string>("dragon", "Dragon"),
            new KeyValuePair<string, string>("elemental", "Elemental"),
            new KeyValuePair<string, string>("fey", "Fey"),
            new KeyValuePair<string, string>("fiend", "Fiend"),
            new KeyValuePair<string, string>("giant", "Giant"),
            new KeyValuePair<string, string>("monstrosity", "Monstrosity"),
            new KeyValuePair<string, string>("ooze", "Ooze"),
            new KeyValuePair<string, string>("plant", "Plant"),
            new KeyValuePair<string, string>("undead", "Undead"),
        };

        public override List<KeyValuePair<string, string>> Powers { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(" 0 (0 XP)", "0" ),
            new KeyValuePair<string, string>(" 1/8 (25 XP)", "1/8" ),
            new KeyValuePair<string, string>(" 1/4 (50 XP)", "1/4" ),
            new KeyValuePair<string, string>(" 1/2 (100 XP)", "1/2" ),
            new KeyValuePair<string, string>(" 1 (200 XP)", "1" ),
            new KeyValuePair<string, string>(" 2 (450 XP)", "2" ),
            new KeyValuePair<string, string>(" 3 (700 XP)", "3" ),
            new KeyValuePair<string, string>(" 4 (1100 XP)", "4" ),
            new KeyValuePair<string, string>(" 5 (1800 XP)", "5" ),
            new KeyValuePair<string, string>(" 6 (2300 XP)", "6" ),
            new KeyValuePair<string, string>(" 7 (2900 XP)", "7" ),
            new KeyValuePair<string, string>(" 8 (3900 XP)", "8" ),
            new KeyValuePair<string, string>(" 9 (5000 XP)", "9" ),
            new KeyValuePair<string, string>(" 10 (5900 XP)", "10" ),
            new KeyValuePair<string, string>(" 11 (7200 XP)", "11" ),
            new KeyValuePair<string, string>(" 12 (8400 XP)", "12" ),
            new KeyValuePair<string, string>(" 13 (10000 XP)", "13" ),
            new KeyValuePair<string, string>(" 14 (11500 XP)", "14" ),
            new KeyValuePair<string, string>(" 15 (13000 XP)", "15" ),
            new KeyValuePair<string, string>(" 16 (15000 XP)", "16" ),
            new KeyValuePair<string, string>(" 17 (18000 XP)", "17" ),
            new KeyValuePair<string, string>(" 18 (20000 XP)", "18" ),
            new KeyValuePair<string, string>(" 19 (22000 XP)", "19" ),
            new KeyValuePair<string, string>(" 20 (25000 XP)", "20" ),
            new KeyValuePair<string, string>(" 21 (33000 XP)", "21" ),
            new KeyValuePair<string, string>(" 22 (41000 XP)", "22" ),
            new KeyValuePair<string, string>(" 23 (50000 XP)", "23" ),
            new KeyValuePair<string, string>(" 24 (62000 XP)", "24" ),
            new KeyValuePair<string, string>(" 30 (155000 XP)", "30" ),
        };

        public override List<KeyValuePair<string, string>> Sizes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("Tiny", "Tiny"),
            new KeyValuePair<string, string>("Small", "Small"),
            new KeyValuePair<string, string>("Medium", "Medium"),
            new KeyValuePair<string, string>("Large", "Large"),
            new KeyValuePair<string, string>("Huge", "Huge"),
            new KeyValuePair<string, string>("Gargantuan", "Gargantuan"),
        };

        public override List<KeyValuePair<string, string>> Legendaries { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("si", "Si"),
            new KeyValuePair<string, string>("no", "Non"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            //new KeyValuePair<string, string>("Monster Manual", "MM"),
            //new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            //new KeyValuePair<string, string>("supno", "AL, AideDD"),
        };
    }

    public class HDMonsterFilterViewModel : MonsterFilterViewModel
    {
        public override List<KeyValuePair<string, string>> Categories { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("M", "Monstres" ),
            new KeyValuePair<string, string>("A", "Animaux" ),
            new KeyValuePair<string, string>("P", "PNJ" ),
        };

        public override List<KeyValuePair<string, string>> Types { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous" ),
            new KeyValuePair<string, string>("Humanoïde", "Humanoïde"),
            new KeyValuePair<string, string>("Aberration", "Aberration"),
            new KeyValuePair<string, string>("Bête", "Bête"),
            new KeyValuePair<string, string>("Céleste", "Céleste"),
            new KeyValuePair<string, string>("Créature artificielle", "Créature artificielle"),
            new KeyValuePair<string, string>("Créature monstrueuse", "Créature monstrueuse"),
            new KeyValuePair<string, string>("Dragon", "Dragon"),
            new KeyValuePair<string, string>("Élémentaire", "Élémentaire"),
            new KeyValuePair<string, string>("Fée", "Fée"),
            new KeyValuePair<string, string>("Fiélon", "Fiélon"),
            new KeyValuePair<string, string>("Géant", "Géant"),
            new KeyValuePair<string, string>("Mort-vivant", "Mort-vivant"),
            new KeyValuePair<string, string>("Plante", "Plante"),
            new KeyValuePair<string, string>("Vase", "Vase"),
        };

        public override List<KeyValuePair<string, string>> Powers { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>(" 0 (0 PX)", "0" ),
            new KeyValuePair<string, string>(" 1/8 (25 PX)", "1/8" ),
            new KeyValuePair<string, string>(" 1/4 (50 PX)", "1/4" ),
            new KeyValuePair<string, string>(" 1/2 (100 PX)", "1/2" ),
            new KeyValuePair<string, string>(" 1 (200 PX)", "1" ),
            new KeyValuePair<string, string>(" 2 (450 PX)", "2" ),
            new KeyValuePair<string, string>(" 3 (700 PX)", "3" ),
            new KeyValuePair<string, string>(" 4 (1100 PX)", "4" ),
            new KeyValuePair<string, string>(" 5 (1800 PX)", "5" ),
            new KeyValuePair<string, string>(" 6 (2300 PX)", "6" ),
            new KeyValuePair<string, string>(" 7 (2900 PX)", "7" ),
            new KeyValuePair<string, string>(" 8 (3900 PX)", "8" ),
            new KeyValuePair<string, string>(" 9 (5000 PX)", "9" ),
            new KeyValuePair<string, string>(" 10 (5900 PX)", "10" ),
            new KeyValuePair<string, string>(" 11 (7200 PX)", "11" ),
            new KeyValuePair<string, string>(" 12 (8400 PX)", "12" ),
            new KeyValuePair<string, string>(" 13 (10000 PX)", "13" ),
            new KeyValuePair<string, string>(" 14 (11500 PX)", "14" ),
            new KeyValuePair<string, string>(" 15 (13000 PX)", "15" ),
            new KeyValuePair<string, string>(" 16 (15000 PX)", "16" ),
            new KeyValuePair<string, string>(" 17 (18000 PX)", "17" ),
            new KeyValuePair<string, string>(" 18 (20000 PX)", "18" ),
            new KeyValuePair<string, string>(" 19 (22000 PX)", "19" ),
            new KeyValuePair<string, string>(" 20 (25000 PX)", "20" ),
            new KeyValuePair<string, string>(" 21 (33000 PX)", "21" ),
            new KeyValuePair<string, string>(" 22 (41000 PX)", "22" ),
            new KeyValuePair<string, string>(" 23 (50000 PX)", "23" ),
            new KeyValuePair<string, string>(" 24 (62000 PX)", "24" ),
            new KeyValuePair<string, string>(" 30 (155000 PX)", "30" ),
        };

        public override List<KeyValuePair<string, string>> Sizes { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("TP", "Très petite"),
            new KeyValuePair<string, string>("P", "Petite"),
            new KeyValuePair<string, string>("M", "Moyenne"),
            new KeyValuePair<string, string>("G", "Grande"),
            new KeyValuePair<string, string>("TG", "Très grande"),
            new KeyValuePair<string, string>("Gig", "Gigantesque"),
        };

        public override List<KeyValuePair<string, string>> Legendaries { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("si", "Si"),
            new KeyValuePair<string, string>("no", "Non"),
        };

        public override List<KeyValuePair<string, string>> Sources { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            //new KeyValuePair<string, string>("Monster Manual", "MM"),
            //new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            //new KeyValuePair<string, string>("supno", "AL, AideDD"),
        };
    }
    #endregion Monsters

    #region Equipments
    public abstract class EquipmentFilterViewModel : FilterViewModel
    {
        private IEnumerable<Filter> _Filters = null;
        public override IEnumerable<Filter> Filters
        {
            get
            {
                if (_Filters == null)
                {
                    _Filters = new List<Filter>()
                    {
                        new Filter() { Key = FilterKeys.Type, Name = "Type", KeyValues = Types, _Index = 0 },
                        new Filter() { Key = FilterKeys.MinPrice, Name = "Prix Minimum", KeyValues = Prices, _Index = 0 },
                        new Filter() { Key = FilterKeys.MaxPrice, Name = "Prix Maximum", KeyValues = Prices, _Index = 9 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public override async Task<IEnumerable<Item>> FilterItems(IEnumerable<Item> items, CancellationToken token = default)
        {
            return await Task.Run(() =>
            {
                var priceComparer = new PriceComparer();
                var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
                var minPrice = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinPrice).SelectedKey ?? "0 pc";
                var maxPrice = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxPrice).SelectedKey ?? "1 000 000 po";
                token.ThrowIfCancellationRequested();
                return items.Where(item =>
                {
                    var equipment = item as Equipment;
                    return equipment.Type.ToLower().Contains(type.ToLower()) &&
                        priceComparer.Compare(equipment.Price, minPrice) >= 0 &&
                        priceComparer.Compare(equipment.Price, maxPrice) <= 0 &&
                        (
                            (Helpers.RemoveDiacritics(equipment.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(equipment.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        );
                }).OrderBy(eq => eq.Name)
                            .AsEnumerable();
            }, token);

        }

        public abstract List<KeyValuePair<string, string>> Types { get; }

        public abstract List<KeyValuePair<string, string>> Prices { get; }
    }

    public class VFEquipmentFilterViewModel : EquipmentFilterViewModel
    {

        public override List<KeyValuePair<string, string>> Types { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous" ),
            new KeyValuePair<string, string>("Armure", "Armure" ),
            new KeyValuePair<string, string>("Armure légère", "  Armure légère" ),
            new KeyValuePair<string, string>("Armure intermédiaire", "  Armure intermédiaire" ),
            new KeyValuePair<string, string>("Armure lourde", "  Armure lourde" ),
            new KeyValuePair<string, string>("Bouclier", "  Bouclier" ),
            new KeyValuePair<string, string>("Arme", "Arme" ),
            new KeyValuePair<string, string>("Arme de corps-à-corps", "  Arme de corps-à-corps" ),
            new KeyValuePair<string, string>("Arme à distance", "  Arme à distance" ),
            new KeyValuePair<string, string>("Équipement d'aventurier", "Équipement d'aventurier" ),
            new KeyValuePair<string, string>("Focaliseur arcanique", "  Focaliseur arcanique" ),
            new KeyValuePair<string, string>("Focaliseur druidique", "  Focaliseur druidique" ),
            new KeyValuePair<string, string>("Munitions", "  Munitions" ),
            new KeyValuePair<string, string>("Symbole sacré", "  Symbole sacré" ),
            new KeyValuePair<string, string>("Vêtements", "  Vêtements" ),
            new KeyValuePair<string, string>("Outil", "Outil" ),
            new KeyValuePair<string, string>("Instrument de musique", "  Instrument de musique" ),
            new KeyValuePair<string, string>("Jeu", "  Jeu" ),
            new KeyValuePair<string, string>("Outil d'artisan", "  Outil d'artisan" ),
            new KeyValuePair<string, string>("Monture", "Monture" ),
            new KeyValuePair<string, string>("Équipement, sellerie et véhicules à traction", "Équipement, sellerie et véhicules à traction" ),
            new KeyValuePair<string, string>("Bateau", "Bateau" ),
            new KeyValuePair<string, string>("Marchandise", "Marchandise" ),
            new KeyValuePair<string, string>("Nourriture, boisson et logement", "Nourriture, boisson et logement" ),
        };

        public override List<KeyValuePair<string, string>> Prices { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("0 pc", "0 pc" ),
            new KeyValuePair<string, string>("1 pc", "1 pc" ),
            new KeyValuePair<string, string>("1 pa", "1 pa" ),
            new KeyValuePair<string, string>("1 po", "1 po" ),
            new KeyValuePair<string, string>("10 po", "10 po" ),
            new KeyValuePair<string, string>("100 po", "100 po" ),
            new KeyValuePair<string, string>("1 000 po", "1 000 po" ),
            new KeyValuePair<string, string>("10 000 po", "10 000 po" ),
            new KeyValuePair<string, string>("100 000 po", "100 000 po" ),
            new KeyValuePair<string, string>("1 000 000 po", "1 000 000 po" ),
        };
    }

    #endregion Equipments
    }
