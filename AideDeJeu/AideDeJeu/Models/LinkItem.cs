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
            }
        }
        public override string Markdown
        {
            get
            {
                return $"# {NameLink}\n\n";
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block is HeadingBlock)
                {
                    var headingBlock = block as HeadingBlock;
                    if (headingBlock.HeaderChar == '#' && (headingBlock.Level == 1 || headingBlock.Level == 2))
                    {
                        if (this.NameLink == null)
                        {
                            this.NameLink = headingBlock.Inline.ToMarkdownString();
                        }
                    }
                }
                else if (block is ParagraphBlock)
                {
                    if (block.IsNewItem())
                    {
                        return;
                    }
                }
                else if (block is ListBlock)
                {
                    var listBlock = block as ListBlock;
                    if (listBlock.BulletType == '-')
                    {
                        var regex = new Regex("(?<key>.*?): (?<value>.*)");
                        var str = block.ToMarkdownString();
                        var properties = new List<Tuple<string, Action<LinkItem, string>>>()
                        {
                            new Tuple<string, Action<LinkItem, string>>("- AltName: ", (m, s) =>
                            {
                                m.AltName = s;
                            })
                        };

                        foreach (var property in properties)
                        {
                            if (str.StartsWith(property.Item1))
                            {
                                property.Item2.Invoke(this, str.Substring(property.Item1.Length));
                                break;
                            }
                        }
                    }
                }
                enumerator.MoveNext();
            }
        }
    }
}
