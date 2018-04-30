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

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/Nioux/AideDeJeu")));
        }

        public ICommand OpenWebCommand { get; }
    }
}