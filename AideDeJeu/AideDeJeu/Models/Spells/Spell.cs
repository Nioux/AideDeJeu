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
    public abstract class Spell : Item
    {
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Rituel { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml { get; set; }
        public string Source { get; set; }

        public abstract string LevelType { get; set; }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                    {
                        if (this.Name != null)
                        {
                            return;
                        }
                        this.Name = headingBlock.Inline.ToMarkdownString();
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    if (block.IsNewItem())
                    {
                        return;
                    }
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;

                    this.DescriptionHtml += paragraphBlock.ToMarkdownString() + "\n";
                }
                if (block is Markdig.Syntax.ListBlock)
                {
                    var listBlock = block as Markdig.Syntax.ListBlock;
                    if (listBlock.BulletType == '-')
                    {
                        this.Source = "";
                        foreach (var inblock in listBlock)
                        {
                            var regex = new Regex("(?<key>.*?): (?<value>.*)");
                            if (inblock is Markdig.Syntax.ListItemBlock)
                            {
                                var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        var str = paragraphBlock.Inline.ToMarkdownString();

                                        var properties = new List<Tuple<string, Action<Spell, string>>>()
                                        {
                                            new Tuple<string, Action<Spell, string>>("AltName: ", (m, s) => m.AltName = s),
                                            new Tuple<string, Action<Spell, string>>("CastingTime: ", (m, s) => m.CastingTime = s),
                                            new Tuple<string, Action<Spell, string>>("Components: ", (m, s) => m.Components = s),
                                            new Tuple<string, Action<Spell, string>>("Duration: ", (m, s) => m.Duration = s),
                                            new Tuple<string, Action<Spell, string>>("LevelType: ", (m, s) => m.LevelType = s),
                                            new Tuple<string, Action<Spell, string>>("Range: ", (m, s) => m.Range = s),
                                            new Tuple<string, Action<Spell, string>>("Source: ", (m, s) => m.Source = s),
                                            new Tuple<string, Action<Spell, string>>("Classes: ", (m, s) => m.Source += s),
                                            new Tuple<string, Action<Spell, string>>("", (m,s) =>
                                            {
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
                            }
                        }
                    }
                    else
                    {
                        foreach (var inblock in listBlock)
                        {
                            if (inblock is Markdig.Syntax.ListItemBlock)
                            {
                                var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        this.DescriptionHtml += listBlock.BulletType + " " + paragraphBlock.ToMarkdownString() + "\n";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    this.DescriptionHtml += "\n\n" + tableBlock.ToMarkdownString() + "\n\n";
                }
                enumerator.MoveNext();
            }

        }
    }
}
