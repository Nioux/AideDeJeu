using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage : ContentPage
    {
        public ClassPage()
        {
            InitializeComponent();

            BindingContext = DependencyService.Get<AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterEditorViewModel>();
        }
    }
}