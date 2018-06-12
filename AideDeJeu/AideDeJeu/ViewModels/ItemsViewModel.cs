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
        //public abstract void ExecuteLoadItemsCommand();
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
                    case ItemSourceType.MonsterVF:
                        resourceName = "AideDeJeu.Data.monsters_vf.json";
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Monster>>(resourceName);
                        break;
                    case ItemSourceType.MonsterVO:
                        resourceName = "AideDeJeu.Data.monsters_vo.json";
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Monster>>(resourceName);
                        break;
                    case ItemSourceType.MonsterHD:
                        resourceName = "AideDeJeu.Data.monsters_hd.json";
                        var mdm = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/monsters_hd.md");
                        _AllItems = Tools.MarkdownExtensions.MarkdownToMonsters(mdm);
                        //_AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Monster>>(resourceName);
                        break;
                    case ItemSourceType.SpellVF:
                        resourceName = "AideDeJeu.Data.spells_vf.json";
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Spell>>(resourceName);
                        //var md2 = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
                        //_AllItems = Tools.MarkdownExtensions.MarkdownToSpells(md2).ToList();
                        break;
                    case ItemSourceType.SpellVO:
                        resourceName = "AideDeJeu.Data.spells_vo.json";
                        _AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Spell>>(resourceName);
                        break;
                    case ItemSourceType.SpellHD:
                        resourceName = "AideDeJeu.Data.spells_hd.json";
                        var mds = await Tools.Helpers.GetStringFromUrl("https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/spells_hd.md");
                        _AllItems = Tools.MarkdownExtensions.MarkdownToSpells(mds);
                        //_AllItems = Tools.Helpers.GetResourceObject<IEnumerable<Spell>>(resourceName);
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
                // Yan : c'est pas plutôt cette partie qui devrait être dans une autre Task ?
                var filterViewModel = Main.GetFilterViewModel(ItemSourceType);
                var items = await filterViewModel.FilterItems(await GetAllItemsAsync(), token);
                Main.Items = items;
                //await Task.Run(async () => {
                // Yan : plus besoin de boucle si on change toute la liste d'un coup ;)
                // Yan : indispensable de repasser sur l'ui thread pour la version uwp
                //Device.BeginInvokeOnMainThread(() => Main.Items = items);
                //Main.Items.Clear();
                //foreach (var item in items)
                //{
                //    token.ThrowIfCancellationRequested();
                //    Main.Items.Add(item);
                //}
                //}, cancellationToken: token); // Yan : c'est ici qu'il faudrait coller le token non ?

                //On arrete le loading ici car on annule toujours avant de lancer une nouvelle opération
                // Yan : ?? du coup le IsLoading repasse pas à false en cas de cancel ou d'autre exception ?
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
