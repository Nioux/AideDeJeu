using AideDeJeu.ViewModels.Library;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Library
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Path", "path")]
    public partial class ItemPage : ContentPage
    {
        public ItemViewModel BindingItem
        {
            get
            {
                return BindingContext as ItemViewModel;
            }
        }

        public ItemPage()
        {
            InitializeComponent();
        }

        public ItemPage(string id)
        {
            InitializeComponent();
            Path = id;
        }

        public ItemPage(ItemViewModel itemViewModel)
        {
            InitializeComponent();
            Path = itemViewModel.Item.Id;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingItem.Main.CurrentItem = BindingItem;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BindingItem.Main.CurrentItem = null;
        }
        private string _Path { get; set; } = null; //"index.md";
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
                if (Path != null)
                {
                    BindingItem?.LoadPageAsync(Path);
                }
            }
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            var lv = sender as ListView;
            if (lv != null)
            {
                lv.SelectedItem = null;
            }
        }
    }
}