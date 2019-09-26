using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainShell : Shell
    {
        public MainShell()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public ICommand ShellNavigateCommand
        {
            get
            {
                return new Command<string>(async (path) => await ExecuteShellNavigateCommandAsync(path));
            }
        }
        private async Task ExecuteShellNavigateCommandAsync(string path)
        {
            await Shell.Current.GoToAsync(path);
        }
    }
}