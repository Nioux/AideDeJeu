using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage //TabbedPage
    {
		public MainPage ()
		{
			InitializeComponent ();
		}
	}
}