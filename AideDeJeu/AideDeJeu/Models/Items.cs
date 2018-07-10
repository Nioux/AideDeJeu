using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig.Syntax;

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
                var item = new Spells.SpellHD();
                item.Parse(ref enumerator);
                items.Add(item);
                //enumerator.MoveNext();
            }
            _Items = items;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Items.GetEnumerator();
        }
    }
}
