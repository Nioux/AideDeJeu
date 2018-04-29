using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AideDeJeu.Models;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using AideDeJeuLib.Spells;
using AideDeJeuLib.Monsters;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterDetailPage : ContentPage
	{
        MonsterDetailViewModel viewModel;

        public MonsterDetailPage(MonsterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public MonsterDetailPage()
        {
            InitializeComponent();

            var item = new Monster
            {
                Name = "",
                //Description = "This is an item description."
            };

            viewModel = new MonsterDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}