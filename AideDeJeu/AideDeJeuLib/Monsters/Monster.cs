using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeuLib.Monsters
{
    public class Monster
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameVO { get; set; }
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
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }



        public static Monster FromHtml(HtmlNode divBloc)
        {
            var monster = new Monster();
            var divMonster = divBloc.SelectSingleNode("div[contains(@class,'monstre')]");
            monster.Name = divMonster.SelectSingleNode("h1").InnerText;
            monster.NameVO = divMonster.SelectSingleNode("div[contains(@class,'trad')]/a").InnerText;
            var divSansSerif = divMonster.SelectSingleNode("div[contains(@class,'sansSerif')]");
            var typeSizeAlignment = divSansSerif.SelectSingleNode("h2/em").InnerText;
            var matchesTypeSizeAlignment = new Regex("(?<type>.*) de taille (?<size>.*), (?<alignment>.*)").Match(typeSizeAlignment);
            monster.Type = matchesTypeSizeAlignment.Groups["type"].Value;
            monster.Size = matchesTypeSizeAlignment.Groups["size"].Value;
            monster.Alignment = matchesTypeSizeAlignment.Groups["alignment"].Value;
            var divRed = divSansSerif.SelectSingleNode("div[contains(@class,'red')]");
            monster.Strength = divRed.SelectSingleNode("strong").NextSibling.InnerText;
            //monster.LevelType = nodeSpell.SelectSingleNode("h2/em").InnerText;
            //monster.Level = spell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
            //monster.Type = spell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
            //monster.CastingTime = nodeSpell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            //monster.Range = nodeSpell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            //monster.Components = nodeSpell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            //monster.Duration = nodeSpell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
            //monster.DescriptionDiv = nodeSpell.SelectSingleNode("div[contains(@class,'description')]");
            //monster.Overflow = nodeSpell.SelectSingleNode("div[@class='overflow']")?.InnerText;
            //monster.NoOverflow = nodeSpell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
            //monster.Source = nodeSpell.SelectSingleNode("div[@class='source']").InnerText;
            return monster;
        }

    }
}
