using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    public class Property : Dictionary<string, string>
    {

    }

    public class Properties : Dictionary<string, Property>
    {

    }

    public abstract class Item
    {
        public string Name { get; set; }
        public string NameVO { get; set; }
        public string NameVOText
        {
            get
            {
                var regex = new Regex("\\[(?<text>.*?)\\]");
                var match = regex.Match(NameVO ?? string.Empty);
                return match.Groups["text"].Value;
            }
        }

        //public Properties Properties { get; set; }

        public abstract string Markdown { get; }
        public abstract void Parse(ref Markdig.Syntax.ContainerBlock.Enumerator enumerator);
    }
}
