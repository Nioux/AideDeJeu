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
        public override string Markdown => throw new NotImplementedException();

        public IEnumerator<Item> GetEnumerator()
        {
            return _Items.GetEnumerator();
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            var items = new List<Item>();
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                if(enumerator.Current.IsNewItem())
                {
                    break;
                }
                else if(enumerator.Current is HeadingBlock)
                {
                    var headingBlock = enumerator.Current as HeadingBlock;
                    if(headingBlock.Level == 1 && headingBlock.HeaderChar == '#')
                    {
                        this.Name = headingBlock.Inline.ToMarkdownString();

                    }
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
            return _Items.GetEnumerator();
        }

        public virtual FilterViewModel GetNewFilterViewModel()
        {
            return null;
        }
    }
}
