using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AideDeJeu.Models;
using AideDeJeuLib;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class SpellDetailViewModel : BaseViewModel
    {
        Spell _Item = null;
        public Spell Item
        {
            get { return _Item; }
            set { SetProperty(ref _Item, value); }
        }

        public Command LoadItemCommand { get; set; }

        public SpellDetailViewModel(Spell item = null)
        {
            Title = item?.Title;
            Item = item;
            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
        }
        async Task ExecuteLoadItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Items.Clear();
                var item = await new Scrappers().GetSpell(Item.Id);
                Item = item;
                //foreach (var item in items)
                //{
                //    Items.Add(item);
                //}
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
