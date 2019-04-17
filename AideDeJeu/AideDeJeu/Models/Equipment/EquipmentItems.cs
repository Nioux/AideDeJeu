using AideDeJeu.ViewModels;
using AideDeJeu.ViewModels.Library;

namespace AideDeJeuLib
{
    public class EquipmentItems : FilteredItems
    {
        public string Types { get; set; }
        public string Prices { get; set; }

        public override FilterViewModel GetNewFilterViewModel()
        {
            return new EquipmentFilterViewModel(Family,
                Split(Types),
                Split(Prices)
                );
        }
    }
}
