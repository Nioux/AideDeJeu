using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;

namespace AideDeJeuLib
{
    public class MagicItems : FilteredItems
    {
        public string Types { get; set; }
        public string Rarities { get; set; }
        public string Attunements { get; set; }

        public override FilterViewModel GetNewFilterViewModel()
        {
            return new MagicItemFilterViewModel(Family,
                Split(Types),
                Split(Rarities),
                Split(Attunements)
            );
        }
    }
}
