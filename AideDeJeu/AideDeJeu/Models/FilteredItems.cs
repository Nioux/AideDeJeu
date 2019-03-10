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
        public string Family { get; set; }

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

        [IgnoreDataMember]
        [YamlIgnore]
        public override string YamlMarkdown
        {
            get
            {
                return $"---\n{Yaml}---\n{SubMarkdown}";
            }
        }

    }
}
