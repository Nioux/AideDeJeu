using AideDeJeu.Tools;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AideDeJeu.Services
{
    public class ItemDatabaseContext : DbContext
    {
        public ItemDatabaseContext(string databasePath)
        {
            DatabasePath = databasePath;
        }
        private string DatabasePath = null;
        private string DatabaseName = "database.db";

        public DbSet<Spell> Spells { get; set; }

        public DbSet<Monster> Monsters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (DatabasePath == null)
            //{
            //    switch (Device.RuntimePlatform)
            //    {
            //        case Device.iOS:
            //            SQLitePCL.Batteries_V2.Init();
            //            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName); ;
            //            break;
            //        case Device.Android:
            //            DatabasePath = DependencyService.Get<INativeAPI>().GetDatabasePath(DatabaseName);
            //            break;
            //        case Device.UWP:
            //            DatabasePath = DependencyService.Get<INativeAPI>().GetDatabasePath(DatabaseName);
            //            break;
            //        default:
            //            throw new NotImplementedException("Platform not supported");
            //    }
            //}
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={DatabasePath}");
        }
    }
}
