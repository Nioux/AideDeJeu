using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeuLib.Spells
{
    public class Spell : Item
    {
        public string LevelType { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Rituel { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml
        {
            get
            {
                return DescriptionDiv?.InnerHtml;
            }
            set
            {
                if (value != null)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(value);
                    DescriptionDiv = doc.DocumentNode;
                }
                else
                {
                    DescriptionDiv = null;
                }
            }
        }
        public string DescriptionText
        {
            get
            {
                return DescriptionDiv?.InnerText?.Replace("\n", "\r\n\r\n");
            }
        }
        [IgnoreDataMember]
        [NotMapped]
        public HtmlNode DescriptionDiv { get; set; }

        public string Overflow { get; set; }
        public string NoOverflow { get; set; }
        public string Source { get; set; }

        public void ParseHtml()
        {
            var pack = new HtmlDocument();
            pack.LoadHtml(this.Html);
            var divSpell = pack.DocumentNode.SelectNodes("//div[contains(@class,'bloc')]").FirstOrDefault();
            ParseNode(divSpell);
        }

        public void ParseNode(HtmlNode nodeSpell)
        {
            this.Name = nodeSpell.SelectSingleNode("h1").InnerText;
            var altNames = nodeSpell.SelectSingleNode("div[@class='trad']")?.InnerText;
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
            this.LevelType = nodeSpell.SelectSingleNode("h2/em").InnerText;
            this.Level = this.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
            this.Type = this.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
            this.CastingTime = nodeSpell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            this.Range = nodeSpell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            this.Components = nodeSpell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            this.Duration = nodeSpell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            this.DescriptionDiv = nodeSpell.SelectSingleNode("div[contains(@class,'description')]");
            this.Overflow = nodeSpell.SelectSingleNode("div[@class='overflow']")?.InnerText;
            this.NoOverflow = nodeSpell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
            this.Source = nodeSpell.SelectSingleNode("div[@class='source']").InnerText;
        }

        public static Spell FromHtml(HtmlNode nodeSpell)
        {
            var spell = new Spell();
            spell.Html = nodeSpell.OuterHtml;
            spell.ParseNode(nodeSpell);
            return spell;
        }
    }
}
