using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class ClassItem : Item
    {
        [YamlMember(Order = 10)]
        public ClassHitPointsItem HitPoints
        {
            get
            {
                return GetChild<ClassHitPointsItem>();
            }
        }

        [YamlMember(Order = 11)]
        public ClassProficienciesItem Proficiencies
        {
            get
            {
                return GetChild<ClassProficienciesItem>();
            }
        }

        [YamlMember(Order = 12)]
        public ClassEquipmentItem Equipment
        {
            get
            {
                return GetChild<ClassEquipmentItem>();
            }
        }

        [YamlMember(Order = 13)]
        public ClassEvolutionItem Evolution
        {
            get
            {
                return GetChild<ClassEvolutionItem>();
            }
        }

        [YamlMember(Order = 14)]
        public IEnumerable<ClassFeatureItem> Features
        {
            get
            {
                return GetChildren<ClassFeatureItem>();
            }
            private set
            { }
        }

        [YamlMember(Order = 15)]
        public IEnumerable<SubClassItem> SubClasses
        {
            get
            {
                return GetChildren<SubClassItem>();
            }
        }
    }
}
