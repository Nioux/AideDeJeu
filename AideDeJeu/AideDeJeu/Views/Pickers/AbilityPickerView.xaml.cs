using AideDeJeu.ViewModels.PlayerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.Pickers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbilityPickerView : ContentView
    {
        public AbilityPickerView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(ItemPickerView),
            defaultValue: default(string));

        public AbilityViewModel Ability
        {
            get { return (AbilityViewModel)GetValue(AbilityProperty); }
            set { SetValue(AbilityProperty, value); }
        }
        public static readonly BindableProperty AbilityProperty = BindableProperty.Create(
            nameof(Ability),
            typeof(AbilityViewModel),
            typeof(AbilityPickerView),
            defaultValue: default(AbilityViewModel));
        /*public int RacialBonus
        {
            get { return (int)GetValue(RacialBonusProperty); }
            set { SetValue(RacialBonusProperty, value); }
        }
        public static readonly BindableProperty RacialBonusProperty = BindableProperty.Create(
            nameof(RacialBonus),
            typeof(int),
            typeof(AbilityPickerView),
            defaultValue: default(int));
        public int DispatchedRacialBonus
        {
            get { return (int)GetValue(DispatchedRacialBonusProperty); }
            set { SetValue(DispatchedRacialBonusProperty, value); }
        }
        public static readonly BindableProperty DispatchedRacialBonusProperty = BindableProperty.Create(
            nameof(DispatchedRacialBonus),
            typeof(int),
            typeof(AbilityPickerView),
            defaultValue: default(int));*/

    }
}