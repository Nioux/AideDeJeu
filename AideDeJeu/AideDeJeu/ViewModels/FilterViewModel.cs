using AideDeJeu.Tools;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AideDeJeu.ViewModels
{
    public abstract class FilterViewModel : BaseViewModel
    {
        public ICommand LoadItemsCommand { get; protected set; }
        public abstract IEnumerable<Item> FilterItems(IEnumerable<Item> items);
        public abstract IEnumerable<Filter> Filters { get; }

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
    }

    public class Filter : BaseViewModel
    {
        public FilterKeys Key { get; set; }
        public string Name { get; set; }
        public List<KeyValuePair<string, string>> KeyValues { get; set; }
        private int _Index;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                if (_Index != value)
                {
                    SetProperty(ref _Index, value);
                    Main.LoadItemsCommand.Execute(null);
                }
            }
        }

        public string SelectedKey
        {
            get
            {
                return KeyValues[Index].Key;
            }
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
                if(_Filters == null)
                {
                    _Filters = new List<Filter>()
                    {
                        new Filter() { Key = FilterKeys.Class, Name = "Classe", KeyValues = Classes, Index = 0 },
                        new Filter() { Key = FilterKeys.MinLevel, Name = "Niveau Min", KeyValues = Niveaux, Index = 0 },
                        new Filter() { Key = FilterKeys.MaxLevel, Name = "Niveau Max", KeyValues = Niveaux, Index = 9 },
                        new Filter() { Key = FilterKeys.School, Name = "Ecole", KeyValues = Ecoles, Index = 0 },
                        new Filter() { Key = FilterKeys.Ritual, Name = "Rituel", KeyValues = Rituels, Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, Index = 0 },
                    };
                }
                return _Filters;
            }
        }


        public override IEnumerable<Item> FilterItems(IEnumerable<Item> items)
        {
            var classe = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Class).SelectedKey;
            var niveauMin = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinLevel).SelectedKey;
            var niveauMax = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxLevel).SelectedKey;
            var ecole = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.School).SelectedKey;
            var rituel = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Ritual).SelectedKey;
            var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey;

            return items
                    .Where(item =>
                    {
                        var spell = item as Spell;
                        return (int.Parse(spell.Level) >= int.Parse(niveauMin)) &&
                            (int.Parse(spell.Level) <= int.Parse(niveauMax)) &&
                            spell.Type.ToLower().Contains(ecole.ToLower()) &&
                            spell.Source.Contains(source) &&
                            spell.Source.Contains(classe) &&
                            spell.Type.Contains(rituel);
                    })
                    .OrderBy(spell => spell.NamePHB)
                    .ToList();
        }

        public abstract List<KeyValuePair<string, string>> Classes { get; }

        public abstract List<KeyValuePair<string, string>> Niveaux { get; }

        public abstract List<KeyValuePair<string, string>> Ecoles { get; }

        public abstract List<KeyValuePair<string, string>> Rituels { get; }

        public abstract List<KeyValuePair<string, string>> Sources { get; }


        private int _Classe = 0;
        public int Classe
        {
            get
            {
                return _Classe;
            }
            set
            {
                if (_Classe != value)
                {
                    SetProperty(ref _Classe, value);
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _NiveauMin = 0;
        public int NiveauMin
        {
            get
            {
                return _NiveauMin;
            }
            set
            {
                if (_NiveauMin != value)
                {
                    SetProperty(ref _NiveauMin, value);
                    if (_NiveauMax < _NiveauMin)
                    {
                        SetProperty(ref _NiveauMax, value, nameof(NiveauMax));
                    }
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _NiveauMax = 9;
        public int NiveauMax
        {
            get
            {
                return _NiveauMax;
            }
            set
            {
                if (_NiveauMax != value)
                {
                    SetProperty(ref _NiveauMax, value);
                    if (_NiveauMax < _NiveauMin)
                    {
                        SetProperty(ref _NiveauMin, value, nameof(NiveauMin));
                    }
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _Ecole = 0;
        public int Ecole
        {
            get
            {
                return _Ecole;
            }
            set
            {
                if (_Ecole != value)
                {
                    SetProperty(ref _Ecole, value);
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _Rituel = 0;
        public int Rituel
        {
            get
            {
                return _Rituel;
            }
            set
            {
                if (_Rituel != value)
                {
                    SetProperty(ref _Rituel, value);
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _Source = 1;
        public int Source
        {
            get
            {
                return _Source;
            }
            set
            {
                if (_Source != value)
                {
                    SetProperty(ref _Source, value);
                    LoadItemsCommand.Execute(null);
                }
            }
        }

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
            new KeyValuePair<string, string>("Ensorceleur", "Ensorceleur" ),
            new KeyValuePair<string, string>("Wizard", "Wizard" ),
            new KeyValuePair<string, string>("Paladin", "Paladin" ),
            new KeyValuePair<string, string>("Rôdeur", "Rôdeur" ),
            new KeyValuePair<string, string>("Sorcier", "Sorcier" ),
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
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantement", "Enchantement"),
            new KeyValuePair<string, string>("évocation", "Evocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("invocation", "Invocation"),
            new KeyValuePair<string, string>("necromancie", "Necromancie"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public override List<KeyValuePair<string, string>> Rituels { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "All"),
            new KeyValuePair<string, string>("(rituel)", "Rituel"),
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
                        new Filter() { Key = FilterKeys.Category, Name = "Catégories", KeyValues = Categories, Index = 0 },
                        new Filter() { Key = FilterKeys.Type, Name = "Type", KeyValues = Types, Index = 0 },
                        new Filter() { Key = FilterKeys.MinPower, Name = "Challenge Min", KeyValues = Powers, Index = 0 },
                        new Filter() { Key = FilterKeys.MaxPower, Name = "Challenge Max", KeyValues = Powers, Index = 28 },
                        new Filter() { Key = FilterKeys.Size, Name = "Taille", KeyValues = Sizes, Index = 0 },
                        new Filter() { Key = FilterKeys.Legendary, Name = "Légendaire", KeyValues = Legendaries, Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, Index = 0 },
                    };
                }
                return _Filters;
            }
        }

        public override IEnumerable<Item> FilterItems(IEnumerable<Item> items)
        {
            var powerComparer = new PowerComparer();

            var category = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Category).SelectedKey;
            var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey;
            var minPower = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinPower).SelectedKey;
            var maxPower = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxPower).SelectedKey;
            var size = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Size).SelectedKey;
            var legendary = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Legendary).SelectedKey;
            var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey;

            return items.Where(item =>
                {
                    var monster = item as Monster;
                    return monster.Type.Contains(type) &&
                        (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                        monster.Source.Contains(source) &&
                        powerComparer.Compare(monster.Challenge, minPower) >= 0 &&
                        powerComparer.Compare(monster.Challenge, maxPower) <= 0;
                })
                .OrderBy(monster => monster.NamePHB)
                .ToList();
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
            new KeyValuePair<string, string>("Monster Manual", "MM"),
            new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            new KeyValuePair<string, string>("supno", "AL, AideDD"),
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
            new KeyValuePair<string, string>("Monster Manual", "MM"),
            new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            new KeyValuePair<string, string>("supno", "AL, AideDD"),
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
            new KeyValuePair<string, string>("Monster Manual", "MM"),
            new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            new KeyValuePair<string, string>("supno", "AL, AideDD"),
        };
    }
    #endregion Monsters
}
