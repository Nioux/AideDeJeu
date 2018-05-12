using AideDeJeuLib.Spells;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.Services
{
    public class ItemDatabaseHelper<T> where T : ItemDatabaseContext
    {
        protected T CreateContext()
        {
            T postDatabaseContext = (T)Activator.CreateInstance(typeof(T));
            postDatabaseContext.Database.EnsureCreated();
            postDatabaseContext.Database.Migrate();
            return postDatabaseContext;
        }

        public async Task<IEnumerable<Spell>> GetSpellsAsync()
        {
            using (var context = CreateContext())
            {
                //We use OrderByDescending because Posts are generally displayed from most recent to oldest
                return await context.Spells
                                    .AsNoTracking()
                                    .OrderByDescending(spell => spell.Id)
                                    .ToListAsync();
            }
        }

        public async Task AddOrUpdateSpellsAsync(IEnumerable<Spell> spells)
        {
            using (var context = CreateContext())
            {
                // add posts that do not exist in the database
                var newSpells = spells.Where(
                    spell => context.Spells.Any(dbSpell => dbSpell.Id == spell.Id) == false
                );
                await context.Spells.AddRangeAsync(newSpells);
                await context.SaveChangesAsync();
            }
        }
    }
}
