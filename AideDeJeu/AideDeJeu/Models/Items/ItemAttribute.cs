using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AideDeJeuLib
{
    public class ItemAttribute
    {
        public ItemAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }

        public static OrderedDictionary ExtractKeyValues(OrderedDictionary attributes)
        {
            var dico = new OrderedDictionary();
            foreach (string akey in attributes.Keys)
            {
                if (akey.EndsWith("Key"))
                {
                    var key = akey.Substring(0, akey.Length - 3);
                    if (attributes.Contains(key + "Value"))
                    {
                        dico[key] = new ItemAttribute(attributes[key + "Key"] as string, attributes[key + "Value"] as string);
                    }
                }
            }
            return dico;
        }
    }
}
