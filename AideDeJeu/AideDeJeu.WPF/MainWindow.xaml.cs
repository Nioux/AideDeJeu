using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace AideDeJeu.WPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            //var assemblies = new List<System.Reflection.Assembly>();
            ////assemblies.AddRange(Rg.Plugins.Popup.Popup.GetExtraAssemblies());
            //assemblies.Add(typeof(Urho.Forms.WpfSurfaceRenderer).GetTypeInfo().Assembly);
            Forms.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            LoadApplication(new AideDeJeu.App());
        }
    }
}
