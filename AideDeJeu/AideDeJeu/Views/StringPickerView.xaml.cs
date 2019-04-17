using AideDeJeu.ViewModels;
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
    public partial class StringPickerView : StackLayout
    {
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }
        public StringPickerView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title), 
            typeof(string), 
            typeof(StringPickerView),
            defaultValue: default(string));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(StringPickerView),
            defaultValue: default(string));

        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem), 
            typeof(string), 
            typeof(StringPickerView), 
            defaultValue: default(string), 
            defaultBindingMode: BindingMode.TwoWay);

        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        //public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        //    nameof(ItemsSource), 
        //    typeof(System.Collections.IList), 
        //    typeof(StringPickerView),
        //    defaultValue: new List<string>());
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource), 
                typeof(System.Collections.IEnumerable), 
                typeof(StringPickerView), 
                default(System.Collections.IEnumerable));

        public ICommand PickerCommand
        {
            get
            {
                return new Command<System.Collections.IList>(async (strings) => SelectedItem = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        private async Task<string> ExecuteStringPickerCommandAsync(System.Collections.IEnumerable strings)
        {
            if (strings != null)
            {
                var picker = new Views.StringPicker();
                var vm = picker.ViewModel;
                vm.Title = Title;
                vm.Description = Description;
                vm.Items = strings;
                await Main.Navigator.Navigation.PushModalAsync(picker, true);
                var result = await vm.PickValueAsync();
                await Main.Navigator.Navigation.PopModalAsync(true);
                return result;
            }
            return SelectedItem;
        }
    }
}