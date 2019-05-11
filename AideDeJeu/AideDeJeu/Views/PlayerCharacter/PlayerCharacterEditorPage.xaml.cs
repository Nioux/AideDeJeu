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
            //if(DependencyService.Get<PlayerCharacterEditorViewModel>() == null)
            //{
            //    DependencyService.Register<PlayerCharacterEditorViewModel>();
            //}

            BindingContext = DependencyService.Get<PlayerCharacterEditorViewModel>(); // new PlayerCharacterEditorViewModel();

            InitializeComponent();

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    Device.BeginInvokeOnMainThread(async () => {
        //        var result = await this.DisplayAlert("Attention", "Si vous revenez au menu, vous perdrez le personnage en cours de création", "Menu", "Annuler");
        //        if (result) await this.Navigation.PopAsync();
        //    });
        //    return true;
        //    //return base.OnBackButtonPressed();
        //}

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

        public Command PdfViewCommand
        {
            get
            {
                return new Command<object>(ExecutePdfViewCommand);
            }
        }

        public void ExecutePdfViewCommand(object param)
        {
            var vm = BindingContext as PlayerCharacterEditorViewModel;
            var page = new PdfViewPage();
            page.PdfFile = new Tools.NotifyTaskCompletion<string>(Task.Run(async() => await vm.GeneratePdfAsync(param as PlayerCharacterViewModel)));
            page.BindingContext = page;
            Navigation.PushModalAsync(page, true);
        }
    }
}