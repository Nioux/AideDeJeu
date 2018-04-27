using Akavache;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AideDeJeuLib.Spells
{
    public class Scrappers
    {
        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr"));
            client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr-FR"));
            return client;
        }

        public async Task<IEnumerable<Spell>> GetSpells(string classe = "", int niveauMin = 0, int niveauMax = 9, string ecole = "", string rituel = "", string source = "srd")
        {
            string html = null;
            using (var client = GetHttpClient())
            {
                // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
                // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre
                // https://www.aidedd.org/regles/sorts/

                html = await client.GetStringAsync(string.Format("https://www.aidedd.org/regles/sorts/?c={0}&min=1{1}&max=1{2}&e={3}&r={4}&s={5}", classe, niveauMin, niveauMax, ecole, rituel, source));
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var tdssort = pack.GetElementbyId("liste").Element("table").Elements("tr").ToList();
            var spells = new List<Spell>();
            foreach (var tdsort in tdssort)
            {
                var thssort = tdsort.Elements("td").ToArray();
                if (thssort.Length > 0)
                {
                    Spell spell = new Spell();
                    spell.Title = thssort[0].InnerText;
                    var href = thssort[0].Element("a").GetAttributeValue("href", "");
                    var regex = new Regex("vf=(?<id>.*)");
                    spell.Id = regex.Match(href).Groups["id"].Value;
                    spell.Level = thssort[1].InnerText;
                    spell.Type = thssort[2].InnerText;
                    spell.CastingTime = thssort[3].InnerText;
                    spell.Concentration = thssort[4].InnerText;
                    spell.Rituel = thssort[5].InnerText;
                    spells.Add(spell);
                }
            }
            return spells;
        }

        public async Task<Spell> GetSpellFromSource(string id)
        {
            string html = null;
            using (var client = GetHttpClient())
            {
                // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
                // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre
                // https://www.aidedd.org/regles/sorts/

                html = await client.GetStringAsync(string.Format("https://www.aidedd.org/dnd/sorts.php?vf={0}", id));
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var divSpell = pack.DocumentNode.SelectNodes("//div[contains(@class,'bloc')]").FirstOrDefault();
            return Spell.FromHtml(divSpell);
        }

        public async Task<Spell> GetSpell(string id)
        {
            BlobCache.ApplicationName = "AkavacheExperiment";
            //await BlobCache.UserAccount.InsertObject(id, newSpell);
            var spell = await BlobCache.LocalMachine.GetOrFetchObject<Spell>(id, () => GetSpellFromSource(id));
            await BlobCache.LocalMachine.Flush();
            return spell;
        }

        public async Task<IEnumerable<string>> GetSpellIds(string classe, int niveauMin = 0, int niveauMax = 9)
        {
            string html = null;
            using (var client = GetHttpClient())
            {
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
            using (var client = GetHttpClient())
            {
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
                //newSpell.DescriptionHtml = newSpell.DescriptionDiv.InnerHtml;
                //newSpell.Description = newSpell.DescriptionDiv.InnerText;
                newSpell.Overflow = spell.SelectSingleNode("div[@class='overflow']")?.InnerText;
                newSpell.NoOverflow = spell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
                newSpell.Source = spell.SelectSingleNode("div[@class='source']").InnerText;
                newSpells.Add(newSpell);
            }
            return newSpells;
        }
        /*
        public async Task<string> OnGetAsync(IReadOnlyDictionary<string, string> context)
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

            
            //<option value="b">Barde</option>
            //<option value="c">Clerc</option>
            //<option value="d">Druide</option>
            //<option value="s">Ensorceleur</option>
            //<option value="w">Magicien</option>
            //<option value="p">Paladin</option>
            //<option value="r">Rôdeur</option>
            //<option value="k">Sorcier</option>
            
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
                //newSpell.Description = newSpell.DescriptionDiv.InnerHtml;
                newSpell.Overflow = spell.SelectSingleNode("div[@class='overflow']")?.InnerText;
                newSpell.NoOverflow = spell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
                newSpell.Source = spell.SelectSingleNode("div[@class='source']").InnerText;
                newSpells.Add(newSpell);
                cardDatas.AddRange(Converters.ToCardDatas(context, newSpell));
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
        */

    }
}
