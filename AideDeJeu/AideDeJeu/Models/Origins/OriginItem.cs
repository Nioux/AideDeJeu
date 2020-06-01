using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class OriginItem : Item
    {
        IEnumerable<string> Expand(string input)
        {
            return input != null ? input.Trim('.').Split(new String[] { ", ", " et " }, StringSplitOptions.None) : new string[] { };
        }
        [YamlIgnore]
        public string RegionsOfOrigin { get; set; }
        [YamlMember(Alias  = "regions_of_origin")]
        public IEnumerable<string> RegionsOfOriginExpanded
        {
            get
            {
                return Expand(RegionsOfOrigin);
            }
        }
        [YamlIgnore]
        public string MainLanguages { get; set; }
        [YamlMember(Alias = "main_languages")]
        public IEnumerable<string> MainLanguagesExpanded
        {
            get
            {
                return Expand(MainLanguages);
            }
        }
        [YamlIgnore]
        public string Aspirations { get; set; }
        [YamlMember(Alias = "aspirations")]
        public IEnumerable<string> AspirationsExpanded
        {
            get
            {
                return Expand(Aspirations);
            }
        }
        [YamlIgnore]
        public string AvailableSkills { get; set; }
        [YamlMember(Alias = "available_skills")]
        public IEnumerable<string> AvailableSkillsExpanded
        {
            get
            {
                return Expand(AvailableSkills);
            }
        }

        [YamlMember]
        public OriginFeatureItem Feature
        {
            get
            {
                return GetChild<OriginFeatureItem>();
            }
        }

    }
}
