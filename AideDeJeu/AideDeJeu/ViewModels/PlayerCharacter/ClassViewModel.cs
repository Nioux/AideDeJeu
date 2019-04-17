using AideDeJeuLib;
using System.Collections.Generic;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class ClassViewModel : BaseViewModel
    {
        public ClassItem Class { get; set; }
        public SubClassItem SubClass { get; set; }
        public ClassHitPointsItem HitPoints { get; set; }
        public ClassProficienciesItem Proficiencies { get; set; }
        public ClassEquipmentItem Equipment { get; set; }
        public ClassEvolutionItem Evolution { get; set; }
        public List<ClassFeatureItem> Features { get; set; }

        public string Name { get { return Class?.Name; } }
        public string Description { get { return Class?.Description; } }
        public string Markdown { get { return Class?.Markdown; } }
    }
}
