using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class Equipment : Items
    {
        public string Type { get; set; }
        public string Price { get; set; }
        public string ArmorClass { get; set; }
        public string Discretion { get; set; }
        public string Weight { get; set; }
        public string Strength { get; set; }
        public string Rarity { get; set; }
        public string Damages { get; set; }
        public string Properties { get; set; }
        public string Unity { get; set; }
        public string Capacity { get; set; }
        public string WeightCapacity { get; set; }
        public string Speed { get; set; }
    }
}
