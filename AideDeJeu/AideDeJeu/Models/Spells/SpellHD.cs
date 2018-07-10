using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;

namespace AideDeJeuLib.Spells
{
    public class SpellHD : Spell
    {
        public override string LevelType
        {
            get
            {
                if (int.Parse(Level) > 0)
                {
                    if (string.IsNullOrEmpty(Rituel))
                    {
                        return $"{Type} de niveau {Level}";
                    }
                    else
                    {
                        return $"{Type} de niveau {Level} {Rituel}";
                    }
                }
                else
                {
                    return $"{Type}, tour de magie";
                }
            }
            set
            {
                var re = new Regex("(?<type>.*) de niveau (?<level>\\d).?(?<rituel>\\(rituel\\))?");
                var match = re.Match(value);
                this.Type = match.Groups["type"].Value;
                this.Level = match.Groups["level"].Value;
                this.Rituel = match.Groups["rituel"].Value;
                if (string.IsNullOrEmpty(this.Type))
                {
                    re = new Regex("(?<type>.*), (?<level>tour de magie)");
                    match = re.Match(value);
                    if (match.Groups["level"].Value == "tour de magie")
                    {
                        this.Type = match.Groups["type"].Value;
                        this.Level = "0"; // match.Groups["level"].Value;
                        this.Rituel = match.Groups["rituel"].Value;
                    }
                }
            }
        }
        public override string Markdown
        {
            get
            {
                return
                    $"# {Name}\n" +
                    $"{NameVO}\n" +
                    $"_{LevelType}_\n" +
                    $"**Temps d'incantation :** {CastingTime}\n" +
                    $"**Portée :** {Range}\n" +
                    $"**Composantes :** {Components}\n" +
                    $"**Durée :** {Duration}\n\n" +
                    $"{DescriptionHtml}\n\n" +
                    $"**Source :** {Source}";

            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                    {
                        if(this.Name != null)
                        {
                            return;
                        }
                        this.Name = headingBlock.Inline.ToMarkdownString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    this.DescriptionHtml += MarkdownExtensions.MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                    ////DumpParagraphBlock(paragraphBlock);
                    //Console.WriteLine(paragraphBlock.IsBreakable);
                    //spell.DescriptionHtml += paragraphBlock.Inline.ToContainerString();
                    //if(paragraphBlock.IsBreakable)
                    //{
                    //    spell.DescriptionHtml += "\n";
                    //}
                }
                if (block is Markdig.Syntax.ListBlock)
                {
                    var listBlock = block as Markdig.Syntax.ListBlock;
                    //DumpListBlock(listBlock);
                    if (listBlock.BulletType == '-')
                    {
                        this.Source = "";
                        foreach (var inblock in listBlock)
                        {
                            //DumpBlock(inblock);
                            var regex = new Regex("(?<key>.*?): (?<value>.*)");
                            if (inblock is Markdig.Syntax.ListItemBlock)
                            {
                                var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    //DumpBlock(ininblock);
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        //DumpParagraphBlock(paragraphBlock);
                                        var str = paragraphBlock.Inline.ToMarkdownString();

                                        var properties = new List<Tuple<string, Action<Spell, string>>>()
                                        {
                                            new Tuple<string, Action<Spell, string>>("NameVO: ", (m, s) => m.NameVO = s),
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
                                    //DumpBlock(ininblock);
                                    if (ininblock is Markdig.Syntax.ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                        this.DescriptionHtml += listBlock.BulletType + " " + MarkdownExtensions.MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
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
                //if (enumerator.Current is Markdig.Syntax.LinkReferenceDefinitionGroup)
                //{
                //    var linkReferenceDefinitionGroup = enumerator.Current as Markdig.Syntax.LinkReferenceDefinitionGroup;
                //    var linkReferenceDefinition = linkReferenceDefinitionGroup.FirstOrDefault() as Markdig.Syntax.LinkReferenceDefinition;
                //    var label = linkReferenceDefinition.Label;
                //    var title = linkReferenceDefinition.Title;
                //    var url = linkReferenceDefinition.Url;
                //    if (label == "//")
                //    {
                //        return;
                //    }
                //}
                enumerator.MoveNext();
            }

        }
    }
}
