using System.Runtime.Serialization;

namespace AideDeJeuLib
{
    //interface IRaceItem : IItem
    //{
    //    string FullName { get; set; }
    //    bool HasSubRaces { get; set; }

    //    string StrengthBonus { get; set; }
    //    string DexterityBonus { get; set; }
    //    string ConstitutionBonus { get; set; }
    //    string IntelligenceBonus { get; set; }
    //    string WisdomBonus { get; set; }
    //    string CharismaBonus { get; set; }

    //    string DispatchedBonus { get; set; }
    //    string MaxDispatchedStrengthBonus { get; set; }
    //    string MaxDispatchedDexterityBonus { get; set; }
    //    string MaxDispatchedConstitutionBonus { get; set; }
    //    string MaxDispatchedIntelligenceBonus { get; set; }
    //    string MaxDispatchedWisdomBonus { get; set; }
    //    string MaxDispatchedCharismaBonus { get; set; }

    //    string AbilityScoreIncrease { get; set; }
    //    string Age { get; set; }
    //    string Alignment { get; set; }
    //    string Size { get; set; }
    //    string Speed { get; set; }
    //    string Darkvision { get; set; }
    //    string Languages { get; set; }
    //}
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
        public virtual string DispatchedBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedStrengthBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedDexterityBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedConstitutionBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedIntelligenceBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedWisdomBonus { get; set; }
        [DataMember]
        public virtual string MaxDispatchedCharismaBonus { get; set; }

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
