using System.Runtime.Serialization;

namespace AideDeJeuLib
{
    public class SubClassItem : ClassItem
    {
        [DataMember]
        public string ParentClassId { get; set; }
    }
}
