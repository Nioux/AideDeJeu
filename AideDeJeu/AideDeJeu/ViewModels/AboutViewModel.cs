using AideDeJeu.Services;
using AideDeJeu.Tools;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "À propos de ...";

            OpenWebCommand = new Command<string>((param) => Device.OpenUri(new Uri(param)));
            UpdateDataCommand = new Command(async() => await ExecuteUpdateDataCommandAsync());
        }

        public ICommand OpenWebCommand { get; }
        public ICommand UpdateDataCommand { get; }

        public string Version {
            get
            {
                return DependencyService.Get<INativeAPI>().GetVersion();
            }
        }

        private int countUpdateData = 0;
        private async Task ExecuteUpdateDataCommandAsync()
        {
            if (countUpdateData++ != 5) return;

            IsBusy = true;
            var helper = new ItemDatabaseHelper<ItemDatabaseContext>();

            using (var spellsScrappers = new SpellsScrappers())
            {
                var partialSpells = await spellsScrappers.GetSpells();
                var spells = new List<Spell>();
                foreach (var partialSpell in partialSpells)
                {
                    var spell = await spellsScrappers.GetSpell(partialSpell.Id);
                    spells.Add(spell);
                }
                await helper.AddOrUpdateSpellsAsync(spells);
            }

            using (var monstersScrappers = new MonstersScrappers())
            {
                var partialMonsters = await monstersScrappers.GetMonsters();
                var monsters = new List<Monster>();
                foreach (var partialMonster in partialMonsters)
                {
                    var monster = await monstersScrappers.GetMonster(partialMonster.Id);
                    monsters.Add(monster);
                }
                await helper.AddOrUpdateMonstersAsync(monsters);
            }

            IsBusy = false;
        }
    }
}