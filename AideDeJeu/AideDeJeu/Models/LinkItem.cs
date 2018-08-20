using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class LinkItem : Item
    {
        public string Link { get; set; }
        public string NameLink
        {
            get
            {
                if (Name != null && Link != null)
                {
                    return $"[{Name}]({Link})";
                }
                return null;
            }
            set
            {
                var regex = new Regex("\\[(?<name>.*?)\\]\\((?<link>.*?)\\)");
                var match = regex.Match(value);
                Name = match.Groups["name"].Value;
                Link = match.Groups["link"].Value;
                Markdown = $"# {NameLink}\n\n";
            }
        }
    }
}
