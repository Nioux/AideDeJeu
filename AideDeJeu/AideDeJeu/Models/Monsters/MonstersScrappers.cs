using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

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

            //html = await client.GetStringAsync(string.Format($"https://www.aidedd.org/regles/monstres/?c={category}&t={type}&min={minPower}&max={maxPower}&sz={size}&lg={legendary}&s={source}", category, type, minPower, maxPower, size, legendary, source));
            var url = string.Format($"https://www.aidedd.org/dnd-filters/monstres.php?c={category}&t={type}&min={minPower}&max={maxPower}&sz={size}&lg={legendary}&s={source}", category, type, minPower, maxPower, size, legendary, source);
            html = await client.GetStringAsync(url);

            var pack = new XmlDocument();
            pack.LoadXml(html);
            //var trs = pack.GetElementbyId("liste").Element("table").Elements("tr").ToList();
            var trs = pack.DocumentElement.SelectSingleNode("//table[contains(@class,'liste')]").SelectNodes("tr");
            var monsters = new List<Monster>();
            foreach (var tro in trs)
            {
                var tr = tro as XmlNode;
                var tds = tr.SelectNodes("td");
                if (tds.Count > 0)
                {
                    var monster = new Monster();
                    var aname = tds[1].SelectSingleNode("a");
                    var spanname = aname.SelectSingleNode("span");
                    if (spanname != null)
                    {
                        monster.NamePHB = spanname.Attributes["title"].InnerText;
                        monster.Name = spanname.InnerText;
                    }
                    else
                    {
                        monster.NamePHB = aname.InnerText;
                        monster.Name = aname.InnerText;
                    }

                    //monster.Name = tds[0].InnerText;
                    var href = aname.Attributes["href"].InnerText;
                    var regex = new Regex("vf=(?<id>.*)");
                    monster.Id = regex.Match(href).Groups["id"].Value;
                    monster.Power = tds[2].InnerText;
                    monster.Type = tds[3].InnerText;
                    monster.Size = tds[4].InnerText;
                    monster.Alignment = tds[5].InnerText;
                    monster.Legendary = tds[6].InnerText;
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

            var pack = new XmlDocument();
            pack.LoadXml(html);
            var divBloc = pack.DocumentElement.SelectSingleNode("//div[contains(@class,'bloc')]");
            var monster = Monster.FromHtml(divBloc);
            monster.Id = id;
            return monster;
        }

    }
}
