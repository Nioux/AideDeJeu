using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using AideDeJeu.ViewModels;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class RaceItem : Item
    {
        public string StrengthBonus { get; set; }
        public string DexterityBonus { get; set; }
        public string ConstitutionBonus { get; set; }
        public string IntelligenceBonus { get; set; }
        public string WisdomBonus { get; set; }
        public string CharismaBonus { get; set; }
        public string AnyAbilityBonus { get; set; }

        public string Age { get; set; }
        public string Alignment { get; set; }
        public string Size { get; set; }
        public string Speed { get; set; }
        public string Darkvision { get; set; }
        public string Languages { get; set; }


    }
}
