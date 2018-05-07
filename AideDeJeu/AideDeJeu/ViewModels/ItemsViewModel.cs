using AideDeJeuLib;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public abstract class ItemsViewModel : BaseViewModel
    {
        public ItemsViewModel(INavigator navigator, ObservableCollection<Item> items)
        {
            Navigator = navigator;
            Items = items;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
        }
        protected ObservableCollection<Item> AllItems { get; set; } = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items { get; protected set; }
        public ICommand LoadItemsCommand { get; protected set; }
        public abstract Task ExecuteLoadItemsCommandAsync();
        public abstract Task ExecuteGotoItemCommandAsync(Item item);
        protected INavigator Navigator { get; set; }

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
                FilterItems();
            }
        }

        public void FilterItems()
        {
            Items.Clear();
            foreach (var item in AllItems)
            {
                if (item.NamePHB.ToLower().Contains(SearchText.ToLower()))
                {
                    Items.Add(item);
                }
            }
        }


    }
}
