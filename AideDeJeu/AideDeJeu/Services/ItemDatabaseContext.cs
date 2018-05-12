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
        private const string databaseName = "database.db";

        public DbSet<Spell> Spells { get; set; }

        public DbSet<Monster> Monsters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath = "";
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case Device.Android:
                    databasePath = DependencyService.Get<INativeAPI>().GetDatabasePath(databaseName);
                    break;
                case Device.UWP:
                    databasePath = DependencyService.Get<INativeAPI>().GetDatabasePath(databaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}
