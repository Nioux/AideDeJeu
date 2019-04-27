using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class BackgroundViewModel : BaseViewModel
    {
        private BackgroundItem _Background = null;
        public BackgroundItem Background { get { return _Background; } set { SetProperty(ref _Background, value); } }

        private SubBackgroundItem _SubBackground = null;
        public SubBackgroundItem SubBackground { get { return _SubBackground; } set { SetProperty(ref _SubBackground, value); } }
    }
}
