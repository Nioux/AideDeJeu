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
            set => SetProperty(ref _isLoading, value);
        }

        void AddAnchor(Dictionary<string, Item> anchors, Item item)
        {
            if (item != null && item.Name != null)
            {
                var basename = Helpers.IdFromName(item.Name);
                var name = basename;
                int index = 0;
                while (true)
                {
                    if (!anchors.ContainsKey(name))
                    {
                        anchors.Add(name, item);
                        return;
                    }
                    index++;
                    name = $"{basename}{index}";
                }
            }
        }
        void MakeAnchors(Dictionary<string, Item> anchors, Item baseItem)
        {
            AddAnchor(anchors, baseItem);
            if(baseItem is Items)
            {
                foreach(var item in (baseItem as Items))
                {
                    MakeAnchors(anchors, item);
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
                                MakeAnchors(anchors, item);
                                _AllItems[source] = new ItemWithAnchors() { Item = item, Anchors = anchors };
                            }
                        }
                    }
                }
            }
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
                        MakeAnchors(anchors, item);
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

        public MainViewModel()
        {
        }
    }
}