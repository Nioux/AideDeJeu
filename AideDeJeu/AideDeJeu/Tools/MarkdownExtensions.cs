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
using AideDeJeuLib;

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
            //var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            //return Markdown.ToHtml(md, pipeline);
            return md;
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
                        spell.Name = headingBlock.Inline.ToMarkdownString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    spell.DescriptionHtml += MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
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
                                                //if (m.Alignment != null)
                                                //{
                                                    //App.Current.MainPage.DisplayAlert("Erreur de parsing", s, "OK");
                                                //}
                                                ////Debug.Assert(monster.Alignment == null, str);
                                                //var regexx = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)");
                                                //var matchh = regexx.Match(s);
                                                //m.Alignment = matchh.Groups["alignment"].Value;
                                                //m.Size = matchh.Groups["size"].Value;
                                                //m.Type = matchh.Groups["type"].Value;
                                            })
                                        };

                                        foreach (var property in properties)
                                        {
                                            if (str.StartsWith(property.Item1))
                                            {
                                                property.Item2.Invoke(spell, str.Substring(property.Item1.Length));
                                                break;
                                            }
                                        }

                                        /*var match = regex.Match(str);
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
                                        }*/
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
                                        spell.DescriptionHtml += listBlock.BulletType + " " + MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    spell.DescriptionHtml += "\n\n" + tableBlock.ToMarkdownString() + "\n\n";
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
            List<string> reactions = null;
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
                            monster.Reactions = reactions;
                            monster.LegendaryActions = legendaryActions;
                            specialFeatures = null;
                            actions = null;
                            reactions = null;
                            legendaryActions = null;
                            features = null;
                            monsters.Add(monster);
                            //yield return monster;
                        }
                        monster = new Monster();
                        monster.Name = headingBlock.Inline.ToMarkdownString();
                        //Console.WriteLine(spell.Name);
                    }
                    if (headingBlock.HeaderChar == '#' && headingBlock.Level == 2)
                    {
                        switch (headingBlock.Inline.ToMarkdownString())
                        {
                            case "Capacités":
                            case "Special Features":
                                features = specialFeatures = new List<string>();
                                break;
                            case "Actions":
                                features = actions = new List<string>();
                                break;
                            case "Réactions":
                                features = reactions = new List<string>();
                                break;
                            case "Actions légendaires":
                            case "Legendary Actions":
                                features = legendaryActions = new List<string>();
                                break;
                        }
                    }
                }
                else if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    features?.Add(paragraphBlock.ToMarkdownString());
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
                                        var str = paragraphBlock.Inline.ToMarkdownString();

                                        var properties = new List<Tuple<string, Action<Monster, string>>>()
                                        {
                                            new Tuple<string, Action<Monster, string>>("**Classe d'armure** ", (m, s) => m.ArmorClass = s),
                                            new Tuple<string, Action<Monster, string>>("**Points de vie** ", (m, s) => m.HitPoints = s),
                                            new Tuple<string, Action<Monster, string>>("**Vitesse** ", (m, s) => m.Speed = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistance aux dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistances aux dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistance contre les dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistances contre les dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre les dégâts** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre des dégâts** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité aux dégâts** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité à l'état** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunités à l'état** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre l'état** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre les états** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunités contre les états** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Vulnérabilité aux dégâts** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Vulnérabilité contre les dégâts** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Vulnérabilité** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Sens** ", (m, s) => m.Senses = s),
                                            new Tuple<string, Action<Monster, string>>("**Langue** ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("**Langues** ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("**Dangerosité** ", (m, s) => m.Challenge = s),
                                            new Tuple<string, Action<Monster, string>>("**Jets de sauvegarde** ", (m, s) => m.SavingThrows = s),
                                            new Tuple<string, Action<Monster, string>>("**Jet de sauvegarde** ", (m, s) => m.SavingThrows = s),
                                            new Tuple<string, Action<Monster, string>>("**Compétences** ", (m, s) => m.Skills = s),
                                            new Tuple<string, Action<Monster, string>>("**Compétence** ", (m, s) => m.Skills = s),

                                            new Tuple<string, Action<Monster, string>>("**Armor Class** ", (m, s) => m.ArmorClass = s),
                                            new Tuple<string, Action<Monster, string>>("**Hit Points** ", (m, s) => m.HitPoints = s),
                                            new Tuple<string, Action<Monster, string>>("**Speed** ", (m, s) => m.Speed = s),
                                            new Tuple<string, Action<Monster, string>>("**Damage Resistance** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Damage Resistances** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistance contre les dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Résistances contre les dégâts** ", (m, s) => m.DamageResistances = s),
                                            new Tuple<string, Action<Monster, string>>("**Damage Immunities** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre des dégâts** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité aux dégâts** ", (m, s) => m.DamageImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**State Immunities** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunités à l'état** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre l'état** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunité contre les états** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Immunités contre les états** ", (m, s) => m.ConditionImmunities = s),
                                            new Tuple<string, Action<Monster, string>>("**Damage Vulnerabilities** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Vulnérabilité contre les dégâts** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Vulnérabilité** ", (m, s) => m.DamageVulnerabilities = s),
                                            new Tuple<string, Action<Monster, string>>("**Senses** ", (m, s) => m.Senses = s),
                                            new Tuple<string, Action<Monster, string>>("**Languages** ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("**Langues** ", (m, s) => m.Languages = s),
                                            new Tuple<string, Action<Monster, string>>("**Challenge** ", (m, s) => m.Challenge = s),
                                            new Tuple<string, Action<Monster, string>>("**Saving Throws** ", (m, s) => m.SavingThrows = s),
                                            new Tuple<string, Action<Monster, string>>("**Jet de sauvegarde** ", (m, s) => m.SavingThrows = s),
                                            new Tuple<string, Action<Monster, string>>("**Skills** ", (m, s) => m.Skills = s),
                                            new Tuple<string, Action<Monster, string>>("**Compétence** ", (m, s) => m.Skills = s),

                                            new Tuple<string, Action<Monster, string>>("NameVO: ", (m, s) => m.NameVO = s),

                                            new Tuple<string, Action<Monster, string>>("", (m,s) =>
                                            {
                                                if (!string.IsNullOrEmpty(m.Alignment))
                                                {
                                                    App.Current.MainPage.DisplayAlert("Erreur de parsing", s, "OK");
                                                }
                                                else
                                                {
                                                    //Debug.Assert(monster.Alignment == null, str);
                                                    var regexx = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)");
                                                    var matchh = regexx.Match(s);
                                                    m.Alignment = matchh.Groups["alignment"].Value;
                                                    m.Size = matchh.Groups["size"].Value;
                                                    m.Type = matchh.Groups["type"].Value;
                                                    if(string.IsNullOrEmpty(m.Alignment))
                                                    {
                                                        regexx = new Regex("(?<size>.*?) (?<type>.*?), (?<alignment>.*)");
                                                        matchh = regexx.Match(s);
                                                        m.Alignment = matchh.Groups["alignment"].Value;
                                                        m.Size = matchh.Groups["size"].Value;
                                                        m.Type = matchh.Groups["type"].Value;
                                                    }
                                                }
                                            })
                                        };

                                        foreach (var property in properties)
                                        {
                                            if (str.StartsWith(property.Item1))
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
                                        features?.Add(listBlock.BulletType + " " + paragraphBlock.ToMarkdownString());
                                    }
                                }
                            }
                        }
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    var table = tableBlock.ToTable();
                    if (table.ContainsKey("FOR"))
                    {
                        monster.Strength = table["FOR"].FirstOrDefault();
                        monster.Dexterity = table["DEX"].FirstOrDefault();
                        monster.Constitution = table["CON"].FirstOrDefault();
                        monster.Intelligence = table["INT"].FirstOrDefault();
                        monster.Wisdom = table["SAG"].FirstOrDefault();
                        monster.Charisma = table["CHA"].FirstOrDefault();
                    }
                    else if (table.ContainsKey("STR"))
                    {
                        monster.Strength = table["STR"].FirstOrDefault();
                        monster.Dexterity = table["DEX"].FirstOrDefault();
                        monster.Constitution = table["CON"].FirstOrDefault();
                        monster.Intelligence = table["INT"].FirstOrDefault();
                        monster.Wisdom = table["WIS"].FirstOrDefault();
                        monster.Charisma = table["CHA"].FirstOrDefault();
                    }
                    //else
                    //{
                        features?.Add(tableBlock.ToMarkdownString());
                    //}
                }
                else if (block is Markdig.Syntax.LinkReferenceDefinitionGroup)
                {

                    var linkReferenceDefinitionGroup = block as Markdig.Syntax.LinkReferenceDefinitionGroup;

                    foreach (var linkBlock in linkReferenceDefinitionGroup)
                    {
                        var linkReferenceDefinition = linkBlock as Markdig.Syntax.LinkReferenceDefinition;
                        //linkReferenceDefinition.
                    }
                }
                else if (block is Markdig.Syntax.LinkReferenceDefinition)
                {
                    Debug.WriteLine(block.GetType());
                }
                else
                {
                    Debug.WriteLine(block.GetType());
                }
            }
            if (monster != null)
            {
                monster.SpecialFeatures = specialFeatures;
                monster.Actions = actions;
                monster.Reactions = reactions;
                monster.LegendaryActions = legendaryActions;
                monsters.Add(monster);
                //yield return monster;
            }
            return monsters;
        }

        public static string ToMarkdownString(this Markdig.Syntax.Inlines.ContainerInline inlines)
        {
            var str = string.Empty;
            foreach (var inline in inlines)
            {
                str += inline.ToMarkdownString();
            }
            return str;
        }

        public static string ToMarkdownString(this Markdig.Syntax.Inlines.Inline inline)
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
                add = delimiterChar + emphasisInline.ToMarkdownString() + delimiterChar;
            }
            else if (inline is Markdig.Syntax.Inlines.LinkInline)
            {
                var linkInline = inline as Markdig.Syntax.Inlines.LinkInline;
                add = string.Empty;
                if (linkInline.IsImage)
                {
                    add = "!";
                }
                var label = linkInline.ToMarkdownString();
                var url = linkInline.Url;
                var title = linkInline.Title;
                if (!string.IsNullOrEmpty(title))
                {
                    add += string.Format($"[{label}]({url} \"{title}\")");
                }
                else
                {
                    add += string.Format($"[{label}]({url})");
                }
            }
            else if (inline is Markdig.Syntax.Inlines.ContainerInline)
            {
                var containerInline = inline as Markdig.Syntax.Inlines.ContainerInline;
                add = containerInline.ToMarkdownString();
            }
            else
            {
                add = inline.ToString();
            }
            //Debug.WriteLine(add);
            return add;
        }

        public static string ToMarkdownString(this Markdig.Syntax.ParagraphBlock paragraphBlock)
        {
            var str = string.Empty;
            str += paragraphBlock.Inline.ToMarkdownString();
            if (paragraphBlock.IsBreakable)
            {
                str += "\n";
            }
            return str;
        }
        public static string ToMarkdownString(this Markdig.Extensions.Tables.Table tableBlock)
        {
            var ret = string.Empty;
            foreach(Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                var line = "|";
                foreach(Markdig.Extensions.Tables.TableCell cell in row)
                {
                    foreach(Markdig.Syntax.ParagraphBlock block in cell)
                    {
                        line += block.ToMarkdownString().Replace("\n", "");
                    }
                    line += "|";
                }
                if(row.IsHeader)
                {
                    line += "\n|";
                    for(int i = 0; i < row.Count; i++)
                    {
                        line += "---|";
                    }
                }
                ret += line + "\n";
            }
            return ret;
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
                        var name = par.ToMarkdownString().Trim();
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
            foreach (var spell in spells.OrderBy(s => s.Name))
            {
                md += spell.ToMarkdownString();
            }

            return md;
        }
        public static string ToMarkdownString(this Spell spell)
        {
            var md = string.Empty;
            md += string.Format("# {0}\n", spell.Name);
            md += string.Format("- NameVO: [{0}]\n", spell.NameVO);
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
                .Replace("<div class=\"description \">", "")
                .Replace("</div>", "")
                .Replace("<strong>", "**")
                .Replace("</strong>", "**")
                .Replace("<em>", "_")
                .Replace("</em>", "_")
                .Replace("<li>", "* ")
                .Replace("</li>", "")
                //.Replace("\n", "\n\n")
                //.Replace("<br/>", "\n\n")
                .Replace("\n", "\n")
                .Replace("<br/>", "\n")
                ;
            md += string.Format("[{0}]: spells_hd.md#{1}\n", spell.NameVO, Helpers.IdFromName(spell.NameVO));
            md += "\n\n";
            return md;
        }

        public static string ToMarkdownString(this IEnumerable<Monster> monsters)
        {
            var md = string.Empty;
            foreach (var monster in monsters)
            {
                md += monster.ToMarkdownString();
            }
            return md;
        }
        public static string ToMarkdownString(this Monster monster)
        {
            var md = string.Empty;
            md += string.Format("# {0}\n", monster.Name?.Trim());
            md += string.Format("- NameVO: [{0}]\n", monster.NameVO?.Trim());
            md += string.Format("- {0} {1}, {2}\n", monster.Size?.Trim(), monster.Type?.Trim(), monster.Alignment?.Trim());
            if (monster.ArmorClass != null) md += string.Format("- **Armor Class** {0}\n", monster.ArmorClass?.Trim());
            if (monster.HitPoints != null) md += string.Format("- **Hit Points** {0}\n", monster.HitPoints?.Trim());
            if (monster.Speed != null) md += string.Format("- **Speed** {0}\n", monster.Speed?.Trim());
            md += "\n";
            md += "|  STR  |  DEX  |  CON  |  INT  |  WIS  |  CHA  |\n";
            md += "| ---   | ---   | ---   | ---   | ---   | ---   |\n";
            md += string.Format("|{0,7}|{1,7}|{2,7}|{3,7}|{4,7}|{5,7}|\n", monster.Strength?.Trim(), monster.Dexterity?.Trim(), monster.Constitution?.Trim(), monster.Intelligence?.Trim(), monster.Wisdom?.Trim(), monster.Charisma?.Trim());
            md += "\n";
            if (monster.SavingThrows != null) md += string.Format("- **Saving Throws** {0}\n", monster.SavingThrows?.Trim());
            if (monster.Skills != null) md += string.Format("- **Skills** {0}\n", monster.Skills?.Trim());
            if (monster.Senses != null) md += string.Format("- **Senses** {0}\n", monster.Senses?.Trim());
            if (monster.Languages != null) md += string.Format("- **Languages** {0}\n", monster.Languages?.Trim());
            if (monster.Challenge != null) md += string.Format("- **Challenge** {0}\n", monster.Challenge?.Trim());

            if (monster.ConditionImmunities != null) md += string.Format("- **Condition Immunities** {0}\n", monster.ConditionImmunities?.Trim());
            if (monster.DamageImmunities!= null) md += string.Format("- **Damage Immunities** {0}\n", monster.DamageImmunities?.Trim());
            if (monster.DamageResistances != null) md += string.Format("- **Damage Resistances** {0}\n", monster.DamageResistances?.Trim());
            if (monster.DamageVulnerabilities != null) md += string.Format("- **Damage Vulnerabilities** {0}\n", monster.DamageVulnerabilities?.Trim());
            //md += string.Format("- Components: {0}\n", monster.Components);
            //md += string.Format("- Duration: {0}\n", monster.Duration);
            //md += string.Format("- LevelType: {0}\n", monster.LevelType);
            //md += string.Format("- Range: {0}\n", monster.Range);
            //var regex = new Regex("(?<source>\\(.*\\)) (?<classes>.*)");
            //var match = regex.Match(monster.Source);
            //var source = match.Groups["source"].Value;
            //var classes = match.Groups["classes"].Value;
            //md += string.Format("- Source: {0}\n", source);
            //md += string.Format("- Classes: {0}\n", classes.Replace(" ;", ",").Trim().Trim(','));
            md += "\n";

            if (monster.SpecialFeatures != null)
            {
                md += "### Special Features\n\n";
                foreach (var specialFeature in monster.SpecialFeatures)
                {
                    md += HtmlToMarkdownString(specialFeature);
                }
            }

            if (monster.Actions != null)
            {
                md += "### Actions\n\n";
                foreach (var action in monster.Actions)
                {
                    md += HtmlToMarkdownString(action);
                }
            }

            if (monster.Reactions != null)
            {
                md += "### Reactions\n\n";
                foreach (var reaction in monster.Reactions)
                {
                    md += HtmlToMarkdownString(reaction);
                }
            }

            if (monster.LegendaryActions != null)
            {
                md += "### Legendary Actions\n\n";
                foreach (var legendaryAction in monster.LegendaryActions)
                {
                    md += HtmlToMarkdownString(legendaryAction);
                }
            }

            //md += monster
            //    .Description
            //    .Replace("<strong>", "**")
            //    .Replace("</strong>", "**")
            //    .Replace("<em>", "_")
            //    .Replace("</em>", "_")
            //    .Replace("<li>", "* ")
            //    .Replace("</li>", "")
            //    .Replace("\n", "\n\n")
            //    .Replace("<br/>", "\n\n")
            //    ;
            md += string.Format("[{0}]: monsters_hd.md#{1}\n", monster.NameVO, Helpers.IdFromName(monster.NameVO));
            md += "\n\n";
            return md;
        }

        public static string HtmlToMarkdownString(string html)
        {
            var regex = new Regex("(<a .*?>)");
            html = regex.Replace(html, "[");
            return html
                .Replace("</a>", "]")
                .Replace("<strong>", "**")
                .Replace("</strong>", "**")
                .Replace("<em>", "_")
                .Replace("</em>", "_")
                .Replace("<li>", "* ")
                .Replace("</li>", "")
                .Replace("\n", "\n\n")
                .Replace("<br/>", "\n\n")
                .Replace("<br />", "\n\n")
                .Replace("<p>", "")
                .Replace("</p>", "\n\n")
                ;
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
