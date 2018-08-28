using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public virtual string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int NameLevel { get; set; }
        [DataMember]
        public string AltName { get; set; }
        [IgnoreDataMember]
        public string AltNameText
        {
            get
            {
                var regex = new Regex("\\[(?<text>.*?)\\]");
                var match = regex.Match(AltName ?? string.Empty);
                if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                {
                    return match.Groups["text"].Value;
                }
                else
                {
                    regex = new Regex("(?<text>.*?)( \\(SRD p\\d*\\))");
                    match = regex.Match(AltName ?? string.Empty);
                    if (!string.IsNullOrEmpty(match.Groups["text"].Value))
                    {
                        return match.Groups["text"].Value;
                    }
                    return AltName ?? string.Empty;
                }
            }
        }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public virtual string Markdown { get; set; }
    }
}
