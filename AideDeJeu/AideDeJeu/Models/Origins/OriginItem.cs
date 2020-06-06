using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class NameValueNode
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public NameValueNode(string namevalue, string separator)
        {
            var split = namevalue.Split(new String[] { " " }, StringSplitOptions.None);
            if (split.Length == 2)
            {
                this.Name = split[0];
                this.Value = split[1];
            }
        }
    }
    public class OriginItem : Item
    {
        [YamlIgnore]
        public string RegionsOfOrigin { get; set; }
        [YamlMember(Alias  = "regions_of_origin", Order = 10)]
        public IEnumerable<string> RegionsOfOriginExpanded
        {
            get
            {
                return Expand(RegionsOfOrigin);
            }
        }
        
        [YamlIgnore]
        public string MainLanguages { get; set; }
        [YamlMember(Alias = "main_languages", Order = 11)]
        public IEnumerable<string> MainLanguagesExpanded
        {
            get
            {
                return Expand(MainLanguages);
            }
        }
        
        [YamlIgnore]
        public string Aspirations { get; set; }
        //[YamlMember(Alias = "aspirations", Order = 12)]
        //public IEnumerable<object> AspirationsExpanded
        //{
        //    get
        //    {
        //        return Expand(Aspirations).Select(aspi => new NameValueNode(aspi, " "));
        //    }
        //}
        [YamlMember(Alias = "aspirations", Order = 12)]
        public Dictionary<string, string> AspirationsExpanded2
        {
            get
            {
                var aspis = Expand(Aspirations);
                var dico = new Dictionary<string, string>();
                foreach(var aspi in aspis)
                {
                    var nv = new NameValueNode(aspi, " ");
                    dico[nv.Name] = nv.Value;
                }
                return dico;
            }
        }

        [YamlIgnore]
        public string AvailableSkills { get; set; }
        [YamlMember(Alias = "available_skills", Order = 13)]
        public IEnumerable<string> AvailableSkillsExpanded
        {
            get
            {
                return Expand(AvailableSkills);
            }
        }

        [YamlMember(Order = 14)]
        public OriginFeatureItem Feature
        {
            get
            {
                return GetChild<OriginFeatureItem>();
            }
        }

    }
}
