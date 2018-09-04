using AideDeJeu.Tools;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace AideDeJeuLib
{
    public class Spell : Item
    {
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Ritual { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml { get; set; }
        //public string Source { get; set; }
        public string Classes { get; set; }
    }
}
