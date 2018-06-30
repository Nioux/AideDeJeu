using System.Collections.Generic;
using System.Xml;

namespace AideDeJeuLib
{
    public class Property : Dictionary<string, string>
    {

    }

    public class Properties : Dictionary<string, Property>
    {

    }

    public class Item
    {
        public string Id
        {
            get
            {
                return AideDeJeu.Tools.Helpers.IdFromName(Name);
            }
        }
        public string Name { get; set; }
        public string NameVO { get; set; }

        public Properties Properties { get; set; }

    }
}
