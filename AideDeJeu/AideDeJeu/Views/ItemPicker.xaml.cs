using AideDeJeu.ViewModels;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPicker : ContentPage
    {
        public PickerViewModel<object> ViewModel { get; set; } = new PickerViewModel<object>();
        public ItemPicker()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }
    }
}