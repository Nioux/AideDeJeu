using AideDeJeu.Tools;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "À propos de ...";
        }

        public string Version {
            get
            {
                return DependencyService.Get<INativeAPI>().GetVersion();
            }
        }

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