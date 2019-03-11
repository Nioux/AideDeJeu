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
        public abstract Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken cancellationToken = default);
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

        public bool MatchSearch(Item item)
        {
            return
                    Helpers.RemoveDiacritics(item.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()) ||
                    Helpers.RemoveDiacritics(item.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower());
        }

        public bool MatchContains(string itemValue, string filterValue)
        {
            return string.IsNullOrEmpty(filterValue) || (itemValue != null && itemValue.ToLower().Contains(filterValue.ToLower()));
        }

        public bool MatchEquals(string itemValue, string filterValue)
        {
            return string.IsNullOrEmpty(filterValue) || (itemValue != null && itemValue.ToLower().Equals(filterValue.ToLower()));
        }

        public bool MatchRange(string itemValue, string filterMinValue, string filterMaxValue, IComparer<string> comparer)
        {
            return
                (string.IsNullOrEmpty(filterMinValue) || comparer.Compare(itemValue, filterMinValue) >= 0) &&
                (string.IsNullOrEmpty(filterMaxValue) || comparer.Compare(itemValue, filterMaxValue) <= 0);
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

        public void FilterWith(string key, string val)
        {
            var filter = Filters.FirstOrDefault(f => f.Key.ToString().ToLower() == key.ToLower());
            if (filter != null)
            {
                filter.Index = filter.KeyValues.FindIndex(kv => kv.Value.Simplify().Contains(val));
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
        MinChallenge,
        MaxChallenge,
        Size,
        Legendary,
        MinPrice,
        MaxPrice,
        MinWeight,
        MaxWeight,
        Rarity,
        Attunement,
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

    //public class SearchFilterViewModel : FilterViewModel
    //{
    //    private IEnumerable<Filter> _Filters = null;
    //    public override IEnumerable<Filter> Filters
    //    {
    //        get
    //        {
    //            if (_Filters == null)
    //            {
    //                _Filters = new List<Filter>()
    //                {
    //                };
    //                RegisterFilters();
    //            }
    //            return _Filters;
    //        }
    //    }


    //    public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
    //    {
    //        return await Task.Run(() =>
    //        {
    //            return items.Where(item =>
    //            {
    //                var spell = item;
    //                return
    //                    (
    //                        (Helpers.RemoveDiacritics(spell.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
    //                        (Helpers.RemoveDiacritics(spell.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
    //                    );
    //            }).AsEnumerable();
    //        }, token);

    //    }

    //}


    #region Spells
    public class SpellFilterViewModel : FilterViewModel
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
                        new Filter() { Key = FilterKeys.MinLevel, Name = "Niveau Minimum", KeyValues = Levels, _Index = 0 },
                        new Filter() { Key = FilterKeys.MaxLevel, Name = "Niveau Maximum", KeyValues = Levels, _Index = 0 },
                        new Filter() { Key = FilterKeys.School, Name = "École", KeyValues = Schools, _Index = 0 },
                        new Filter() { Key = FilterKeys.Ritual, Name = "Rituel", KeyValues = Rituals, _Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, _Index = 0 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public string Family { get; set; }

        public SpellFilterViewModel(
            string family,
            List<KeyValuePair<string, string>> classes,
            List<KeyValuePair<string, string>> levels,
            List<KeyValuePair<string, string>> schools,
            List<KeyValuePair<string, string>> rituals,
            List<KeyValuePair<string, string>> sources)
        {
            this.Family = family;
            this.Classes = classes;
            this.Levels = levels;
            this.Schools = schools;
            this.Rituals = rituals;
            this.Sources = sources;
        }

        public string LevelConverter(string level)
        {
            if (level == "") return null;
            if (level.StartsWith("Niveau ")) return level.Substring(7);
            if (level.StartsWith("Level ")) return level.Substring(6);
            return "0";
        }
        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
            var levelComparer = new LevelComparer();
            var classe = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Class).SelectedKey ?? "";
            var levelMin = LevelConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinLevel).SelectedKey) ?? "0";
            var levelMax = LevelConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxLevel).SelectedKey) ?? "9";
            var school = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.School).SelectedKey ?? "";
            var ritual = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Ritual).SelectedKey ?? "";
            var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey ?? "";
            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    return context.Spells.Where(spell =>
                        spell.Family == this.Family &&
                        levelComparer.Compare(spell.Level, levelMin) >= 0 &&
                        levelComparer.Compare(spell.Level, levelMax) <= 0 &&
                        spell.Type.ToLower().Contains(school.ToLower()) &&
                        (spell.Source != null && spell.Source.Contains(source)) &&
                        (spell.Classes != null && spell.Classes.Contains(classe)) &&
                        (string.IsNullOrEmpty(ritual) || (spell.Ritual != null && spell.Ritual.Contains(ritual))) &&
                        (
                            (Helpers.RemoveDiacritics(spell.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(spell.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        )).OrderBy(spell => Helpers.RemoveDiacritics(spell.Name)).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
            finally
            {
                StoreViewModel.SemaphoreLibrary.Release();
            }
        }
        
        public List<KeyValuePair<string, string>> Classes { get; }

        public List<KeyValuePair<string, string>> Levels { get; }

        public List<KeyValuePair<string, string>> Schools { get; }

        public List<KeyValuePair<string, string>> Rituals { get; }

        public List<KeyValuePair<string, string>> Sources { get; }

    }
    
    #endregion Spells

    #region Monsters
    public class MonsterFilterViewModel : FilterViewModel
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
                        new Filter() { Key = FilterKeys.MinChallenge, Name = "FP Minimum", KeyValues = Challenges, _Index = 0 },
                        new Filter() { Key = FilterKeys.MaxChallenge, Name = "FP Maximum", KeyValues = Challenges, _Index = 0 },
                        new Filter() { Key = FilterKeys.Size, Name = "Taille", KeyValues = Sizes, _Index = 0 },
                        //new Filter() { Key = FilterKeys.Legendary, Name = "Légendaire", KeyValues = Legendaries, _Index = 0 },
                        new Filter() { Key = FilterKeys.Source, Name = "Source", KeyValues = Sources, _Index = 0 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public string Family { get; set; }

        public MonsterFilterViewModel(
            string family,
            List<KeyValuePair<string, string>> types,
            List<KeyValuePair<string, string>> challenges,
            List<KeyValuePair<string, string>> sizes,
            List<KeyValuePair<string, string>> sources)
        {
            this.Family = family;
            this.Types = types;
            this.Challenges = challenges;
            this.Sizes = sizes;
            this.Sources = sources;
        }

        public List<KeyValuePair<string, string>> Categories { get; }

        public List<KeyValuePair<string, string>> Types { get; }

        public List<KeyValuePair<string, string>> Challenges { get; }

        public List<KeyValuePair<string, string>> Sizes { get; }

        public List<KeyValuePair<string, string>> Legendaries { get; }

        public List<KeyValuePair<string, string>> Sources { get; }

        public string ChallengeConverter(string challenge)
        {
            if (challenge == "") return null;
            return challenge;
        }
        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
            var challengeComparer = new ChallengeComparer();

            var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
            var minChallenge = ChallengeConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinChallenge).SelectedKey) ?? "0 (0 PX)";
            var maxChallenge = ChallengeConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxChallenge).SelectedKey) ?? "30 (155000 PX)";
            var size = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Size).SelectedKey ?? "";
            var source = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Source).SelectedKey ?? "";
            token.ThrowIfCancellationRequested();

            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    return context.Monsters.Where(monster =>
                        monster != null &&
                        monster.Family == this.Family &&
                        monster.Type.Contains(type) &&
                        (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                        (string.IsNullOrEmpty(source) || (monster.Source != null && monster.Source.Contains(source))) &&
                        challengeComparer.Compare(monster.Challenge, minChallenge) >= 0 &&
                        challengeComparer.Compare(monster.Challenge, maxChallenge) <= 0 &&
                        (
                            (Helpers.RemoveDiacritics(monster.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(monster.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        )
                    ).OrderBy(monster => Helpers.RemoveDiacritics(monster.Name)).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
            finally
            {
                StoreViewModel.SemaphoreLibrary.Release();
            }
        }

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

        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
            var priceComparer = new PriceComparer();
            var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
            var minPrice = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinPrice).SelectedKey ?? "0 pc";
            var maxPrice = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxPrice).SelectedKey ?? "1 000 000 po";

            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    return context.Equipments.Where(equipment =>
                        equipment.Type.ToLower().Contains(type.ToLower()) &&
                        priceComparer.Compare(equipment.Price, minPrice) >= 0 &&
                        priceComparer.Compare(equipment.Price, maxPrice) <= 0 &&
                        (
                            (Helpers.RemoveDiacritics(equipment.Name).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower())) ||
                            (Helpers.RemoveDiacritics(equipment.AltNameText ?? string.Empty).ToLower().Contains(Helpers.RemoveDiacritics(SearchText ?? string.Empty).ToLower()))
                        )
                    ).OrderBy(eq => eq.Name)
                            .ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
            finally
            {
                StoreViewModel.SemaphoreLibrary.Release();
            }
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
            new KeyValuePair<string, string>("Armure légère", "    Armure légère" ),
            new KeyValuePair<string, string>("Armure intermédiaire", "    Armure intermédiaire" ),
            new KeyValuePair<string, string>("Armure lourde", "    Armure lourde" ),
            new KeyValuePair<string, string>("Bouclier", "    Bouclier" ),
            new KeyValuePair<string, string>("Arme", "Arme" ),
            new KeyValuePair<string, string>("Arme de corps-à-corps", "    Arme de corps-à-corps" ),
            new KeyValuePair<string, string>("Arme à distance", "    Arme à distance" ),
            new KeyValuePair<string, string>("Équipement d'aventurier", "Équipement d'aventurier" ),
            new KeyValuePair<string, string>("Focaliseur arcanique", "    Focaliseur arcanique" ),
            new KeyValuePair<string, string>("Focaliseur druidique", "    Focaliseur druidique" ),
            new KeyValuePair<string, string>("Munitions", "    Munitions" ),
            new KeyValuePair<string, string>("Symbole sacré", "    Symbole sacré" ),
            new KeyValuePair<string, string>("Vêtements", "    Vêtements" ),
            new KeyValuePair<string, string>("Outil", "Outil" ),
            new KeyValuePair<string, string>("Instrument de musique", "    Instrument de musique" ),
            new KeyValuePair<string, string>("Jeu", "    Jeu" ),
            new KeyValuePair<string, string>("Outil d'artisan", "    Outil d'artisan" ),
            new KeyValuePair<string, string>("Monture", "Monture" ),
            new KeyValuePair<string, string>("Équipement, sellerie et véhicules à traction", "Équipement, sellerie et véhicules à traction" ),
            new KeyValuePair<string, string>("Bateau", "Bateau" ),
            new KeyValuePair<string, string>("Marchandise", "Marchandise" ),
            new KeyValuePair<string, string>("Service", "Service" ),
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

    #region Magic Items
    public abstract class MagicItemFilterViewModel : FilterViewModel
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
                        new Filter() { Key = FilterKeys.Rarity, Name = "Rareté", KeyValues = Rarity, _Index = 0 },
                        new Filter() { Key = FilterKeys.Attunement, Name = "Harmonisation", KeyValues = Attunement, _Index = 0 },
                        //new Filter() { Key = FilterKeys.MinPrice, Name = "Prix Minimum", KeyValues = Prices, _Index = 0 },
                        //new Filter() { Key = FilterKeys.MaxPrice, Name = "Prix Maximum", KeyValues = Prices, _Index = 9 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
            var priceComparer = new PriceComparer();
            var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
            var rarity = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Rarity).SelectedKey ?? "";
            var attunement = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Attunement).SelectedKey ?? "";

            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    return context.MagicItems.Where(magicitem =>
                        MatchContains(magicitem.Type, type) &&
                        MatchContains(magicitem.Rarity, rarity) &&
                        MatchContains(magicitem.Attunement, attunement) &&
                        MatchSearch(magicitem)
                    ).OrderBy(eq => eq.Name).ToList();
                }
            }
            catch
            {
                return new List<Item>();
            }
            finally
            {
                StoreViewModel.SemaphoreLibrary.Release();
            }
        }

        public abstract List<KeyValuePair<string, string>> Types { get; }

        public abstract List<KeyValuePair<string, string>> Rarity { get; }

        public abstract List<KeyValuePair<string, string>> Attunement { get; }
    }

    public class VFMagicItemFilterViewModel : MagicItemFilterViewModel
    {

        public override List<KeyValuePair<string, string>> Types { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous" ),
            new KeyValuePair<string, string>("Anneau", "Anneau" ),
            new KeyValuePair<string, string>("Arme", "Arme" ),
            new KeyValuePair<string, string>("Armure", "Armure" ),
            new KeyValuePair<string, string>("Baguette", "Baguette" ),
            new KeyValuePair<string, string>("Bâton", "Bâton" ),
            new KeyValuePair<string, string>("Objet merveilleux", "Objet merveilleux" ),
            new KeyValuePair<string, string>("Parchemin", "Parchemin" ),
            new KeyValuePair<string, string>("Potion", "Potion" ),
            new KeyValuePair<string, string>("Sceptre", "Sceptre" ),
        };

        public override List<KeyValuePair<string, string>> Rarity { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("peu courant", "Peu courant" ),
            new KeyValuePair<string, string>("rare", "Rare" ),
            new KeyValuePair<string, string>("très rare", "Très rare" ),
            new KeyValuePair<string, string>("légendaire", "Légendaire" ),
            new KeyValuePair<string, string>("rareté variable", "Rareté variable" ),
        };

        public override List<KeyValuePair<string, string>> Attunement { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tout" ),
            new KeyValuePair<string, string>("requise", "Requise" ),
            new KeyValuePair<string, string>("lanceur de sorts", "Lanceur de sorts" ),
            new KeyValuePair<string, string>("barde", "  Barde" ),
            new KeyValuePair<string, string>("clerc", "  Clerc" ),
            new KeyValuePair<string, string>("druide", "  Druide" ),
            new KeyValuePair<string, string>("ensorceleur", "  Ensorceleur" ),
            new KeyValuePair<string, string>("magicien", "  Magicien" ),
            new KeyValuePair<string, string>("sorcier", "  Sorcier" ),
            new KeyValuePair<string, string>("paladin", "  Paladin" ),
            new KeyValuePair<string, string>("alignement bon", "Alignement bon" ),
            new KeyValuePair<string, string>("alignement mauvais", "Alignement mauvais" ),
            new KeyValuePair<string, string>("nain", "Nain" ),
        };
    }

    #endregion Equipments
}
