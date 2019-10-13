using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackgroundPage : ContentPage
    {
        public BackgroundPage()
        {
            InitializeComponent();

            BindingContext = DependencyService.Get<AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterEditorViewModel>();
        }
    }
}