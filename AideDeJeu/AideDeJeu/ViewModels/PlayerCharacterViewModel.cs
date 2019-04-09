using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels
{
    public class PlayerCharacterViewModel : BaseViewModel
    {
        private RaceItem _Race = null;
        public RaceItem Race
        {
            get
            {
                return _Race;
            }
            set
            {
                SetProperty(ref _Race, value);
            }
        }
        private ClassItem _Class = null;
        public ClassItem Class
        {
            get
            {
                return _Class;
            }
            set
            {
                SetProperty(ref _Class, value);
            }
        }
        private BackgroundItem _Background = null;
        public BackgroundItem Background
        {
            get
            {
                return _Background;
            }
            set
            {
                SetProperty(ref _Background, value);
            }
        }
        private SubBackgroundItem _SubBackground = null;
        public SubBackgroundItem SubBackground
        {
            get
            {
                return _SubBackground;
            }
            set
            {
                SetProperty(ref _SubBackground, value);
            }
        }
        private string _PersonalityTrait = null;
        public string PersonalityTrait
        {
            get
            {
                return _PersonalityTrait;
            }
            set
            {
                SetProperty(ref _PersonalityTrait, value);
            }
        }
    }
}
