using AideDeJeu.Tools;
using AideDeJeuLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AideDeJeu.ViewModels.Library
{
    public abstract class FilterViewModel : BaseViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public abstract Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken cancellationToken = default(CancellationToken));
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
        Terrain,
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
                        )).OrderBy(spell => Helpers.RemoveDiacritics(spell.Name)
                        ).ToList();
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
                        new Filter() { Key = FilterKeys.Terrain, Name = "Terrain", KeyValues = Terrains, _Index = 0 },
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
            List<KeyValuePair<string, string>> sources,
            List<KeyValuePair<string, string>> terrains)
        {
            this.Family = family;
            this.Types = types;
            this.Challenges = challenges;
            this.Sizes = sizes;
            this.Sources = sources;
            this.Terrains = terrains;
        }

        public List<KeyValuePair<string, string>> Categories { get; }

        public List<KeyValuePair<string, string>> Types { get; }

        public List<KeyValuePair<string, string>> Challenges { get; }

        public List<KeyValuePair<string, string>> Sizes { get; }

        public List<KeyValuePair<string, string>> Legendaries { get; }

        public List<KeyValuePair<string, string>> Sources { get; }
        public List<KeyValuePair<string, string>> Terrains { get; }

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
            var terrain = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Terrain).SelectedKey ?? "";
            token.ThrowIfCancellationRequested();

            try
            {
                await StoreViewModel.SemaphoreLibrary.WaitAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    return context.Monsters.Where(monster =>
                        MatchEquals(monster.Family, this.Family) &&
                        MatchContains(monster.Type, type) &&
                        MatchEquals(monster.Size, size) &&
                        MatchContains(monster.Terrain, terrain) &&
                        MatchRange(monster.Challenge, minChallenge, maxChallenge, challengeComparer) &&
                        MatchSearch(monster) 
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
    public class EquipmentFilterViewModel : FilterViewModel
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
                        new Filter() { Key = FilterKeys.MaxPrice, Name = "Prix Maximum", KeyValues = Prices, _Index = 0 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public EquipmentFilterViewModel(string family, List<KeyValuePair<string, string>> types, List<KeyValuePair<string, string>> prices)
        {
            this.Types = types;
            this.Prices = prices;
        }

        public string PriceConverter(string price)
        {
            if (price == "") return null;
            return price;
        }
        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
            var priceComparer = new PriceComparer();
            var type = Filters.SingleOrDefault(filter => filter.Key == FilterKeys.Type).SelectedKey ?? "";
            var minPrice = PriceConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MinPrice).SelectedKey) ?? "0 pc";
            var maxPrice = PriceConverter(Filters.SingleOrDefault(filter => filter.Key == FilterKeys.MaxPrice).SelectedKey) ?? "1 000 000 po";

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

        public List<KeyValuePair<string, string>> Types { get; }

        public List<KeyValuePair<string, string>> Prices { get; }
    }
    #endregion Equipments

    #region Magic Items
    public class MagicItemFilterViewModel : FilterViewModel
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
                        new Filter() { Key = FilterKeys.Rarity, Name = "Rareté", KeyValues = Rarities, _Index = 0 },
                        new Filter() { Key = FilterKeys.Attunement, Name = "Harmonisation", KeyValues = Attunements, _Index = 0 },
                        //new Filter() { Key = FilterKeys.MinPrice, Name = "Prix Minimum", KeyValues = Prices, _Index = 0 },
                        //new Filter() { Key = FilterKeys.MaxPrice, Name = "Prix Maximum", KeyValues = Prices, _Index = 9 },
                    };
                    RegisterFilters();
                }
                return _Filters;
            }
        }

        public MagicItemFilterViewModel(string family, List<KeyValuePair<string, string>> types, List<KeyValuePair<string, string>> rarities, List<KeyValuePair<string, string>> attunements)
        {
            this.Types = types;
            this.Rarities = rarities;
            this.Attunements = attunements;
        }

        public override async Task<IEnumerable<Item>> GetFilteredItemsAsync(CancellationToken token = default)
        {
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

        public List<KeyValuePair<string, string>> Types { get; }

        public List<KeyValuePair<string, string>> Rarities { get; }

        public List<KeyValuePair<string, string>> Attunements { get; }
    }
    #endregion Equipments
}
