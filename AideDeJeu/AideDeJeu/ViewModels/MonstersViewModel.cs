using AideDeJeu.Tools;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class MonstersViewModel : ItemsViewModel
    {
        public MonstersViewModel(ItemSourceType itemSourceType) : base(itemSourceType)
        {
        }

        public override async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Main.Navigator.GotoMonsterDetailPageAsync(item as Monster);
        }
    }
}