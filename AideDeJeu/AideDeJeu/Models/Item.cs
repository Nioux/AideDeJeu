using AideDeJeu.ViewModels;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace AideDeJeuLib
{
    [DataContract]
    public class Item //: IList<Item>
    {
        private List<Item> _Items;

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
        [DataMember]
        public virtual string Markdown { get; set; }
        [DataMember]
        public string FullText { get; set; }
    }
}
