using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);
                DescriptionDiv = doc.DocumentNode;
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
        public HtmlNode DescriptionDiv { get; set; }
        public string Overflow { get; set; }
        public string NoOverflow { get; set; }
        public string Source { get; set; }

        public static Spell FromHtml(HtmlNode nodeSpell)
        {
            var spell = new Spell();
            spell.Name = nodeSpell.SelectSingleNode("h1").InnerText;
            var altNames = nodeSpell.SelectSingleNode("div[@class='trad']").InnerText;
            var matchNames = new Regex(@"\[ (?<vo>.*?) \](?: \[ (?<alt>.*?) \])?").Match(altNames);
            spell.NameVO = matchNames.Groups["vo"].Value;
            spell.NamePHB = string.IsNullOrEmpty(matchNames.Groups["alt"].Value) ? spell.Name : matchNames.Groups["alt"].Value;
            spell.LevelType = nodeSpell.SelectSingleNode("h2/em").InnerText;
            spell.Level = spell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
            spell.Type = spell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
            spell.CastingTime = nodeSpell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            spell.Range = nodeSpell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            spell.Components = nodeSpell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            spell.Duration = nodeSpell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            spell.DescriptionDiv = nodeSpell.SelectSingleNode("div[contains(@class,'description')]");
            spell.Overflow = nodeSpell.SelectSingleNode("div[@class='overflow']")?.InnerText;
            spell.NoOverflow = nodeSpell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
            spell.Source = nodeSpell.SelectSingleNode("div[@class='source']").InnerText;
            return spell;
        }
    }
}
