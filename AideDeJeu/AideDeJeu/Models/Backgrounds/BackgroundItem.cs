using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public class BackgroundItem : Items
    {
        public string SkillProficiencies { get; set; }
        public string MasteredTools { get; set; }
        public string MasteredLanguages { get; set; }
        public string Equipment { get; set; }

        [YamlMember]
        public FeatureItem Feature
        {
            get
            {
                return GetChild<FeatureItem>();
            }
        }

        [YamlMember]
        public BackgroundSpecialtyItem Specialty
        {
            get
            {
                return GetChild<BackgroundSpecialtyItem>();
            }
        }

        [YamlMember]
        public PersonalityTraitItem PersonalityTraits
        {
            get
            {
                return GetChild<PersonalityTraitItem>();
            }
        }

        [YamlMember]
        public PersonalityIdealItem Ideal
        {
            get
            {
                return GetChild<PersonalityIdealItem>();
            }
        }

        [YamlMember]
        public PersonalityLinkItem Bond
        {
            get
            {
                return GetChild<PersonalityLinkItem>();
            }
        }

        [YamlMember]
        public PersonalityDefectItem Flaw
        {
            get
            {
                return GetChild<PersonalityDefectItem>();
            }
        }

        [YamlMember]
        public IEnumerable<SubBackgroundItem> SubBackgrounds
        {
            get
            {
                return GetChildren<SubBackgroundItem>();
            }
        }
    }
}
