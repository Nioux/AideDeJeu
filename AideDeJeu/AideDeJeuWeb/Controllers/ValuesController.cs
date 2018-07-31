using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using AideDeJeuLib.Cards;
//using AideDeJeuLib.Spells;
using Microsoft.AspNetCore.Mvc;

namespace AideDeJeuWeb.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet("{classe}")]
        public async Task<IEnumerable<CardData>> Get(string classe)
        {
            //var items = AideDeJeu.Tools.Helpers.GetResourceObject<IEnumerable<Spell>>("AideDeJeu.Data.spells_vf.json");
            var md = await AideDeJeu.Tools.Helpers.GetResourceStringAsync("AideDeJeu.Data.spells_hd.md");
            var item = AideDeJeu.Tools.MarkdownExtensions.ToItem(md);
            var items = item as Items;

            var fitems = items.Where(it => (it as Spell).Source.Contains(classe)).OrderBy(it => (it as Spell).Level).ThenBy(it => it.Name);

            var cardDatas = new List<CardData>();
            foreach (var spell in fitems)
            {
                cardDatas.AddRange(Converters.ToCardDatas(spell as Spell));
            }
            return cardDatas;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
