using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AideDeJeu.Models;
using AideDeJeuLib;
using AideDeJeuLib.Spells;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Services.SpellDataStore))]
namespace AideDeJeu.Services
{
    public class SpellDataStore : IDataStore<Spell>
    {
        List<Spell> items;

        public SpellDataStore()
        {
            items = new List<Spell>();
            var mockItems = new List<Spell>
            {
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Spell item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Spell item)
        {
            var _item = items.Where((Spell arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Spell item)
        {
            var _item = items.Where((Spell arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Spell> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Spell>> GetItemsAsync(bool forceRefresh = false)
        {
            var scrapper = new Scrappers();
            //items = (await scrapper.GetSpells(await scrapper.GetSpellIds(""))).ToList();
            items = (await scrapper.GetSpells()).ToList();

            //items = spells.Select(spell => new Item() { Text = spell.Title, Description = spell.DescriptionText }).ToList();
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Spell>> GetItemsAsync(string classe, int niveauMin, int niveauMax, string ecole, string rituel, string source)
        {
            var scrapper = new Scrappers();
            items = (await scrapper.GetSpells(classe: classe, niveauMin: niveauMin, niveauMax: niveauMax, ecole: ecole, rituel: rituel, source: source)).ToList();

            return await Task.FromResult(items);
        }

    }
}