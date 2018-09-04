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
        public override string Markdown
        {
            get
            {
                return $"\n\n<!--LinkItem-->\n\n{new string('#', NameLevel + 1)} {NameLink}\n\n<!--/LinkItem-->\n\n";
            }
            set => base.Markdown = value;
        }
        public string Link { get; set; }
        public string NameLink
        {
            get
            {
                if (Name != null && Link != null)
                {
                    return $"<!--NameLink-->[{Name}]({Link})<!--/NameLink-->";
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
