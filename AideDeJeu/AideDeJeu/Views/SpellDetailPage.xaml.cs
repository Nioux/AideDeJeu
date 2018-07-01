
using AideDeJeu.ViewModels;
using AideDeJeuLib.Spells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SpellDetailPage : ContentPage
	{
        SpellDetailViewModel viewModel;

        public SpellDetailPage(SpellDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            //mdNameVO.NavigateToLink = async (s) => await viewModel.Main.NavigateToLink(s);
            mdDescription.NavigateToLink = async (s) => await viewModel.Main.NavigateToLink(s);

        }

        public SpellDetailPage()
        {
            InitializeComponent();

            var item = new SpellHD
            {
                Name = "",
                NameVO = "",
                //Description = "This is an item description."
            };

            viewModel = new SpellDetailViewModel(item);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}