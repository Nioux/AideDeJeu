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
    public partial class FinalizePage : ContentPage
    {
        public FinalizePage()
        {
            InitializeComponent();

            BindingContext = DependencyService.Get<AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterEditorViewModel>();
        }
    }
}