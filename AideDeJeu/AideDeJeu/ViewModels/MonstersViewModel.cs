using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class MonstersViewModel : ItemsViewModel
    {
        public List<KeyValuePair<string, string>> Categories { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("M", "Monstres" ),
            new KeyValuePair<string, string>("A", "Animaux" ),
            new KeyValuePair<string, string>("P", "PNJ" ),
        };

        public List<KeyValuePair<string, string>> Types { get; set; } = new List<KeyValuePair<string, string>>()
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

        public List<KeyValuePair<string, string>> Powers { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("Z", "0" ),
            new KeyValuePair<string, string>(".12", "1/8" ),
            new KeyValuePair<string, string>(".25", "1/4" ),
            new KeyValuePair<string, string>(".5", "1/2" ),
            new KeyValuePair<string, string>("1", "1" ),
            new KeyValuePair<string, string>("2", "2" ),
            new KeyValuePair<string, string>("4", "4" ),
            new KeyValuePair<string, string>("6", "6" ),
            new KeyValuePair<string, string>("8", "8" ),
            new KeyValuePair<string, string>("10", "10" ),
            new KeyValuePair<string, string>("15", "15" ),
            new KeyValuePair<string, string>("20", "20" ),
            new KeyValuePair<string, string>("30", "30" ),
        };

        public List<KeyValuePair<string, string>> Sizes { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("TP", "Très petite"),
            new KeyValuePair<string, string>("P", "Petite"),
            new KeyValuePair<string, string>("M", "Moyenne"),
            new KeyValuePair<string, string>("G", "Grande"),
            new KeyValuePair<string, string>("TG", "Très grande"),
            new KeyValuePair<string, string>("Gig", "Gigantesque"),
        };

        public List<KeyValuePair<string, string>> Legendaries { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("si", "Si"),
            new KeyValuePair<string, string>("no", "Non"),
        };

        public List<KeyValuePair<string, string>> Sources { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("srd", "SRD"),
            new KeyValuePair<string, string>("mm", "MM"),
            new KeyValuePair<string, string>("sup", "VGtM, MToF"),
            new KeyValuePair<string, string>("supno", "AL, AideDD"),
        };

        private int _Category = 0;
        public int Category
        {
            get
            {
                return _Category;
            }
            set
            {
                SetProperty(ref _Category, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Type = 0;
        public int Type
        {
            get
            {
                return _Type;
            }
            set
            {
                SetProperty(ref _Type, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _MinPower = 0;
        public int MinPower
        {
            get
            {
                return _MinPower;
            }
            set
            {
                SetProperty(ref _MinPower, value);
                if (_MaxPower < _MinPower)
                {
                    SetProperty(ref _MaxPower, value, nameof(MaxPower));
                }
                LoadItemsCommand.Execute(null);
            }
        }
        private int _MaxPower = 11;
        public int MaxPower
        {
            get
            {
                return _MaxPower;
            }
            set
            {
                SetProperty(ref _MaxPower, value);
                if (_MaxPower < _MinPower)
                {
                    SetProperty(ref _MinPower, value, nameof(MinPower));
                }
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Size = 0;
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                SetProperty(ref _Size, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Legendary = 0;
        public int Legendary
        {
            get
            {
                return _Legendary;
            }
            set
            {
                SetProperty(ref _Legendary, value);
                LoadItemsCommand.Execute(null);
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
                SetProperty(ref _Source, value);
                LoadItemsCommand.Execute(null);
            }
        }


        public MonstersViewModel(INavigator navigator, ObservableCollection<Item> items)
            : base(navigator, items)
        {
        }

        public override async Task ExecuteLoadItemsCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AllItems.Clear();
                var monsters = await new MonstersScrappers().GetMonsters(category: Categories[Category].Key, type: Types[Type].Key, minPower: Powers[MinPower].Key, maxPower: Powers[MaxPower].Key, size: Sizes[Size].Key, legendary:Legendaries[Legendary].Key, source: Sources[Source].Key);
                foreach (var monster in monsters)
                {
                    AllItems.Add(monster);
                }
                FilterItems();
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
            await Navigator.GotoMonsterDetailPageAsync(item as Monster);
        }

    }
}