using AideDeJeu.Tools;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "À propos de ...";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://nioux.github.io/AideDeJeu/")));
        }

        public ICommand OpenWebCommand { get; }

        public string Version {
            get
            {
                return DependencyService.Get<INativeAPI>().GetVersion();
            }
        }
    }
}