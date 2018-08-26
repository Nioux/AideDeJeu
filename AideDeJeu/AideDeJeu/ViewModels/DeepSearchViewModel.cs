using AideDeJeu.Views;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class DeepSearchViewModel : BaseViewModel
    {
        private Command<string> _SearchCommand = null;
        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new Command<string>(async (searchText) => await ExecuteSearchCommandAsync(searchText)));
            }
        }

        public async Task ExecuteSearchCommandAsync(string searchText)
        {
            Main.IsLoading = true;
            await Task.Run(async () => await Main.PreloadAllItemsAsync());
            Items = await Task.Run(async () => await Main.DeepSearchAllItemsAsync(searchText));
            Main.IsLoading = false;
        }

        public IEnumerable<MainViewModel.SearchedItem> _Items = null;
        public IEnumerable<MainViewModel.SearchedItem> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                SetProperty(ref _Items, value);
            }
        }
    }
}
