using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using AideDeJeuLib.Monsters;

namespace AideDeJeu.Tools
{
    public static class MarkdownExtensions
    {
        public static IEnumerable<Spell> ToSpells(this Markdig.Syntax.MarkdownDocument document)
        {
            var spells = new List<Spell>();
            Spell spell = null;
            foreach (var block in document)
            {
                //DumpBlock(block);
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                    {
                        if (spell != null)
                        {
                            spells.Add(spell);
                        }
                        spell = new Spell();
                        spell.Name = spell.NamePHB = headingBlock.Inline.ToContainerString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    spell.DescriptionHtml += paragraphBlock.ToParagraphString();
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
                        spell.Source = "";
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
                                        var str = paragraphBlock.Inline.ToContainerString();
                                        var match = regex.Match(str);
                                        var key = match.Groups["key"].Value;
                                        var value = match.Groups["value"].Value;
                                        switch (key)
                                        {
                                            case "NameVO":
                                                spell.NameVO = value;
                                                break;
                                            case "CastingTime":
                                                spell.CastingTime = value;
                                                break;
                                            case "Components":
                                                spell.Components = value;
                                                break;
                                            case "Duration":
                                                spell.Duration = value;
                                                break;
                                            case "LevelType":
                                                spell.LevelType = value;
                                                break;
                                            case "Range":
                                                spell.Range = value;
                                                break;
                                            case "Source":
                                                spell.Source += value + " ";
                                                break;
                                            case "Classes":
                                                spell.Source += value;
                                                break;
                                        }
                                    }
                                }

                                //DumpListItemBlock(inblock as Markdig.Syntax.ListItemBlock);
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
                                        spell.DescriptionHtml += listBlock.BulletType + " " + paragraphBlock.ToParagraphString();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            if (spell != null)
            {
                spells.Add(spell);
            }
            return spells;
        }

        public static IEnumerable<Monster> ToMonsters(this Markdig.Syntax.MarkdownDocument document)
        {
            var monsters = new List<Monster>();
            Monster monster = null;
            List<string> actions = new List<string>();
            foreach (var block in document)
            {
                //DumpBlock(block);
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                    {
                        if (monster != null)
                        {
                            monsters.Add(monster);
                        }
                        monster = new Monster();
                        monster.Name = monster.NamePHB = headingBlock.Inline.ToContainerString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                else if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    actions.Add(paragraphBlock.ToParagraphString());
                    ////DumpParagraphBlock(paragraphBlock);
                    //Console.WriteLine(paragraphBlock.IsBreakable);
                    //spell.DescriptionHtml += paragraphBlock.Inline.ToContainerString();
                    //if(paragraphBlock.IsBreakable)
                    //{
                    //    spell.DescriptionHtml += "\n";
                    //}
                }
                else if (block is Markdig.Syntax.ListBlock)
                {
                    var listBlock = block as Markdig.Syntax.ListBlock;
                    //DumpListBlock(listBlock);
                    if (listBlock.BulletType == '-')
                    {
                        monster.Source = "";
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
                                        var str = paragraphBlock.Inline.ToContainerString();
                                        var match = regex.Match(str);
                                        var key = match.Groups["key"].Value;
                                        var value = match.Groups["value"].Value;
                                        switch (key)
                                        {
                                            case "NameVO":
                                                monster.NameVO = value;
                                                break;
                                            case "CastingTime":
                                                {
                                                    var regexx = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)");
                                                    var matchh = regexx.Match(value);
                                                    monster.Alignment = matchh.Groups["alignment"].Value;
                                                    monster.Size = matchh.Groups["size"].Value;
                                                    monster.Type = matchh.Groups["type"].Value;
                                                }
                                                break;
                                            case "ArmorClass":
                                                monster.ArmorClass = value;
                                                break;
                                            case "HitPoints":
                                                monster.HitPoints = value;
                                                break;
                                            case "Speed":
                                                monster.Speed = value;
                                                break;
                                            case "SavingThrows":
                                                monster.SavingThrows = value;
                                                break;
                                            case "Skills":
                                                monster.Skills = value ;
                                                break;
                                            case "Senses":
                                                monster.Senses = value;
                                                break;
                                            case "Languages":
                                                monster.Languages = value;
                                                break;
                                            case "Challenge":
                                                monster.Challenge = value;
                                                break;
                                        }
                                    }
                                }

                                //DumpListItemBlock(inblock as Markdig.Syntax.ListItemBlock);
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
                                        actions.Add(listBlock.BulletType + " " + paragraphBlock.ToParagraphString());
                                    }
                                }
                            }
                        }
                    }
                }
                else if(block is Markdig.Extensions.Tables.Table)
                {
                    var table = block as Markdig.Extensions.Tables.Table;
                    foreach(var blockrow in table)
                    {
                        var row = blockrow as Markdig.Extensions.Tables.TableRow;
                        foreach(var blockcell in row)
                        {
                            var cell = blockcell as Markdig.Extensions.Tables.TableCell;
                            foreach(var blockpar in cell)
                            {
                                var par = blockpar as Markdig.Syntax.ParagraphBlock;
                                Debug.WriteLine(par.ToParagraphString());
                            }

                        }
                    }
                }

            }
            if (monster != null)
            {
                monsters.Add(monster);
            }
            return monsters;
        }

        public static string ToString(this Markdig.Syntax.SourceSpan span, string md)
        {
            return md.Substring(span.Start, span.Length);
        }
        public static string ToContainerString(this Markdig.Syntax.Inlines.ContainerInline inlines)
        {
            var str = string.Empty;
            foreach (var inline in inlines)
            {
                Debug.WriteLine(inline.GetType());
                string add = string.Empty;
                if (inline is Markdig.Syntax.Inlines.LineBreakInline)
                {
                    add = "\n";
                }
                else if (inline is Markdig.Syntax.Inlines.LiteralInline)
                {
                    var literalInline = inline as Markdig.Syntax.Inlines.LiteralInline;
                    add = literalInline.Content.ToString();
                }
                else if (inline is Markdig.Syntax.Inlines.EmphasisInline)
                {
                    var emphasisInline = inline as Markdig.Syntax.Inlines.EmphasisInline;
                    var delimiterChar = emphasisInline.DelimiterChar.ToString();
                    if (emphasisInline.IsDouble)
                    {
                        delimiterChar += delimiterChar;
                    }
                    add = delimiterChar + emphasisInline.ToContainerString() + delimiterChar;
                }
                else if (inline is Markdig.Syntax.Inlines.ContainerInline)
                {
                    var containerInline = inline as Markdig.Syntax.Inlines.ContainerInline;
                    add = containerInline.ToContainerString();
                }
                else
                {
                    add = inline.ToString();
                }
                Debug.WriteLine(add);
                str += add;
            }
            return str;
        }
        public static string ToParagraphString(this Markdig.Syntax.ParagraphBlock paragraphBlock)
        {
            var str = string.Empty;
            str += paragraphBlock.Inline.ToContainerString();
            if (paragraphBlock.IsBreakable)
            {
                str += "\n";
            }
            return str;
        }

        public static string ToMarkdownString(this IEnumerable<Spell> spells)
        {
            var md = string.Empty;
            foreach (var spell in spells)
            {
                md += spell.ToMarkdownString();
            }
            return md;
        }
        public static string ToMarkdownString(this Spell spell)
        {
            var md = string.Empty;
            md += string.Format("# {0}\n", spell.NamePHB);
            md += string.Format("- NameVO: {0}\n", spell.NameVO);
            md += string.Format("- CastingTime: {0}\n", spell.CastingTime);
            md += string.Format("- Components: {0}\n", spell.Components);
            md += string.Format("- Duration: {0}\n", spell.Duration);
            md += string.Format("- LevelType: {0}\n", spell.LevelType);
            md += string.Format("- Range: {0}\n", spell.Range);
            var regex = new Regex("(?<source>\\(.*\\)) (?<classes>.*)");
            var match = regex.Match(spell.Source);
            var source = match.Groups["source"].Value;
            var classes = match.Groups["classes"].Value;
            md += string.Format("- Source: {0}\n", source);
            md += string.Format("- Classes: {0}\n", classes.Replace(" ;", ",").Trim().Trim(','));
            md += "\n";
            md += "### Description\n\n";
            md += spell
                .DescriptionHtml
                .Replace("<strong>", "**")
                .Replace("</strong>", "**")
                .Replace("<em>", "_")
                .Replace("</em>", "_")
                .Replace("<li>", "* ")
                .Replace("</li>", "")
                .Replace("\n", "\r\n\r\n")
                .Replace("<br/>", "\r\n\r\n")
                ;
            md += "\n\n";
            return md;
        }

        public static void Dump(this Markdig.Syntax.ParagraphBlock block)
        {
            //if (block.Lines != null)
            //{
            //    foreach (var line in block.Lines)
            //    {
            //        var stringline = line as Markdig.Helpers.StringLine?;
            //        Debug.WriteLine(stringline.ToString());
            //    }
            //}
        }
        public static void Dump(this Markdig.Syntax.ListBlock block)
        {
            Debug.WriteLine(block.BulletType);
            foreach (var inblock in block)
            {
                inblock.Dump();
            }
        }
        public static void Dump(Markdig.Syntax.ListItemBlock block)
        {
            foreach (var inblock in block)
            {
                inblock.Dump();
            }
        }
        public static void Dump(this Markdig.Syntax.HeadingBlock block)
        {
            Debug.WriteLine(block.HeaderChar);
            Debug.WriteLine(block.Level);
            //foreach(var line in block.Lines.Lines)
            //{
            //    DumpStringLine(line);
            //}
        }
        public static void Dump(this Markdig.Helpers.StringLine line)
        {
            Console.WriteLine(line.ToString());
        }
        public static void Dump(this Markdig.Syntax.Block block)
        {
            Debug.WriteLine(block.Column);
            Debug.WriteLine(block.IsBreakable);
            Debug.WriteLine(block.IsOpen);
            Debug.WriteLine(block.Line);
            Debug.WriteLine(block.RemoveAfterProcessInlines);
            Debug.WriteLine(block.Span.ToString());
            //Debug.WriteLine(block.Span.ToString(MD));
            Debug.WriteLine(block.ToString());
            if (block is Markdig.Syntax.ParagraphBlock)
            {
                (block as Markdig.Syntax.ParagraphBlock).Dump();
            }
            if (block is Markdig.Syntax.ListBlock)
            {
                (block as Markdig.Syntax.ListBlock).Dump();
            }
            if (block is Markdig.Syntax.HeadingBlock)
            {
                (block as Markdig.Syntax.HeadingBlock).Dump();
            }
            if (block is Markdig.Syntax.ListItemBlock)
            {
                (block as Markdig.Syntax.ListItemBlock).Dump();
            }
        }
        public static void Dump(this Markdig.Syntax.MarkdownDocument document)
        {
            foreach (var block in document)
            {
                Debug.WriteLine(block.GetType());
                //block.Dump();
            }
        }


    }
}
