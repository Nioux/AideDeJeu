using AideDeJeu.Tools;
using AideDeJeu.ViewModels.PlayerCharacter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfViewPage : ContentPage
    {
        public PdfViewPage()
        {
            InitializeComponent();
        }

        public NotifyTaskCompletion<string> PdfFile { get; set; }

        public ICommand CloseCommand
        {
            get
            {
                return new Command(async() => await ExecuteCloseCommandAsync());
            }
        }

        private async Task ExecuteCloseCommandAsync()
        {
            await Navigation.PopModalAsync(true);
        }

        public ICommand ShareCommand
        {
            get
            {
                return new Command(async() => await ExecuteShareCommandAsync());
            }
        }

        private async Task ExecuteShareCommandAsync()
        {
            var result = await DisplayActionSheet("Actions", "Annuler", null, "Envoyer vers...", "Ouvrir avec...");
            if(result == "Ouvrir avec...")
            {
                string filePath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Path.Combine("pdf", WebUtility.UrlEncode(PdfFile.Result)));
                await DependencyService.Get<PlayerCharacterEditorViewModel>().OpenPdfAsync(filePath);

            }
        }
    }
}