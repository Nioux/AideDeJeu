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
    public class Items : Item, IEnumerable<Item>
    {
        private IEnumerable<Item> _Items;

        public Items(IEnumerable<Item> items)
        {
            _Items = items;
        }

        public Items()
        {
            _Items = new List<Item>();
        }

        private string _Markdown = "";
        public override string Markdown
        {
            get
            {
                return _Markdown;
            }
        }

        public string Header { get; set; }


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
    }
}
