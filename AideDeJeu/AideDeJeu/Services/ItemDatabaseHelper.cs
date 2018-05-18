using AideDeJeu.Tools;
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
        protected async Task<T> CreateContextAsync()
        {
            T postDatabaseContext = (T)Activator.CreateInstance(typeof(T));
            await postDatabaseContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            await postDatabaseContext.Database.MigrateAsync().ConfigureAwait(false);
            return postDatabaseContext;
        }

        public async Task<IEnumerable<Spell>> GetSpellsAsync(string classe, string niveauMin, string niveauMax, string ecole, string rituel, string source)
        {
            using (var context = await CreateContextAsync().ConfigureAwait(false))
            {
                return await context.Spells
                                    .AsNoTracking()
                                    .Where(spell => 
                                        (int.Parse(spell.Level) >= int.Parse(niveauMin)) && 
                                        (int.Parse(spell.Level) <= int.Parse(niveauMax)) &&
                                        spell.Type.Contains(ecole) &&
                                        spell.Source.Contains(source) &&
                                        spell.Source.Contains(classe) &&
                                        spell.Type.Contains(rituel)
                                        )
                                    //.OrderByDescending(spell => spell.NamePHB)
                                    .ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task AddOrUpdateSpellsAsync(IEnumerable<Spell> spells)
        {
            using (var context = await CreateContextAsync().ConfigureAwait(false))
            {
                // add posts that do not exist in the database
                var newSpells = spells.Where(
                    spell => context.Spells.Any(dbSpell => dbSpell.Id == spell.Id) == false
                );
                await context.Spells.AddRangeAsync(newSpells).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Monster>> GetMonstersAsync(string category, string type, string minPower, string maxPower, string size, string legendary, string source)
        {
            using (var context = await CreateContextAsync().ConfigureAwait(false))
            {
                var powerComparer = new PowerComparer();
                return await context.Monsters
                                    .AsNoTracking()
                                    .Where(monster =>
                                        monster.Type.Contains(type) &&
                                        (string.IsNullOrEmpty(size) || monster.Size.Equals(size)) &&
                                        monster.Source.Contains(source) &&
                                        powerComparer.Compare(monster.Challenge, minPower) >= 0 &&
                                        powerComparer.Compare(monster.Challenge, maxPower) <= 0
                                        )
                                    //.OrderByDescending(monster => monster.Id)
                                    .ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task AddOrUpdateMonstersAsync(IEnumerable<Monster> monsters)
        {
            using (var context = await CreateContextAsync().ConfigureAwait(false))
            {
                // add posts that do not exist in the database
                var newMonsters = monsters.Where(
                    monster => context.Monsters.Any(dbMonster => dbMonster.Id == monster.Id) == false
                );
                await context.Monsters.AddRangeAsync(newMonsters).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
