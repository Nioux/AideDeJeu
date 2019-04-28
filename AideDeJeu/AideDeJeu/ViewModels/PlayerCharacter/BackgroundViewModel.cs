using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class BackgroundViewModel : BaseViewModel
    {
        private BackgroundItem _Background = null;
        public BackgroundItem Background { get { return _Background; } set { SetProperty(ref _Background, value); OnPropertyChanged(nameof(BackgroundOrSubBackground)); } }

        private SubBackgroundItem _SubBackground = null;
        public SubBackgroundItem SubBackground { get { return _SubBackground; } set { SetProperty(ref _SubBackground, value); OnPropertyChanged(nameof(BackgroundOrSubBackground)); } }

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
                OnPropertyChanged(nameof(SelectedBackgroundSpecialty));
            }
        }
        private string _SubBackgroundSpecialty = null;
        public string SubBackgroundSpecialty
        {
            get
            {
                return _SubBackgroundSpecialty;
            }
            set
            {
                SetProperty(ref _SubBackgroundSpecialty, value);
                OnPropertyChanged(nameof(SelectedBackgroundSpecialty));
            }
        }
        public string SelectedBackgroundSpecialty
        {
            get
            {
                return _SubBackgroundSpecialty ?? _BackgroundSpecialty;
            }
        }

        private string _PickedBackgroundSpecialty = null;
        public string PickedBackgroundSpecialty
        {
            get
            {
                return _PickedBackgroundSpecialty;
            }
            set
            {
                SetProperty(ref _PickedBackgroundSpecialty, value);
            }
        }

        private SkillItem _BackgroundSkill = null;
        public SkillItem BackgroundSkill
        {
            get
            {
                return _BackgroundSkill;
            }
            set
            {
                SetProperty(ref _BackgroundSkill, value);
                OnPropertyChanged(nameof(SelectedBackgroundSkill));
            }
        }
        private SkillItem _SubBackgroundSkill = null;
        public SkillItem SubBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill;
            }
            set
            {
                SetProperty(ref _SubBackgroundSkill, value);
                OnPropertyChanged(nameof(SelectedBackgroundSkill));
            }
        }
        public SkillItem SelectedBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill ?? _BackgroundSkill;
            }
        }
    }
}
