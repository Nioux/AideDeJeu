using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class AbilitiesViewModel : BaseViewModel
    {
        public AbilitiesViewModel()
        {
            Listen();
        }

        public bool _Listening = false;
        public void Listen()
        {
            if (!_Listening)
            {
                _Listening = true;
                Strength.PropertyChanged += Strength_PropertyChanged;
                Dexterity.PropertyChanged += Dexterity_PropertyChanged;
                Constitution.PropertyChanged += Constitution_PropertyChanged;
                Intelligence.PropertyChanged += Intelligence_PropertyChanged;
                Wisdom.PropertyChanged += Wisdom_PropertyChanged;
                Charisma.PropertyChanged += Charisma_PropertyChanged;
            }
        }

        public void Unlisten()
        {
            if (_Listening)
            {
                _Listening = false;
                Strength.PropertyChanged -= Strength_PropertyChanged;
                Dexterity.PropertyChanged -= Dexterity_PropertyChanged;
                Constitution.PropertyChanged -= Constitution_PropertyChanged;
                Intelligence.PropertyChanged -= Intelligence_PropertyChanged;
                Wisdom.PropertyChanged -= Wisdom_PropertyChanged;
                Charisma.PropertyChanged -= Charisma_PropertyChanged;
            }
        }

        private void Charisma_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Charisma, () =>
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Strength);
                list.AddLast(Dexterity);
                list.AddLast(Constitution);
                list.AddLast(Intelligence);
                list.AddLast(Wisdom);
                return list;
            });
        }

        private void Wisdom_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Wisdom, () =>
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Charisma);
                list.AddLast(Strength);
                list.AddLast(Dexterity);
                list.AddLast(Constitution);
                list.AddLast(Intelligence);
                return list;
            });
        }

        private void Intelligence_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Intelligence, () =>
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Wisdom);
                list.AddLast(Charisma);
                list.AddLast(Strength);
                list.AddLast(Dexterity);
                list.AddLast(Constitution);
                return list;
            });
        }

        private void Constitution_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Constitution, () =>
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Intelligence);
                list.AddLast(Wisdom);
                list.AddLast(Charisma);
                list.AddLast(Strength);
                list.AddLast(Dexterity);
                return list;
            });
        }

        private void Dexterity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Dexterity, () =>
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Constitution);
                list.AddLast(Intelligence);
                list.AddLast(Wisdom);
                list.AddLast(Charisma);
                list.AddLast(Strength);
                return list;
            });
        }

        private void Strength_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckRacialDispatchedBonus(e.PropertyName, Strength, () => 
            {
                var list = new LinkedList<AbilityViewModel>();
                list.AddLast(Dexterity);
                list.AddLast(Constitution);
                list.AddLast(Intelligence);
                list.AddLast(Wisdom);
                list.AddLast(Charisma);
                return list;
            });
        }

        private void CheckRacialDispatchedBonus(string propertyName, AbilityViewModel ability, Func<LinkedList<AbilityViewModel>> funcList)
        {
            if (propertyName == nameof(AbilityViewModel.RacialDispatchedBonus))
            {
                if (ability.RacialDispatchedBonus > 0 && SumRacialDispatchedBonus > MaxRacialDispatchedBonus)
                {
                    DecrementNext(funcList());
                }
            }
        }

            private void DecrementNext(LinkedList<AbilityViewModel> list)
        {
            var ability = list.First;
            while (ability != null)
            {
                if (ability.Value.RacialDispatchedBonus > 0)
                {
                    ability.Value.RacialDispatchedBonus--;
                    break;
                }
                ability = ability.Next;
            }
        }

        private int SumRacialDispatchedBonus
        {
            get
            {
                return
                    Strength.RacialDispatchedBonus +
                    Dexterity.RacialDispatchedBonus +
                    Constitution.RacialDispatchedBonus +
                    Intelligence.RacialDispatchedBonus +
                    Wisdom.RacialDispatchedBonus +
                    Charisma.RacialDispatchedBonus;
            }
        }

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

        private int _MaxRacialDispatchedBonus = 0;
        public int MaxRacialDispatchedBonus { get { return _MaxRacialDispatchedBonus; } set { SetProperty(ref _MaxRacialDispatchedBonus, value); OnPropertyChanged(nameof(HasRacialDispatchedBonus)); } }
        public bool HasRacialDispatchedBonus { get { return _MaxRacialDispatchedBonus > 0; } }
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
        private int _MaxRacialDispatchedBonus = 0;
        public int MaxRacialDispatchedBonus
        {
            get
            {
                return _MaxRacialDispatchedBonus;
            }
            set
            {
                SetProperty(ref _MaxRacialDispatchedBonus, value);
                OnPropertyChanged(nameof(HasRacialDispatchedBonus));
            }
        }
        public bool HasRacialDispatchedBonus
        {
            get
            {
                return _MaxRacialDispatchedBonus > 0;
            }
        }

        public int? Value { get { return BaseValue != null ? BaseValue + RacialBonus + RacialDispatchedBonus : null; } }
        public string ValueString { get { return Value != null ? Value.ToString() : null; } }
        public int? Mod { get { return Value != null ? Value / 2 - 5 : null; } }
        public string ModString { get { return Mod != null ? Mod.Value.ToString("+0;-#") : null; } }

    }
}
