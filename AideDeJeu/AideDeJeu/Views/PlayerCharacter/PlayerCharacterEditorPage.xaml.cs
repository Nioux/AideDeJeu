using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.PlayerCharacter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
                return new Command<PlayerCharacterViewModel>(async(pc) => await ExecutePdfViewCommandAsync(pc));
            }
        }

        public async Task ExecutePdfViewCommandAsync(PlayerCharacterViewModel pc)
        {
            var vm = BindingContext as PlayerCharacterEditorViewModel;
            var page = new PdfViewPage();
            page.PdfFile = new Tools.NotifyTaskCompletion<string>(Task.Run(async() => await vm.GeneratePdfAsync(pc)));
            page.BindingContext = page;
            //Navigation.PushModalAsync(page, true);
            await Navigation.PushAsync(page, true);
        }

        private void Abilities_Appearing(object sender, EventArgs e)
        {
            try
            {
                if (!Accelerometer.IsMonitoring)
                {
                    Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                    Accelerometer.Start(SensorSpeed.Game);
                }
            }
            catch { }
            try
            {
                if (!Gyroscope.IsMonitoring)
                {
                    Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
                    Gyroscope.Start(SensorSpeed.Game);
                }
            }
            catch { }
        }

        private void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            if(e.Reading.AngularVelocity.Z >= 1)
            {
                var vm = BindingContext as PlayerCharacterEditorViewModel;
                vm.ResetDicesCommand.Execute(null);
            }
        }


        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            var vm = BindingContext as PlayerCharacterEditorViewModel;
            vm.RollDicesMRickCommand.Execute(null);
        }

        private void Abilities_Disappearing(object sender, EventArgs e)
        {
            try
            {
                if (Gyroscope.IsMonitoring)
                {
                    Gyroscope.Stop();
                    Gyroscope.ReadingChanged -= Gyroscope_ReadingChanged;
                }
            }
            catch { }
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                }
            }
            catch { }
        }
    }
}