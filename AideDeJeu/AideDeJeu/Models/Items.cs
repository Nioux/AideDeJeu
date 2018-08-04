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
                //return "\n\n# test\n\n";
                return _Markdown;
            }
        }


        public IEnumerator<Item> GetEnumerator()
        {
            return _Items?.GetEnumerator();
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            var items = new List<Item>();
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block.IsNewItem())
                {
                    break;
                }
                else if(block is HeadingBlock)
                {
                    var headingBlock = block as HeadingBlock;
                    if(headingBlock.Level == 1 && headingBlock.HeaderChar == '#')
                    {
                        this.Name = headingBlock.Inline.ToMarkdownString();

                    }
                    else
                    {
                        _Markdown += headingBlock.ToMarkdownString();
                    }
                }
                else
                {
                    _Markdown += block.ToMarkdownString();
                }
                enumerator.MoveNext();
            }
            while (enumerator.Current != null)
            {
                var item = enumerator.Current.GetNewItem();
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
