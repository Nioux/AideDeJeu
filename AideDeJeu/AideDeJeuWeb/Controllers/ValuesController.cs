using System.Collections.Generic;
using System.Threading.Tasks;
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
            var scrapper = new SpellsScrappers();
            var spellIds = await scrapper.GetSpellIds(classe);
            var spells = await scrapper.GetSpells(spellIds);


            var cardDatas = new List<CardData>();
            foreach (var spell in spells)
            {
                cardDatas.AddRange(Converters.ToCardDatas(spell));
            }
            return cardDatas;
            //var spellIds = spells.Select(spell => spell.Id);
            //var fullspells = await scrapper.GetSpells(spellIds);
            //return spells;
            //return await scrapper.GetSpellIds("");
            //return await new SpellsScrappers().GetSpells();
            //return new string[] { "value1", "value2" };
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
