using AideDeJeu.Tools;
using AideDeJeuLib;
using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        public static Item ToItem(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = MarkdownParser.Parse(md, pipeline);

            var enumerator = document.GetEnumerator();
            try
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (block is HtmlBlock)
                    {
                        if (IsNewItem(block))
                        {
                            var item = ParseItem(ref enumerator);
                            return item;
                        }
                    }
                    enumerator.MoveNext();
                }

            }
            finally
            {
                enumerator.Dispose();
            }
            return null;
        }

        public static Item ParseItem(ref ContainerBlock.Enumerator enumerator)
        {
            var currentItem = GetNewItem(enumerator.Current);

            if (currentItem != null)
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (block is HtmlBlock)
                    {
                        if (IsClosingItem(block))
                        {
                            return currentItem;
                        }
                        else if (IsNewItem(block))
                        {
                            var subItem = ParseItem(ref enumerator);

                            var propertyName = subItem.GetType().Name;

                            if (currentItem.GetType().GetProperty(propertyName) != null)
                            {
                                PropertyInfo prop = currentItem.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(currentItem, subItem, null);
                                }
                            }
                            else if (currentItem is Items)
                            {
                                var items = currentItem as Items;
                                items.Add(subItem);
                            }
                        }
                    }

                    else // if (block is ContainerBlock)
                    {
                        ParseItemProperties(currentItem, block);
                    }

                    currentItem.Markdown += enumerator.Current.ToMarkdownString();

                    enumerator.MoveNext();
                }
            }

            return currentItem;
        }

        public static void ParseItemProperties(Item item, Block block)
        {
            switch (block)
            {
                case Markdig.Extensions.Tables.Table table:
                    ParseItemProperties(item, table);
                    break;
                case ContainerBlock blocks:
                    ParseItemProperties(item, blocks);
                    break;
                case LeafBlock leaf:
                    ParseItemProperties(item, leaf.Inline);
                    break;
            }
        }

        public static void ParseItemProperties(Item item, ContainerBlock blocks)
        {
            foreach (var block in blocks)
            {
                ParseItemProperties(item, block);
            }
        }

        public static void ParseItemProperties(Item item, ContainerInline inlines)
        {
            if (inlines == null)
            {
                return;
            }
            PropertyInfo prop = null;
            foreach (var inline in inlines)
            {
                if (inline is HtmlInline)
                {
                    var tag = (inline as HtmlInline).Tag;
                    if (tag == "<!--br-->" || tag == "<br>")
                    {

                    }
                    else if (tag.StartsWith("<!--/"))
                    {
                        prop = null;
                    }
                    else if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        var propertyName = tag.Substring(4, tag.Length - 7);
                        prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    }
                }
                else
                {
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, inline.ToMarkdownString(), null);
                    }
                }
            }
        }



        public static bool IsNewItem(Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsClosingItem(Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--/"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Item GetNewItem(Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        var name = $"AideDeJeuLib.{tag.Substring(4, tag.Length - 7)}, AideDeJeu";
                        var type = Type.GetType(name);
                        var instance = Activator.CreateInstance(type) as Item;
                        return instance;
                    }
                }
            }
            return null;
        }






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
            if (baseItem is Items)
            {
                foreach (var item in (baseItem as Items))
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

        public Dictionary<string, ItemWithAnchors> _AllItems = new Dictionary<string, ItemWithAnchors>();

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
                            var item = ToItem(md);
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

        public async Task<Item> GetItemFromDataAsync(string source, string anchor)
        {
            //await Task.Delay(3000);
            if (!_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                if (md != null)
                {
                    var item = ToItem(md);
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


    }
}
