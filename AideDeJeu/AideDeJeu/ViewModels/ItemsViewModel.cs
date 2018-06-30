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
    public abstract class ItemsViewModel : BaseViewModel
    {
        CancellationTokenSource cancellationTokenSource;

        public ItemsViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync().ConfigureAwait(false));
        }
        public ICommand LoadItemsCommand { get; protected set; }
        public abstract Task ExecuteGotoItemCommandAsync(Item item);
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
                            _AllItems = Tools.MarkdownExtensions.MarkdownToMonsters(md);
                        }
                        break;
                    case ItemSourceType.MonsterHD:
                        {
                            resourceName = "AideDeJeu.Data.monsters_hd.md";
                            //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/monsters_hd.md");
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToMonsters(md);
                        }
                        break;
                    case ItemSourceType.SpellVO:
                        {
                            resourceName = "AideDeJeu.Data.spells_vo.md";
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToSpells(md);
                        }
                        break;
                    case ItemSourceType.SpellHD:
                        {
                            resourceName = "AideDeJeu.Data.spells_hd.md";
                            //var md = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
                            var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                            _AllItems = Tools.MarkdownExtensions.MarkdownToSpells(md);
                        }
                        break;
                }
            }
            return _AllItems;
        }

        async Task LoadItemsAsync(CancellationToken token = default)
        {
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                var filterViewModel = Main.GetFilterViewModel(ItemSourceType);
                var items = await filterViewModel.FilterItems(await GetAllItemsAsync(), token);
                Main.Items = items.ToList();
                Main.IsLoading = false;
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteLoadItemsCommandAsync()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            cancellationTokenSource = new CancellationTokenSource();
            await LoadItemsAsync(cancellationTokenSource.Token);
        }
    }
}
