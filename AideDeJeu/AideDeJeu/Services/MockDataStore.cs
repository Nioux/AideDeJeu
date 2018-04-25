using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AideDeJeu.Models;
using AideDeJeuLib;
using AideDeJeuLib.Spells;

[assembly: Xamarin.Forms.Dependency(typeof(AideDeJeu.Services.MockDataStore))]
namespace AideDeJeu.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            var scrapper = new Scrappers();
            var spells = await scrapper.GetSpells(await scrapper.GetSpellIds("c"));
            items = spells.Select(spell => new Item() { Text = spell.Title, Description = spell.DescriptionText }).ToList();
            return await Task.FromResult(items);
        }
        public async Task<IEnumerable<Item>> GetItemsAsync(string classe, int minLevel, int maxLevel, string ecole, string rituel, string source)
        {
            return await GetItemsAsync();
        }

    }
}