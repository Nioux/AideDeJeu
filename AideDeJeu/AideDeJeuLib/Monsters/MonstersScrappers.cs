using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AideDeJeuLib.Monsters
{
    public class MonstersScrappers
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

        public async Task<IEnumerable<Monster>> GetMonsters(string categorie = "", string type = "", string niveauMin = "", string niveauMax = "", string taille = "", string legendaire = "", string source = "srd")
        {
            string html = null;
            using (var client = GetHttpClient())
            {
                // https://www.aidedd.org/regles/monstres/?min=.25&max=20&c=M&sz=TP&lg=si&t=Humano%C3%AFde&s=srd

                html = await client.GetStringAsync(string.Format($"https://www.aidedd.org/regles/monstres/?c={categorie}&t={type}&min={niveauMin}&max={niveauMax}&sz={taille}&lg={legendaire}&s={source}", categorie, type, niveauMin, niveauMax, taille, legendaire, source));
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var trs = pack.GetElementbyId("liste").Element("table").Elements("tr").ToList();
            var monsters = new List<Monster>();
            foreach (var tr in trs)
            {
                var tds = tr.Elements("td").ToArray();
                if (tds.Length > 0)
                {
                    var monster = new Monster();
                    monster.Name = tds[0].InnerText;
                    var href = tds[0].Element("a").GetAttributeValue("href", "");
                    var regex = new Regex("vf=(?<id>.*)");
                    monster.Id = regex.Match(href).Groups["id"].Value;
                    monster.Power = tds[1].InnerText;
                    monster.Type = tds[2].InnerText;
                    monster.Size = tds[3].InnerText;
                    monster.Alignment = tds[4].InnerText;
                    monster.Legendary = tds[5].InnerText;
                    monsters.Add(monster);
                }
            }
            return monsters;
        }

        public async Task<Monster> GetMonster(string id)
        {
            string html = null;
            using (var client = GetHttpClient())
            {
                // https://www.aidedd.org/dnd/monstres.php?vf=aarakocra

                html = await client.GetStringAsync(string.Format($"https://www.aidedd.org/dnd/monstres.php?vf={id}", id));
            }
            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var divBloc = pack.DocumentNode.SelectNodes("//div[contains(@class,'bloc')]").FirstOrDefault();
            return Monster.FromHtml(divBloc);
        }

    }
}
