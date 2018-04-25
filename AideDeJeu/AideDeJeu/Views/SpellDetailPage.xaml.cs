using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AideDeJeu.Models;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using AideDeJeuLib.Spells;

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
                Title = "",
                //Description = "This is an item description."
            };

            viewModel = new SpellDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}