using AideDeJeuLib;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public abstract class ItemsViewModel : BaseViewModel
    {
        public ItemsViewModel(ObservableCollection<Item> items)
        {
            Items = items;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
        }
        public ObservableCollection<Item> Items { get; protected set; }
        public ICommand LoadItemsCommand { get; protected set; }
        public abstract Task ExecuteLoadItemsCommandAsync();
    }
}
