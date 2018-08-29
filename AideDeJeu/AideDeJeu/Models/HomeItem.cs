using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    [DataContract]
    public class HomeItem : Item
    {
        [DataMember]
        public override string Markdown
        {
            get
            {
                return AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.index.md");
            }
        }
    }
}
