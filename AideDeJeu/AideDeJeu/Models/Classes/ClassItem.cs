using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class ClassItem : Item
    {
        [YamlMember]
        public ClassHitPointsItem HitPoints
        {
            get
            {
                return GetChild<ClassHitPointsItem>();
            }
        }

        [YamlMember]
        public ClassProficienciesItem Proficiencies
        {
            get
            {
                return GetChild<ClassProficienciesItem>();
            }
        }

        [YamlMember]
        public ClassEquipmentItem Equipment
        {
            get
            {
                return GetChild<ClassEquipmentItem>();
            }
        }

        [YamlMember]
        public ClassEvolutionItem Evolution
        {
            get
            {
                return GetChild<ClassEvolutionItem>();
            }
        }

        [YamlMember]
        public IEnumerable<ClassFeatureItem> Features
        {
            get
            {
                return GetChildren<ClassFeatureItem>();
            }
        }

        [YamlMember]
        public IEnumerable<SubClassItem> SubClasses
        {
            get
            {
                return GetChildren<SubClassItem>();
            }
        }
    }
}
