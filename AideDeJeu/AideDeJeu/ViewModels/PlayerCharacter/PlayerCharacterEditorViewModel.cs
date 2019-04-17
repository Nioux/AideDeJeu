using AideDeJeu.Tools;
using AideDeJeuLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class PlayerCharacterEditorViewModel : BaseViewModel
    {
        public PlayerCharacterEditorViewModel()
        {
            ResetAlignments();
            Races = new NotifyTaskCompletion<List<RaceViewModel>>(Task.Run(() => LoadRacesAsync()));
            Classes = new NotifyTaskCompletion<List<ClassViewModel>>(Task.Run(() => LoadClassesAsync()));

            Backgrounds = new NotifyTaskCompletion<List<BackgroundItem>>(Task.Run(() => LoadBackgroundsAsync()));
            SelectedBackground = null;
            NotifySelectedBackground = new NotifyTaskCompletion<BackgroundItem>(null);
            SubBackgrounds = null;
            SelectedSubBackground = null;
            NotifySelectedSubBackground = new NotifyTaskCompletion<SubBackgroundItem>(null);
            PersonalityTraits = null;
            PersonalityIdeals = null;
            PersonalityLinks = null;
            PersonalityDefects = null;
            SelectedPersonalityTrait = null;
            SelectedPersonalityIdeal = null;
            SelectedPersonalityLink = null;
            SelectedPersonalityDefect = null;
            BackgroundSpecialties = null;
            SubBackgroundSpecialties = null;
            BackgroundSpecialty = null;
            BackgroundSkill = null;
            SubBackgroundSkill = null;
        }

        #region Selected PC
        private PlayerCharacterViewModel _SelectedPlayerCharacter = new PlayerCharacterViewModel();
        public PlayerCharacterViewModel SelectedPlayerCharacter
        {
            get
            {
                return _SelectedPlayerCharacter;
            }
            set
            {
                SetProperty(ref _SelectedPlayerCharacter, value);
            }
        }
        #endregion Selected PC

        #region Alignment
        private NotifyTaskCompletion<List<AlignmentItem>> _Alignments = null;
        public NotifyTaskCompletion<List<AlignmentItem>> Alignments
        {
            get
            {
                return _Alignments;
            }
            private set
            {
                SetProperty(ref _Alignments, value);
            }
        }

        private int _AlignmentSelectedIndex = -1;
        public int AlignmentSelectedIndex
        {
            get
            {
                return _AlignmentSelectedIndex;
            }
            set
            {
                SetProperty(ref _AlignmentSelectedIndex, value);
                if (0 <= _AlignmentSelectedIndex && _AlignmentSelectedIndex < Alignments.Result.Count)
                {
                    SelectedAlignment = Alignments.Result[_AlignmentSelectedIndex];
                }
                else
                {
                    SelectedAlignment = null;
                }
            }
        }
        private AlignmentItem _SelectedAlignment = null;
        public AlignmentItem SelectedAlignment
        {
            get
            {
                return _SelectedAlignment;
            }
            set
            {
                SetProperty(ref _SelectedAlignment, value);
                //if (0 <= _AlignmentSelectedIndex && _AlignmentSelectedIndex < Alignments.Result.Count)
                //{
                    SelectedPlayerCharacter.Alignment = SelectedAlignment;
                //}
            }
        }

        public async Task<List<AlignmentItem>> LoadAlignmentsAsync(string alignment = null)
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                if (alignment == null)
                {
                    return await context.Alignments.OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync().ConfigureAwait(false);
                }
                else
                {
                    return await context.Alignments.Where(a => a.Name.ToLower().Contains(alignment.ToLower())).OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync().ConfigureAwait(false);
                }
            }
        }

        private void ResetAlignments()
        {
            Alignments = new NotifyTaskCompletion<List<AlignmentItem>>(Task.Run(() => LoadAlignmentsAsync()));
            if (!string.IsNullOrEmpty(SelectedPlayerCharacter.PersonalityIdeal))
            {
                var regex = new Regex(".*\\((?<alignment>.*?)\\)$");
                var match = regex.Match(SelectedPlayerCharacter.PersonalityIdeal);
                var alignment = match.Groups["alignment"].Value;
                if (!string.IsNullOrEmpty(alignment) && alignment.ToLower() != "tous")
                {
                    Alignments = new NotifyTaskCompletion<List<AlignmentItem>>(Task.Run(() => LoadAlignmentsAsync(alignment)));
                    SelectedPlayerCharacter.Alignment = null;
                }
                else
                {
                    Alignments = new NotifyTaskCompletion<List<AlignmentItem>>(Task.Run(() => LoadAlignmentsAsync()));
                }
            }
            else
            {
                Alignments = new NotifyTaskCompletion<List<AlignmentItem>>(Task.Run(() => LoadAlignmentsAsync()));
            }
        }
        #endregion Alignment

        #region Race
        public NotifyTaskCompletion<List<RaceViewModel>> Races { get; private set; }
        private int _RaceSelectedIndex = -1;
        public int RaceSelectedIndex
        {
            get
            {
                return _RaceSelectedIndex;
            }
            set
            {
                SetProperty(ref _RaceSelectedIndex, value);
                if (Races.Result.Count > _RaceSelectedIndex && _RaceSelectedIndex >= 0)
                {
                    SelectedRace = Races.Result[_RaceSelectedIndex];
                }
            }
        }
        private RaceViewModel _SelectedRace = null;
        public RaceViewModel SelectedRace
        {
            get
            {
                return _SelectedRace;
            }
            set
            {
                SetProperty(ref _SelectedRace, value);
                //SelectedPlayerCharacter.Race = _SelectedRace;
            }
        }

        public class RaceViewModel : BaseViewModel
        {
            public RaceItem Race { get; set; }
            public SubRaceItem SubRace { get; set; }

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
                    if(SubRace?.AbilityScoreIncrease != null)
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
                    if(SubRace == null)
                    {
                        return Race.Attributes;
                    }
                    var dico = new OrderedDictionary();
                    foreach(DictionaryEntry attr in Race.Attributes)
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
        public async Task<List<RaceViewModel>> LoadRacesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                var expandedRaces = new List<RaceViewModel>();
                var races = context.Races.Where(r => r.GetType() == typeof(RaceItem));
                foreach(var race in races)
                {
                    if(race.HasSubRaces)
                    {
                        var subraces = context.SubRaces.Where(sr => sr.ParentLink == race.Id);
                        foreach(var subrace in subraces)
                        {
                            expandedRaces.Add(new RaceViewModel() { Race = race, SubRace = subrace });
                        }
                    }
                    else
                    {
                        expandedRaces.Add(new RaceViewModel() { Race = race, SubRace = null });
                    }
                }
                return expandedRaces.OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToList();
                //return await context.Races.Where(r => !r.HasSubRaces).OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync().ConfigureAwait(false);
            }
        }
        #endregion Race

        #region Class
        public NotifyTaskCompletion<List<ClassViewModel>> Classes { get; private set; }

        private int _ClassSelectedIndex = -1;
        public int ClassSelectedIndex
        {
            get
            {
                return _ClassSelectedIndex;
            }
            set
            {
                SetProperty(ref _ClassSelectedIndex, value);
                SelectedClass = Classes.Result[_ClassSelectedIndex];
            }
        }
        private ClassViewModel _SelectedClass = null;
        public ClassViewModel SelectedClass
        {
            get
            {
                return _SelectedClass;
            }
            set
            {
                SetProperty(ref _SelectedClass, value);
                //SelectedPlayerCharacter.Class = _SelectedClass;
            }
        }

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

        public async Task<List<ClassViewModel>> LoadClassesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Classes.Where(c => !(c is SubClassItem)).OrderBy(c => Tools.Helpers.RemoveDiacritics(c.Name)).Select(c => new ClassViewModel() { Class = c }).ToListAsync().ConfigureAwait(false);
            }
        }
        #endregion Class

        #region Background
        public NotifyTaskCompletion<List<BackgroundItem>> Backgrounds { get; private set; }

        private int _BackgroundSelectedIndex = -1;
        public int BackgroundSelectedIndex
        {
            get
            {
                return _BackgroundSelectedIndex;
            }
            set
            {
                SetProperty(ref _BackgroundSelectedIndex, value);
                SelectedBackground = Backgrounds.Result[_BackgroundSelectedIndex];
            }
        }

        private BackgroundItem _SelectedBackground = null;
        public BackgroundItem SelectedBackground
        {
            get
            {
                return _SelectedBackground;
            }
            set
            {
                SetProperty(ref _SelectedBackground, value);
                OnPropertyChanged(nameof(PreferedBackground));
                NotifySelectedBackground = new NotifyTaskCompletion<BackgroundItem>(Task.Run(() => LoadBackgroundAsync(_SelectedBackground)));
            }
        }
        private NotifyTaskCompletion<BackgroundItem> _NotifySelectedBackground = null;
        public NotifyTaskCompletion<BackgroundItem> NotifySelectedBackground
        {
            get
            {
                return _NotifySelectedBackground;
            }
            private set
            {
                SetProperty(ref _NotifySelectedBackground, value);
            }
        }

        private async Task<BackgroundItem> LoadBackgroundAsync(BackgroundItem background)
        {
            SelectedPlayerCharacter.SubBackground = null;
            SelectedPlayerCharacter.PersonalityTrait = null;
            SelectedPlayerCharacter.PersonalityIdeal = null;
            SelectedPlayerCharacter.PersonalityLink = null;
            SelectedPlayerCharacter.PersonalityDefect = null;
            SelectedPlayerCharacter.BackgroundSpecialty = null;
            SelectedPlayerCharacter.Background = background;

            if (background != null)
            {
                SubBackgrounds = await LoadSubBackgroundsAsync(background);
                SelectedSubBackground = null;
                NotifySelectedSubBackground = new NotifyTaskCompletion<SubBackgroundItem>(null);
                PersonalityTraits = await LoadPersonalityTraitsAsync(background);
                PersonalityIdeals = await LoadPersonalityIdealsAsync(background);
                PersonalityLinks = await LoadPersonalityLinksAsync(background);
                PersonalityDefects = await LoadPersonalityDefectsAsync(background);
                SelectedPersonalityTrait = null;
                SelectedPersonalityIdeal = null;
                SelectedPersonalityLink = null;
                SelectedPersonalityDefect = null;
                BackgroundSpecialties = await LoadBackgroundsSpecialtiesAsync(background);
                BackgroundSpecialty = null;
                SubBackgroundSpecialties = null;
                BackgroundSkill = await LoadSkillAsync(background);
                SubBackgroundSkill = null;
                ResetAlignments();
            }
            return background;
        }

        private List<SubBackgroundItem> _SubBackgrounds = null;
        public List<SubBackgroundItem> SubBackgrounds
        {
            get
            {
                return _SubBackgrounds;
            }
            set
            {
                SetProperty(ref _SubBackgrounds, value);
            }
        }

        private int _SubBackgroundSelectedIndex = -1;
        public int SubBackgroundSelectedIndex
        {
            get
            {
                return _SubBackgroundSelectedIndex;
            }
            set
            {
                SetProperty(ref _SubBackgroundSelectedIndex, value);
                if (_SubBackgroundSelectedIndex == 0)
                {
                    //SelectedPlayerCharacter.SubBackground = null;
                    SubBackgroundSelectedIndex = -1;
                    SelectedSubBackground = null;
                }
                else if (_SubBackgroundSelectedIndex > 0)
                {
                    SelectedSubBackground = SubBackgrounds[_SubBackgroundSelectedIndex];
                }
            }
        }

        private SubBackgroundItem _SelectedSubBackground = null;
        public SubBackgroundItem SelectedSubBackground
        {
            get
            {
                return _SelectedSubBackground;
            }
            set
            {
                SetProperty(ref _SelectedSubBackground, value);
                OnPropertyChanged(nameof(PreferedBackground));
                NotifySelectedSubBackground = new NotifyTaskCompletion<SubBackgroundItem>(Task.Run(() => LoadSubBackgroundAsync(SelectedSubBackground)));
            }
        }
        private NotifyTaskCompletion<SubBackgroundItem> _NotifySelectedSubBackground = null;
        public NotifyTaskCompletion<SubBackgroundItem> NotifySelectedSubBackground
        {
            get
            {
                return _NotifySelectedSubBackground;
            }
            private set
            {
                SetProperty(ref _NotifySelectedSubBackground, value);
            }
        }

        private async Task<SubBackgroundItem> LoadSubBackgroundAsync(SubBackgroundItem subbackground)
        {
            SelectedPlayerCharacter.SubBackground = subbackground;
            if (subbackground == null)
            {
                SubBackgroundSpecialties = null;
                SubBackgroundSkill = null;
                SubBackgroundSpecialty = null;
            }
            else
            {
                SubBackgroundSpecialties = await LoadBackgroundsSpecialtiesAsync(subbackground);
                SubBackgroundSkill = await LoadSkillAsync(subbackground);
            }
            return subbackground;
        }

        public BackgroundItem PreferedBackground
        {
            get
            {
                return SelectedSubBackground ?? SelectedBackground;
            }
        }

        private List<string> _PersonalityTraits = null;
        public List<string> PersonalityTraits
        {
            get
            {
                return _PersonalityTraits;
            }
            set
            {
                SetProperty(ref _PersonalityTraits, value);
            }
        }
        private string _SelectedPersonalityTrait = null;
        public string SelectedPersonalityTrait
        {
            get
            {
                return _SelectedPersonalityTrait;
            }
            set
            {
                SetProperty(ref _SelectedPersonalityTrait, value);
                SelectedPlayerCharacter.PersonalityTrait = value;
            }
        }

        private List<string> _PersonalityIdeals = null;
        public List<string> PersonalityIdeals
        {
            get
            {
                return _PersonalityIdeals;
            }
            set
            {
                SetProperty(ref _PersonalityIdeals, value);
            }
        }
        private string _SelectedPersonalityIdeal = null;
        public string SelectedPersonalityIdeal
        {
            get
            {
                return _SelectedPersonalityIdeal;
            }
            set
            {
                SetProperty(ref _SelectedPersonalityIdeal, value);
                SelectedPlayerCharacter.PersonalityIdeal = value;
            }
        }

        private List<string> _PersonalityLinks = null;
        public List<string> PersonalityLinks
        {
            get
            {
                return _PersonalityLinks;
            }
            set
            {
                SetProperty(ref _PersonalityLinks, value);
            }
        }
        private string _SelectedPersonalityLink = null;
        public string SelectedPersonalityLink
        {
            get
            {
                return _SelectedPersonalityLink;
            }
            set
            {
                SetProperty(ref _SelectedPersonalityLink, value);
                SelectedPlayerCharacter.PersonalityLink = value;
            }
        }

        private List<string> _PersonalityDefects = null;
        public List<string> PersonalityDefects
        {
            get
            {
                return _PersonalityDefects;
            }
            set
            {
                SetProperty(ref _PersonalityDefects, value);
            }
        }
        private string _SelectedPersonalityDefect = null;
        public string SelectedPersonalityDefect
        {
            get
            {
                return _SelectedPersonalityDefect;
            }
            set
            {
                SetProperty(ref _SelectedPersonalityDefect, value);
                SelectedPlayerCharacter.PersonalityDefect = value;
            }
        }

        private BackgroundSpecialtyItem _BackgroundSpecialties = null;
        public BackgroundSpecialtyItem BackgroundSpecialties
        {
            get
            {
                return _BackgroundSpecialties;
            }
            set
            {
                SetProperty(ref _BackgroundSpecialties, value);
                OnPropertyChanged(nameof(PreferedBackgroundSpecialties));
                OnPropertyChanged(nameof(HasBackgroundSpecialties));
            }
        }
        private BackgroundSpecialtyItem _SubBackgroundSpecialties = null;
        public BackgroundSpecialtyItem SubBackgroundSpecialties
        {
            get
            {
                return _SubBackgroundSpecialties;
            }
            set
            {
                SetProperty(ref _SubBackgroundSpecialties, value);
                OnPropertyChanged(nameof(PreferedBackgroundSpecialties));
                OnPropertyChanged(nameof(HasBackgroundSpecialties));
            }
        }
        public BackgroundSpecialtyItem PreferedBackgroundSpecialties
        {
            get
            {
                return _SubBackgroundSpecialties ?? _BackgroundSpecialties;
            }
        }
        public bool HasBackgroundSpecialties
        {
            get
            {
                return PreferedBackgroundSpecialties != null;
            }
        }

        private string _BackgroundSpecialty = null;
        public string BackgroundSpecialty
        {
            get
            {
                return _BackgroundSpecialty;
            }
            set
            {
                SetProperty(ref _BackgroundSpecialty, value);
                SelectedPlayerCharacter.BackgroundSpecialty = BackgroundSpecialty;
            }
        }
        private string _SubBackgroundSpecialty = null;
        public string SubBackgroundSpecialty
        {
            get
            {
                return _SubBackgroundSpecialty;
            }
            set
            {
                SetProperty(ref _SubBackgroundSpecialty, value);
                SelectedPlayerCharacter.SubBackgroundSpecialty = SubBackgroundSpecialty;
            }
        }

        private SkillItem _BackgroundSkill = null;
        public SkillItem BackgroundSkill
        {
            get
            {
                return _BackgroundSkill;
            }
            set
            {
                SetProperty(ref _BackgroundSkill, value);
                OnPropertyChanged(nameof(PreferedBackgroundSkill));
                OnPropertyChanged(nameof(HasBackgroundSkill));
            }
        }
        private SkillItem _SubBackgroundSkill = null;
        public SkillItem SubBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill;
            }
            set
            {
                SetProperty(ref _SubBackgroundSkill, value);
                OnPropertyChanged(nameof(PreferedBackgroundSkill));
                OnPropertyChanged(nameof(HasBackgroundSkill));
            }
        }
        public SkillItem PreferedBackgroundSkill
        {
            get
            {
                return _SubBackgroundSkill ?? _BackgroundSkill;
            }
        }
        public bool HasBackgroundSkill
        {
            get
            {
                return PreferedBackgroundSkill != null;
            }
        }
        public async Task<List<BackgroundItem>> LoadBackgroundsAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                var list = await context.Backgrounds.Where(b => b.GetType() == typeof(BackgroundItem)).OrderBy(b => Tools.Helpers.RemoveDiacritics(b.Name)).ToListAsync().ConfigureAwait(false);
                return list;
            }
        }

        public List<string> ExtractSimpleTable(string table)
        {
            var lines = table.Split('\n');
            var result = new List<string>();
            foreach (var line in lines.Skip(2))
            {
                if (line.StartsWith("|"))
                {
                    var cols = line.Split('|');
                    var text = cols[2].Replace("<!--br-->", " ").Replace("  ", " ");
                    result.Add(text);
                }
            }
            return result;
        }
        public async Task<List<string>> LoadPersonalityTraitsAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.PersonalityTraits.Where(it => it.ParentLink.StartsWith(background.RootId)).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return ExtractSimpleTable(item.Table);
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<List<string>> LoadPersonalityIdealsAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.PersonalityIdeals.Where(it => it.ParentLink.StartsWith(background.RootId)).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return ExtractSimpleTable(item.Table);
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<List<string>> LoadPersonalityLinksAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.PersonalityLinks.Where(it => it.ParentLink.StartsWith(background.RootId)).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return ExtractSimpleTable(item.Table);
                }
            }
            else
            {
                return null;
            }
        }
        public async Task<List<string>> LoadPersonalityDefectsAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.PersonalityDefects.Where(it => it.ParentLink.StartsWith(background.RootId)).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return ExtractSimpleTable(item.Table);
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<BackgroundSpecialtyItem> LoadBackgroundsSpecialtiesAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.BackgroundSpecialties.Where(it => it.ParentLink == background.Id).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return item;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<SkillItem> LoadSkillAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.Skills.Where(it => it.ParentLink == background.Id).ToListAsync().ConfigureAwait(false);
                    var item = list.FirstOrDefault();
                    return item;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<List<SubBackgroundItem>> LoadSubBackgroundsAsync(BackgroundItem background)
        {
            if (background != null)
            {
                using (var context = await StoreViewModel.GetLibraryContextAsync())
                {
                    var list = await context.SubBackgrounds.Where(item => item.ParentLink == background.Id).OrderBy(b => Tools.Helpers.RemoveDiacritics(b.Name)).ToListAsync().ConfigureAwait(false);
                    list.Insert(0, new SubBackgroundItem() { Name = "-" });
                    return list;
                }
            }
            else
            {
                return new List<SubBackgroundItem>();
            }
        }
        /*
        public ICommand BackgroundSpecialtyPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPlayerCharacter.BackgroundSpecialty = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        public ICommand SubBackgroundSpecialtyPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPlayerCharacter.SubBackgroundSpecialty = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        public ICommand PersonalityTraitPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPersonalityTrait = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        public ICommand PersonalityIdealPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) =>
                    {
                        SelectedPlayerCharacter.PersonalityIdeal = await ExecuteStringPickerCommandAsync(strings);
                        ResetAlignments();
                    }
                );
            }
        }
        public ICommand PersonalityLinkPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPlayerCharacter.PersonalityLink = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        public ICommand PersonalityDefectPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPlayerCharacter.PersonalityDefect = await ExecuteStringPickerCommandAsync(strings));
            }
        }
        */
        /*
        private async Task<string> ExecuteStringPickerCommandAsync(List<string> strings)
        {
            var picker = new Views.StringPicker();
            var vm = picker.ViewModel;
            vm.Items = strings;
            await Main.Navigator.Navigation.PushModalAsync(picker, true);
            var result = await vm.PickValueAsync();
            await Main.Navigator.Navigation.PopModalAsync(true);
            return result;
        }
        */
        #endregion Background

        #region Abilities
        public List<string> Abilities { get; set; } = new List<string>()
        {
            "2 (-4)", "3 (-4)", "4 (-3)", "5 (-3)", "6 (-2)", "7 (-2)", "8 (-1)", "9 (-1)", "10 (+0)", "11 (+0)", "12 (+1)", "13 (+1)", "14 (+2)", "15 (+2)", "16 (+3)", "17 (+3)", "18 (+4)", "19 (+4)", "20 (+5)", "21 (+5)"
        };
        #endregion Abilities

        #region Level
        public List<string> Levels { get; set; } = new List<string>()
        {
            "1", //"2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
        };
        #endregion Level

    }
}
