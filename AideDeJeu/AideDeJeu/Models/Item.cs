using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string AltName { get; set; }
        public string AltNameText
        {
            get
            {
                var regex = new Regex("\\[(?<text>.*?)\\]");
                var match = regex.Match(AltName ?? string.Empty);
                return match.Groups["text"].Value;
            }
        }

        public abstract string Markdown { get; }
        public abstract void Parse(ref Markdig.Syntax.ContainerBlock.Enumerator enumerator);
    }
}
