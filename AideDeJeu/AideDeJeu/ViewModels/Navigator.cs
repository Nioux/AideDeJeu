using AideDeJeu.Tools;
using AideDeJeu.Views;
using AideDeJeuLib;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public interface INavigator
    {
        Task GotoAboutPageAsync();
        Task GotoItemDetailPageAsync(Item item);
    }
    public class Navigator : BaseViewModel, INavigator
    {
        public INavigation Navigation;

        public Navigator(INavigation navigation)
        {
            Navigation = navigation;
        }

        private Command _AboutCommand = null;
        public Command AboutCommand
        {
            get
            {
                return _AboutCommand ?? (_AboutCommand = new Command(async () => await GotoAboutPageAsync()));
            }
        }

        public async Task GotoAboutPageAsync()
        {
            await Navigation.PushAsync(new Views.AboutPage(), true);
        }

        private Command _LibraryCommand = null;
        public Command LibraryCommand
        {
            get
            {
                return _LibraryCommand ?? (_LibraryCommand = new Command(async () => await GotoLibraryPageAsync()));
            }
        }

        //private NotifyTaskCompletion<bool> _TestNotify = null;
        //public NotifyTaskCompletion<bool> TestNotify
        //{
        //    get
        //    {
        //        return _TestNotify;
        //    }
        //    set
        //    {
        //        SetProperty(ref _TestNotify, value);
        //    }
        //}

        //public async Task<bool> TestGotoAsync()
        //{
        //    var page = new Views.MainTabbedPage();
        //    Device.BeginInvokeOnMainThread(async() => 
        //    await Navigation.PushAsync(page, true));
        //    return true;
        //}
        public async Task GotoLibraryPageAsync()
        {
            //await Navigation.PushAsync(new Views.ItemDetailPage(),true);
            await Navigation.PushAsync(new Views.MainTabbedPage(), true);
            //TestNotify = new NotifyTaskCompletion<bool>(TestGotoAsync(), true);
        }

        private Command _BookmarksCommand = null;
        public Command BookmarksCommand
        {
            get
            {
                return _BookmarksCommand ?? (_BookmarksCommand = new Command(async () => await GotoBookmarksPageAsync()));
            }
        }

        public async Task GotoBookmarksPageAsync()
        {
            await Navigation.PushAsync(new Views.BookmarksPage(), true);
        }



        private Command _PlayerCharacterEditorCommand = null;
        public Command PlayerCharacterEditorCommand
        {
            get
            {
                return _PlayerCharacterEditorCommand ?? (_PlayerCharacterEditorCommand = new Command(async () => await GotoPlayerCharacterEditorPageAsync()));
            }
        }

        public async Task GotoPlayerCharacterEditorPageAsync()
        {
            await Navigation.PushAsync(new Views.PlayerCharacterEditorPage(), true);
        }

        private Command _DeepSearchCommand = null;
        public Command DeepSearchCommand
        {
            get
            {
                return _DeepSearchCommand ?? (_DeepSearchCommand = new Command(async () => await GotoDeepSearchPageAsync()));
            }
        }

        public async Task GotoDeepSearchPageAsync()
        {
            await Navigation.PushAsync(new Views.DeepSearchPage(), true);
        }

        private Command _AddToFavoritesCommand = null;
        public Command AddToFavoritesCommand
        {
            get
            {
                return _AddToFavoritesCommand ?? (_AddToFavoritesCommand = new Command(async () => await ExecuteAddToFavoritesCommandAsync()));
            }
        }

        public async Task ExecuteAddToFavoritesCommandAsync()
        {
            var tabbedPage = App.Current.MainPage as MainTabbedPage;
            var navigationPage = tabbedPage; //.MainNavigationPage;
            var lastPage = navigationPage.Navigation.NavigationStack.LastOrDefault();
            var context = lastPage.BindingContext;
            Item item = null;
            if(context is ItemDetailViewModel)
            {
                item = (context as ItemDetailViewModel).Item;
            }
            else if(context is ItemsViewModel)
            {
                item = (context as ItemsViewModel).AllItems;
            }
            //await Application.Current.MainPage.DisplayAlert("Id", item.Id, "OK");
            var vm = Main.Bookmarks;
            var result = await Application.Current.MainPage.DisplayActionSheet("Ajouter à", "Annuler", "Nouvelle liste", vm.BookmarkCollectionNames.ToArray<string>());
            if (result != "Annuler")
            {
                if(result == "Nouvelle liste")
                {
                    int i = 1;
                    while(vm.BookmarkCollectionNames.Contains(result = $"Nouvelle liste ({i})"))
                    {
                        i++;
                    }
                }
                await vm.AddBookmarkAsync(result, item);
            }
        }

        public async Task GotoItemDetailPageAsync(Item item)
        {
            if (item == null)
                return;

            var items = item as Item;
            var filterViewModel = items.GetNewFilterViewModel();
            var itemsViewModel = new ItemsViewModel() { AllItems = items, Filter = filterViewModel };
            itemsViewModel.LoadItemsCommand.Execute(null);

            SwitchToMainTab();

            if (filterViewModel == null)
            {
                await GotoItemsPageAsync(itemsViewModel);
            }
            else
            {
                await GotoFilteredItemsPageAsync(itemsViewModel);
            }
        }

        public void SwitchToMainTab()
        {
            var tabbedPage = App.Current.MainPage as MainTabbedPage;
            if (tabbedPage != null)
            {
                tabbedPage.SelectedItem = null;
                tabbedPage.SelectedItem = tabbedPage; //.MainNavigationPage;
            }
        }

        public async Task GotoItemsPageAsync(ItemsViewModel itemsVM)
        {
            if (itemsVM == null)
                return;

            await Navigation.PushAsync(new ItemsPage(itemsVM), true);
        }

        public async Task GotoFilteredItemsPageAsync(ItemsViewModel itemsVM)
        {
            if (itemsVM == null)
                return;

            await Navigation.PushAsync(new FilteredItemsPage(itemsVM), true);
        }

        private ICommand _NavigateToLinkCommand = null;
        public ICommand NavigateToLinkCommand
        {
            get
            {
                return _NavigateToLinkCommand ?? (_NavigateToLinkCommand = new Command<string>(async(s) => await NavigateToLinkAsync(s)));
            }
        }

        public async Task NavigateToLinkAsync(string s)
        {
            if (s != null)
            {
                var regex = new Regex("/?(?<file>.*?)(_with_(?<with>.*))?\\.md(#(?<anchor>.*))?");
                var match = regex.Match(s);
                var file = match.Groups["file"].Value;
                var anchor = match.Groups["anchor"].Value;
                var with = match.Groups["with"].Value;
                Item item = null;
                try
                {
                    Main.IsBusy = true;
                    Main.IsLoading = true;
                    item = await Task.Run(async () => await Store.GetItemFromDataAsync(file, anchor));
                }
                finally
                {
                    Main.IsBusy = false;
                    Main.IsLoading = false;
                }
                if (item != null)
                {
                    var items = item; // as Items;
                    var filterViewModel = items.GetNewFilterViewModel();
                    var itemsViewModel = new ItemsViewModel() { AllItems = items, Filter = filterViewModel };
                    itemsViewModel.LoadItemsCommand.Execute(null);
                    if(!string.IsNullOrEmpty(with))
                    {
                        var swith = with.Split('_');
                        for (int i = 0; i < swith.Length / 2; i++)
                        {
                            var key = swith[i * 2 + 0];
                            var val = swith[i * 2 + 1];
                            filterViewModel.FilterWith(key, val);
                        }
                    }
                    SwitchToMainTab();
                    if (filterViewModel == null)
                    {
                        await GotoItemsPageAsync(itemsViewModel);
                    }
                    else
                    {
                        await GotoFilteredItemsPageAsync(itemsViewModel);
                    }

                }
                else
                {
                    //await App.Current.MainPage.DisplayAlert("Lien invalide", s, "OK");
                }
            }
        }



        public enum PopupResultEnum
        {
            Save,
            Cancel,
            Delete
        }
        public async Task<Tuple<string, PopupResultEnum>> OpenCancellableTextInputAlertDialog(string inputText)
        {
            // create the TextInputView
            var inputView = new TextInputCancellableView(
                "Nom de la liste ?", "Nouveau nom...", inputText, "Enregistrer", "Annuler", "Supprimer", "Le nom ne peut pas être vide.");

            // create the Transparent Popup Page
            // of type string since we need a string return
            var popup = new InputAlertDialogBase<Tuple<string, PopupResultEnum>>(inputView);
            

            // subscribe to the TextInputView's Button click event
            inputView.SaveButtonEventHandler +=
                (sender, obj) =>
                {
                    if (!string.IsNullOrEmpty(((TextInputCancellableView)sender).TextInputResult))
                    {
                        ((TextInputCancellableView)sender).IsValidationLabelVisible = false;
                        popup.PageClosedTaskCompletionSource.SetResult(new Tuple<string, PopupResultEnum>(((TextInputCancellableView)sender).TextInputResult, PopupResultEnum.Save));
                    }
                    else
                    {
                        ((TextInputCancellableView)sender).IsValidationLabelVisible = true;
                    }
                };

            // subscribe to the TextInputView's Button click event
            inputView.CancelButtonEventHandler +=
                (sender, obj) =>
                {
                    popup.PageClosedTaskCompletionSource.SetResult(new Tuple<string, PopupResultEnum>(null, PopupResultEnum.Cancel));
                };

            inputView.DeleteButtonEventHandler +=
                (sender, obj) =>
                {
                    popup.PageClosedTaskCompletionSource.SetResult(new Tuple<string, PopupResultEnum>(null, PopupResultEnum.Delete));
                };

            // Push the page to Navigation Stack
            await PopupNavigation.Instance.PushAsync(popup);

            // await for the user to enter the text input
            var result = await popup.PageClosedTask;

            // Pop the page from Navigation Stack
            await PopupNavigation.Instance.PopAsync();

            // return user inserted text value
            return result;
        }

    }
}
