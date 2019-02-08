using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AideDeJeu.ViewModels;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class BackgroundItem : Item
    {
        public string Abilities { get; set; }
        public string MasteredTools { get; set; }
        public string MasteredLanguages { get; set; }
        public string Equipment { get; set; }

    }
}
