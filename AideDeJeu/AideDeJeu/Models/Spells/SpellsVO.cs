using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class SpellsVO : Item
    {
        public override FilterViewModel GetNewFilterViewModel()
        {
            return new VOSpellFilterViewModel();
        }
    }
}
