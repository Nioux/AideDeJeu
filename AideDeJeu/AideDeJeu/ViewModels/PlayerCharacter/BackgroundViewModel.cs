using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class BackgroundViewModel : BaseViewModel
    {
        private BackgroundItem _Background = null;
        public BackgroundItem Background
        {
            get
            {
                return _Background;
            }
            set
            {
                SubBackground = null;
                SetProperty(ref _Background, value);
                OnPropertyChanged(nameof(BackgroundOrSubBackground));
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
                OnPropertyChanged(nameof(BackgroundOrSubBackground));
            }
        }

        public BackgroundItem BackgroundOrSubBackground
        {
            get
            {
                return _SubBackground ?? _Background;
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
        private string _PersonalityIdeal = null;
        public string PersonalityIdeal
        {
            get
            {
                return _PersonalityIdeal;
            }
            set
            {
                SetProperty(ref _PersonalityIdeal, value);
            }
        }
        private string _PersonalityLink = null;
        public string PersonalityLink
        {
            get
            {
                return _PersonalityLink;
            }
            set
            {
                SetProperty(ref _PersonalityLink, value);
            }
        }
        private string _PersonalityDefect = null;
        public string PersonalityDefect
        {
            get
            {
                return _PersonalityDefect;
            }
            set
            {
                SetProperty(ref _PersonalityDefect, value);
            }
        }
        private string _BackgroundSpecialty = null;
        public string BackgroundSpecialty
        {
            get
            {
                return _BackgroundSpecialty;
            }
            set
            {
                SetProperty(ref _BackgroundSpecialty, value);
            }
        }
        private FeatureItem _BackgroundSkill = null;
        public FeatureItem BackgroundSkill
        {
            get
            {
                return _BackgroundSkill;
            }
            set
            {
                SetProperty(ref _BackgroundSkill, value);
                OnPropertyChanged(nameof(BackgroundOrSubBackgroundSkill));
            }
        }
        private FeatureItem _SubBackgroundSkill = null;
        public FeatureItem SubBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill;
            }
            set
            {
                SetProperty(ref _SubBackgroundSkill, value);
                OnPropertyChanged(nameof(BackgroundOrSubBackgroundSkill));
            }
        }
        public FeatureItem BackgroundOrSubBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill ?? _BackgroundSkill;
            }
        }
    }
}
