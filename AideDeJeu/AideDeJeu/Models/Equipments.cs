using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class Equipments : Item
    {
        public override FilterViewModel GetNewFilterViewModel()
        {
            return new VFEquipmentFilterViewModel();
        }
    }
}
