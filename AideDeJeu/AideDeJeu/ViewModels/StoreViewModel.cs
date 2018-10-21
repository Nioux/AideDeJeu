using AideDeJeu.Tools;
using AideDeJeuLib;
using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        public Item ToItem(string source, string md, Dictionary<string, Item> allItems)
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
                            var item = ParseItem(source, ref enumerator, allItems);
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

        public Item ParseItem(string source, ref ContainerBlock.Enumerator enumerator, Dictionary<string, Item> allItems)
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
                            currentItem.Id = GetNewAnchorId(source, currentItem.Name, allItems);
                            if (currentItem.Id != null && allItems != null)
                            {
                                allItems[currentItem.Id] = currentItem;
                            }
                            return currentItem;
                        }
                        else if (IsNewItem(block))
                        {
                            var subItem = ParseItem(source, ref enumerator, allItems);

                            var propertyName = subItem.GetType().Name;

                            if (currentItem.GetType().GetProperty(propertyName) != null)
                            {
                                PropertyInfo prop = currentItem.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(currentItem, subItem, null);
                                }
                            }
                            else //if (currentItem is Items)
                            {
                                if (allItems != null && currentItem.GetNewFilterViewModel() == null)
                                {
                                    var name = subItem.Name;
                                    var level = Math.Max(1, Math.Min(6, subItem.NameLevel));
                                    var link = (subItem is LinkItem ? (subItem as LinkItem).Link : subItem.Id);
                                    currentItem.Markdown += $"\n\n{new String('#', level)} [{name}]({link})";
                                    if(!string.IsNullOrEmpty(subItem.AltNameText))
                                    {
                                        var altname = subItem.AltNameText;
                                        var altlevel = Math.Max(1, Math.Min(6, subItem.NameLevel + 3));
                                        currentItem.Markdown += $"\n\n{new String('#', altlevel)} _[{altname}]({link})_";
                                    }
                                    currentItem.Markdown += "\n\n";
                                }
                                else
                                {
                                    var items = currentItem; // as Items;
                                    items.AddChild(subItem);
                                }
                            }
                            enumerator.MoveNext();
                        }
                        else
                        {
                            currentItem.Markdown += enumerator.Current.ToMarkdownString();
                            enumerator.MoveNext();
                        }
                    }
                    else
                    {
                        ParseItemProperties(source, currentItem, block);
                        currentItem.Markdown += enumerator.Current.ToMarkdownString();
                        enumerator.MoveNext();
                    }
                }
                currentItem.Id = GetNewAnchorId(source, currentItem.Name, allItems);
                if (allItems != null)
                {
                    allItems[currentItem.Id] = currentItem;
                }
            }

            return currentItem;
        }

        public bool ParseItemProperties(string source, Item item, Block block)
        {
            switch (block)
            {
                case Markdig.Extensions.Tables.Table table:
                    return ParseItemProperties(source, item, table);
                case ContainerBlock blocks:
                    return ParseItemProperties(source, item, blocks);
                case LeafBlock leaf:
                    bool isname = ParseItemProperties(source, item, leaf.Inline);
                    if(isname)
                    {
                        if(leaf is HeadingBlock)
                        {
                            var headingBlock = leaf as HeadingBlock;
                            item.NameLevel = headingBlock.Level;
                        }
                    }
                    return isname;
            }
            return false;
        }

        public bool ParseItemProperties(string source, Item item, ContainerBlock blocks)
        {
            bool isname = false;
            foreach (var block in blocks)
            {
                isname |= ParseItemProperties(source, item, block);
            }
            return isname;
        }

        public bool ParseItemProperties(string source, Item item, ContainerInline inlines)
        {
            bool isname = false;
            if (inlines == null)
            {
                return isname;
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
                        if(propertyName == "Name")
                        {
                            isname = true;
                        }
                        prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    }
                }
                else
                {
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, prop.GetValue(item) + inline.ToMarkdownString(), null);
                    }
                }
            }
            return isname;
        }



        public bool IsNewItem(Block block)
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
                        if (type != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsClosingItem(Block block)
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

        public Item GetNewItem(Block block)
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
                        if (type != null)
                        {
                            var instance = Activator.CreateInstance(type) as Item;
                            return instance;
                        }
                    }
                }
            }
            return null;
        }





        public string GetNewAnchorId(string source, string name, Dictionary<string, Item> allItems)
        {
            if (name != null)
            {
                var baseid = Helpers.IdFromName(name);
                var id = $"{source}.md#{baseid}";
                int index = 0;
                while (true)
                {
                    if (allItems == null || !allItems.ContainsKey(name))
                    {
                        return id;
                    }
                    index++;
                    name = $"{source}.md#{baseid}{index}";
                }
            }
            return null;
        }
        /*
        void AddAnchor(string source, Dictionary<string, Item> anchors, Item item)
        {
            if (item != null && item.Name != null)
            {
                var basename = Helpers.IdFromName(item.Name);
                var name = $"{source}.md#{basename}";
                //var name = $"{basename}";
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
                    name = $"{source}.md#{basename}{index}";
                    //name = $"{basename}{index}";
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
        */
        public Dictionary<string, Item> _AllItems = new Dictionary<string, Item>();

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
                            var item = ToItem(source, md, _AllItems);
                            if (item != null)
                            {
                                var anchors = new Dictionary<string, Item>();
                                //MakeAnchors(source, anchors, item);
                                item.Id = $"{source}.md";
                                _AllItems[source] = item;
                            }
                        }
                    }
                }
            }
        }


        public class AideDeJeuContext : DbContext
        {
            public string DbPath { get; set; }
            public DbSet<Item> Items { get; set; }
            public DbSet<Equipment> Equipments { get; set; }
            public DbSet<MagicItem> MagicItems { get; set; }
            public DbSet<Spell> Spells { get; set; }
            public DbSet<Monster> Monsters { get; set; }
            public DbSet<SpellHD> SpellsHD { get; set; }
            public DbSet<MonsterHD> MonstersHD { get; set; }
            public DbSet<SpellVO> SpellsVO { get; set; }
            public DbSet<MonsterVO> MonstersVO { get; set; }

            public AideDeJeuContext(string dbPath)
            {
                this.DbPath = dbPath;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite($"Data Source='{DbPath}'");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<MonstersHD>();
                modelBuilder.Entity<MonstersVO>();
                modelBuilder.Entity<SpellsHD>();
                modelBuilder.Entity<SpellsVO>();
                modelBuilder.Entity<Equipments>();
                modelBuilder.Entity<MagicItems>();
            }
        }

        public static async Task<AideDeJeuContext> GetLibraryContextAsync()
        {
            var dbPath = await DependencyService.Get<INativeAPI>().GetDatabasePathAsync("library.db");
            return new AideDeJeuContext(dbPath);
        }

        public async Task<Item> GetItemFromDataAsync(string source, string anchor)
        {
            var id = $"{source}.md".TrimStart('/');
            if (!string.IsNullOrEmpty(anchor))
            {
                id += $"#{anchor}";
            }
            using (var context = await GetLibraryContextAsync())
            {
                return await context.Items.Where(item => item.Id == id).FirstOrDefaultAsync();
            }
        }

        public async Task<Item> GetItemFromDataAsyncOld(string source, string anchor)
        {
            var id = $"{source}.md".TrimStart('/');
            //await Task.Delay(3000);
            if (!_AllItems.ContainsKey(id) && !_AllItems.ContainsKey(source))
            {
                //var md = await Tools.Helpers.GetStringFromUrl($"https://raw.githubusercontent.com/Nioux/AideDeJeu/master/Data/{source}.md");
                var md = await Tools.Helpers.GetResourceStringAsync($"AideDeJeu.Data.{source}.md");
                if (md != null)
                {
                    var item = ToItem(source, md, _AllItems);
                    if (item != null)
                    {
                        var anchors = new Dictionary<string, Item>();
                        //MakeAnchors(source, anchors, item);
                        _AllItems[source] = item;
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
            if (_AllItems.ContainsKey(id))
            {
                return _AllItems[id];
            }
            else if (_AllItems.ContainsKey(source))
            {
                return _AllItems[source];
            }
            return null;
        }


    }
}
