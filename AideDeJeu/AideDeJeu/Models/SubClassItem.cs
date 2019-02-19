using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AideDeJeu.ViewModels;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class SubClassItem : ClassItem
    {
        [DataMember]
        public string ParentClassId { get; set; }
    }
}
