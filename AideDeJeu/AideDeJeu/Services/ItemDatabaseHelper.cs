using AideDeJeuLib.Monsters;
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

        public async Task<IEnumerable<Spell>> GetSpellsAsync(string classe, string niveauMin, string niveauMax, string ecole, string rituel, string source)
        {
            using (var context = CreateContext())
            {
                //We use OrderByDescending because Posts are generally displayed from most recent to oldest
                return await context.Spells
                                    .AsNoTracking()
                                    .Where(spell => 
                                        (int.Parse(spell.Level) >= int.Parse(niveauMin)) && 
                                        (int.Parse(spell.Level) <= int.Parse(niveauMax)) &&
                                        spell.Type.Contains(ecole) &&
                                        spell.Source.Contains(source) &&
                                        spell.Type.Contains(classe) &&
                                        spell.Type.Contains(rituel)
                                        )
                                    .OrderByDescending(spell => spell.NamePHB)
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

        public async Task<IEnumerable<Monster>> GetMonstersAsync()
        {
            using (var context = CreateContext())
            {
                //We use OrderByDescending because Posts are generally displayed from most recent to oldest
                return await context.Monsters
                                    .AsNoTracking()
                                    .OrderByDescending(monster => monster.Id)
                                    .ToListAsync();
            }
        }

        public async Task AddOrUpdateMonstersAsync(IEnumerable<Monster> monsters)
        {
            using (var context = CreateContext())
            {
                // add posts that do not exist in the database
                var newMonsters = monsters.Where(
                    monster => context.Monsters.Any(dbMonster => dbMonster.Id == monster.Id) == false
                );
                await context.Monsters.AddRangeAsync(newMonsters);
                await context.SaveChangesAsync();
            }
        }
    }
}
