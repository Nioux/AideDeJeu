using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.PlayerCharacter;
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
    public partial class PlayerCharacterEditorPage : TabbedPage
    {
        public PlayerCharacterEditorPage()
        {
            BindingContext = new PlayerCharacterEditorViewModel();

            InitializeComponent();

        }
    }
}