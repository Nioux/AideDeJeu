using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Pickers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StringPicker : ContentPage
    {
        public PickerViewModel<string> ViewModel { get; set; } = new PickerViewModel<string>();
        public StringPicker()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }
    }
}