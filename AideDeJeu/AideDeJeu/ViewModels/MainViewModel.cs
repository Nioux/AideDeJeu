using AideDeJeuLib;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public enum ItemType
        {
            Spell,
            Monster,
        }
        public SpellsViewModel Spells { get; private set; }
        public MonstersViewModel Monsters { get; private set; }

        private ItemType _ItemsType = ItemType.Spell;
        public ItemType ItemsType
        {
            get
            {
                return _ItemsType;
            }
            set
            {
                CurrentViewModel.SearchText = "";
                SetProperty(ref _ItemsType, value);
                CurrentViewModel.SearchText = "";
                OnPropertyChanged(nameof(CurrentViewModel));
                LoadItemsCommand.Execute(null);
            }
        }

        public ItemsViewModel CurrentViewModel
        {
            get
            {
                if (ItemsType == ItemType.Spell)
                {
                    return Spells;
                }
                if (ItemsType == ItemType.Monster)
                {
                    return Monsters;
                }
                return null;
            }
        }
        public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();

        public Command LoadItemsCommand { get; private set; }
        public Command<Item> GotoItemCommand { get; private set; }

        public Command SwitchToSpells { get; private set; }
        public Command SwitchToMonsters { get; private set; }
        public Command AboutCommand { get; private set; }
        public Command<string> SearchCommand { get; private set; }

        public MainViewModel(INavigator navigator)
        {
            Spells = new SpellsViewModel(navigator, Items);
            Monsters = new MonstersViewModel(navigator, Items);
            LoadItemsCommand = new Command(async () => await CurrentViewModel.ExecuteLoadItemsCommandAsync());
            GotoItemCommand = new Command<Item>(async (item) => await CurrentViewModel.ExecuteGotoItemCommandAsync(item));
            SwitchToSpells = new Command(() => ItemsType = ItemType.Spell);
            SwitchToMonsters = new Command(() => ItemsType = ItemType.Monster);
            AboutCommand = new Command(async() => await navigator.GotoAboutPageAsync());
            SearchCommand = new Command<string>((text) => CurrentViewModel.SearchText = text);
        }
    }
}