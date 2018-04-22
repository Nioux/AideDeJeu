using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AideDeJeu.Models;
using AideDeJeu.Views;
using AideDeJeuLib;

namespace AideDeJeu.ViewModels
{
    public class SpellsViewModel : BaseViewModel
    {
        public ObservableCollection<Spell> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public SpellsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Spell>();
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
                Items.Clear();
                var items = await SpellDataStore.GetItemsAsync(true);
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