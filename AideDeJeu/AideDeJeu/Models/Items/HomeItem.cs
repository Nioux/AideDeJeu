using System.Runtime.Serialization;

namespace AideDeJeuLib
{
    [DataContract]
    public class HomeItem : Item
    {
        [DataMember]
        public override string Markdown
        {
            get
            {
                return AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.index.md");
            }
        }
    }
}
