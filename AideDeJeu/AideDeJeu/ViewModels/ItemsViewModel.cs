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

namespace AideDeJeu.ViewModels
{
    public abstract class ItemsViewModel : BaseViewModel
    {
        CancellationTokenSource cancellationTokenSource;

        public ItemsViewModel(ItemSourceType itemSourceType)
        {
            this.ItemSourceType = itemSourceType;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
        }
        public ICommand LoadItemsCommand { get; protected set; }
        //public abstract void ExecuteLoadItemsCommand();
        public abstract Task ExecuteGotoItemCommandAsync(Item item);
        protected ItemSourceType ItemSourceType;


        private IEnumerable<Item> _AllItems = null;
        public IEnumerable<Item> AllItems
        {
            get
            {
                if (_AllItems == null)
                {
                    string resourceName = null;
                    switch (ItemSourceType)
                    {
                        case ItemSourceType.MonsterVF:
                            resourceName = "AideDeJeu.Data.monsters_vf.json";
                            break;
                        case ItemSourceType.MonsterVO:
                            resourceName = "AideDeJeu.Data.monsters_vo.json";
                            break;
                        case ItemSourceType.MonsterHD:
                            resourceName = "AideDeJeu.Data.monsters_hd.json";
                            break;
                        case ItemSourceType.SpellVF:
                            resourceName = "AideDeJeu.Data.spells_vf.json";
                            break;
                        case ItemSourceType.SpellVO:
                            resourceName = "AideDeJeu.Data.spells_vo.json";
                            break;
                        case ItemSourceType.SpellHD:
                            resourceName = "AideDeJeu.Data.spells_hd.json";
                            break;
                    }
                    if (ItemSourceType.HasFlag(ItemSourceType.Spell))
                    {
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Spell>>(resourceName);
                    }
                    else if (ItemSourceType.HasFlag(ItemSourceType.Monster))
                    {
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Monster>>(resourceName);
                    }
                }
                return _AllItems;
            }
        }

        async Task LoadItemsAsync(CancellationToken token = default)
        {
            IsBusy = true;
            Main.IsLoading = true;
            try
            {
                var filterViewModel = Main.GetFilterViewModel(ItemSourceType);
                var items = await filterViewModel.FilterItems(AllItems, token);
                await Task.Run(() =>
                {
                    Main.Items.Clear();
                    foreach (var item in items)
                    {
                        token.ThrowIfCancellationRequested();
                        Main.Items.Add(item);
                    }
                });

                //On arrete le loading ici car on annule toujours avant de lancer une nouvelle opération
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
