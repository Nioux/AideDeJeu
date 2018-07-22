using AideDeJeu.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FilteredItemsPage : MasterDetailPage
    {
        MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }
        //INavig//ator Navigator;

        public ItemsViewModel _ItemsViewModel;
        public ItemsViewModel ItemsViewModel
        {
            get
            {
                return _ItemsViewModel;
            }
        }
        public FilteredItemsPage (ItemsViewModel itemsViewModel)
		{
			InitializeComponent ();

            BindingContext = _ItemsViewModel = itemsViewModel; // Main;

            //this.SizeChanged += (o, e) => {
            //    if(this.Width > 0 && this.Height > 0)
            //    {
            //        this.IsPresented = this.Width > this.Height;
            //    }
            //};
        }
        public FilteredItemsPage()
        {
            InitializeComponent();

            BindingContext = Main;

            //this.SizeChanged += (o, e) => {
            //    if (this.Width > 0 && this.Height > 0)
            //    {
            //        this.IsPresented = this.Width > this.Height;
            //    }
            //};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.MasterBehavior = MasterBehavior.Popover;

            //if (Main.Items.Count() == 0)
                //Main.LoadItemsCommand.Execute(null);
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.IsPresented = !this.IsPresented;
        }
    }
}