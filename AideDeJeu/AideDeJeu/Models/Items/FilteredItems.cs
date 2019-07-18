using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class FilteredItems : Items
    {
        public string Family { get; set; }

        public List<KeyValuePair<string, string>> Split(string collapsed)
        {
            if (collapsed == null) return new List<KeyValuePair<string, string>>();
            var split = collapsed.Split(new string[] { "|" }, StringSplitOptions.None).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
            split.Insert(0, new KeyValuePair<string, string>("", "*"));
            return split;
        }

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
