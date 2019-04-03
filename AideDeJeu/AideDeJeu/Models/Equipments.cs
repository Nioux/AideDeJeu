using AideDeJeu.ViewModels;

namespace AideDeJeuLib
{
    public class Equipments : FilteredItems
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
