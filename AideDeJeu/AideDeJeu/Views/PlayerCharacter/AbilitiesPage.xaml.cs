using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbilitiesPage : ContentPage
    {
        public AbilitiesPage()
        {
            InitializeComponent();

            BindingContext = DependencyService.Get<AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterEditorViewModel>();
        }
    }
}