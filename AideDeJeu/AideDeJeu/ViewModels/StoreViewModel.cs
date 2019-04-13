using AideDeJeu.Tools;
using AideDeJeuLib;
using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class StoreViewModel : BaseViewModel
    {
        public Item ToItem(string source, string md, Dictionary<string, Item> allItems)
        {
            var pipeline = new MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
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
            var currentProps = new Dictionary<string, PropertyInfo>();
            currentItem.Markdown = string.Empty;
            currentProps["Markdown"] = currentItem.GetType().GetProperty("Markdown", BindingFlags.Public | BindingFlags.Instance);

            if (currentItem != null)
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (block is HtmlBlock)
                    {
                        var htmlBlock = block as HtmlBlock;
                        if (htmlBlock.Type == HtmlBlockType.Comment)
                        {
                            var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                            var parsedComment = new ParsedComment(tag, withSigns: true);

                            if (parsedComment.Type == ParsedCommentType.Item)
                            {
                                if (parsedComment.IsClosing)
                                {
                                    currentItem.Id = GetNewAnchorId(source, currentItem.Name, allItems);
                                    if (currentItem.Id != null && allItems != null)
                                    {
                                        allItems[currentItem.Id] = currentItem;
                                    }
                                    return currentItem;
                                }
                                else
                                {
                                    var subItem = ParseItem(source, ref enumerator, allItems);
                                    subItem.ParentLink = GetNewAnchorId(source, currentItem.Name, allItems);
                                    subItem.ParentName = currentItem.Name;
                                    subItem.Markdown = $"> {subItem.ParentNameLink}\n\n---\n\n{subItem.Markdown}";

                                    var propertyName = subItem.GetType().Name;

                                    if (currentItem.GetType().GetProperty(propertyName) != null)
                                    {
                                        PropertyInfo prop = currentItem.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                                        if (null != prop && prop.CanWrite)
                                        {
                                            prop.SetValue(currentItem, subItem, null);
                                        }
                                    }
                                    else
                                    {
                                        if (allItems != null && currentItem.GetNewFilterViewModel() == null)
                                        {
                                            var name = subItem.Name;
                                            var level = Math.Max(1, Math.Min(6, subItem.NameLevel));
                                            var link = (subItem is LinkItem ? (subItem as LinkItem).Link : subItem.Id);
                                            currentItem.Markdown += $"\n\n{new String('#', level)} [{name}]({link})";
                                            if (!string.IsNullOrEmpty(subItem.AltNameText))
                                            {
                                                var altname = subItem.AltNameText;
                                                var altlevel = Math.Max(1, Math.Min(6, subItem.NameLevel + 3));
                                                currentItem.Markdown += $"\n\n{new String('#', altlevel)} _[{altname}]({link})_";
                                            }
                                            currentItem.Markdown += "\n\n";
                                        }
                                        else
                                        {
                                            currentItem.AddChild(subItem);
                                        }
                                    }
                                    enumerator.MoveNext();
                                }
                            }
                            else if (parsedComment.Type == ParsedCommentType.Property)
                            {
                                if (!parsedComment.IsClosing)
                                {
                                    PropertyInfo prop = currentItem.GetType().GetProperty(parsedComment.Name, BindingFlags.Public | BindingFlags.Instance);
                                    if (null != prop && prop.CanWrite)
                                    {
                                        prop.SetValue(currentItem, string.Empty, null);
                                        currentProps[parsedComment.Name] = prop;
                                    }
                                    enumerator.MoveNext();
                                }
                                else
                                {
                                    currentProps.Remove(parsedComment.Name);
                                    enumerator.MoveNext();
                                }
                            }
                            else // comment html différent de item et property
                            {
                                AddBlockContent(currentItem, currentProps, enumerator.Current);
                                enumerator.MoveNext();
                            }
                        }
                        else // autre chose qu'un comment html
                        {
                            AddBlockContent(currentItem, currentProps, enumerator.Current);
                            enumerator.MoveNext();
                        }
                    }
                    else // autre chose qu'un block html
                    {
                        ParseItemProperties(source, currentItem, block);
                        AddBlockContent(currentItem, currentProps, enumerator.Current);
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

        void AddBlockContent(Item currentItem, Dictionary<string, PropertyInfo> props, Block block)
        {
            var md = block.ToMarkdownString();
            foreach (var propkv in props)
            {
                var prop = propkv.Value;
                prop.SetValue(currentItem, prop.GetValue(currentItem) + md, null);
            }
        }


        public void ParseItemProperties(string source, Item item, Block block)
        {
            switch (block)
            {
                case Markdig.Extensions.Tables.Table table:
                    ParseItemProperties(source, item, table);
                    break;
                case ContainerBlock blocks:
                    ParseItemProperties(source, item, blocks);
                    break;
                case HeadingBlock heading:
                    ParseItemProperties(source, item, heading.Inline, heading);
                    break;
                case LeafBlock leaf:
                    ParseItemProperties(source, item, leaf.Inline);
                    break;
            }
        }

        public void ParseItemProperties(string source, Item item, ContainerBlock blocks)
        {
            foreach (var block in blocks)
            {
                ParseItemProperties(source, item, block);
            }
        }

        public void ParseItemProperties(string source, Item item, ContainerInline inlines, HeadingBlock headingBlock = null)
        {
            if (inlines == null)
            {
                return;
            }
            PropertyInfo prop = null;
            string propertyName = null;
            foreach (var inline in inlines)
            {
                if (inline is HtmlInline)
                {
                    var tag = (inline as HtmlInline).Tag;
                    if(tag.StartsWith("<!--"))
                    {
                        var parsedComment = new ParsedComment(tag, true);
                        if(parsedComment.Type == ParsedCommentType.Property)
                        {
                            if(!parsedComment.IsClosing)
                            {
                                propertyName = parsedComment.Name;
                                if (propertyName == "Name")
                                {
                                    if (headingBlock != null)
                                    {
                                        item.NameLevel = headingBlock.Level;
                                    }
                                }
                                prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                            }
                            else
                            {
                                prop = null;
                                propertyName = null;
                            }
                        }
                    }
                    //if (tag == "<!--br-->" || tag == "<br>")
                    //{

                    //}
                    //else if (tag.StartsWith("<!--/"))
                    //{
                    //    prop = null;
                    //    propertyName = null;
                    //}
                    //else if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    //{
                    //    propertyName = tag.Substring(4, tag.Length - 7);
                    //    if(propertyName == "Name")
                    //    {
                    //        if (headingBlock != null)
                    //        {
                    //            item.NameLevel = headingBlock.Level;
                    //        }
                    //    }
                    //    prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    //}
                }
                else
                {
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, prop.GetValue(item) + inline.ToMarkdownString(), null);
                    }
                    else if(propertyName != null)
                    {
                        Debug.WriteLine(propertyName);
                        if (item.Attributes.ContainsKey(propertyName))
                        {
                            item.Attributes[propertyName] += inline.ToMarkdownString();
                        }
                        else
                        {
                            item.Attributes[propertyName] = inline.ToMarkdownString();
                        }
                    }
                }
            }
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
                        //var name = $"AideDeJeuLib.{tag.Substring(4, tag.Length - 7)}, AideDeJeu";
                        var name = tag.Substring(4, tag.Length - 7);
                        if (CheckNewItem(name))
                        {
                            return true;
                        }
                        else
                        {
                            Debug.WriteLine(tag);
                        }
                        //var type = Type.GetType(name);
                        //if (type != null)
                        //{
                        //    return true;
                        //}
                    }
                }
            }
            return false;
        }

        public enum ParsedCommentType
        {
            Item,
            Property,
            Key,
            Value,
            None,
        }
        public class ParsedComment
        {
            public string Name { get; private set; }
            public string ShortName { get; private set; }
            public ParsedCommentType Type { get; private set; }
            public bool IsClosing { get; private set; }
            public Dictionary<string, string> Attributes { get; private set; }

            public ParsedComment(string tag, bool withSigns = false)
            {
                var comment = withSigns ? tag.Substring(4, tag.Length - 7) : tag;
                var regex = new Regex("(?<closing>/?)(?<item>\\w+)(\\s+((?<name>\\w+)=\"(?<value>.*?)\"))*");
                var match = regex.Match(comment);
                Name = match.Groups["item"].Value;
                Dictionary<string, ParsedCommentType> typeMatching = new Dictionary<string, ParsedCommentType>()
                {
                    { "Item", ParsedCommentType.Item },
                    { "Items", ParsedCommentType.Item },
                    //{ "Key", ParsedCommentType.Key },
                    //{ "Value", ParsedCommentType.Value },
                };
                if (Name == "br")
                {
                    Type = ParsedCommentType.None;
                    ShortName = Name;
                }
                else
                {
                    Type = ParsedCommentType.Property;
                    ShortName = Name;
                    foreach (var typeMatch in typeMatching)
                    {
                        if (Name.EndsWith(typeMatch.Key))
                        {
                            Type = typeMatch.Value;
                            ShortName = Name.Substring(0, Name.Length - typeMatch.Key.Length);
                        }
                    }
                }
                IsClosing = !string.IsNullOrEmpty(match.Groups["closing"].Value);
                var names = match.Groups["name"].Captures;
                var values = match.Groups["value"].Captures;
                Attributes = new Dictionary<string, string>();
                for (int i = 0; i < names.Count; i++)
                {
                    Attributes[names[i].Value] = values[i].Value;
                }
            }
        }

        bool CheckNewItem(string itemString)
        {
            var parsedComment = new ParsedComment(itemString);
            if (parsedComment.Type == ParsedCommentType.Item)
            {
                var name = $"AideDeJeuLib.{parsedComment.Name}, AideDeJeu";
                var type = Type.GetType(name);
                if (type != null)
                {
                    return true;
                }
            }
            else if(parsedComment.Type == ParsedCommentType.Property)
            {
                Debug.WriteLine(parsedComment.Name);
            }
            return false;
        }

        Item CreateNewItem(string itemString)
        {
            var parsedComment = new ParsedComment(itemString);
            if (parsedComment.Name.EndsWith("Item") || parsedComment.Name.EndsWith("Items"))
            {
                var name = $"AideDeJeuLib.{parsedComment.Name}, AideDeJeu";
                var type = Type.GetType(name);
                if (type != null)
                {
                    var item = Activator.CreateInstance(type) as Item;
                    foreach (var attribute in parsedComment.Attributes)
                    {
                        var prop = item.GetType().GetProperty(attribute.Key, BindingFlags.Public | BindingFlags.Instance);
                        if (prop?.CanWrite == true)
                        {
                            prop.SetValue(item, prop.GetValue(item) + attribute.Value, null);
                        }
                        else
                        {
                            item.Attributes[attribute.Key] = attribute.Value;
                        }
                    }
                    return item;
                }
            }
            return null;
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
                        if (tag.EndsWith("Item-->") || tag.EndsWith("Items-->"))
                        {
                            return true;
                        }
                        else
                        {
                            Debug.WriteLine(tag);
                        }
                    }
                }
            }
            return false;
        }

        //public bool IsClosingProperty(Block block)
        //{
        //    var htmlBlock = block as HtmlBlock;
        //    if (htmlBlock.Type == HtmlBlockType.Comment)
        //    {
        //        var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
        //        var comment = tag.Substring(4, tag.Length - 7);
        //        var parsedComment = new ParsedComment(comment);
        //        return parsedComment.IsClosing && parsedComment.Type == ParsedCommentType.Property;
        //    }
        //    return false;
        //}

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
                        //var name = $"AideDeJeuLib.{tag.Substring(4, tag.Length - 7)}, AideDeJeu";
                        var name = tag.Substring(4, tag.Length - 7);
                        var instance = CreateNewItem(name);
                        if(instance != null)
                        {
                            return instance;
                        }
                        //var type = Type.GetType(name);
                        //if (type != null)
                        //{
                        //    var instance = Activator.CreateInstance(type) as Item;
                        //    return instance;
                        //}
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
                                item.RootId = $"{source}.md";
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
            public DbSet<EquipmentItem> Equipments { get; set; }
            public DbSet<MagicItem> MagicItems { get; set; }
            public DbSet<SpellItem> Spells { get; set; }
            public DbSet<MonsterItem> Monsters { get; set; }
            //public DbSet<Spell> Spells { get; set; }
            //public DbSet<MonsterHD> MonstersHD { get; set; }
            //public DbSet<SpellVO> SpellsVO { get; set; }
            //public DbSet<MonsterVO> MonstersVO { get; set; }
            public DbSet<RaceItem> Races { get; set; }
            public DbSet<ClassItem> Classes { get; set; }
            public DbSet<SubRaceItem> SubRaces { get; set; }
            public DbSet<SubClassItem> SubClasses { get; set; }
            public DbSet<BackgroundItem> Backgrounds { get; set; }
            public DbSet<SubBackgroundItem> SubBackgrounds { get; set; }
            public DbSet<PersonalityTraitItem> PersonalityTraits { get; set; }
            public DbSet<PersonalityIdealItem> PersonalityIdeals { get; set; }
            public DbSet<PersonalityLinkItem> PersonalityLinks { get; set; }
            public DbSet<PersonalityDefectItem> PersonalityDefects { get; set; }
            public DbSet<SkillItem> Skills { get; set; }
            public DbSet<BackgroundSpecialtyItem> BackgroundSpecialties { get; set; }
            public DbSet<AlignmentItem> Alignments { get; set; }

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
                modelBuilder.Entity<MonsterItems>();
                //modelBuilder.Entity<MonstersVO>();
                //modelBuilder.Entity<SpellsHD>().HasBaseType<Spells>();
                //modelBuilder.Entity<SpellsVO>().HasBaseType<Spells>();
                modelBuilder.Entity<SpellItems>();
                modelBuilder.Entity<EquipmentItems>();
                modelBuilder.Entity<MagicItems>();



                modelBuilder.Entity<AlignmentItem>();
                modelBuilder.Entity<GenericItem>();
                modelBuilder.Entity<MonsterItem>();
                modelBuilder.Entity<MonsterItems>();
                modelBuilder.Entity<SpellItem>();
                modelBuilder.Entity<SpellItems>();
                modelBuilder.Entity<EquipmentItem>();
                modelBuilder.Entity<EquipmentItems>();
                modelBuilder.Entity<LinkItem>();
                modelBuilder.Entity<MagicItem>();
                modelBuilder.Entity<MagicItems>();
                modelBuilder.Entity<PageItem>();
                modelBuilder.Entity<SubRaceItem>();
                modelBuilder.Entity<SubClassItem>();
                modelBuilder.Entity<SubBackgroundItem>();
                modelBuilder.Entity<RaceItem>();
                modelBuilder.Entity<ClassItem>();
                modelBuilder.Entity<FeatItem>();
                modelBuilder.Entity<PersonalityTraitItem>();
                modelBuilder.Entity<PersonalityIdealItem>();
                modelBuilder.Entity<PersonalityLinkItem>();
                modelBuilder.Entity<PersonalityDefectItem>();
                modelBuilder.Entity<BackgroundSpecialtyItem>();
                modelBuilder.Entity<BackgroundItem>();
                modelBuilder.Entity<SkillItem>();
                modelBuilder.Entity<ClassEquipmentItem>();
                modelBuilder.Entity<ClassEvolutionItem>();
                modelBuilder.Entity<ClassFeatureItem>();
                modelBuilder.Entity<ClassHitPointsItem>();
                modelBuilder.Entity<ClassProficienciesItem>();
                
                modelBuilder.Entity<Items>();
                modelBuilder.Entity<Item>();

            }
        }

        public static SemaphoreSlim SemaphoreLibrary = new SemaphoreSlim(1, 1);

        public static async Task<AideDeJeuContext> GetLibraryContextAsync()
        {
            var dbPath = await DependencyService.Get<INativeAPI>().GetDatabasePathAsync("library");
            return new AideDeJeuContext(dbPath);
        }

        public async Task<Item> GetItemFromDataAsync(string source, string anchor)
        {
            var id = $"{source}.md".TrimStart('/');
            if (!string.IsNullOrEmpty(anchor))
            {
                id += $"#{anchor}";
            }
            try
            {
                await SemaphoreLibrary.WaitAsync();
                using (var context = await GetLibraryContextAsync())
                {
                    return await context.Items.Where(item => item.Id == id || item.RootId == id).FirstOrDefaultAsync();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                await App.Current.MainPage.DisplayAlert("Lien invalide", ex.Message, "OK");
                return null;
            }
            finally
            {
                SemaphoreLibrary.Release();
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
