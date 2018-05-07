
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
        }

        public SpellDetailPage()
        {
            InitializeComponent();

            var item = new Spell
            {
                Name = "",
                NameVO = "",
                NamePHB = "",
                //Description = "This is an item description."
            };

            viewModel = new SpellDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}