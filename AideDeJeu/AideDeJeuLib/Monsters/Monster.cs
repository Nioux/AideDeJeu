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
            var divMonster = divBloc?.SelectSingleNode("div[contains(@class,'monstre')]");
            monster.Name = divMonster?.SelectSingleNode("h1").InnerText;
            monster.NameVO = divMonster?.SelectSingleNode("div[contains(@class,'trad')]/a")?.InnerText;
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
            monster.Senses = divRed?.SelectSingleNode("strong[contains(text(),'Sens')]")?.NextSibling?.InnerText;
            monster.Languages = divRed?.SelectSingleNode("strong[contains(text(),'Langues')]")?.NextSibling?.InnerText;
            monster.Power = divRed?.SelectSingleNode("strong[contains(text(),'Puissance')]")?.NextSibling?.InnerText;
            return monster;
        }

    }
}
