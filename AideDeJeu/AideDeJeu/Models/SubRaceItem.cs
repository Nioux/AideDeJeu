using System.Runtime.Serialization;

namespace AideDeJeuLib
{
    public class SubRaceItem : RaceItem
    {
        [DataMember]
        public string ParentRaceId { get; set; }
    }
}
