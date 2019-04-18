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

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemView : StackLayout
    {
        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }
        public ItemView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name), 
            typeof(string), 
            typeof(ItemView),
            defaultValue: default(string));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(ItemView),
            defaultValue: default(string));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem), 
            typeof(object), 
            typeof(ItemView), 
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
                typeof(ItemView), 
                default(System.Collections.IEnumerable));

    }
}