using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainShell : Shell, INotifyPropertyChanged
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

        private string _HeaderTitle = string.Empty;
        public string HeaderTitle
        {
            get
            {
                return _HeaderTitle;
            }
            set
            {
                SetProperty(ref _HeaderTitle, value);
            }
        }
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            Debug.WriteLine(this.CurrentItem.CurrentItem.CurrentItem);
            HeaderTitle = this.CurrentItem.CurrentItem.CurrentItem.Route;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
    [CallerMemberName]string propertyName = "",
    Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            CallOnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void CallOnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}