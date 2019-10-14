using AideDeJeu.Tools;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged(nameof(IsEnabled));

            }
        }

        public bool IsEnabled
        {
            get => !_isLoading;
        }

        public NotifyTaskCompletion<int> DebugCount = new NotifyTaskCompletion<int>(Task.Run(() => GetItemsCountAsync()));

        public static async Task<int> GetItemsCountAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return context.Items.Count();
            }
        }
        /*
        void AddAnchor(string source, Dictionary<string, Item> anchors, Item item)
        {
            if (item != null && item.Name != null)
            {
                var basename = Helpers.IdFromName(item.Name);
                //var name = $"{source}.md#{basename}";
                var name = $"{basename}";
                int index = 0;
                while (true)
                {
                    if (!anchors.ContainsKey(name))
                    {
                        item.Id = name;
                        anchors.Add(name, item);
                        return;
                    }
                    index++;
                    //name = $"{source}.md#{basename}{index}";
                    name = $"{basename}{index}";
                }
            }
        }
        void MakeAnchors(string source, Dictionary<string, Item> anchors, Item baseItem)
        {
            AddAnchor(source, anchors, baseItem);
            if(baseItem is Items)
            {
                foreach(var item in (baseItem as Items))
                {
                    MakeAnchors(source, anchors, item);
                }
            }
        }

        public class ItemWithAnchors
        {
            public Item Item { get; set; }
            public Dictionary<string, Item> Anchors { get; set; } = new Dictionary<string, Item>();
        }

        private Dictionary<string, ItemWithAnchors> _AllItems = new Dictionary<string, ItemWithAnchors>();

        public async Task PreloadAllItemsAsync()
        {
            foreach (var resourceName in Tools.Helpers.GetResourceNames())
            {
                var regex = new Regex(@"AideDeJeu\.Data\.(?<name>.*?)\.md");
                var match = regex.Match(resourceName);
                var source = match.Groups["name"].Value;
                if (!string.IsNullOrEmpty(source))
                {
                    if (!_AllItems.ContainsKey(source))
                    {
                        var md = await Tools.Helpers.GetResourceStringAsync(resourceName);
                        if (md != null)
                        {
                            var item = Tools.MarkdownExtensions.ToItem(md);
                            if (item != null)
                            {
                                var anchors = new Dictionary<string, Item>();
                                MakeAnchors(source, anchors, item);
                                _AllItems[source] = new ItemWithAnchors() { Item = item, Anchors = anchors };
                            }
                        }
                    }
                }
            }
        }

        public class SearchedItem
        {
            public string Preview { get; set; }
            public Item Item { get; set; }
        }

        public async Task<IEnumerable<SearchedItem>> DeepSearchAllItemsAsync(string searchText)
        {
            List<SearchedItem> primaryItems = new List<SearchedItem>();
            List<SearchedItem> secondaryItems = new List<SearchedItem>();
            var cleanSearchText = Tools.Helpers.RemoveDiacritics(searchText).ToLower();
            foreach (var allItem in _AllItems)
            {
                foreach(var item in allItem.Value.Anchors)
                {
                    var name = item.Value.Name;
                    var cleanName = Tools.Helpers.RemoveDiacritics(name).ToLower();
                    if (cleanName.Contains(cleanSearchText))
                    {
                        primaryItems.Add(new SearchedItem() { Item = item.Value, Preview = name });
                    }
                    else
                    {
                        var markdown = item.Value.Markdown;
                        var cleanMarkdown = Tools.Helpers.RemoveDiacritics(markdown).ToLower();
                        if (cleanMarkdown.Contains(cleanSearchText))
                        {
                            int position = cleanMarkdown.IndexOf(cleanSearchText);
                            int startPosition = Math.Max(0, position - 30);
                            int endPosition = Math.Min(markdown.Length, position + searchText.Length + 30);
                            var preview = markdown.Substring(startPosition, endPosition - startPosition - 1);
                            secondaryItems.Add(new SearchedItem() { Item = item.Value, Preview = preview });
                        }
                    }
                }
            }
            primaryItems.AddRange(secondaryItems);
            return primaryItems;
        }

        public async Task<Item> GetItemFromDataAsync(string source, string anchor)
        {
            //await Task.Delay(3000);
            if (!_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                if (md != null)
                {
                    var item = Tools.MarkdownExtensions.ToItem(md);
                    if (item != null)
                    {
                        var anchors = new Dictionary<string, Item>();
                        MakeAnchors(source, anchors, item);
                        _AllItems[source] = new ItemWithAnchors() { Item = item, Anchors = anchors };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            var itemWithAnchors = _AllItems[source];
            if (!string.IsNullOrEmpty(anchor))
            {
                if (itemWithAnchors.Anchors.ContainsKey(anchor))
                {
                    return itemWithAnchors.Anchors[anchor];
                }
            }
            return itemWithAnchors.Item;
        }
        */
        public Command LoadItemsCommand { get; private set; }

        private Navigator _Navigator = null;
        public Navigator Navigator
        {
            get
            {
                return _Navigator;
            }
            set
            {
                SetProperty(ref _Navigator, value);
            }
        }

        private SpeechViewModel _Speech = null;
        public SpeechViewModel Speech
        {
            get
            {
                return _Speech ?? (_Speech = new SpeechViewModel());
            }
        }


        public MainViewModel()
        {
        }

        private Library.ItemViewModel _CurrentItem = null;
        public Library.ItemViewModel CurrentItem
        {
            get
            {
                return _CurrentItem;
            }
            set
            {
                SetProperty(ref _CurrentItem, value);
            }
        }

        private bool _FilterIsPresented = false;
        public bool FilterIsPresented
        {
            get
            {
                return _FilterIsPresented;
            }
            set
            {
                SetProperty(ref _FilterIsPresented, value);
            }
        }

        private Command _ChangeFilterIsPresentedCommand = null;
        public Command ChangeFilterIsPresentedCommand
        {
            get
            {
                return _ChangeFilterIsPresentedCommand ?? (_ChangeFilterIsPresentedCommand = new Command(() => FilterIsPresented = !FilterIsPresented));
            }
        }
    }
}