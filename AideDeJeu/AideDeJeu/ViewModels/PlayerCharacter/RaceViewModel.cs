using AideDeJeuLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class RaceViewModel : BaseViewModel
    {
        private RaceItem _Race = null;
        public RaceItem Race { get { return _Race; } set { SetProperty(ref _Race, value); } }

        private SubRaceItem _SubRace = null;
        public SubRaceItem SubRace { get { return _SubRace; } set { SetProperty(ref _SubRace, value); } }

        private RaceItem RaceOrSubRace { get { return SubRace ?? Race; } }
        public string Name { get { return RaceOrSubRace.Name; } }
        public string Description { get { return RaceOrSubRace.Description; } }
        public string NewId { get { return RaceOrSubRace.NewId; } }
        public string Id { get { return RaceOrSubRace.Id; } }
        public string RootId { get { return RaceOrSubRace.RootId; } }

        public string AbilityScoreIncrease
        {
            get
            {
                if (SubRace?.AbilityScoreIncrease != null)
                {
                    return Race.AbilityScoreIncrease + "\n\n" + SubRace.AbilityScoreIncrease;
                }
                return Race.AbilityScoreIncrease;
            }
        }
        public OrderedDictionary Attributes
        {
            get
            {
                if (SubRace == null)
                {
                    return Race.Attributes;
                }
                var dico = new OrderedDictionary();
                foreach (DictionaryEntry attr in Race.Attributes)
                {
                    dico[attr.Key] = attr.Value;
                }
                foreach (DictionaryEntry attr in SubRace.Attributes)
                {
                    dico[attr.Key] = attr.Value;
                }
                return dico;
            }
        }
        public virtual OrderedDictionary AttributesKeyValue
        {
            get
            {
                return AideDeJeuLib.ItemAttribute.ExtractKeyValues(Attributes);
            }
        }

        public string Age { get { return Race.Age; } }
        public string Alignment { get { return Race.Alignment; } }
        public string Size { get { return Race.Size; } }
        public string Speed { get { return Race.Speed; } }
        public string Darkvision { get { return Race.Darkvision; } }
        public string Languages { get { return Race.Languages; } }
    }

}
