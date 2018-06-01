using System.Collections.Generic;
using System.Threading.Tasks;
using AideDeJeu.ViewModels;
using AideDeJeuLib.Cards;
using AideDeJeuLib.Spells;
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
            var items = AideDeJeu.Tools.Helpers.GetResourceObject<IEnumerable<Spell>>("AideDeJeu.Data.spells_vf.json");
            var filter = new VFSpellFilterViewModel();
            var fitems = await filter.FilterItems(items);

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
