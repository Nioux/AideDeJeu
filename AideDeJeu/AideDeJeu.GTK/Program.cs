using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace AideDeJeu.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationIcon("main.png");
            window.SetApplicationTitle("Haches & Dés");
            window.Show();

            Gtk.Application.Run();
        }
    }
}
