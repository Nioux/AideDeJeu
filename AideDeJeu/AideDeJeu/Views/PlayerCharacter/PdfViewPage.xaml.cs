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
            var SendTo = "Envoyer vers...";
            var OpenWith = "Ouvrir avec...";
            var SaveTo = "Enregistrer sous...";
            var commands = new List<string>();
            switch(Device.RuntimePlatform)
            {
                case Device.Android:
                    commands.Add(SendTo);
                    commands.Add(OpenWith);
                    break;
                case Device.UWP:
                    commands.Add(SendTo);
                    commands.Add(OpenWith);
                    commands.Add(SaveTo);
                    break;
                case Device.iOS:
                    commands.Add(SendTo);
                    break;
            }
            string result = null;
            if (commands.Count > 1)
            {
                result = await DisplayActionSheet("Actions", "Annuler", null, commands.ToArray());
            }
            else
            {
                result = commands.FirstOrDefault();
            }
            if(result == OpenWith)
            {
                await OpenWithAsync(PdfFile.Result);
            }
            else if(result == SendTo)
            {
                await SendToAsync(PdfFile.Result);
            }
            else if (result == SaveTo)
            {
                await SaveToAsync(PdfFile.Result);
            }
        }

        private async Task OpenWithAsync(string filename)
        {
            string filePath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Path.Combine("pdf", WebUtility.UrlEncode(filename)));
            await DependencyService.Get<PlayerCharacterEditorViewModel>().OpenPdfAsync(filePath);
        }

        private async Task SendToAsync(string filename)
        {
            string filePath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Path.Combine("pdf", WebUtility.UrlEncode(filename)));
            await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest
            {
                Title = PdfFile.Result,
                File = new Xamarin.Essentials.ShareFile(filePath)
            });
        }
        private async Task SaveToAsync(string filename)
        {
            string filePath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, Path.Combine("pdf", WebUtility.UrlEncode(filename)));
        }
    }
}