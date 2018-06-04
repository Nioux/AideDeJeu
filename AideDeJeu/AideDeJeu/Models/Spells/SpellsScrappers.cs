using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AideDeJeuLib.Spells
{
    public class SpellsScrappers : IDisposable
    {
        private HttpClient _Client = null;
        public HttpClient GetHttpClient()
        {
            if (_Client == null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr"));
                client.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("fr-FR"));
                _Client = client;
            }
            return _Client;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                    if (_Client != null)
                    {
                        _Client.Dispose();
                        _Client = null;
                    }
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~SpellsScrappers() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public async Task<IEnumerable<Spell>> GetSpells(string classe = "", string niveauMin = "", string niveauMax = "", string ecole = "", string rituel = "", string source = "srd")
        {
            string html = null;
            var client = GetHttpClient();
            // https://www.aidedd.org/regles/sorts/

            //var url = string.Format("https://www.aidedd.org/regles/sorts/?c={0}&min={1}&max={2}&e={3}&r={4}&s={5}", classe, niveauMin, niveauMax, ecole, rituel, source);
            var url = string.Format("https://www.aidedd.org/dnd-filters/sorts.php?c={0}&min={1}&max={2}&e={3}&r={4}&s={5}", classe, niveauMin, niveauMax, ecole, rituel, source);
            html = await client.GetStringAsync(url);

            var pack = new XmlDocument();
            pack.LoadXml(html);
            //var tdssort = pack.GetElementbyId("liste").Element("table").Elements("tr").ToList();
            var tdssort = pack.DocumentElement.SelectSingleNode("//table[contains(@class,'liste')]").SelectNodes("tr");
            var spells = new List<Spell>();
            foreach (var tdsortt in tdssort)
            {
                var tdsort = tdsortt as XmlNode;
                var thssort = tdsort.SelectNodes("td");
                if (thssort.Count > 0)
                {
                    Spell spell = new Spell();
                    var aname = thssort[1].SelectSingleNode("a");
                    var spanname = aname.SelectSingleNode("span");
                    if(spanname != null)
                    {
                        spell.NamePHB = spanname.Attributes["title"].InnerText;
                        spell.Name = spanname.InnerText;
                    }
                    else
                    {
                        spell.NamePHB = aname.InnerText;
                        spell.Name = aname.InnerText;
                    }
                    var href = aname.Attributes["href"].InnerText;
                    var regex = new Regex("vf=(?<id>.*)");
                    spell.Id = regex.Match(href).Groups["id"].Value;

                    spell.Level = thssort[2].InnerText;
                    spell.Type = thssort[3].InnerText;
                    spell.CastingTime = thssort[4].InnerText;
                    spell.Concentration = thssort[5].InnerText;
                    spell.Rituel = thssort[6].InnerText;
                    spells.Add(spell);
                }
            }
            return spells;
        }

        public async Task<Spell> GetSpell(string id)
        {
            string html = null;
            var client = GetHttpClient();
            // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
            // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre

            html = await client.GetStringAsync(string.Format("https://www.aidedd.org/dnd/sorts.php?vf={0}", id));

            var pack = new XmlDocument();
            pack.LoadXml(html);
            var divSpell = pack.DocumentElement.SelectSingleNode("//div[contains(@class,'bloc')]");
            var spell = Spell.FromHtml(divSpell);
            spell.Id = id;
            return spell;
        }

        public async Task<IEnumerable<string>> GetSpellIds(string classe, string niveauMin = "Z", string niveauMax = "9")
        {
            string html = null;
            var client = GetHttpClient();
            // https://www.aidedd.org/dnd/sorts.php?vo=ray-of-frost
            // https://www.aidedd.org/dnd/sorts.php?vf=rayon-de-givre
            // https://www.aidedd.org/regles/sorts/

            html = await client.GetStringAsync(string.Format("https://www.aidedd.org/adj/livre-sorts/?c={0}&min={1}&max={2}", classe, niveauMin, niveauMax));

            var pack = new XmlDocument();
            pack.LoadXml(html);
            return pack.DocumentElement.SelectNodes("//input[@name='select_sorts[]']").Cast<XmlNode>().Select(node => node.Attributes["value"].InnerText);
        }

        public async Task<IEnumerable<Spell>> GetSpells(IEnumerable<string> spellIds)
        {
            string html = null;
            var client = GetHttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("card"), "format");
            foreach (var spellId in spellIds)
            {
                content.Add(new StringContent(spellId), "select_sorts[]");
            }
            var response = await client.PostAsync("http://www.aidedd.org/dnd/sorts.php", content);
            html = await response.Content.ReadAsStringAsync();

            var pack = new XmlDocument();
            pack.LoadXml(html);
            var newSpells = new List<Spell>();
            var spells = pack.DocumentElement.SelectNodes("//div[contains(@class,'blocCarte')]");
            foreach (var spell in spells)
            {
                //var newSpell = new Spell();
                var newSpell = Spell.FromHtml(spell as XmlNode);
                //newSpell.Name = spell.SelectSingleNode("h1").InnerText;
                //newSpell.NameVO = spell.SelectSingleNode("div[@class='trad']").InnerText;
                //newSpell.LevelType = spell.SelectSingleNode("h2/em").InnerText;
                //newSpell.Level = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[0].Split(' ')[1];
                //newSpell.Type = newSpell.LevelType.Split(new string[] { " - " }, StringSplitOptions.None)[1];
                //newSpell.CastingTime = spell.SelectSingleNode("div[@class='paragraphe']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                //newSpell.Range = spell.SelectSingleNode("div[strong/text()='Portée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                //newSpell.Components = spell.SelectSingleNode("div[strong/text()='Composantes']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                //newSpell.Duration = spell.SelectSingleNode("div[strong/text()='Durée']").InnerText.Split(new string[] { " : " }, StringSplitOptions.None)[1];
                //newSpell.DescriptionDiv = spell.SelectSingleNode("div[contains(@class,'description')]");
                ////newSpell.DescriptionHtml = newSpell.DescriptionDiv.InnerHtml;
                ////newSpell.Description = newSpell.DescriptionDiv.InnerText;
                //newSpell.Overflow = spell.SelectSingleNode("div[@class='overflow']")?.InnerText;
                //newSpell.NoOverflow = spell.SelectSingleNode("div[@class='nooverflow']")?.InnerText;
                //newSpell.Source = spell.SelectSingleNode("div[@class='source']").InnerText;
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
