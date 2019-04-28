using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
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
        private RaceViewModel _Race = null;
        public RaceViewModel Race
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
        private ClassViewModel _Class = null;
        public ClassViewModel Class
        {
            get
            {
                return _Class;
            }
            set
            {
                SetProperty(ref _Class, value);
                _Class.LoadDetailsAsync().ConfigureAwait(true);
            }
        }
        private BackgroundViewModel _Background = null;
        public BackgroundViewModel Background
        {
            get
            {
                return _Background;
            }
            set
            {
                SetProperty(ref _Background, value);
                //_Background.LoadDetailsAsync().ConfigureAwait(true);
            }
        }





        #region Background
        //private BackgroundItem _Background = null;
        //public BackgroundItem Background
        //{
        //    get
        //    {
        //        return _Background;
        //    }
        //    set
        //    {
        //        SetProperty(ref _Background, value);
        //        OnPropertyChanged(nameof(SelectedBackground));
        //    }
        //}
        //private SubBackgroundItem _SubBackground = null;
        //public SubBackgroundItem SubBackground
        //{
        //    get
        //    {
        //        return _SubBackground;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SubBackground, value);
        //        OnPropertyChanged(nameof(SelectedBackground));
        //    }
        //}
        #endregion Background
    }
}
