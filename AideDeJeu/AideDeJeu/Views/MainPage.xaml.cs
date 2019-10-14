using AideDeJeu.ViewModels;
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
	public partial class MainPage : ContentPage
	{
        //public MainViewModel Main
        //{
        //    get
        //    {
        //        return DependencyService.Get<MainViewModel>();
        //    }
        //}

        public MainPage ()
		{
			InitializeComponent ();
            //BindingContext = this;
        }
    }
}