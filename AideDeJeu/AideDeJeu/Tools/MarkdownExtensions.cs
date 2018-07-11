using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Markdig;
using AideDeJeuLib;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace AideDeJeu.Tools
{
    public static class MarkdownExtensions
    {
        public static IEnumerable<TSpell> MarkdownToSpells<TSpell>(string md) where TSpell : Spell, new()
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
            return document.ToSpells<TSpell>();
        }

        public static IEnumerable<Monster> MarkdownToMonsters<TMonster>(string md) where TMonster : Monster, new()
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
            return document.ToMonsters<TMonster>();
        }

        public static IEnumerable<TCondition> MarkdownToConditions<TCondition>(string md) where TCondition : Condition, new()
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
            return document.ToConditions<TCondition>();
        }

        public static string MarkdownToHtml(string md)
        {
            //var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            //return Markdown.ToHtml(md, pipeline);
            return md;
        }


        public static Item ToItem(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);

            var enumerator = document.GetEnumerator();
            try
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (enumerator.Current is Markdig.Syntax.ParagraphBlock)
                    {
                        if(block.IsNewItem())
                        {
                            var item = block.GetNewItem();
                            item.Parse(ref enumerator);
                            return item;
                        }
                    }
                    enumerator.MoveNext();
                }
                
            }
            finally
            {
                enumerator.Dispose();
            }
            return null;
        }

        public static bool IsNewItem(this Block block)
        {
            var paragraphBlock = block as ParagraphBlock;
            var linkInline = paragraphBlock?.Inline?.FirstChild as LinkInline;
            if (linkInline != null)
            {
                var title = linkInline.Title;
                if (title == string.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static Item GetNewItem(this Block block)
        {
            var paragraphBlock = block as ParagraphBlock;
            var linkInline = paragraphBlock?.Inline?.FirstChild as LinkInline;
            if (linkInline != null)
            {
                var label = linkInline.Label;
                var title = linkInline.Title;
                var url = linkInline.Url;
                if (title == string.Empty)
                {
                    var name = $"AideDeJeuLib.{label}, AideDeJeu";
                    var type = Type.GetType(name);
                    var instance = Activator.CreateInstance(type) as Item;
                    return instance;
                }
            }
            return null;
        }

        public static IEnumerable<TSpell> ToSpells<TSpell>(this Markdig.Syntax.MarkdownDocument document) where TSpell : Spell, new()
        {
            var spells = new List<TSpell>();
            TSpell spell = null;
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
                        spell = new TSpell();
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

                                        var properties = new List<Tuple<string, Action<TSpell, string>>>()
                                        {
                                            new Tuple<string, Action<TSpell, string>>("NameVO: ", (m, s) => m.NameVO = s),
                                            new Tuple<string, Action<TSpell, string>>("CastingTime: ", (m, s) => m.CastingTime = s),
                                            new Tuple<string, Action<TSpell, string>>("Components: ", (m, s) => m.Components = s),
                                            new Tuple<string, Action<TSpell, string>>("Duration: ", (m, s) => m.Duration = s),
                                            new Tuple<string, Action<TSpell, string>>("LevelType: ", (m, s) => m.LevelType = s),
                                            new Tuple<string, Action<TSpell, string>>("Range: ", (m, s) => m.Range = s),
                                            new Tuple<string, Action<TSpell, string>>("Source: ", (m, s) => m.Source = s),
                                            new Tuple<string, Action<TSpell, string>>("Classes: ", (m, s) => m.Source += s),
                                            new Tuple<string, Action<TSpell, string>>("", (m,s) =>
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

        public static IEnumerable<TMonster> ToMonsters<TMonster>(this Markdig.Syntax.MarkdownDocument document) where TMonster : Monster, new()
        {
            var monsters = new List<TMonster>();
            TMonster monster = null;
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
                        monster = new TMonster();
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
                            case "Réaction":
                            case "Réactions":
                            case "Reaction":
                            case "Reactions":
                                features = reactions = new List<string>();
                                break;
                            case "Actions légendaires":
                            case "Legendary Actions":
                                features = legendaryActions = new List<string>();
                                break;
                            default:
                                App.Current.MainPage.DisplayAlert("Erreur de parsing", headingBlock.Inline.ToMarkdownString(), "OK");
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
                                            new Tuple<string, Action<Monster, string>>("**Condition Immunities** ", (m, s) => m.ConditionImmunities = s),
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
                    //Debug.WriteLine(block.GetType());
                }
                else
                {
                    //Debug.WriteLine(block.GetType());
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


        public static IEnumerable<TCondition> ToConditions<TCondition>(this Markdig.Syntax.MarkdownDocument document) where TCondition : Condition, new()
        {
            var spells = new List<TCondition>();
            TCondition spell = null;
            foreach (var block in document)
            {
                //DumpBlock(block);
                if (block is Markdig.Syntax.HeadingBlock)
                {
                    var headingBlock = block as Markdig.Syntax.HeadingBlock;
                    //DumpHeadingBlock(headingBlock);
                    if (headingBlock.HeaderChar == '#' && (headingBlock.Level == 1 || headingBlock.Level == 2))
                    {
                        if (spell != null)
                        {
                            spells.Add(spell);
                            //yield return spell;
                        }
                        spell = new TCondition();
                        spell.Name = headingBlock.Inline.ToMarkdownString();
                        //Console.WriteLine(spell.Name);
                    }
                }
                if (block is Markdig.Syntax.ParagraphBlock)
                {
                    var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                    spell.Text += MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                }
                if (block is Markdig.Syntax.ListBlock)
                {
                    var listBlock = block as Markdig.Syntax.ListBlock;
                    //DumpListBlock(listBlock);
                    if (listBlock.BulletType == '-')
                    {
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

                                        var properties = new List<Tuple<string, Action<TCondition, string>>>()
                                        {
                                            new Tuple<string, Action<TCondition, string>>("NameVO: ", (m, s) => m.NameVO = s),
                                        };

                                        foreach (var property in properties)
                                        {
                                            if (str.StartsWith(property.Item1))
                                            {
                                                property.Item2.Invoke(spell, str.Substring(property.Item1.Length));
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
                                        spell.Text += listBlock.BulletType + " " + MarkdownToHtml(paragraphBlock.ToMarkdownString()) + "\n";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    var tableBlock = block as Markdig.Extensions.Tables.Table;
                    spell.Text += "\n\n" + tableBlock.ToMarkdownString() + "\n\n";
                }


            }
            if (spell != null)
            {
                //yield return spell;
                spells.Add(spell);
            }
            return spells;
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
                    add += $"[{label}]({url} \"{title}\")";
                }
                else
                {
                    add += $"[{label}]({url})";
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

        public static string HtmlToMarkdownString(string html)
        {
            return html
                .Replace("\n", "\n\n")
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
