using AideDeJeu;
using AideDeJeu.Tools;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    public abstract class Monster : Item
    {
        public string Type { get; set; }
        public string Size { get; set; }
        public string Alignment { get; set; }
        public string Legendary { get; set; }
        public string Source { get; set; }
        public string ArmorClass { get; set; }
        public string HitPoints { get; set; }
        public string Speed { get; set; }
        public string Strength { get; set; }
        public string Dexterity { get; set; }
        public string Constitution { get; set; }
        public string Intelligence { get; set; }
        public string Wisdom { get; set; }
        public string Charisma { get; set; }
        public string SavingThrows { get; set; }
        public string Skills { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string DamageResistances { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string Description { get; set; }

        public IEnumerable<string> SpecialFeatures { get; set; }
        public IEnumerable<string> Actions { get; set; }
        public IEnumerable<string> Reactions { get; set; }
        public IEnumerable<string> LegendaryActions { get; set; }


        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            List<string> features = null;
            //List<string> specialFeatures = null;
            //List<string> actions = null;
            //List<string> reactions = null;
            //List<string> legendaryActions = null;
            enumerator.MoveNext();
            //try
            //{
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;
                    //Debug.WriteLine(block.GetType());
                    //DumpBlock(block);
                    if (block is Markdig.Syntax.HeadingBlock)
                    {
                        var headingBlock = block as Markdig.Syntax.HeadingBlock;
                        //DumpHeadingBlock(headingBlock);
                        if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                        {
                            if (this.Name != null)
                            {
                                return;
                            }
                            this.Name = headingBlock.Inline.ToMarkdownString();
                            //Console.WriteLine(spell.Name);
                        }
                        if (headingBlock.HeaderChar == '#' && headingBlock.Level == 2)
                        {
                            switch (headingBlock.Inline.ToMarkdownString())
                            {
                                case "Capacités":
                                case "Special Features":
                                    SpecialFeatures = features = new List<string>();
                                    break;
                                case "Actions":
                                    Actions = features = new List<string>();
                                    break;
                                case "Réaction":
                                case "Réactions":
                                case "Reaction":
                                case "Reactions":
                                    Reactions = features = new List<string>();
                                    break;
                                case "Actions légendaires":
                                case "Legendary Actions":
                                    LegendaryActions = features = new List<string>();
                                    break;
                                default:
                                    App.Current.MainPage.DisplayAlert("Erreur de parsing", headingBlock.Inline.ToMarkdownString(), "OK");
                                    break;
                            }
                        }
                    }
                    else if (block is Markdig.Syntax.ParagraphBlock)
                    {
                        if (block.IsNewItem())
                        {
                            return;
                        }
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

                                            var properties = new List<Tuple<string, System.Action<Monster, string>>>()
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
                                                    property.Item2.Invoke(this, str.Substring(property.Item1.Length));
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
                            this.Strength = table["FOR"].FirstOrDefault();
                            this.Dexterity = table["DEX"].FirstOrDefault();
                            this.Constitution = table["CON"].FirstOrDefault();
                            this.Intelligence = table["INT"].FirstOrDefault();
                            this.Wisdom = table["SAG"].FirstOrDefault();
                            this.Charisma = table["CHA"].FirstOrDefault();
                        }
                        else if (table.ContainsKey("STR"))
                        {
                            this.Strength = table["STR"].FirstOrDefault();
                            this.Dexterity = table["DEX"].FirstOrDefault();
                            this.Constitution = table["CON"].FirstOrDefault();
                            this.Intelligence = table["INT"].FirstOrDefault();
                            this.Wisdom = table["WIS"].FirstOrDefault();
                            this.Charisma = table["CHA"].FirstOrDefault();
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
                    enumerator.MoveNext();
                }
            //}
            //finally

            ////if (monster != null)
            //{
            //    this.SpecialFeatures = specialFeatures;
            //    this.Actions = actions;
            //    this.Reactions = reactions;
            //    this.LegendaryActions = legendaryActions;
            //    //yield return monster;
            //}
        }


    }
}
