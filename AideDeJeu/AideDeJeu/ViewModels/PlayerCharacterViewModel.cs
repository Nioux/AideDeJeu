using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels
{
    public class PlayerCharacterViewModel
    {
        public RaceItem Race { get; set; }
        public ClassItem Class { get; set; }
        public BackgroundItem Background { get; set; }
    }
}
