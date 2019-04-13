using System.Runtime.Serialization;

namespace AideDeJeuLib
{

    public class RaceItem : Item
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public bool HasSubRaces { get; set; }

        [DataMember]
        public string StrengthBonus { get; set; }
        [DataMember]
        public string DexterityBonus { get; set; }
        [DataMember]
        public string ConstitutionBonus { get; set; }
        [DataMember]
        public string IntelligenceBonus { get; set; }
        [DataMember]
        public string WisdomBonus { get; set; }
        [DataMember]
        public string CharismaBonus { get; set; }
        [DataMember]
        public string AnyAbilityBonus { get; set; }

        [DataMember]
        public string Age { get; set; }
        [DataMember]
        public string Alignment { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public string Speed { get; set; }
        [DataMember]
        public string Darkvision { get; set; }
        [DataMember]
        public string Languages { get; set; }
        [DataMember]
        public string AbilityScoreIncrease { get; set; }
        [DataMember]
        public string CelestialResistance { get; set; }
        [DataMember]
        public string DraconicAncestry { get; set; }
        [DataMember]
        public string BreathWeapon { get; set; }
        [DataMember]
        public string DamageResistance { get; set; }


    }
}
