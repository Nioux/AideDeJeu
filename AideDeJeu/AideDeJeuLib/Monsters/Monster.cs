using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeuLib.Monsters
{
    public class Monster : Item
    {
        public string Power { get; set; }
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
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string DamageResistances { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public List<HtmlNode> SpecialFeatures { get; set; }
        public List<HtmlNode> Actions { get; set; }
        public List<HtmlNode> LegendaryActions { get; set; }



        public static Monster FromHtml(HtmlNode divBloc)
        {
            var monster = new Monster();
            var divMonster = divBloc?.SelectSingleNode("div[contains(@class,'monstre')]");
            monster.Name = divMonster?.SelectSingleNode("h1").InnerText;

            var altNames = divMonster.SelectSingleNode("div[@class='trad']").InnerText;
            var matchNames = new Regex(@"\[ (?<vo>.*?) \](?: \[ (?<alt>.*?) \])?").Match(altNames);
            monster.NameVO = matchNames.Groups["vo"].Value;
            monster.NamePHB = matchNames.Groups["alt"].Value;

            var divSansSerif = divMonster?.SelectSingleNode("div[contains(@class,'sansSerif')]");
            var typeSizeAlignment = divSansSerif?.SelectSingleNode("h2/em")?.InnerText;
            if (typeSizeAlignment != null)
            {
                var matchesTypeSizeAlignment = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)").Match(typeSizeAlignment);
                monster.Type = matchesTypeSizeAlignment?.Groups["type"]?.Value?.Trim();
                monster.Size = matchesTypeSizeAlignment?.Groups["size"]?.Value?.Trim();
                monster.Alignment = matchesTypeSizeAlignment?.Groups["alignment"]?.Value?.Trim();
            }
            var divRed = divSansSerif?.SelectSingleNode("div[contains(@class,'red')]");
            monster.ArmorClass = divRed?.SelectSingleNode("strong[contains(text(),'armure')]")?.NextSibling?.InnerText;
            monster.HitPoints = divRed?.SelectSingleNode("strong[contains(text(),'Points de vie')]")?.NextSibling?.InnerText;
            monster.Speed = divRed?.SelectSingleNode("strong[contains(text(),'Vitesse')]")?.NextSibling?.InnerText;

            monster.Strength = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'FOR')]")?.NextSibling?.NextSibling?.InnerText;
            monster.Dexterity = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'DEX')]")?.NextSibling?.NextSibling?.InnerText;
            monster.Constitution = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CON')]")?.NextSibling?.NextSibling?.InnerText;
            monster.Intelligence = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'INT')]")?.NextSibling?.NextSibling?.InnerText;
            monster.Wisdom = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'SAG')]")?.NextSibling?.NextSibling?.InnerText;
            monster.Charisma = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CHA')]")?.NextSibling?.NextSibling?.InnerText;


            monster.SavingThrows = divRed?.SelectSingleNode("strong[contains(text(),'Jets de sauvegarde')]")?.NextSibling?.InnerText;
            monster.Skills = divRed?.SelectSingleNode("strong[contains(text(),'Compétences')]")?.NextSibling?.InnerText;
            monster.DamageResistances = divRed?.SelectSingleNode("strong[contains(text(),'Résistances aux dégâts')]")?.NextSibling?.InnerText;
            monster.DamageImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux dégâts')]")?.NextSibling?.InnerText;
            monster.ConditionImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux conditions')]")?.NextSibling?.InnerText;
            monster.Senses = divRed?.SelectSingleNode("strong[contains(text(),'Sens')]")?.NextSibling?.InnerText;
            monster.Languages = divRed?.SelectSingleNode("strong[contains(text(),'Langues')]")?.NextSibling?.InnerText;
            monster.Challenge = divRed?.SelectSingleNode("strong[contains(text(),'Puissance')]")?.NextSibling?.InnerText;

            List<HtmlNode> nodes = new List<HtmlNode>();
            List<HtmlNode> specialFeatures = null;
            List<HtmlNode> actions = null;
            List<HtmlNode> legendaryActions = null;
            var node = divSansSerif.SelectSingleNode("p");
            while(node != null)
            {
                if(node.NodeType == HtmlNodeType.Element && node.Name == "div")
                {
                    if(node.InnerText == "ACTIONS")
                    {
                        specialFeatures = nodes;
                        nodes = new List<HtmlNode>();
                    }
                    else if (node.InnerText == "ACTIONS LÉGENDAIRES")
                    {
                        actions = nodes;
                        nodes = new List<HtmlNode>();
                    }
                }
                else
                {
                    nodes.Add(node);
                }
                node = node.NextSibling;
            }
            if(actions == null)
            {
                if(specialFeatures == null)
                {
                    specialFeatures = nodes;
                }
                else
                {
                    actions = nodes;
                }
            }
            else
            {
                legendaryActions = nodes;
            }

            monster.SpecialFeatures = specialFeatures;
            monster.Actions = actions;
            monster.LegendaryActions = legendaryActions;

            var divDescription = divBloc?.SelectSingleNode("div[contains(@class,'description')]");
            monster.Description = divDescription?.InnerText;

            var divSource = divBloc?.SelectSingleNode("div[contains(@class,'source')]");
            monster.Source = divSource?.InnerText;

            var img = divBloc?.SelectSingleNode("div[contains(@class,'center')]/img[contains(@class,'picture')]");
            monster.Picture = img?.GetAttributeValue("src", null);
            return monster;
        }

    }
}
