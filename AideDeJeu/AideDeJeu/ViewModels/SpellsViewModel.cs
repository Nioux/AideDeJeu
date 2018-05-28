using AideDeJeuLib;
using AideDeJeuLib.Spells;
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
    public class SpellsViewModel : ItemsViewModel
    {
        public SpellsViewModel(ItemSourceType itemSourceType) : base(itemSourceType)
        {
        }

        public override async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Main.Navigator.GotoSpellDetailPageAsync(item as Spell);
        }

    }
}