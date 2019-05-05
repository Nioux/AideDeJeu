using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class AbilitiesViewModel : BaseViewModel
    {
        private AbilityViewModel _Strength = new AbilityViewModel();
        public AbilityViewModel Strength { get { return _Strength; } set { SetProperty(ref _Strength, value); } }

        private AbilityViewModel _Dexterity = new AbilityViewModel();
        public AbilityViewModel Dexterity { get { return _Dexterity; } set { SetProperty(ref _Dexterity, value); } }

        private AbilityViewModel _Constitution = new AbilityViewModel();
        public AbilityViewModel Constitution { get { return _Constitution; } set { SetProperty(ref _Constitution, value); } }

        private AbilityViewModel _Intelligence = new AbilityViewModel();
        public AbilityViewModel Intelligence { get { return _Intelligence; } set { SetProperty(ref _Intelligence, value); } }

        private AbilityViewModel _Wisdom = new AbilityViewModel();
        public AbilityViewModel Wisdom { get { return _Wisdom; } set { SetProperty(ref _Wisdom, value); } }

        private AbilityViewModel _Charisma = new AbilityViewModel();
        public AbilityViewModel Charisma { get { return _Charisma; } set { SetProperty(ref _Charisma, value); } }
    }

    public class AbilityViewModel : BaseViewModel
    {
        private int? _BaseValue = null;
        public int? BaseValue
        {
            get { return _BaseValue; }
            set
            {
                SetProperty(ref _BaseValue, value);
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Mod));
                OnPropertyChanged(nameof(ValueString));
                OnPropertyChanged(nameof(ModString));
            }
        }
        private int _RacialBonus = 0;
        public int RacialBonus
        {
            get { return _RacialBonus; }
            set
            {
                SetProperty(ref _RacialBonus, value);
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Mod));
                OnPropertyChanged(nameof(ValueString));
                OnPropertyChanged(nameof(ModString));
            }
        }
        private int _RacialDispatchedBonus = 0;
        public int RacialDispatchedBonus
        {
            get { return _RacialDispatchedBonus; }
            set
            {
                SetProperty(ref _RacialDispatchedBonus, value);
                OnPropertyChanged(nameof(Value));
                OnPropertyChanged(nameof(Mod));
                OnPropertyChanged(nameof(ValueString));
                OnPropertyChanged(nameof(ModString));
            }
        }

        public int? Value { get { return BaseValue != null ? BaseValue + RacialBonus + RacialDispatchedBonus : null; } }
        public string ValueString { get { return Value != null ? Value.ToString() : null; } }
        public int? Mod { get { return Value != null ? Value / 2 - 5 : null; } }
        public string ModString { get { return Mod != null ? Mod.Value.ToString("+0;-#") : null; } }

    }
}
