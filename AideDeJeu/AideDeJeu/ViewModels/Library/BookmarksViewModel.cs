using AideDeJeu.Repositories;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AideDeJeu.ViewModels.Library
{
    public class BookmarksViewModel : BaseViewModel
    {
        public BookmarksRepository Repository
        {
            get
            {
                return DependencyService.Get<BookmarksRepository>();
            }
        }
        public BookmarksViewModel()
        {
            //LoadBookmarkCollectionAsync(BookmarkCollectionNames[BookmarkCollectionIndex]).ConfigureAwait(true);
            LoadBookmarkCollectionsAsync().ConfigureAwait(true);
        }

        //public ObservableCollection<string> BookmarkCollectionNames { get; set; } = new ObservableCollection<string>()
        //{
        //    "Général",
        //    "Grimoire",
        //    "Bestiaire",
        //    "Sac",
        //    "Nouvelle liste",
        //};

        //private int _BookmarkCollectionIndex = 0;
        //public int BookmarkCollectionIndex
        //{
        //    get
        //    {
        //        return _BookmarkCollectionIndex;
        //    }
        //    set
        //    {
        //        SetProperty(ref _BookmarkCollectionIndex, value);
        //        //LoadBookmarkCollection(BookmarkCollectionNames[BookmarkCollectionIndex]);
        //    }
        //}

        //private Dictionary<string, ObservableCollection<Item>> _BookmarkCollections = new Dictionary<string, ObservableCollection<Item>>();

        //private ObservableCollection<Item> _BookmarkCollection = new ObservableCollection<Item>();
        //public ObservableCollection<Item> BookmarkCollection
        //{
        //    get
        //    {
        //        return _BookmarkCollection;
        //    }
        //    set
        //    {
        //        SetProperty(ref _BookmarkCollection, value);
        //    }
        //}

        private ICommand _SelectedIndexChangedCommand = null;
        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                return _SelectedIndexChangedCommand ?? (_SelectedIndexChangedCommand = new Command(async () => await ExecuteSelectedIndexChangedCommandAsync()));
            }
        }

        private async Task ExecuteSelectedIndexChangedCommandAsync()
        {
            if (Repository.BookmarkCollectionIndex >= 0 && Repository.BookmarkCollectionIndex < Repository.BookmarkCollectionNames.Count - 1)
            {
                await LoadBookmarkCollectionAsync(Repository.BookmarkCollectionNames[Repository.BookmarkCollectionIndex]);
            }
            else if (Repository.BookmarkCollectionIndex == Repository.BookmarkCollectionNames.Count - 1)
            {
                var result = await Main.Navigator.OpenCancellableTextInputAlertDialog("");
                if (result.Item2 == Navigator.PopupResultEnum.Save)
                {
                    var index = Repository.BookmarkCollectionNames.Count - 1;
                    Repository.BookmarkCollectionNames.Insert(index, result.Item1);
                    //BookmarkCollectionIndex = index;
                    Repository.BookmarkCollectionIndex = 0;
                    await SaveBookmarksAsync();
                }
                else
                {
                    Repository.BookmarkCollectionIndex = 0;
                }
            }
        }

        private ICommand _GotoItemCommand = null;
        public ICommand GotoItemCommand
        {
            get
            {
                return _GotoItemCommand ?? (_GotoItemCommand = new Command<Item>(async (item) => await ExecuteGotoItemCommandAsync(item)));
            }
        }

        private async Task ExecuteGotoItemCommandAsync(Item item)
        {
            var litem = item as LinkItem;
            var Main = DependencyService.Get<MainViewModel>();
            await Main.Navigator.NavigateToLinkAsync(litem.Link);

        }

        private ICommand _RemoveItemCommand = null;
        public ICommand RemoveItemCommand
        {
            get
            {
                return _RemoveItemCommand ?? (_RemoveItemCommand = new Command<Item>(async (item) => await ExecuteRemoveItemCommandAsync(item)));
            }
        }

        private async Task ExecuteRemoveItemCommandAsync(Item item)
        {
            Repository.BookmarkCollection.Remove(item);
            await SaveBookmarksAsync();
        }

        private ICommand _MoveUpItemCommand = null;
        public ICommand MoveUpItemCommand
        {
            get
            {
                return _MoveUpItemCommand ?? (_MoveUpItemCommand = new Command<Item>(async (item) => await ExecuteMoveUpItemCommandAsync(item)));
            }
        }

        private async Task ExecuteMoveUpItemCommandAsync(Item item)
        {
            var index = Repository.BookmarkCollection.IndexOf(item);
            if (index > 0)
            {
                Repository.BookmarkCollection.Move(index, index - 1);
                await SaveBookmarksAsync();
            }
        }

        private ICommand _MoveDownItemCommand = null;
        public ICommand MoveDownItemCommand
        {
            get
            {
                return _MoveDownItemCommand ?? (_MoveDownItemCommand = new Command<Item>(async (item) => await ExecuteMoveDownItemCommandAsync(item)));
            }
        }

        private async Task ExecuteMoveDownItemCommandAsync(Item item)
        {
            var index = Repository.BookmarkCollection.IndexOf(item);
            if (index < Repository.BookmarkCollection.Count - 1)
            {
                Repository.BookmarkCollection.Move(index, index + 1);
                await SaveBookmarksAsync();
            }
        }

        private ICommand _ConfigureCommand = null;
        public ICommand ConfigureCommand
        {
            get
            {
                return _ConfigureCommand ?? (_ConfigureCommand = new Command(async () => await ExecuteConfigureCommandAsync()));
            }
        }

        private async Task ExecuteConfigureCommandAsync()
        {
            var result = await Main.Navigator.OpenCancellableTextInputAlertDialog(Repository.BookmarkCollectionNames[Repository.BookmarkCollectionIndex]);
            if (result.Item2 == Navigator.PopupResultEnum.Delete)
            {
                var confirm = await App.Current.MainPage.DisplayAlert("Supprimer ?", "Etes vous sûr de vouloir supprimer la liste ?", "Supprimer", "Annuler");
                if (confirm)
                {
                    var index = Repository.BookmarkCollectionIndex;
                    var name = Repository.BookmarkCollectionNames[Repository.BookmarkCollectionIndex];
                    await SaveBookmarksAsync(name, null);
                    Repository.BookmarkCollectionNames.Remove(name);
                    Repository.BookmarkCollectionIndex = 0;
                }
            }
            else if (result.Item2 == Navigator.PopupResultEnum.Save)
            {
                var index = Repository.BookmarkCollectionIndex;
                var items = await GetBookmarkCollectionAsync(Repository.BookmarkCollectionNames[index]);
                await SaveBookmarksAsync(Repository.BookmarkCollectionNames[index], null);
                Repository.BookmarkCollectionNames[index] = result.Item1;
                await SaveBookmarksAsync(Repository.BookmarkCollectionNames[index], items);
                Repository.BookmarkCollectionIndex = index;
            }
        }



        public async Task<List<Item>> GetBookmarkCollectionAsync(string key)
        {
            if (key != null)
            {
                if (App.Current.Properties.ContainsKey(key))
                {
                    var property = App.Current.Properties[key] as string;
                    if (property != null)
                    {
                        return (await ToItems(property)).ToList();
                    }
                }
            }
            return null;
        }
        public async Task LoadBookmarkCollectionAsync(string key)
        {
            var items = await GetBookmarkCollectionAsync(key);
            Repository.BookmarkCollection.Clear();
            if (items != null)
            {
                items.ForEach(item => Repository.BookmarkCollection.Add(item));
            }
        }

        public async Task AddBookmarkAsync(string key, Item item)
        {
            var linkItem = new LinkItem() { Name = item.Name, AltName = item.AltName, Link = item.Id };
            var items = await GetBookmarkCollectionAsync(key);
            if (items == null)
            {
                items = new List<Item>();
            }
            items.Add(linkItem);
            await SaveBookmarksAsync(key, items);
            if (Repository.BookmarkCollectionNames.IndexOf(key) >= 0)
            {
                Repository.BookmarkCollectionIndex = Repository.BookmarkCollectionNames.IndexOf(key);
            }
            else
            {
                Repository.BookmarkCollectionNames.Insert(Repository.BookmarkCollectionNames.Count - 1, key);
                Repository.BookmarkCollectionIndex = Repository.BookmarkCollectionNames.IndexOf(key);
            }
            await LoadBookmarkCollectionAsync(key);
        }

        public async Task SaveBookmarksAsync()
        {
            if (Repository.BookmarkCollectionIndex >= 0)
            {
                App.Current.Properties[Repository.BookmarkCollectionNames[Repository.BookmarkCollectionIndex]] = ToString(Repository.BookmarkCollection);
                await App.Current.SavePropertiesAsync();
            }
        }

        public async Task SaveBookmarksAsync(string key, List<Item> items)
        {
            if (items == null)
            {
                App.Current.Properties.Remove(key);
            }
            else
            {
                App.Current.Properties[key] = ToString(items);
            }
            await App.Current.SavePropertiesAsync();
        }


        public async Task InitBookmarkCollectionsAsync()
        {
            Repository._BookmarkCollections = new Dictionary<string, ObservableCollection<Item>>()
            {
                { "Général", new ObservableCollection<Item>() },
                { "Grimoire", new ObservableCollection<Item>() },
                { "Bestiaire", new ObservableCollection<Item>() },
                { "Sac", new ObservableCollection<Item>() },
            };
        }
        public async Task LoadBookmarkCollectionsAsync()
        {
            if(true)
            {
                await InitBookmarkCollectionsAsync();
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var spells = context.Spells.ToList();
                    var spell = spells.Take(1).FirstOrDefault();
                    Repository._BookmarkCollections["Grimoire"].Add(new Item() { Id = spell.Id, Name = spell.Name });
                }
                await SaveBookmarkCollectionsAsync();
            }
        }
        public async Task SaveBookmarkCollectionsAsync()
        {
            var yaml = new YamlDotNet.Serialization.Serializer().Serialize(Repository._BookmarkCollections);
            Debug.WriteLine(yaml);
        }
        public string ToString(IEnumerable<Item> items)
        {
            string md = string.Empty;
            md += "\n<!--Items-->\n\n";
            foreach (var item in items)
            {
                md += item.Markdown;
            }
            md += "\n\n<!--/Items-->\n";
            return md;
        }

        public async Task<IEnumerable<Item>> ToItems(string md)
        {
            var item = Store.ToItem(null, md, null);
            //if(item is Items)
            //{
            var items = item; // as Items;
            return await items.GetChildrenAsync();
            //}
            //return new List<Item> { item };
        }

        /*
        public string ToString(List<Item> items)
        {
            var serializer = ItemJsonSerializer;
            using(var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, items);
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public List<Item> ToItems(string str)
        {
            var serializer = ItemJsonSerializer;
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            using (var stream = new MemoryStream(byteArray))
            {
                return serializer.ReadObject(stream) as List<Item>;
            }
        }

        public DataContractJsonSerializer ItemJsonSerializer
        {
            get
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.KnownTypes = new List<Type>()
                {
                    typeof(HomeItem),
                    typeof(Spell),
                    typeof(Monster),
                    //typeof(Items),
                    typeof(LinkItem),
                    typeof(Equipment),
                    //typeof(Spells),
                    //typeof(Monsters),
                    //typeof(Equipments),
                    typeof(PageItem),
                };
                return new DataContractJsonSerializer(typeof(List<Item>), settings);
            }
        }
        */
    }
}
