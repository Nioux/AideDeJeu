using AideDeJeuLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AideDeJeu.Tools
{
    public class ItemComparer : Comparer<Item>
    {
        public override int Compare(Item x, Item y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public class PowerComparer : Comparer<string>
    {
        public override int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)) return 0;
            if (string.IsNullOrEmpty(x)) return 1;
            if (string.IsNullOrEmpty(y)) return -1;
            var regex = new Regex(@"\((?<xp>\d?\d?\d?\s?\d?\d?\d??) (PX|XP)\)");
            int xpx;
            int.TryParse(regex.Match(x).Groups["xp"].Value.Replace(" ",""), out xpx);
            int xpy;
            int.TryParse(regex.Match(y).Groups["xp"].Value.Replace(" ",""), out xpy);
            return xpx - xpy;
        }
    }

    public class PriceComparer : Comparer<string>
    {
        int ToCopperPieces(string price)
        {
            price = price.Trim(new char[] { ' ', '\n' });
            var regex = new Regex("(?<count>\\d?\\d?\\d?\\s?\\d?\\d?\\d?\\s?\\d?\\d?\\d?)\\s?(?<type>p[caeop])");
            var match = regex.Match(price);
            int count = 0;
            int.TryParse(match.Groups["count"].Value.Replace(" ", ""), out count);
            switch (match.Groups["type"].Value)
            {
                case "pc":
                    return count;
                case "pa":
                    return count * 10;
                case "pe":
                    return count * 50;
                case "po":
                    return count * 100;
                case "pp":
                    return count * 1000;
            }
            return 0;
        }
        public override int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)) return 0;
            if (string.IsNullOrEmpty(x)) return 1;
            if (string.IsNullOrEmpty(y)) return -1;
            return ToCopperPieces(x) - ToCopperPieces(y);
        }
    }
}
