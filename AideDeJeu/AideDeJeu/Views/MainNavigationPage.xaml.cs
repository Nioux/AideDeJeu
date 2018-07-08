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
	public partial class MainNavigationPage : NavigationPage
	{
		public MainNavigationPage ()
		{
			InitializeComponent ();
		}

        public MainNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}