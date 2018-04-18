using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AideDeJeuLib
{
    public class Scrappers
    {
        public async Task<IEnumerable<string>> GetSpellIds(string classe, int niveauMin = 0, int niveauMax = 9)
        {
            string html = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr-FR"));
                // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
                // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre
                // https://www.aidedd.org/regles/sorts/

                html = await client.GetStringAsync(string.Format("https://www.aidedd.org/adj/livre-sorts/?c={0}&min=1{1}&max=1{2}", classe, niveauMin, niveauMax));
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            return pack.DocumentNode.SelectNodes("//input[@name='select_sorts[]']").Select(node => node.GetAttributeValue("value", ""));
        }

        public async Task<IEnumerable<Spell>> GetSpells(IEnumerable<string> spellIds)
        {
            string html = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr-FR"));
                var content = new MultipartFormDataContent();
                content.Add(new StringContent("card"), "format");
                foreach (var spellId in spellIds)
                {
                    content.Add(new StringContent(spellId), "select_sorts[]");
                }
                var response = await client.PostAsync("http://www.aidedd.org/dnd/sorts.php", content);
                html = await response.Content.ReadAsStringAsync();
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var newSpells = new List<Spell>();
            var cardDatas = new List<CardData>();
            var spells = pack.DocumentNode.SelectNodes("//div[contains(@class,'blocCarte')]").ToList();
            foreach (var spell in spells)
            {
                var newSpell = new Spell();
                newSpell.Title = spell.SelectSingleNode("h1").InnerText;
                newSpell.TitleUS = spell.SelectSingleNode("div[@class='trad']").InnerText;
                newSpell.LevelType = spell.SelectSingleNode("h2/em").InnerText;
                newSpell.Level = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
                newSpell.Type = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
                newSpell.CastingTime = spell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Range = spell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Components = spell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Duration = spell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.DescriptionDiv = spell.SelectSingleNode("div[contains(@class,'description')]");
                newSpell.Description = newSpell.DescriptionDiv.InnerHtml;
                newSpell.DescriptionText = newSpell.DescriptionDiv.InnerText;
                newSpell.Overflow = spell.SelectSingleNode("div[@class='overflow']")?.InnerText;
                newSpell.NoOverflow = spell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
                newSpell.Source = spell.SelectSingleNode("div[@class='source']").InnerText;
                newSpells.Add(newSpell);
            }
            return newSpells;
        }
        public async Task<string> OnGetAsync(Dictionary<string, string> context)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(new System.Net.Http.Headers.ProductHeaderValue("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:59.0) Gecko/20100101 Firefox/59.0")));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr-FR"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));
            //client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("br"));
            var content = new MultipartFormDataContent();
            //content.Add(new StringContent("Afficher+%28FR%29"), "displayFR");
            content.Add(new StringContent("card"), "format");
            //content.Add(new StringContent("%27amis%27"), "select_sorts%5B%5D");
            //content.Add(new StringContent("\"amis\""), "select_sorts[]");
            //var bod = await content.ReadAsStringAsync();
            //Debug.WriteLine(bod);

            // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
            // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre
            // https://www.aidedd.org/regles/sorts/

            /*
            <option value="b">Barde</option>
            <option value="c">Clerc</option>
            <option value="d">Druide</option>
            <option value="s">Ensorceleur</option>
            <option value="w">Magicien</option>
            <option value="p">Paladin</option>
            <option value="r">Rôdeur</option>
            <option value="k">Sorcier</option>
            */
            string c = context["c"];
            var htmlSpellBook = await client.GetStringAsync("https://www.aidedd.org/adj/livre-sorts/?c=" + c + "&min=10&max=19");
            var pack = new HtmlDocument();
            pack.LoadHtml(htmlSpellBook);
            var selectSorts = pack.DocumentNode.SelectNodes("//input[@name='select_sorts[]']").ToList();
            foreach (var selectSort in selectSorts)
            {
                content.Add(new StringContent(selectSort.GetAttributeValue("value", "")), "select_sorts[]");
            }
            var response = await client.PostAsync("http://www.aidedd.org/dnd/sorts.php", content);
            var htmlSpell = await response.Content.ReadAsStringAsync();
            pack.LoadHtml(htmlSpell);
            var newSpells = new List<Spell>();
            var cardDatas = new List<CardData>();
            var spells = pack.DocumentNode.SelectNodes("//div[contains(@class,'blocCarte')]").ToList();
            foreach (var spell in spells)
            {
                var newSpell = new Spell();
                newSpell.Title = spell.SelectSingleNode("h1").InnerText;
                newSpell.TitleUS = spell.SelectSingleNode("div[@class='trad']").InnerText;
                newSpell.LevelType = spell.SelectSingleNode("h2/em").InnerText;
                newSpell.Level = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
                newSpell.Type = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
                newSpell.CastingTime = spell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Range = spell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Components = spell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.Duration = spell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                newSpell.DescriptionDiv = spell.SelectSingleNode("div[contains(@class,'description')]");
                newSpell.Description = newSpell.DescriptionDiv.InnerHtml;
                newSpell.Overflow = spell.SelectSingleNode("div[@class='overflow']")?.InnerText;
                newSpell.NoOverflow = spell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
                newSpell.Source = spell.SelectSingleNode("div[@class='source']").InnerText;
                newSpells.Add(newSpell);
                cardDatas.AddRange(ToCardDatas(context, newSpell));
            }
            //Debug.WriteLine(htmlSpell);

            var sampleCardDatas = cardDatas.Take(4).ToArray();
            foreach (var scd in sampleCardDatas)
            {
                int totalHeight = 0;
                foreach (var cc in scd.Contents)
                {
                    totalHeight += cc.Height;
                    Debug.WriteLine(string.Format("{0} - {1} => {2}", cc.Height, totalHeight, cc.ToString()));
                }
            }
            //var own = new CardDataOwner() { CardDatas = cardDatas.ToArray() };
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(CardData[]));
            serializer.WriteObject(stream, cardDatas.ToArray());
            stream.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            //context.Response.Headers["encoding"]
            return await sr.ReadToEndAsync();
            //Result = await sr.ReadToEndAsync();




            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string json = serializer.Serialize((object)yourDictionary);
            //Debug.WriteLine(result);

            //using (var file = new FileStream(@"C:\Users\yanma\Downloads\_\JdR\Chroniques Oubliées\crobi-rpg-cards-065974f\generator\data\out.js", FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            //{
            //    var bytes = System.Text.Encoding.UTF8.GetBytes(result);
            //    Response.Body.Write(bytes, 0, bytes.Length);
            //    //await file.WriteAsync(bytes, 0, bytes.Length);
            //}

        }

        public CardContent TextToCardContent(string text)
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

        public string[] SplitText(string text)
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

        public CardContent[] ToContents(HtmlNode description)
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

        public CardData ToBaseCardData(Dictionary<string, string> context, Spell spell)
        {
            string color = context["color"];
            string backgroundImage = context["background_image"];
            var cardData = new CardData();
            cardData.Count = 1;
            cardData.Color = color;
            cardData.Title = spell.Title;
            cardData.TitleSize = "10";
            cardData.Icon = "white-book-" + spell.Level;
            cardData.IconBack = "robe";
            cardData.BackgroundImage = backgroundImage;
            cardData.Tags = new string[]
            {
                "sort",
                "magicien",
            };
            return cardData;
        }
        public CardData[] ToCardDatas(Dictionary<string, string> context, Spell spell)
        {
            var cardDatas = new List<CardData>();
            var cardData = ToBaseCardData(context, spell);

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

                    cardData = ToBaseCardData(context, spell);
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

        public class Spell
        {
            public string Title { get; set; }
            public string TitleUS { get; set; }
            public string LevelType { get; set; }
            public string Level { get; set; }
            public string Type { get; set; }
            public string CastingTime { get; set; }
            public string Range { get; set; }
            public string Components { get; set; }
            public string Duration { get; set; }
            public string Description { get; set; }
            public string DescriptionText { get; set; }
            public HtmlNode DescriptionDiv { get; set; }
            public string Overflow { get; set; }
            public string NoOverflow { get; set; }
            public string Source { get; set; }
        }

        /*public class CardDataOwner
        {
            public CardData[] CardDatas { get; set; }
        }*/
        [DataContract]
        public class CardData
        {
            [DataMember(Name = "count")]
            public int Count { get; set; }
            [DataMember(Name = "color")]
            public string Color { get; set; }
            [DataMember(Name = "title")]
            public string Title { get; set; }
            [DataMember(Name = "title_size")]
            public string TitleSize { get; set; }
            [DataMember(Name = "icon")]
            public string Icon { get; set; }
            [DataMember(Name = "icon_back")]
            public string IconBack { get; set; }
            [DataMember(Name = "contents")]
            public string[] ContentsToString { get { return Contents.Select(cc => cc.ToString()).ToArray(); } }
            [IgnoreDataMember]
            public CardContent[] Contents { get; set; }
            [DataMember(Name = "tags")]
            public string[] Tags { get; set; }
            [DataMember(Name = "background_image")]
            public string BackgroundImage { get; set; }
        }

        public interface CardContent
        {
            int Height { get; }
        }

        public class SubtitleCardContent : CardContent
        {
            public string Subtitle { get; set; }

            public SubtitleCardContent(string subtitle)
            {
                Subtitle = subtitle;
            }

            public override string ToString()
            {
                return "subtitle | " + Subtitle;
            }

            public int Height { get { return 12; } }
        }

        public class RuleCardContent : CardContent
        {
            public override string ToString()
            {
                return "rule";
            }

            public int Height { get { return 15; } }
        }

        public class PropertyCardContent : CardContent
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public PropertyCardContent(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return "property | " + Name + " | " + Value;
            }

            public int Height { get { return ((Name.Length + Value.Length) / 35 + 1) * 15; } }
        }

        public class FillCardContent : CardContent
        {
            public int Fill { get; set; }

            public FillCardContent(int fill)
            {
                Fill = fill;
            }

            public override string ToString()
            {
                return "fill | " + Fill;
            }

            public int Height { get { return 1; } }
        }

        public class TextCardContent : CardContent
        {
            public string Text { get; set; }

            public TextCardContent(string text)
            {
                Text = text;
            }

            public override string ToString()
            {
                return "text | " + Text;
            }

            public int Height { get { return (Text.Length / 35 + 1) * 15 + 6; } }
        }

        public class SectionCardContent : CardContent
        {
            public string Section { get; set; }

            public SectionCardContent(string section)
            {
                Section = section;
            }

            public override string ToString()
            {
                return "section | " + Section;
            }

            public int Height { get { return 20; } }
        }

        public class DescriptionCardContent : CardContent
        {
            public string Quality { get; set; }

            public string Text { get; set; }

            public DescriptionCardContent(string quality, string text)
            {
                Quality = quality;
                Text = text;
            }

            public override string ToString()
            {
                return "description | " + Quality + " | " + Text;
            }

            public int Height { get { return ((Quality.Length + Text.Length) / 35 + 1) * 15 + 6; } }
        }

        public class BulletCardContent : CardContent
        {
            public string Text { get; set; }

            public BulletCardContent(string text)
            {
                Text = text;
            }

            public override string ToString()
            {
                return "bullet | " + Text;
            }

            public int Height { get { return (Text.Length / 30 + 1) * 15; } }
        }

        public class BoxesCardContent : CardContent
        {
            public int Count { get; set; }

            public int Size { get; set; }

            public BoxesCardContent(int count, int size)
            {
                Count = count;
                Size = size;
            }

            public override string ToString()
            {
                return "boxes | " + Count + " | " + Size;
            }

            public int Height { get { return 16; } }
        }

        public class DndstatsCardContent : CardContent
        {
            public int Strength { get; set; }
            public int Dexterity { get; set; }
            public int Constitution { get; set; }
            public int Intelligence { get; set; }
            public int Wisdom { get; set; }
            public int Charisma { get; set; }
            public override string ToString()
            {
                return "dndstats | " + Strength + " | " + Dexterity + " | " + Constitution + " | " + Intelligence + " | " + Wisdom + " | " + Charisma;
            }

            public int Height { get { return 20; } }
        }

        public class EmptyCardContent : CardContent
        {
            public override string ToString()
            {
                return "";
            }

            public int Height { get { return 0; } }
        }

    }
}
