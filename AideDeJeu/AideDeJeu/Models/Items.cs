using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AideDeJeu.Tools;
using AideDeJeu.ViewModels;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace AideDeJeuLib
{
    public class Items : Item, IList<Item>
    {
        private List<Item> _Items;

        public Items(List<Item> items)
        {
            _Items = items;
        }

        public Items()
        {
            _Items = new List<Item>();
        }

        //private string _Markdown = "";
        //public override string Markdown
        //{
        //    get
        //    {
        //        return _Markdown;
        //    }
        //}

        public string Header { get; set; }

        public int Count => _Items.Count();

        public bool IsReadOnly => false;

        public Item this[int index] { get => _Items[index]; set => _Items[index] = value; }

        public IEnumerator<Item> GetEnumerator()
        {
            return _Items?.GetEnumerator();
        }

        public void ParseHeader(ref ContainerBlock.Enumerator enumerator)
        {
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block.IsNewItem())
                {
                    break;
                }
                else if (block is HeadingBlock)
                {
                    var headingBlock = block as HeadingBlock;
                    if (headingBlock.Level == 1 && headingBlock.HeaderChar == '#')
                    {
                        Name = headingBlock.Inline.ToMarkdownString();

                    }
                }
                Header += block.ToMarkdownString();
                enumerator.MoveNext();
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            var items = new List<Item>();
            enumerator.MoveNext();
            ParseHeader(ref enumerator);
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block.IsClosingItem())
                {
                    break;
                }
                var item = block.GetNewItem();
                item.Parse(ref enumerator);
                items.Add(item);
            }
            _Items = items;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items?.GetEnumerator();
        }

        public virtual FilterViewModel GetNewFilterViewModel()
        {
            return null;
        }

        public int IndexOf(Item item)
        {
            return _Items.IndexOf(item);
        }

        public void Insert(int index, Item item)
        {
            _Items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _Items.RemoveAt(index);
        }

        public void Add(Item item)
        {
            _Items.Add(item);
        }

        public void Clear()
        {
            _Items.Clear();
        }

        public bool Contains(Item item)
        {
            return _Items.Contains(item);
        }

        public void CopyTo(Item[] array, int arrayIndex)
        {
            _Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
