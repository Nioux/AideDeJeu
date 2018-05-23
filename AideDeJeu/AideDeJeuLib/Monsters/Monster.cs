using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public string DamageVulnerabilities { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string DamageResistances { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string Description { get; set; }
        private string _Picture = null;
        public string Picture
        {
            get
            {
                if(_Picture != null)
                {
                    return "https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/Monsters" + _Picture.Substring(_Picture.LastIndexOf('/'));
                }
                return null;
            }
            set
            {
                _Picture = value;
            }
        }

        [IgnoreDataMember]
        public List<HtmlNode> SpecialFeatures { get; set; }
        //public List<string> SpecialFeaturesPersist
        //{
        //    get
        //    {
        //        return SpecialFeatures.Select(node => node.OuterHtml).ToList();
        //    }
        //    set
        //    {
        //        List<HtmlNode> nodes = new List<HtmlNode>();
        //        foreach (var str in value)
        //        {
        //            HtmlDocument doc = new HtmlDocument();
        //            doc.LoadHtml(str);
        //            nodes.Add(doc.DocumentNode);
        //        }
        //        SpecialFeatures = nodes;
        //    }
        //}

        [IgnoreDataMember]
        public List<HtmlNode> Actions { get; set; }
        [IgnoreDataMember]
        public List<HtmlNode> LegendaryActions { get; set; }


        public void ParseHtml()
        {
            var pack = new HtmlDocument();
            pack.LoadHtml(this.Html);
            var divSpell = pack.DocumentNode.SelectNodes("//div[contains(@class,'bloc')]").FirstOrDefault();
            ParseNode(divSpell);
        }

        //public static Monster FromHtml(HtmlNode divBloc)
        //{
        public void ParseNode(HtmlNode divBloc)
        {
            //monster.Html = divBloc.OuterHtml;
            var divMonster = divBloc?.SelectSingleNode("div[contains(@class,'monstre')]");
            this.Name = divMonster?.SelectSingleNode("h1").InnerText;

            var altNames = divMonster.SelectSingleNode("div[@class='trad']")?.InnerText;
            if (altNames != null)
            {
                var matchNames = new Regex(@"\[ (?<vo>.*?) \](?: \[ (?<alt>.*?) \])?").Match(altNames);
                this.NameVO = matchNames.Groups["vo"].Value;
                this.NamePHB = string.IsNullOrEmpty(matchNames.Groups["alt"].Value) ? this.Name : matchNames.Groups["alt"].Value;
            }
            else
            {
                this.NamePHB = this.Name;
            }

            var divSansSerif = divMonster?.SelectSingleNode("div[contains(@class,'sansSerif')]");
            var typeSizeAlignment = divSansSerif?.SelectSingleNode("h2/em")?.InnerText;
            if (typeSizeAlignment != null)
            {
                var matchesTypeSizeAlignment = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)").Match(typeSizeAlignment);
                this.Type = matchesTypeSizeAlignment?.Groups["type"]?.Value?.Trim();
                this.Size = matchesTypeSizeAlignment?.Groups["size"]?.Value?.Trim();
                this.Alignment = matchesTypeSizeAlignment?.Groups["alignment"]?.Value?.Trim();
            }
            var divRed = divSansSerif?.SelectSingleNode("div[contains(@class,'red')]");
            this.ArmorClass = divRed?.SelectSingleNode("strong[contains(text(),'armure')]")?.NextSibling?.InnerText;
            this.HitPoints = divRed?.SelectSingleNode("strong[contains(text(),'Points de vie')]")?.NextSibling?.InnerText;
            this.Speed = divRed?.SelectSingleNode("strong[contains(text(),'Vitesse')]")?.NextSibling?.InnerText;

            this.Strength = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'FOR')]")?.NextSibling?.NextSibling?.InnerText;
            this.Dexterity = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'DEX')]")?.NextSibling?.NextSibling?.InnerText;
            this.Constitution = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CON')]")?.NextSibling?.NextSibling?.InnerText;
            this.Intelligence = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'INT')]")?.NextSibling?.NextSibling?.InnerText;
            this.Wisdom = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'SAG')]")?.NextSibling?.NextSibling?.InnerText;
            this.Charisma = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CHA')]")?.NextSibling?.NextSibling?.InnerText;


            this.SavingThrows = divRed?.SelectSingleNode("strong[contains(text(),'Jets de sauvegarde')]")?.NextSibling?.InnerText;
            this.Skills = divRed?.SelectSingleNode("strong[contains(text(),'Compétences')]")?.NextSibling?.InnerText;
            
            this.DamageVulnerabilities = divRed?.SelectSingleNode("strong[contains(text(),'Vulnérabilités aux dégâts')]")?.NextSibling?.InnerText;
            this.DamageResistances = divRed?.SelectSingleNode("strong[contains(text(),'Résistances aux dégâts')]")?.NextSibling?.InnerText;
            this.DamageImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux dégâts')]")?.NextSibling?.InnerText;
            this.ConditionImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux conditions')]")?.NextSibling?.InnerText;

            this.Senses = divRed?.SelectSingleNode("strong[contains(text(),'Sens')]")?.NextSibling?.InnerText;
            this.Languages = divRed?.SelectSingleNode("strong[contains(text(),'Langues')]")?.NextSibling?.InnerText;
            this.Challenge = divRed?.SelectSingleNode("strong[contains(text(),'Puissance')]")?.NextSibling?.InnerText;

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

            this.SpecialFeatures = specialFeatures;
            this.Actions = actions;
            this.LegendaryActions = legendaryActions;

            var divDescription = divBloc?.SelectSingleNode("div[contains(@class,'description')]");
            this.Description = divDescription?.InnerText;

            var divSource = divBloc?.SelectSingleNode("div[contains(@class,'source')]");
            this.Source = divSource?.InnerText;

            var img = divBloc?.SelectSingleNode("div[contains(@class,'center')]/img[contains(@class,'picture')]");
            this.Picture = img?.GetAttributeValue("src", null);
        }

        public static Monster FromHtml(HtmlNode node)
        {
            var monster = new Monster();
            monster.Html = node.OuterHtml;
            monster.ParseNode(node);
            return monster;
        }
    }
}
