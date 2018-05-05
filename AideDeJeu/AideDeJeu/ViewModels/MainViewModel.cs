using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AideDeJeuLib.Spells;
using System.Collections.Generic;
using AideDeJeuLib;

namespace AideDeJeu.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public enum ItemType
        {
            Spell,
            Monster,
        }
        public SpellsViewModel Spells { get; set; } = new SpellsViewModel();
        public MonstersViewModel Monsters { get; set; } = new MonstersViewModel();

        private ItemType _ItemsType = ItemType.Spell;
        public ItemType ItemsType
        {
            get
            {
                return _ItemsType;
            }
            set
            {
                SetProperty(ref _ItemsType, value);
                OnPropertyChanged(nameof(Items));
                LoadItemsCommand.Execute(null);
            }
        }

        public ObservableCollection<Item> Items
        {
            get
            {
                if(ItemsType == ItemType.Spell)
                {
                    return Spells.Items;
                }
                if (ItemsType == ItemType.Monster)
                {
                    return Monsters.Items;
                }
                return null;
            }
        }

        public Command LoadItemsCommand { get; set; }

        public Command SwitchToSpells { get; set; }
        public Command SwitchToMonsters { get; set; }
        public Command AboutCommand { get; set; }

        public MainViewModel(INavigation navigation)
        {
            //Title = "Browse";
            //Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SwitchToSpells = new Command(() => ItemsType = ItemType.Spell);
            SwitchToMonsters = new Command(() => ItemsType = ItemType.Monster);
            AboutCommand = new Command(async() => await navigation.PushAsync(new Views.AboutPage()));
        }

        async Task ExecuteLoadItemsCommand()
        {
            if(ItemsType == ItemType.Spell)
            {
                await Spells.ExecuteLoadItemsCommand();
            }
            else if (ItemsType == ItemType.Monster)
            {
                await Monsters.ExecuteLoadItemsCommand();
            }
        }
    }
}