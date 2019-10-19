using AideDeJeu.ViewModels;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Pickers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPickerView : ContentView
    {
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }
        public ItemPickerView()
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
            typeof(ItemPickerView),
            defaultValue: default(string));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(ItemPickerView),
            defaultValue: default(string));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem), 
            typeof(object), 
            typeof(ItemPickerView), 
            defaultValue: default(object), 
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
                typeof(ItemPickerView),
                defaultValue: null); // default(System.Collections.IEnumerable));

        public ICommand PickerCommand
        {
            get
            {
                return new Command<System.Collections.IList>(async (items) => SelectedItem = await ExecuteItemPickerCommandAsync(items));
            }
        }
        private async Task<object> ExecuteItemPickerCommandAsync(System.Collections.IEnumerable items)
        {
            var picker = new Views.Pickers.ItemPicker();
            var vm = picker.ViewModel;
            vm.Title = Title;
            vm.Description = Description;
            vm.Items = items;
            await Main.Navigator.Navigation.PushModalAsync(picker, true);
            var result = await vm.PickValueAsync();
            await Main.Navigator.Navigation.PopModalAsync(true);
            return result;
        }
    }
}