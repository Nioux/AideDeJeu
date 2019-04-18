using AideDeJeuLib;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class ClassViewModel : BaseViewModel
    {
        private ClassItem _Class = null;
        public ClassItem Class { get { return _Class; } set { SetProperty(ref _Class, value); } }

        private SubClassItem _SubClass = null;
        public SubClassItem SubClass { get { return _SubClass; } set { SetProperty(ref _SubClass, value); } }

        private ClassHitPointsItem _HitPoints = null;
        public ClassHitPointsItem HitPoints { get { return _HitPoints; } set { SetProperty(ref _HitPoints, value); } }

        public ClassProficienciesItem _Proficiencies = null;
        public ClassProficienciesItem Proficiencies { get { return _Proficiencies; } set { SetProperty(ref _Proficiencies, value); } }

        public ClassEquipmentItem _Equipment = null;
        public ClassEquipmentItem Equipment { get { return _Equipment; } set { SetProperty(ref _Equipment, value); } }

        public ClassEvolutionItem _Evolution = null;
        public ClassEvolutionItem Evolution { get { return _Evolution; } set { SetProperty(ref _Evolution, value); } }

        public List<ClassFeatureItem> _Features = null;
        public List<ClassFeatureItem> Features { get { return _Features; } set { SetProperty(ref _Features, value); } }

        public string Name { get { return Class?.Name; } }
        public string Description { get { return Class?.Description; } }
        public string Markdown { get { return Class?.Markdown; } }

        public List<ClassFeatureItem> LeveledFeatures
        {
            get
            {
                var table = Evolution.ExtractSimpleTable(Evolution.Table);
                var feats = table[1];
                var leveledFeats = new List<ClassFeatureItem>();
                foreach(var feature in Features)
                {
                    if(feats.Contains(feature.Name))
                    {
                        leveledFeats.Add(feature);
                    }
                }
                return leveledFeats;
            }
        }

        public async Task LoadDetailsAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                HitPoints = await context.ClassHitPoints.Where(c => c.ParentLink == Class.Id).FirstOrDefaultAsync();
                Proficiencies = await context.ClassProficiencies.Where(c => c.ParentLink == Class.Id).FirstOrDefaultAsync();
                Equipment = await context.ClassEquipments.Where(c => c.ParentLink == Class.Id).FirstOrDefaultAsync();
                Evolution = await context.ClassEvolutions.Where(c => c.ParentLink == Class.Id).FirstOrDefaultAsync();
                Features = await context.ClassFeatures.Where(c => c.ParentLink == Class.Id).ToListAsync();
            }
        }
    }
}
