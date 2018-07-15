using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib
{
    public class MonstersVO : Monsters
    {
        public override FilterViewModel GetNewFilterViewModel()
        {
            return new VOMonsterFilterViewModel();
        }
    }
}
