using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using AideDeJeuLib.Monsters;
using Markdig;

namespace AideDeJeu.Tools
{
    public static class MarkdownExtensions
    {
        public static IEnumerable<Spell> MarkdownToSpells(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
            return document.ToSpells();
        }

        public static IEnumerable<Monster> MarkdownToMonsters(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
            return document.ToMonsters();
        }

        public static string MarkdownToHtml(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            return Markdown.ToHtml(md, pipeline);
        }

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
                            //yield return spell;
                        }
                        spell = new Spell();
                        spell.Id = spell.IdVF = spell.IdVO = spell.Name = spell.NamePHB = headingBlock.Inline.ToContainerString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    spell.DescriptionHtml += MarkdownToHtml(paragraphBlock.ToParagraphString());
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
                                        spell.DescriptionHtml += listBlock.BulletType + " " + MarkdownToHtml(paragraphBlock.ToParagraphString());
                                    }
                                }
                            }
                        }
                    }
                }

            }
            if (spell != null)
            {
                //yield return spell;
                spells.Add(spell);
            }
            return spells;
        }

        public static IEnumerable<Monster> ToMonsters(this Markdig.Syntax.MarkdownDocument document)
        {
            var monsters = new List<Monster>();
            Monster monster = null;
            List<string> features = null;
            List<string> specialFeatures = null;
            List<string> actions = null;
            List<string> legendaryActions = null;
            foreach (var block in document)
            {
                //Debug.WriteLine(block.GetType());
                //DumpBlock(block);
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                    {
                        if (monster != null)
                        {
                            monster.SpecialFeatures = specialFeatures;
                            monster.Actions = actions;
                            monster.LegendaryActions = legendaryActions;
                            specialFeatures = null;
                            actions = null;
                            legendaryActions = null;
                            features = null;
                            monsters.Add(monster);
                            //yield return monster;
                        }
                        monster = new Monster();
                        monster.Name = monster.NamePHB = headingBlock.Inline.ToContainerString();
                        //Console.WriteLine(spell.Name);
                    }
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 2)
                    {
                        switch(headingBlock.Inline.ToContainerString())
                        {
                            case "Capacités":
                                features = specialFeatures = new List<string>();
                                break;
                            case "Actions":
                                features = actions = new List<string>();
                                break;
                            case "Actions légendaires":
                                features = legendaryActions = new List<string>();
                                break;
                        }
                    }
                }
                else if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    features?.Add(MarkdownToHtml(paragraphBlock.ToParagraphString()));
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

                                        var properties = new List<Tuple<string, Action<Monster, string>>>()
                                        {
                                            new Tuple<string, Action<Monster, string>>("Classe d'armure ", (m, s) => m.ArmorClass = s),
                                            new Tuple<string, Action<Monster, string>>("Points de vie ", (m, s) => m.HitPoints = s),
                                            new Tuple<string, Action<Monster, string>>("Vitesse ", (m, s) => m.Speed = s),
                                            new Tuple<string, Action<Monster, string>>("Résistance aux dégâts ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("Résistances aux dégâts ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("Résistance contre les dégâts ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("Résistances contre les dégâts ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité contre les dégâts ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité contre des dégâts ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité aux dégâts ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité à l'état ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunités à l'état ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité contre l'état ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunité contre les états ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Immunités contre les états ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("Vulnérabilité ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("Sens ", (m, s) => m.Senses = s),
                                            new Tuple<string, Action<Monster, string>>("Langue ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("Dangerosité ", (m, s) => m.Challenge = s),
                                            new Tuple<string, Action<Monster, string>>("Jets de sauvegarde ", (m, s) => m.Challenge = s),
                                            new Tuple<string, Action<Monster, string>>("Jet de sauvegarde ", (m, s) => m.Challenge = s),
                                            new Tuple<string, Action<Monster, string>>("Compétences ", (m, s) => m.Skills = s),
                                            new Tuple<string, Action<Monster, string>>("Compétence ", (m, s) => m.Skills = s),
                                            new Tuple<string, Action<Monster, string>>("Langues ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("", (m,s) =>
                                            {
                                                if (m.Alignment != null)
                                                {
                                                    App.Current.MainPage.DisplayAlert("Erreur de parsing", s, "OK");
                                                }
                                                //Debug.Assert(monster.Alignment == null, str);
                                                var regexx = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)");
                                                var matchh = regexx.Match(s);
                                                m.Alignment = matchh.Groups["alignment"].Value;
                                                m.Size = matchh.Groups["size"].Value;
                                                m.Type = matchh.Groups["type"].Value;
                                            })
                                        };

                                        foreach(var property in properties)
                                        {
                                            if(str.StartsWith(property.Item1))
                                            {
                                                property.Item2.Invoke(monster, str.Substring(property.Item1.Length));
                                                break;
                                            }
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
                                        features?.Add(MarkdownToHtml(listBlock.BulletType + " " + paragraphBlock.ToParagraphString()));
                                    }
                                }
                            }
                        }
                    }
                }
                else if(block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    var table = tableBlock.ToTable();
                    monster.Strength = table["FOR"].FirstOrDefault();
                    monster.Dexterity = table["DEX"].FirstOrDefault();
                    monster.Constitution = table["CON"].FirstOrDefault();
                    monster.Intelligence = table["INT"].FirstOrDefault();
                    monster.Wisdom = table["SAG"].FirstOrDefault();
                    monster.Charisma = table["CHA"].FirstOrDefault();
                }
            }
            if (monster != null)
            {
                monster.SpecialFeatures = specialFeatures;
                monster.Actions = actions;
                monster.LegendaryActions = legendaryActions;
                monsters.Add(monster);
                //yield return monster;
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
                //Debug.WriteLine(inline.GetType());
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
                //Debug.WriteLine(add);
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

        public static Dictionary<string, List<string>> ToTable(this Markdig.Extensions.Tables.Table tableBlock)
        {
            var table = new Dictionary<string, List<string>>();
            var indexes = new Dictionary<int, string>();
            foreach (var blockrow in tableBlock)
            {
                var row = blockrow as Markdig.Extensions.Tables.TableRow;
                int indexCol = 0;
                foreach (var blockcell in row)
                {
                    var cell = blockcell as Markdig.Extensions.Tables.TableCell;
                    foreach (var blockpar in cell)
                    {
                        var par = blockpar as Markdig.Syntax.ParagraphBlock;
                        var name = par.ToParagraphString().Trim();
                        if (row.IsHeader)
                        {
                            indexes[indexCol] = name;
                            table[name] = new List<string>();
                        }
                        else
                        {
                            table[indexes[indexCol]].Add(name);
                        }
                    }
                    indexCol++;
                }
            }
            return table;
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
            //Debug.WriteLine(block.BulletType);
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
            //Debug.WriteLine(block.HeaderChar);
            //Debug.WriteLine(block.Level);
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
            //Debug.WriteLine(block.Column);
            //Debug.WriteLine(block.IsBreakable);
            //Debug.WriteLine(block.IsOpen);
            //Debug.WriteLine(block.Line);
            //Debug.WriteLine(block.RemoveAfterProcessInlines);
            //Debug.WriteLine(block.Span.ToString());
            //Debug.WriteLine(block.Span.ToString(MD));
            //Debug.WriteLine(block.ToString());
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
                //Debug.WriteLine(block.GetType());
                //block.Dump();
            }
        }


    }
}
