using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class Equipments : FilteredItems
    {
        public string Types { get; set; }
        public string Prices { get; set; }

        public override FilterViewModel GetNewFilterViewModel()
        {
            return new EquipmentFilterViewModel(Family,
                Split(Types),
                Split(Prices)
                );
        }
    }
}
