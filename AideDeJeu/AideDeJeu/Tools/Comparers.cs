using AideDeJeuLib;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeu.Tools
{
    public class ItemComparer : Comparer<Item>
    {
        public override int Compare(Item x, Item y)
        {
            return x.NamePHB.CompareTo(y.NamePHB);
        }
    }

    public class PowerComparer : Comparer<string>
    {
        public override int Compare(string x, string y)
        {
            var regex = new Regex(@"\((?<xp>\d*?) PX\)");
            int xpx = int.Parse(regex.Match(x).Groups["xp"].Value);
            int xpy = int.Parse(regex.Match(y).Groups["xp"].Value);
            return xpx - xpy;
        }
    }
}
