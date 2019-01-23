using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AideDeJeu.ViewModels;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class FilteredItems : Items
    {
        [IgnoreDataMember]
        [YamlMember]
        public List<Item> SubItems
        {
            get
            {
                return _Items;
            }
            set
            {

            }
        }
        public override string Markdown
        {
            get
            {
                if (_Items != null)
                {
                    var md = string.Empty;
                    foreach (var item in _Items)
                    {
                        md += item.Markdown;
                    }
                    return md;
                }
                return null;
            }
            set
            {

            }
        }
    }
}
