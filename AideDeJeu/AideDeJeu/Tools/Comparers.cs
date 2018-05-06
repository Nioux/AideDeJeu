using AideDeJeuLib;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.Tools
{
    public class ItemComparer : Comparer<Item>
    {
        public override int Compare(Item x, Item y)
        {
            return x.NamePHB.CompareTo(y.NamePHB);
        }
    }
}
