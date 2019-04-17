using AideDeJeuLib;

namespace AideDeJeu.ViewModels.Library
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
