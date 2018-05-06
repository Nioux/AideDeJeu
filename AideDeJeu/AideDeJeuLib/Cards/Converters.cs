using AideDeJeuLib.Spells;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace AideDeJeuLib.Cards
{
    public static class Converters
    {
        public static CardContent TextToCardContent(string text)
        {
            if (text.StartsWith("•"))
            {
                return new BulletCardContent(text.Substring(1));
            }
            else if (text.Trim(new char[] { ' ', '\n' }) == ".")
            {
                return new EmptyCardContent();
            }
            else
            {
                return new TextCardContent(text);
            }
        }

        public static string[] SplitText(string text)
        {
            var texts = new List<string>();
            string str = "";
            bool autoreturn = true;
            foreach (var car in text)
            {
                if (car == '\n')
                {
                    texts.Add(str);
                    str = "";
                    autoreturn = true;
                }
                else if (car == '.')
                {
                    if (autoreturn)
                    {
                        texts.Add(str + '.');
                        str = "";
                    }
                    else
                    {
                        str += car;
                    }
                }
                else if (car == '•')
                {
                    texts.Add(str);
                    str = "•";
                    autoreturn = false;
                }
                else
                {
                    str += car;
                }
            }
            if (str.Length > 0)
            {
                texts.Add(str);
            }
            return texts.ToArray();
        }

        public static CardContent[] ToContents(HtmlNode description)
        {
            var contents = new List<CardContent>();
            string currentText = "";
            foreach (var content in description.ChildNodes)
            {
                //Debug.WriteLine(content.NodeType + " " + content.Name + " " + content.InnerText);
                if (content.NodeType == HtmlNodeType.Element && content.Name == "strong")
                {
                    if (currentText.Length > 0)
                    {
                        contents.Add(TextToCardContent(currentText));
                        currentText = "";
                    }
                    contents.Add(new SectionCardContent(content.InnerText));
                }
                else if (content.NodeType == HtmlNodeType.Element && content.Name == "em")
                {
                    currentText += "<em>" + content.InnerText + "</em>";
                }
                else if (content.NodeType == HtmlNodeType.Text)
                {
                    var texts = SplitText(content.InnerText);
                    for (int i = 0; i < texts.Length - 1; i++)
                    {
                        contents.Add(TextToCardContent(currentText + texts[i]));
                        currentText = "";
                    }
                    currentText += texts[texts.Length - 1];
                }
            }
            if (currentText.Length > 0)
            {
                contents.Add(TextToCardContent(currentText));
                currentText = "";
            }
            return contents.ToArray();
        }

        public static CardData ToBaseCardData(Spell spell)
        {
            //string color = context["color"];
            //string backgroundImage = context["background_image"];
            string color = "red";
            var cardData = new CardData();
            cardData.Count = 1;
            cardData.Color = color;
            cardData.Title = spell.NamePHB;
            cardData.TitleSize = "10";
            cardData.Icon = "white-book-" + spell.Level;
            cardData.IconBack = "robe";
            //cardData.BackgroundImage = backgroundImage;
            //cardData.Tags = new string[]
            //{
            //    "sort",
            //    "magicien",
            //};
            return cardData;
        }
        public static CardData[] ToCardDatas(Spell spell)
        {
            var cardDatas = new List<CardData>();
            var cardData = ToBaseCardData(spell);

            var contents = new List<CardContent>();
            contents.AddRange(new CardContent[]
            {
                new SubtitleCardContent(spell.LevelType),
                new RuleCardContent(),
                new PropertyCardContent("Temps d'incantation", spell.CastingTime),
                new PropertyCardContent("Portée", spell.Range),
                new PropertyCardContent("Composants", spell.Components),
                new RuleCardContent(),
                //new FillCardContent(1),
                //new TextCardContent(spell.Description),
            });
            var description = ToContents(spell.DescriptionDiv);
            foreach (var line in description)
            {
                int size = contents.Sum(cc => cc.Height);
                if (line.Height == 0)
                {

                }
                else if (size + line.Height <= 295)
                {
                    contents.Add(line);
                }
                else
                {
                    CardContent section = null;
                    if (contents.LastOrDefault() is SectionCardContent)
                    {
                        section = contents.LastOrDefault();
                        contents.RemoveAt(contents.Count - 1);
                    }
                    cardData.Contents = contents.ToArray();
                    cardDatas.Add(cardData);

                    cardData = ToBaseCardData(spell);
                    contents = new List<CardContent>();
                    if (section != null)
                    {
                        contents.Add(section);
                    }
                    contents.Add(line);
                }
            }
            cardData.Contents = contents.ToArray();
            cardDatas.Add(cardData);

            if (cardDatas.Count > 1)
            {
                for (int i = 0; i < cardDatas.Count; i++)
                {
                    cardDatas[i].Title += string.Format(" ({0}/{1})", i + 1, cardDatas.Count);
                }
            }
            return cardDatas.ToArray();
        }
    }
}