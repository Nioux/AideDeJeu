using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerCharacterEditorPage : ContentPage
    {
        public PlayerCharacterEditorPage()
        {
            var vm = new PlayerCharacterEditorViewModel();
            BindingContext = vm;
            vm.InitAsync();

            InitializeComponent();

        }
    }
}