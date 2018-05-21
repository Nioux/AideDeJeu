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
            new KeyValuePair<string, string>("(SRD)", "SRD"),
            new KeyValuePair<string, string>("Monster Manual", "MM"),
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
                if (_Category != value)
                {
                    SetProperty(ref _Category, value);
                    LoadItemsCommand.Execute(null);
                }
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
                if (_Type != value)
                {
                    SetProperty(ref _Type, value);
                    LoadItemsCommand.Execute(null);
                }
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
                if (_MinPower != value)
                {
                    SetProperty(ref _MinPower, value);
                    if (_MaxPower < _MinPower)
                    {
                        SetProperty(ref _MaxPower, value, nameof(MaxPower));
                    }
                    LoadItemsCommand.Execute(null);
                }
            }
        }
        private int _MaxPower = 28;
        public int MaxPower
        {
            get
            {
                return _MaxPower;
            }
            set
            {
                if (_MaxPower != value)
                {
                    SetProperty(ref _MaxPower, value);
                    if (_MaxPower < _MinPower)
                    {
                        SetProperty(ref _MinPower, value, nameof(MinPower));
                    }
                    LoadItemsCommand.Execute(null);
                }
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
                if (_Size != value)
                {
                    SetProperty(ref _Size, value);
                    LoadItemsCommand.Execute(null);
                }
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
                if (_Legendary != value)
                {
                    SetProperty(ref _Legendary, value);
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


        public MonstersViewModel(INavigator navigator, ObservableCollection<Item> items)
            : base(navigator, items)
        {
        }





        private string MonstersJson
        {
            get
            {
                var assembly = typeof(AboutViewModel).GetTypeInfo().Assembly;
                //var names = assembly.GetManifestResourceNames();
                using (var stream = assembly.GetManifestResourceStream("AideDeJeu.monsters.json"))
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        private IEnumerable<Monster> _Monsters = null;
        private IEnumerable<Monster> Monsters
        {
            get
            {
                if (_Monsters == null)
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Monster>));
                    MemoryStream stream = new MemoryStream();
                    var writer = new StreamWriter(stream);
                    writer.Write(MonstersJson);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);

                    _Monsters = serializer.ReadObject(stream) as IEnumerable<Monster>;
                }
                return _Monsters;
            }
        }

        public IEnumerable<Monster> GetMonsters(string category, string type, string minPower, string maxPower, string size, string legendary, string source)
        {
            var powerComparer = new PowerComparer();
            return Monsters.Where(monster =>
                            monster.Type.Contains(type) &&
                            (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                            monster.Source.Contains(source) &&
                            powerComparer.Compare(monster.Challenge, minPower) >= 0 &&
                            powerComparer.Compare(monster.Challenge, maxPower) <= 0
                            )
                        .OrderBy(monster => monster.NamePHB)
                        .ToList();
        }


        public override async Task ExecuteLoadItemsCommandAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AllItems.Clear();
                //var items = await new MonstersScrappers().GetMonsters(category: Categories[Category].Key, type: Types[Type].Key, minPower: Powers[MinPower].Key, maxPower: Powers[MaxPower].Key, size: Sizes[Size].Key, legendary:Legendaries[Legendary].Key, source: Sources[Source].Key);

                //ItemDatabaseHelper helper = new ItemDatabaseHelper();
                //var items = await helper.GetMonstersAsync(category: Categories[Category].Key, type: Types[Type].Key, minPower: Powers[MinPower].Key, maxPower: Powers[MaxPower].Key, size: Sizes[Size].Key, legendary: Legendaries[Legendary].Key, source: Sources[Source].Key);
                var items = GetMonsters(category: Categories[Category].Key, type: Types[Type].Key, minPower: Powers[MinPower].Key, maxPower: Powers[MaxPower].Key, size: Sizes[Size].Key, legendary: Legendaries[Legendary].Key, source: Sources[Source].Key);

                //var aitems = items.ToArray();
                //Array.Sort(aitems, new ItemComparer());
                foreach (var item in items)
                {
                    AllItems.Add(item);
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