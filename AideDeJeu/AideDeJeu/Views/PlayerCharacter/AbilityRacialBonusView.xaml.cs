using AideDeJeu.ViewModels.PlayerCharacter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AideDeJeu.Views.PlayerCharacter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbilityRacialBonusView : ContentView
    {
        public AbilityRacialBonusView()
        {
            InitializeComponent();
        }

        public AbilityViewModel Ability
        {
            get { return (AbilityViewModel)GetValue(AbilityProperty); }
            set { SetValue(AbilityProperty, value); }
        }
        public static readonly BindableProperty AbilityProperty = BindableProperty.Create(
            nameof(Ability),
            typeof(AbilityViewModel),
            typeof(AbilityRacialBonusView),
            defaultValue: default(AbilityViewModel));
    }
}