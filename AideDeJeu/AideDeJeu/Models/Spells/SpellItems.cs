using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.Serialization;
using System.Linq;
using AideDeJeu.ViewModels.Library;

namespace AideDeJeuLib
{
    public class SpellItems : FilteredItems
    {
        public string Classes { get; set; }
        public string Levels { get; set; }
        public string Schools { get; set; }
        public string Rituals { get; set; }
        public string CastingTimes { get; set; }
        public string Ranges { get; set; }
        public string VerbalComponents { get; set; }
        public string SomaticComponents { get; set; }
        public string MaterialComponents { get; set; }
        public string Concentrations { get; set; }
        public string Durations { get; set; }
        public string Sources { get; set; }

        public override FilterViewModel GetNewFilterViewModel()
        {
            return new SpellFilterViewModel(Family,
                Split(Classes),
                Split(Levels),
                Split(Schools),
                Split(Rituals),
                Split(CastingTimes),
                Split(Ranges),
                Split(VerbalComponents),
                Split(SomaticComponents),
                Split(MaterialComponents),
                Split(Concentrations),
                Split(Durations),
                Split(Sources)
            );
        }
    }
}
