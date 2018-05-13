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
    public class MonstersScrappers : IDisposable
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


        public async Task<IEnumerable<Monster>> GetMonsters(string category = "", string type = "", string minPower = "", string maxPower = "", string size = "", string legendary = "", string source = "srd")
        {
            string html = null;
            var client = GetHttpClient();
            // https://www.aidedd.org/regles/monstres/?min=.25&max=20&c=M&sz=TP&lg=si&t=Humano%C3%AFde&s=srd

            html = await client.GetStringAsync(string.Format($"https://www.aidedd.org/regles/monstres/?c={category}&t={type}&min={minPower}&max={maxPower}&sz={size}&lg={legendary}&s={source}", category, type, minPower, maxPower, size, legendary, source));

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
                    var aname = tds[0].Element("a");
                    var spanname = aname.Element("span");
                    if (spanname != null)
                    {
                        monster.NamePHB = spanname.GetAttributeValue("title", "");
                        monster.Name = spanname.Element("strong").InnerText;
                    }
                    else
                    {
                        monster.NamePHB = aname.Element("strong").InnerText;
                        monster.Name = aname.Element("strong").InnerText;
                    }

                    //monster.Name = tds[0].InnerText;
                    var href = aname.GetAttributeValue("href", "");
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
            var client = GetHttpClient();
            // https://www.aidedd.org/dnd/monstres.php?vf=aarakocra

            html = await client.GetStringAsync(string.Format($"https://www.aidedd.org/dnd/monstres.php?vf={id}", id));

            var pack = new HtmlDocument();
            pack.LoadHtml(html);
            var divBloc = pack.DocumentNode.SelectNodes("//div[contains(@class,'bloc')]").FirstOrDefault();
            return Monster.FromHtml(divBloc);
        }

    }
}
