
using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using AideDeJeuLib;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Library
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Path", "path")]
    public partial class ItemPage : ContentPage
	{
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        ItemDetailViewModel viewModel;

        public ItemPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPageAsync();
        }

        private string _Path { get; set; } = "index.md";
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        private async Task LoadPageAsync()
        {
            var regex = new Regex("/?(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
            var match = regex.Match(Path);
            var file = match.Groups["file"].Value;
            var anchor = match.Groups["anchor"].Value;
            var with = match.Groups["with"].Value;
            Item item = null;
            try
            {
                Main.IsBusy = true;
                Main.IsLoading = true;
                item = await Task.Run(async () => await Main.Store.GetItemFromDataAsync(file, anchor));
            }
            finally
            {
                Main.IsBusy = false;
                Main.IsLoading = false;
            }
            if (item != null)
            {
                var items = item; // as Items;
                var filterViewModel = items.GetNewFilterViewModel();
                var itemsViewModel = new ItemDetailViewModel() { Item = items };
                BindingContext = this.viewModel = itemsViewModel;
                //SwitchToMainTab();
                //if (filterViewModel == null)
                //{
                //    await GotoItemsPageAsync(itemsViewModel);
                //}
                //else
                //{
                //    await GotoFilteredItemsPageAsync(itemsViewModel);
                //}

            }
            else
            {
                //await App.Current.MainPage.DisplayAlert("Lien invalide", s, "OK");
            }
        }
    }
}