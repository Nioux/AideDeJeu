using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class SpellsHD : Spells
    {
        public override FilterViewModel GetNewFilterViewModel()
        {
            return new HDSpellFilterViewModel();
        }
    }
}
