using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class MonsterItems : FilteredItems
    {
        public string Types { get; set; }
        public string Challenges { get; set; }
        public string Sizes { get; set; }
        public string Sources { get; set; }
        public string Terrains { get; set; }

        public override FilterViewModel GetNewFilterViewModel()
        {
            return new MonsterFilterViewModel(Family,
                Split(Types),
                Split(Challenges),
                Split(Sizes),
                Split(Sources),
                Split(Terrains)
            );
        }
    }
}
