using System.Runtime.Serialization;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class SubClassItem : ClassItem
    {
        [YamlIgnore]
        [DataMember]
        public string ParentClassId { get; set; }
    }
}
