using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace AideDeJeuLib
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
        //public override string Markdown
        //{
        //    get
        //    {
        //        return
        //            $"# {Name}\n" +
        //            $"{AltName}\n\n" +
        //            $"_{LevelType}_\n" +
        //            $"**Temps d'incantation :** {CastingTime}\n" +
        //            $"**Portée :** {Range}\n" +
        //            $"**Composantes :** {Components}\n" +
        //            $"**Durée :** {Duration}\n" +
        //            $"**Classes :** {Classes}\n" +
        //            $"**Source :** {Source}\n" +
        //            $"\n" +
        //            $"{DescriptionHtml}";
        //    }
        //}

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
                                            new Tuple<string, Action<Spell, string>>("**Temps d'incantation :** ", (m, s) => m.CastingTime = s),
                                            new Tuple<string, Action<Spell, string>>("**Composantes :** ", (m, s) => m.Components = s),
                                            new Tuple<string, Action<Spell, string>>("**Durée :** ", (m, s) => m.Duration = s),
                                            new Tuple<string, Action<Spell, string>>("LevelType: ", (m, s) => m.LevelType = s),
                                            new Tuple<string, Action<Spell, string>>("**Portée :** ", (m, s) => m.Range = s),
                                            new Tuple<string, Action<Spell, string>>("Source: ", (m, s) => m.Source = s),
                                            new Tuple<string, Action<Spell, string>>("Classes: ", (m, s) => m.Classes = s),
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
