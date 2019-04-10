using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels
{
    public class PlayerCharacterViewModel : BaseViewModel
    {
        private AlignmentItem _Alignment = null;
        public AlignmentItem Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                SetProperty(ref _Alignment, value);
            }
        }
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
            }
        }
    }
}
