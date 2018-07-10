using AideDeJeu.Tools;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        Item _Item = null;
        public Item Item
        {
            get { return _Item; }
            set
            {
                SetProperty(ref _Item, value);
            }
        }

        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Name;
            Item = item;
        }
    }




}
