using AideDeJeu.Views;
using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public interface INavigator
    {
        Task GotoAboutPageAsync();
        Task GotoMonsterDetailPageAsync(Item item);
        Task GotoSpellDetailPageAsync(Item item);
    }
    public class Navigator : INavigator
    {
        INavigation Navigation;

        public Navigator(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task GotoAboutPageAsync()
        {
            await Navigation.PushAsync(new Views.AboutPage());
        }

        public async Task GotoMonsterDetailPageAsync(Item item)
        {
            var monster = item as Monster;
            if (item == null)
                return;

            var vm = new MonsterDetailViewModel(monster);
            vm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new MonsterDetailPage(vm));
        }

        public async Task GotoSpellDetailPageAsync(Item item)
        {
            var spell = item as Spell;
            if (item == null)
                return;

            var vm = new SpellDetailViewModel(spell);
            vm.LoadItemCommand.Execute(null);
            await Navigation.PushAsync(new SpellDetailPage(vm));
        }


    }
}
