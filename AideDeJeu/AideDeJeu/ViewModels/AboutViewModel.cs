using AideDeJeu.Tools;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "À propos de ...";
        }

        //private OSAppTheme _Theme = OSAppTheme; // Application.Current.Properties.ContainsKey("OSAppTheme") ? (OSAppTheme)(int)Application.Current.Properties["OSAppTheme"] : OSAppTheme.Unspecified;
        public int ThemeIndex
        {
            get
            {
                return (int)OSAppTheme;
            }
            set
            {
                OSAppTheme = (OSAppTheme)value;
                OnPropertyChanged();
            }
        }

/*        public ICommand ThemeChangedCommand { get; } = new Command<int>((index) => ExecuteThemeChangedCommand(index));
        public static void ExecuteThemeChangedCommand(int index)
        {
            Application.Current.Properties["OSAppTheme"] = index;
            App.Current.UserAppTheme = (OSAppTheme)index;
        }*/

        public string OGL
        {
            get
            {
                var assembly = typeof(AboutViewModel).GetTypeInfo().Assembly;
                //var names = assembly.GetManifestResourceNames();
                using (var stream = assembly.GetManifestResourceStream("AideDeJeu.OGL.txt"))
                {
                    using (var reader = new System.IO.StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

    }
}