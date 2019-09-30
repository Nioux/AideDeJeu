
using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using AideDeJeuLib;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Library
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("Path", "path")]
    public partial class ItemPage : ContentPage, INotifyPropertyChanged
    {
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        private ItemDetailViewModel _Item = null;
        public ItemDetailViewModel Item
        {
            get
            {
                return _Item;
            }
            set
            {
                SetProperty(ref _Item, value);
            }
        }

        public ItemPage()
        {
            BindingContext = this;
            InitializeComponent();
        }

        public ItemPage(string id)
        {
            BindingContext = this;
            InitializeComponent();
            Path = id;
            //LoadPageAsync();
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    LoadPageAsync();
        //}

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
                LoadPageAsync();
            }
        }

        private async Task LoadPageAsync()
        {
            if (Path == null) return;
            var regex = new Regex("/?(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
            var match = regex.Match(Uri.UnescapeDataString(Path));
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
                this.Item = itemsViewModel;
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

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            CallOnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void CallOnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}