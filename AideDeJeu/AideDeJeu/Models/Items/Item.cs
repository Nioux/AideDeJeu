using AideDeJeu.Tools;
using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AideDeJeuLib
{
    interface IItem
    {

    }
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Item : IItem //: IList<Item>
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

        public virtual async Task<IEnumerable<Item>> GetChildrenAsync()
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

        public virtual void AddChild(Item item)
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
        public virtual string ItemType { get; set; }

        [DataMember(Name = "Item_Id", Order = 0)]
        [PrimaryKey]
        public virtual string Id { get; set; }

        [DataMember(Name = "Item_RootId", Order = 1)]
        [Indexed]
        public virtual string RootId { get; set; }

        [DataMember(Name = "Item_ParentLink", Order = 2)]
        [Indexed]
        public virtual string ParentLink { get; set; }

        [DataMember(Name = "Item_Name", Order = 3)]
        [Indexed]
        public virtual string Name { get; set; }

        [DataMember]
        [Indexed]
        public virtual string NormalizedName
        {
            get
            {
                return Helpers.RemoveDiacritics(Name);
            }
            private set { }
        }

        [DataMember(Name = "Item_ParentName", Order = 4)]
        [Indexed]
        public virtual string ParentName { get; set; }

        [YamlIgnore]
        [IgnoreDataMember]
        [Ignore]
        public virtual string ParentNameLink
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

        [DataMember(Name = "Item_NameLevel", Order = 5)]
        public virtual int NameLevel { get; set; }
        [DataMember(Name = "Item_AltName", Order = 6)]
        [Indexed]
        public virtual string AltName { get; set; }

        [DataMember]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [YamlIgnore]
        public virtual string AltNameText
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
            private set { }
        }

        [DataMember]
        [Indexed]
        public virtual string NormalizedAltName
        {
            get
            {
                return Helpers.RemoveDiacritics(AltNameText) ?? string.Empty;
            }
            private set { }
        }

        [DataMember(Name = "Item_Source", Order = 7)]
        [Indexed]
        public virtual string Source { get; set; }

        [YamlIgnore]
        [DataMember(Name = "Item_Markdown", Order = 8)]
        public virtual string Markdown { get; set; }
        [DataMember(Name = "Item_FullText", Order = 9)]
        public virtual string FullText { get; set; }

        [IgnoreDataMember]
        [YamlIgnore]
        public static Dictionary<string, Type> ClassMapping = new Dictionary<string, Type>()
        {
            { "GenericItem", typeof(GenericItem) },
            { "AlignmentItem", typeof(AlignmentItem) },
            { "MonsterItem", typeof(MonsterItem) },
            { "MonsterItems", typeof(MonsterItems) },
            { "SpellItem", typeof(SpellItem) },
            { "SpellItems", typeof(SpellItems) },
            { "EquipmentItem", typeof(EquipmentItem) },
            { "EquipmentItems", typeof(EquipmentItems) },
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
            { "FeatureItem", typeof(FeatureItem) },
            { "ClassEquipmentItem", typeof(ClassEquipmentItem) },
            { "ClassEvolutionItem", typeof(ClassEvolutionItem) },
            { "ClassFeatureItem", typeof(ClassFeatureItem) },
            { "ClassHitPointsItem", typeof(ClassHitPointsItem) },
            { "ClassProficienciesItem", typeof(ClassProficienciesItem) },
            { "OriginItem", typeof(OriginItem) },
            { "OriginItems", typeof(OriginItems) },
            { "OriginFeatureItem", typeof(OriginFeatureItem) },

            { "Items", typeof(Items) },
            { "Item", typeof(Item) },
        };

        [IgnoreDataMember]
        [YamlIgnore]
        public virtual string Yaml
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
                //return $"---\n{Yaml}---\n{CleanMarkdown}";
                return CleanMarkdown;
            }
        }

        [IgnoreDataMember]
        [YamlIgnore]
        public virtual string SubMarkdown
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

        [YamlIgnore]
        public virtual string CleanMarkdown
        {
            get
            {
                var md = Markdown ?? string.Empty;
                var rx = new Regex("<!--.*?-->");
                md = rx.Replace(md, "");
                return md;
            }
        }

        [YamlIgnore]
        public virtual string NewId
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
                    id = id.Replace(".md#", "_");
                    id = id.Replace(".md", "");
                }
                id = id.Replace("-", "_").Replace("?", "").Replace("’", "_").Replace("'", "_").Replace("__", "_").ToLower() + ".md";
                if (id.Contains("#"))
                {
                    Console.WriteLine(id);
                }
                return id;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({NewId})";
        }

        static IDeserializer _Deserializer = new DeserializerBuilder().WithNamingConvention(new PascalCaseNamingConvention()).Build();
        static ISerializer _Serializer = new SerializerBuilder().WithNamingConvention(new PascalCaseNamingConvention()).Build();

        protected OrderedDictionary Attributes { get; private set; } = new OrderedDictionary();
        //[NotMapped]
        //[IgnoreDataMember]
        //protected virtual OrderedDictionary Attributes
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(AttributesDictionary))
        //        {
        //            return new OrderedDictionary();
        //        }
        //        else
        //        {
        //            //var builder = new DeserializerBuilder();
        //            //var deserializer = builder
        //            //    .WithNamingConvention(new PascalCaseNamingConvention())
        //            //    .Build();
        //            return _Deserializer.Deserialize<OrderedDictionary>(AttributesDictionary);
        //        }
        //    }
        //}// = new OrderedDictionary();
        public void SaveAttributes()
        {
            if (Attributes == null)
            {
                AttributesDictionary = null;
            }
            else
            {
                //var builder = new SerializerBuilder();
                //var serializer = builder
                //    .WithNamingConvention(new PascalCaseNamingConvention())
                //    .Build();
                AttributesDictionary = _Serializer.Serialize(Attributes);
            }
        }
        public void LoadAttributes()
        {
            Attributes = GetAttributes();
        }

        public OrderedDictionary GetAttributes()
        {
            if (string.IsNullOrEmpty(AttributesDictionary))
            {
                return new OrderedDictionary();
            }
            else
            {
                return _Deserializer.Deserialize<OrderedDictionary>(AttributesDictionary);
            }
        }

        [YamlIgnore]
        public virtual OrderedDictionary AttributesKeyValue
        {
            get
            {
                return ItemAttribute.ExtractKeyValues(Attributes);
            }
        }

        [DataMember]
        public virtual string AttributesDictionary { get; set; }
    //    {
    //        get
    //        {
    //            var builder = new SerializerBuilder();
    //            var serializer = builder
    //                .WithNamingConvention(new PascalCaseNamingConvention())
    //                .Build();
    //            return serializer.Serialize(Attributes);
    //        }
    //        set
    //        {
    //            var builder = new DeserializerBuilder();
    //var deserializer = builder
    //    .WithNamingConvention(new PascalCaseNamingConvention())
    //    .Build();
    //Attributes = deserializer.Deserialize<OrderedDictionary>(value);
    //        }
    //    }

        public virtual void ResetAttribute(string name)
        {
            if (name != null)
            {
                var prop = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(this, null, null);
                }
                if (this.Attributes.Contains(name))
                {
                    this.Attributes.Remove(name);
                }
                SaveAttributes();
            }
        }
        public virtual void SetAttribute(string name, string value)
        {
            if (name != null && value != null)
            {
                var prop = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(this, prop.GetValue(this) + value, null);
                }
                if (this.Attributes.Contains(name))
                {
                    this.Attributes[name] += value;
                }
                else
                {
                    this.Attributes[name] = value;
                }
                SaveAttributes();
            }
        }
        public virtual string GetAttribute(string name)
        {
            LoadAttributes();
            if (this.Attributes.Contains(name))
            {
                return this.Attributes[name].ToString();
            }
            return null;
        }


        [DataMember]
        public virtual string Description { get; set; }
    }
}
