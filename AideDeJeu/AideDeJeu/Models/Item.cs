using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    public class Item
    {
        public string Name { get; set; }
        public int NameLevel { get; set; }
        public string AltName { get; set; }
        public string AltNameText
        {
            get
            {
                var regex = new Regex("\\[(?<text>.*?)\\]");
                var match = regex.Match(AltName ?? string.Empty);
                if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                {
                    return match.Groups["text"].Value;
                }
                else
                {
                    regex = new Regex("(?<text>.*?)( \\(SRD p\\d*\\))");
                    match = regex.Match(AltName ?? string.Empty);
                    if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                    {
                        return match.Groups["text"].Value;
                    }
                    return AltName ?? string.Empty;
                }
            }
        }

        public string Source { get; set; }

        public string Markdown { get; set; }
        //public virtual void Parse(ref Markdig.Syntax.ContainerBlock.Enumerator enumerator) { }
    }
}
