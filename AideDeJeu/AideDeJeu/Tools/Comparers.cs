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
            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)) return 0;
            if (string.IsNullOrEmpty(x)) return 1;
            if (string.IsNullOrEmpty(y)) return -1;
            var regex = new Regex(@"\((?<xp>\d*?) (PX|XP)\)");
            int xpx;
            int.TryParse(regex.Match(x).Groups["xp"].Value, out xpx);
            int xpy;
            int.TryParse(regex.Match(y).Groups["xp"].Value, out xpy);
            return xpx - xpy;
        }
    }
}
