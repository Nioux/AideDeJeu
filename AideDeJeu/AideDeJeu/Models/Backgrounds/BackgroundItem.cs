using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{

    public static class MarkdownExtensions
    {
        public static string StripMarkdownLink(this string md)
        {
            var regex = new Regex("\\[(?<text>.*?)\\]");
            var match = regex.Match(md ?? string.Empty);
            if (!string.IsNullOrEmpty(match.Groups["text"].Value))
            {
                return match.Groups["text"].Value;
            }
            return md;
        }

    }
    public class AndNode
    {
        [YamlMember(Alias = "and")]
        public IEnumerable<object> Children { get; set; }

    }

    public class OrNode
    {
        [YamlMember(Alias = "or")]
        public IEnumerable<object> Children { get; set; }
    }

    public class BackgroundItem : Item
    {
        protected IEnumerable<string> ExpandAnd(string input)
        {
            return input != null ? input.Trim('.').Split(new String[] { ", ", " et " }, StringSplitOptions.None) : new string[] { };
        }
        protected object ExpandOr(string orstring)
        {
            var split = orstring.Split(new string[] { " ou " }, System.StringSplitOptions.None);
            if(split.Length > 1)
            {
                return new OrNode() { Children = split.Select(str => str.StripMarkdownLink()) };
            }
            return split[0].StripMarkdownLink();
        }

        [YamlIgnore]
        public string SkillProficiencies { get; set; }
        [YamlMember(Alias = "skill_proficiencies", Order = 10)]
        public object SkillProficienciesExpanded 
        { 
            get
            {
                var splitand = ExpandAnd(SkillProficiencies).ToList();
                if (splitand.Count > 1)
                {
                    var ret = new List<object>();
                    foreach (var orstring in splitand)
                    {
                        ret.Add(ExpandOr(orstring));
                    }
                    return new AndNode() { Children = ret };
                }
                else if(splitand.Count == 1)
                {
                    return ExpandOr(splitand[0]);
                }
                return null;
            }
        }

        [YamlMember(Order = 11)]
        public string MasteredTools { get; set; }
        
        [YamlMember(Order = 12)]
        public string MasteredLanguages { get; set; }
        
        [YamlIgnore]
        public string Equipment { get; set; }
        [YamlMember(Alias = "equipment", Order = 13)]
        public IEnumerable<string> EquipmentExpanded
        {
            get
            {
                return Expand(Equipment);
            }
        }

        [YamlMember(Order = 14)]
        public FeatureItem Feature
        {
            get
            {
                return GetChild<FeatureItem>();
            }
        }

        [YamlMember(Order = 15)]
        public BackgroundSpecialtyItem Specialty
        {
            get
            {
                return GetChild<BackgroundSpecialtyItem>();
            }
        }

        [YamlMember(Order = 16)]
        public PersonalityTraitItem PersonalityTraits
        {
            get
            {
                return GetChild<PersonalityTraitItem>();
            }
        }

        [YamlMember(Order = 17)]
        public PersonalityIdealItem Ideal
        {
            get
            {
                return GetChild<PersonalityIdealItem>();
            }
        }

        [YamlMember(Order = 17)]
        public PersonalityLinkItem Bond
        {
            get
            {
                return GetChild<PersonalityLinkItem>();
            }
        }

        [YamlMember(Order = 18)]
        public PersonalityDefectItem Flaw
        {
            get
            {
                return GetChild<PersonalityDefectItem>();
            }
        }

        [IgnoreDataMember]
        [YamlMember(Order = 19)]
        public IEnumerable<SubBackgroundItem> SubBackgrounds
        {
            get
            {
                return GetChildren<SubBackgroundItem>();
            }
            private set
            {

            }
        }
    }
}
