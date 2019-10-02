using AideDeJeu.ViewModels;
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

        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }

        public ICommand NavigateToItemCommand
        {
            get
            {
                return new Command<string>(async (path) => await ExecuteNavigateToItemCommandAsync(path));
            }
        }
        private async Task ExecuteNavigateToItemCommandAsync(string path)
        {
            await Navigation.PushAsync(new Library.ItemPage(path), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToHomeCommand
        {
            get
            {
                return new Command(async() => await ExecuteNavigateToHomeCommandAsync());
            }
        }
        private async Task ExecuteNavigateToHomeCommandAsync()
        {
            await Navigation.PushAsync(new MainPage(), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToPCCommand
        {
            get
            {
                return new Command(async () => await ExecuteNavigateToPCCommandAsync());
            }
        }
        private async Task ExecuteNavigateToPCCommandAsync()
        {
            await Navigation.PushAsync(new PlayerCharacter.PlayerCharacterEditorPage(), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToDicesCommand
        {
            get
            {
                return new Command(async () => await ExecuteNavigateToDicesCommandAsync());
            }
        }
        private async Task ExecuteNavigateToDicesCommandAsync()
        {
            await Navigation.PushAsync(new DicesPage(), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToBookmarksCommand
        {
            get
            {
                return new Command(async () => await ExecuteNavigateToBookmarksCommandAsync());
            }
        }
        private async Task ExecuteNavigateToBookmarksCommandAsync()
        {
            await Navigation.PushAsync(new Library.BookmarksPage(), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToDeepSearchCommand
        {
            get
            {
                return new Command(async () => await ExecuteNavigateToDeepSearchCommandAsync());
            }
        }
        private async Task ExecuteNavigateToDeepSearchCommandAsync()
        {
            await Navigation.PushAsync(new Library.DeepSearchPage(), true);
            this.FlyoutIsPresented = false;
        }

        public ICommand NavigateToAboutCommand
        {
            get
            {
                return new Command(async () => await ExecuteNavigateToAboutCommandAsync());
            }
        }
        private async Task ExecuteNavigateToAboutCommandAsync()
        {
            await Navigation.PushAsync(new AboutPage(), true);
            this.FlyoutIsPresented = false;
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
            Debug.WriteLine(this.CurrentItem?.CurrentItem?.CurrentItem);
            HeaderTitle = this.CurrentItem?.CurrentItem?.CurrentItem?.Route;
            var content = this.CurrentItem?.CurrentItem?.CurrentItem?.Content;
            Debug.WriteLine(content);

            this.CurrentItem.CurrentItem.CurrentItem.PropertyChanged += CurrentItem_PropertyChanged;
            this.CurrentItem.CurrentItem.CurrentItem.Appearing += CurrentItem_Appearing;
            this.CurrentItem.CurrentItem.CurrentItem.BindingContextChanged += CurrentItem_BindingContextChanged;
        }

        private void CurrentItem_BindingContextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private void CurrentItem_Appearing(object sender, EventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }

        private void CurrentItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.PropertyName);
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