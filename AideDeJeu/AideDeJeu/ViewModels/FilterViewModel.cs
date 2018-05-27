using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AideDeJeu.ViewModels
{
    public abstract class FilterViewModel : BaseViewModel
    {
        public ICommand LoadItemsCommand { get; protected set; }
    }

    #region Spells
    public abstract class SpellFilterViewModel : FilterViewModel
    {
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
    public class MonsterFilterViewModel : FilterViewModel
    {

    }
    #endregion Monsters
}
