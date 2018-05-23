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

        public IEnumerable<string> SpecialFeatures { get; set; }
        [IgnoreDataMember]
        public IEnumerable<HtmlNode> SpecialFeaturesNodes
        {
            set
            {
                SpecialFeatures = NodeListToStringList(value);
            }
        }

        public IEnumerable<string> Actions { get; set; }
        [IgnoreDataMember]
        public IEnumerable<HtmlNode> ActionsNodes
        {
            set
            {
                Actions = NodeListToStringList(value);
            }
        }

        public IEnumerable<string> LegendaryActions { get; set; }
        [IgnoreDataMember]
        public IEnumerable<HtmlNode> LegendaryActionsNodes
        {
            set
            {
                LegendaryActions = NodeListToStringList(value);
            }
        }


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

            var divTrad = divMonster.SelectSingleNode("div[@class='trad']");

            var linkVO = divTrad.SelectSingleNode("a").GetAttributeValue("href", "");
            var matchIdVF = new Regex(@"\?vf=(?<idvf>.*)").Match(linkVO);
            this.IdVF = matchIdVF?.Groups["idvf"]?.Value;
            var matchIdVO = new Regex(@"\?vo=(?<idvo>.*)").Match(linkVO);
            this.IdVO = matchIdVO?.Groups["idvo"]?.Value;


            var altNames = divTrad?.InnerText;
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

                if (string.IsNullOrEmpty(this.Type))
                {
                    matchesTypeSizeAlignment = new Regex("(?<size>.*) (?<type>.*), (?<alignment>.*)").Match(typeSizeAlignment);
                    this.Type = matchesTypeSizeAlignment?.Groups["type"]?.Value?.Trim();
                    this.Size = matchesTypeSizeAlignment?.Groups["size"]?.Value?.Trim();
                    this.Alignment = matchesTypeSizeAlignment?.Groups["alignment"]?.Value?.Trim();
                }
            }
            var divRed = divSansSerif?.SelectSingleNode("div[contains(@class,'red')]");
            this.ArmorClass = divRed?.SelectSingleNode("strong[contains(text(),'armure') or contains(text(),'Armor Class')]")?.NextSibling?.InnerText;
            this.HitPoints = divRed?.SelectSingleNode("strong[contains(text(),'Points de vie') or contains(text(),'Hit Points')]")?.NextSibling?.InnerText;
            this.Speed = divRed?.SelectSingleNode("strong[contains(text(),'Vitesse') or contains(text(),'Speed')]")?.NextSibling?.InnerText;

            this.Strength = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'FOR') or contains(text(),'STR')]")?.NextSibling?.NextSibling?.InnerText;
            this.Dexterity = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'DEX')]")?.NextSibling?.NextSibling?.InnerText;
            this.Constitution = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CON')]")?.NextSibling?.NextSibling?.InnerText;
            this.Intelligence = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'INT')]")?.NextSibling?.NextSibling?.InnerText;
            this.Wisdom = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'SAG') or contains(text(),'WIS')]")?.NextSibling?.NextSibling?.InnerText;
            this.Charisma = divRed?.SelectSingleNode("div[contains(@class,'carac')]/strong[contains(text(),'CHA')]")?.NextSibling?.NextSibling?.InnerText;


            this.SavingThrows = divRed?.SelectSingleNode("strong[contains(text(),'Jets de sauvegarde') or contains(text(),'Saving Throws')]")?.NextSibling?.InnerText;
            this.Skills = divRed?.SelectSingleNode("strong[contains(text(),'Compétences') or contains(text(),'Skills')]")?.NextSibling?.InnerText;
            
            this.DamageVulnerabilities = divRed?.SelectSingleNode("strong[contains(text(),'Vulnérabilités aux dégâts') or contains(text(),'Damage vulnerabilities')]")?.NextSibling?.InnerText;
            this.DamageResistances = divRed?.SelectSingleNode("strong[contains(text(),'Résistances aux dégâts') or contains(text(),'Damage Resistances')]")?.NextSibling?.InnerText;
            this.DamageImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux dégâts') or contains(text(),'Damage Immunities')]")?.NextSibling?.InnerText;
            this.ConditionImmunities = divRed?.SelectSingleNode("strong[contains(text(),'Immunités aux conditions') or contains(text(),'Conditions Immunities')]")?.NextSibling?.InnerText;

            this.Senses = divRed?.SelectSingleNode("strong[contains(text(),'Sens') or contains(text(),'Senses')]")?.NextSibling?.InnerText;
            this.Languages = divRed?.SelectSingleNode("strong[contains(text(),'Langues') or contains(text(),'Languages')]")?.NextSibling?.InnerText;
            this.Challenge = divRed?.SelectSingleNode("strong[contains(text(),'Puissance') or contains(text(),'Challenge')]")?.NextSibling?.InnerText;

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
                    else if (node.InnerText == "ACTIONS LÉGENDAIRES" || node.InnerText == "LEGENDARY ACTIONS")
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

            this.SpecialFeaturesNodes = specialFeatures;
            this.ActionsNodes = actions;
            this.LegendaryActionsNodes = legendaryActions;

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
