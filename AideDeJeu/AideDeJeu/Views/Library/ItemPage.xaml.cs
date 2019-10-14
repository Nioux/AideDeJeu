
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
    public partial class ItemPage : ContentPage
    {
        public ItemViewModel Item
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
            Item.Main.CurrentItem = Item;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Item.Main.CurrentItem = null;
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