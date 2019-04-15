using System.Runtime.Serialization;

namespace AideDeJeuLib
{

    public class RaceItem : Item
    {
        [DataMember]
        public virtual string FullName { get; set; }
        [DataMember]
        public virtual bool HasSubRaces { get; set; }

        [DataMember]
        public virtual string StrengthBonus { get; set; }
        [DataMember]
        public virtual string DexterityBonus { get; set; }
        [DataMember]
        public virtual string ConstitutionBonus { get; set; }
        [DataMember]
        public virtual string IntelligenceBonus { get; set; }
        [DataMember]
        public virtual string WisdomBonus { get; set; }
        [DataMember]
        public virtual string CharismaBonus { get; set; }
        [DataMember]
        public virtual string AnyAbilityBonus { get; set; }

        [DataMember]
        public virtual string AbilityScoreIncrease { get; set; }
        [DataMember]
        public virtual string Age { get; set; }
        [DataMember]
        public virtual string Alignment { get; set; }
        [DataMember]
        public virtual string Size { get; set; }
        [DataMember]
        public virtual string Speed { get; set; }
        [DataMember]
        public virtual string Darkvision { get; set; }
        [DataMember]
        public virtual string Languages { get; set; }

    }
}
