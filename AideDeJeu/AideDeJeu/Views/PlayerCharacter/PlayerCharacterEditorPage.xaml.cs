using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.PlayerCharacter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerCharacterEditorPage : CarouselPage
    {
        public PlayerCharacterEditorPage()
        {
            BindingContext = new PlayerCharacterEditorViewModel();

            InitializeComponent();

        }

        public Command ChangePageCommand
        {
            get
            {
                return new Command<object>(ExecuteChangePageCommand);
            }
        }

        public void ExecuteChangePageCommand(object param)
        {
            This.CurrentPage = param as ContentPage;
        }
    }
}