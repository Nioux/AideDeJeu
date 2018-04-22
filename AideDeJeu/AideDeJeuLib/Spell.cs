using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class Spell
    {
        public string Title { get; set; }
        public string TitleUS { get; set; }
        public string LevelType { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
        public string DescriptionText { get; set; }
        public HtmlNode DescriptionDiv { get; set; }
        public string Overflow { get; set; }
        public string NoOverflow { get; set; }
        public string Source { get; set; }
    }
}
