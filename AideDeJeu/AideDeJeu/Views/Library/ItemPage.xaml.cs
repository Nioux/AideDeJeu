
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

        private ItemViewModel _Item = new ItemViewModel();
        public ItemViewModel Item
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Main.CurrentItem = Item;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Main.CurrentItem = null;
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
                    Item?.LoadPageAsync(Path);
                }
            }
        }

        //private async Task LoadPageAsync()
        //{
        //    if (Path == null) return;
        //    var regex = new Regex("/?(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
        //    var match = regex.Match(Uri.UnescapeDataString(Path));
        //    var file = match.Groups["file"].Value;
        //    var anchor = match.Groups["anchor"].Value;
        //    var with = match.Groups["with"].Value;
        //    Item item = null;
        //    try
        //    {
        //        Main.IsBusy = true;
        //        Main.IsLoading = true;
        //        item = await Task.Run(async () => await Main.Store.GetItemFromDataAsync(file, anchor));
        //    }
        //    finally
        //    {
        //        Main.IsBusy = false;
        //        Main.IsLoading = false;
        //    }
        //    if (item != null)
        //    {
        //        var items = item; // as Items;
        //        var filterViewModel = items.GetNewFilterViewModel();
        //        var itemsViewModel = new ItemViewModel() { Item = items, AllItems = items, Filter = filterViewModel };
        //        this.Item = itemsViewModel;
        //        itemsViewModel.LoadItemsCommand.Execute(null);
        //        if (!string.IsNullOrEmpty(with))
        //        {
        //            var swith = with.Split('_');
        //            for (int i = 0; i < swith.Length / 2; i++)
        //            {
        //                var key = swith[i * 2 + 0];
        //                var val = swith[i * 2 + 1];
        //                filterViewModel.FilterWith(key, val);
        //            }
        //        }
        //        //SwitchToMainTab();
        //        //if (filterViewModel == null)
        //        //{
        //        //    await GotoItemsPageAsync(itemsViewModel);
        //        //}
        //        //else
        //        //{
        //        //    await GotoFilteredItemsPageAsync(itemsViewModel);
        //        //}

        //    }
        //    else
        //    {
        //        //await App.Current.MainPage.DisplayAlert("Lien invalide", s, "OK");
        //    }
        //}

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
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