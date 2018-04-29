using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using System.Collections.Generic;
using AideDeJeuLib.Monsters;

namespace AideDeJeu.ViewModels
{
    public class MonstersViewModel : BaseViewModel
    {
        public ObservableCollection<Monster> Items { get; set; }

        public List<KeyValuePair<string, string>> Classes { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes" ),
            new KeyValuePair<string, string>("b", "Barde" ),
            new KeyValuePair<string, string>("c", "Clerc" ),
            new KeyValuePair<string, string>("d", "Druide" ),
            new KeyValuePair<string, string>("s", "Ensorceleur" ),
            new KeyValuePair<string, string>("w", "Magicien" ),
            new KeyValuePair<string, string>("p", "Paladin" ),
            new KeyValuePair<string, string>("r", "Rôdeur" ),
            new KeyValuePair<string, string>("k", "Sorcier" ),
        };

        public List<KeyValuePair<int, string>> Niveaux { get; set; } = new List<KeyValuePair<int, string>>()
        {
            new KeyValuePair<int, string>(0, "Sorts mineurs"),
            new KeyValuePair<int, string>(1, "Niveau 1"),
            new KeyValuePair<int, string>(2, "Niveau 2"),
            new KeyValuePair<int, string>(3, "Niveau 3"),
            new KeyValuePair<int, string>(4, "Niveau 4"),
            new KeyValuePair<int, string>(5, "Niveau 5"),
            new KeyValuePair<int, string>(6, "Niveau 6"),
            new KeyValuePair<int, string>(7, "Niveau 7"),
            new KeyValuePair<int, string>(8, "Niveau 8"),
            new KeyValuePair<int, string>(9, "Niveau 9"),
        };

        public List<KeyValuePair<string, string>> Ecoles { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("abjuration", "Abjuration"),
            new KeyValuePair<string, string>("divination", "Divination"),
            new KeyValuePair<string, string>("enchantement", "Enchantement"),
            new KeyValuePair<string, string>("evocation", "Évocation"),
            new KeyValuePair<string, string>("illusion", "Illusion"),
            new KeyValuePair<string, string>("invocation", "Invocation"),
            new KeyValuePair<string, string>("necromancie", "Nécromancie"),
            new KeyValuePair<string, string>("transmutation", "Transmutation"),
        };

        public List<KeyValuePair<string, string>> Rituels { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Tous"),
            new KeyValuePair<string, string>("1", "Rituel"),
        };

        public List<KeyValuePair<string, string>> Sources { get; set; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("", "Toutes"),
            new KeyValuePair<string, string>("srd", "SRD"),
            new KeyValuePair<string, string>("ph", "PHB"),
            new KeyValuePair<string, string>("sup", "SCAG, XGtE"),
        };

        private int _Classe = 0;
        public int Classe
        {
            get
            {
                return _Classe;
            }
            set
            {
                SetProperty(ref _Classe, value);
                LoadItemsCommand.Execute(null);
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
                SetProperty(ref _NiveauMin, value);
                if (_NiveauMax < _NiveauMin)
                {
                    SetProperty(ref _NiveauMax, value, nameof(NiveauMax));
                }
                LoadItemsCommand.Execute(null);
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
                SetProperty(ref _NiveauMax, value);
                if (_NiveauMax < _NiveauMin)
                {
                    SetProperty(ref _NiveauMin, value, nameof(NiveauMin));
                }
                LoadItemsCommand.Execute(null);
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
                SetProperty(ref _Ecole, value);
                LoadItemsCommand.Execute(null);
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
                SetProperty(ref _Rituel, value);
                LoadItemsCommand.Execute(null);
            }
        }
        private int _Source = 0;
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


        public Command LoadItemsCommand { get; set; }

        public MonstersViewModel()
        {
            //Title = "Browse";
            Items = new ObservableCollection<Monster>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Spell>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as Item;
            //    Items.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //<option value="b">Barde</option>
                //<option value="c">Clerc</option>
                //<option value="d">Druide</option>
                //<option value="s">Ensorceleur</option>
                //<option value="w">Magicien</option>
                //<option value="p">Paladin</option>
                //<option value="r">Rôdeur</option>
                //<option value="k">Sorcier</option>

                Items.Clear();
                var items = await new MonstersScrappers().GetMonsters();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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
    }
}