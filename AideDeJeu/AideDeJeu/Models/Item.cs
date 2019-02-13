using AideDeJeu.Tools;
using AideDeJeu.ViewModels;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AideDeJeuLib
{
    [DataContract]
    public class Item //: IList<Item>
    {
        protected List<Item> _Items;

        public Item(List<Item> items)
        {
            _Items = items;
        }

        public Item(IEnumerable<Item> items)
        {
            _Items = items.ToList();
        }

        public Item()
        {
            _Items = new List<Item>();
        }

        public async Task<IEnumerable<Item>> GetChildrenAsync()
        {
            return _Items;
        }

        //public string Header { get; set; }

        //public int Count => _Items.Count();

        //public bool IsReadOnly => false;

        //public Item this[int index] { get => _Items[index]; set => _Items[index] = value; }

        //public IEnumerator<Item> GetEnumerator()
        //{
        //    return _Items?.GetEnumerator();
        //}

        ////IEnumerator IEnumerable.GetEnumerator()
        ////{
        ////    return _Items?.GetEnumerator();
        ////}

        public virtual FilterViewModel GetNewFilterViewModel()
        {
            return null;
        }

        public virtual async Task LoadFilteredItemsAsync()
        {
            _Items = (await GetChildrenAsync()).ToList();
        }

        //public int IndexOf(Item item)
        //{
        //    return _Items.IndexOf(item);
        //}

        //public void Insert(int index, Item item)
        //{
        //    _Items.Insert(index, item);
        //}

        //public void RemoveAt(int index)
        //{
        //    _Items.RemoveAt(index);
        //}

        public void AddChild(Item item)
        {
            _Items.Add(item);
        }

        //public void Clear()
        //{
        //    _Items.Clear();
        //}

        //public bool Contains(Item item)
        //{
        //    return _Items.Contains(item);
        //}

        //public void CopyTo(Item[] array, int arrayIndex)
        //{
        //    _Items.CopyTo(array, arrayIndex);
        //}

        //public bool Remove(Item item)
        //{
        //    throw new NotImplementedException();
        //}

        [DataMember]
        [PrimaryKey]
        public virtual string Id { get; set; }
        [DataMember]
        [Indexed]
        public string RootId { get; set; }

        [DataMember]
        [Indexed]
        public string ParentLink { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ParentName { get; set; }

        [YamlIgnore]
        [IgnoreDataMember]
        [Ignore]
        public string ParentNameLink
        {
            get
            {
                if (ParentName != null && ParentLink != null)
                {
                    return $"<!--ParentNameLink-->[{ParentName}]({ParentLink})<!--/ParentNameLink-->";
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    var regex = new Regex("\\[(?<name>.*?)\\]\\((?<link>.*?)\\)");
                    var match = regex.Match(value);
                    ParentName = match.Groups["name"].Value;
                    ParentLink = match.Groups["link"].Value;
                }
            }
        }

        [DataMember]
        public int NameLevel { get; set; }
        [DataMember]
        public string AltName { get; set; }

        [YamlIgnore]
        [IgnoreDataMember]
        public string AltNameText
        {
            get
            {
                var regex = new Regex("\\[(?<text>.*?)\\]");
                var match = regex.Match(AltName ?? string.Empty);
                if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                {
                    return match.Groups["text"].Value;
                }
                else
                {
                    regex = new Regex("(?<text>.*?)( \\(SRD p\\d*\\))");
                    match = regex.Match(AltName ?? string.Empty);
                    if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                    {
                        return match.Groups["text"].Value;
                    }
                    return AltName ?? string.Empty;
                }
            }
        }
        [DataMember]
        public string Source { get; set; }

        [YamlIgnore]
        [DataMember]
        public virtual string Markdown { get; set; }
        [DataMember]
        public string FullText { get; set; }

        [IgnoreDataMember]
        [YamlIgnore]
        public static Dictionary<string, Type> ClassMapping = new Dictionary<string, Type>()
        {
            { "Generic", typeof(Generic) },
            { "Monster", typeof(Monster) },
            { "MonsterHD", typeof(MonsterHD) },
            { "MonsterVO", typeof(MonsterVO) },
            { "Monsters", typeof(Monsters) },
            { "MonstersHD", typeof(MonstersHD) },
            { "MonstersVO", typeof(MonstersVO) },
            { "Spell", typeof(Spell) },
            { "SpellHD", typeof(SpellHD) },
            { "SpellVO", typeof(SpellVO) },
            { "Spells", typeof(Spells) },
            { "SpellsHD", typeof(SpellsHD) },
            { "SpellsVO", typeof(SpellsVO) },
            { "Equipment", typeof(Equipment) },
            { "Equipments", typeof(Equipments) },
            { "LinkItem", typeof(LinkItem) },
            { "MagicItem", typeof(MagicItem) },
            { "MagicItems", typeof(MagicItems) },
            { "PageItem", typeof(PageItem) },
            { "ListItems", typeof(List<Items>) },
            { "SubRaceItem", typeof(SubRaceItem) },
            { "SubClassItem", typeof(SubClassItem) },
            { "SubBackgroundItem", typeof(SubBackgroundItem) },
            { "RaceItem", typeof(RaceItem) },
            { "ClassItem", typeof(ClassItem) },
            { "FeatItem", typeof(FeatItem) },
            { "PersonalityTraitItem", typeof(PersonalityTraitItem) },
            { "PersonalityIdealItem", typeof(PersonalityIdealItem) },
            { "PersonalityLinkItem", typeof(PersonalityLinkItem) },
            { "PersonalityDefectItem", typeof(PersonalityDefectItem) },
            { "BackgroundSpecialtyItem", typeof(BackgroundSpecialtyItem) },
            { "BackgroundItem", typeof(BackgroundItem) },
            { "SkillItem", typeof(SkillItem) },
            { "Items", typeof(Items) },
            { "Item", typeof(Item) },
        };

        [IgnoreDataMember]
        [YamlIgnore]
        public string Yaml
        {
            get
            {
                var builder = new SerializerBuilder();
                foreach(var mapping in ClassMapping)
                {
                    builder = builder.WithTagMapping($"!{mapping.Key}", mapping.Value);
                }
                var serializer = builder
                    .EnsureRoundtrip()
                    .WithNamingConvention(new PascalCaseNamingConvention())
                    .Build();
                return serializer.Serialize(this);
            }
        }

        [IgnoreDataMember]
        [YamlIgnore]
        public virtual string YamlMarkdown
        {
            get
            {
                return $"---\n{Yaml}---\n{CleanMarkdown}";
            }
        }

        [IgnoreDataMember]
        [YamlIgnore]
        public string SubMarkdown
        {
            get
            {
                if (_Items != null)
                {
                    var md = string.Empty;
                    foreach (var item in _Items)
                    {
                        md += item.CleanMarkdown;
                    }
                    return md;
                }
                return null;
            }
            set
            {

            }
        }

        public static Item ParseYamlMarkdown(string yamlmd)
        {
            var builder = new DeserializerBuilder();
            foreach (var mapping in ClassMapping)
            {
                builder = builder.WithTagMapping($"!{mapping.Key}", mapping.Value);
            }
            var yamlDeserializer = builder
                .WithNamingConvention(new PascalCaseNamingConvention())
                .Build();

            var parser = new Parser(new System.IO.StringReader(yamlmd));
            parser.Expect<StreamStart>();
            parser.Expect<DocumentStart>();
            var post = yamlDeserializer.Deserialize(parser);
            parser.Expect<DocumentEnd>();
            //parser.MoveNext();
            var item = post as Item;
            item.Markdown = yamlmd.Substring(parser.Current.End.Index + 1);
            return post as Item;
        }

        public string CleanMarkdown
        {
            get
            {
                var md = Markdown;
                var rx = new Regex("<!--.*?-->");
                md = rx.Replace(md, "");
                return md;
            }
        }

        public string NewId
        {
            get
            {
                var id = string.IsNullOrEmpty(RootId) ? Id : RootId;
                id = Helpers.RemoveDiacritics(id);
                if (id.Contains("_hd.md"))
                {
                    id = "hd_" + id.Replace("_hd.md#", "_").Replace("_hd.md", "");                        
                }
                else if(id.Contains("_vo.md"))
                {
                    id = "srd_" + id.Replace("_vo.md#", "_").Replace("_vo.md", "");
                }
                else
                {
                    id = id.Replace(".md", "");
                }
                id = id.Replace("-", "_").Replace("?", "").Replace("’", "_").Replace("'", "_").Replace("__", "_").ToLower() + ".md";
                return id;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({NewId})";
        }

        [NotMapped]
        [IgnoreDataMember]
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }
}
