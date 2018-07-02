using AideDeJeuLib;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading;
using System.Linq;

namespace AideDeJeu.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        CancellationTokenSource cancellationTokenSource;

        public ItemsViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync().ConfigureAwait(false));
        }
        public ICommand LoadItemsCommand { get; protected set; }
        public async Task ExecuteGotoItemCommandAsync(Item item)
        {
            await Main.Navigator.GotoItemDetailPageAsync(item);
        }
        protected ItemSourceType ItemSourceType;


        private IEnumerable<Item> _AllItems = null;
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            if (_AllItems == null)
            {
                string resourceName = null;
                switch (ItemSourceType)
                {
                    case ItemSourceType.MonsterVO:
                        {
                            resourceName = "AideDeJeu.Data.monsters_vo.md";
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToMonsters<MonsterVO>(md);
                        }
                        break;
                    case ItemSourceType.MonsterHD:
                        {
                            resourceName = "AideDeJeu.Data.monsters_hd.md";
                            //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/monsters_hd.md");
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToMonsters<MonsterHD>(md);
                        }
                        break;
                    case ItemSourceType.SpellVO:
                        {
                            resourceName = "AideDeJeu.Data.spells_vo.md";
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToSpells<SpellVO>(md);
                        }
                        break;
                    case ItemSourceType.SpellHD:
                        {
                            resourceName = "AideDeJeu.Data.spells_hd.md";
                            //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToSpells<SpellHD>(md);
                        }
                        break;
                }
            }
            return _AllItems;
        }

        async Task LoadItemsAsync(CancellationToken cancellationToken = default)
        {
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                var filterViewModel = Main.GetFilterViewModel(ItemSourceType);
                var items = await filterViewModel.FilterItems(await GetAllItemsAsync(), cancellationToken: cancellationToken);
                Main.Items = items.ToList();
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                Main.IsLoading = false;
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadItemsCommandAsync()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            await LoadItemsAsync(cancellationTokenSource.Token);
        }
    }
}
