using AideDeJeu.Tools;
using AideDeJeuLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels.PlayerCharacter
{
    public class PlayerCharacterEditorViewModel : BaseViewModel
    {
        private Random _Random;

        public ICommand ResetPlayerCharacterCommand
        {
            get
            {
                return new Command(async () => await ExecuteResetPlayerCharacterCommandAsync());
            }
        }

        private async Task ExecuteResetPlayerCharacterCommandAsync()
        {
            _Random = new Random(DateTime.Now.Millisecond);

            SelectedPlayerCharacter = new PlayerCharacterViewModel() { Background = new BackgroundViewModel(), Abilities = new AbilitiesViewModel() };
            SelectedPlayerCharacter.PropertyChanged += SelectedPlayerCharacter_PropertyChanged;
            SelectedPlayerCharacter.Background.PropertyChanged += Background_PropertyChanged;


            // raz des listes de choix
            await ResetAlignments();
            Races = await Task.Run(async () => await LoadRacesAsync());
            Classes = await Task.Run(async () => await LoadClassesAsync());
            Backgrounds = await Task.Run(async () => await LoadBackgroundsAsync());
            SubBackgrounds = null;
            PersonalityTraits = null;
            PersonalityIdeals = null;
            PersonalityLinks = null;
            PersonalityDefects = null;
            BackgroundSpecialties = null;
            SubBackgroundSpecialties = null;
        }

        private async void Background_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(SelectedPlayerCharacter.Background.Background):
                    await LoadBackgroundAsync(SelectedPlayerCharacter.Background.Background);
                    //SubBackgrounds = await LoadSubBackgroundsAsync(SelectedPlayerCharacter.Background.Background);
                    break;
                case nameof(SelectedPlayerCharacter.Background.SubBackground):
                    await LoadSubBackgroundAsync(SelectedPlayerCharacter.Background.SubBackground);
                    //SubBackgrounds = await LoadSubBackgroundsAsync(SelectedPlayerCharacter.Background.Background);
                    break;
            }
        }

        public PlayerCharacterEditorViewModel()
        {
            ExecuteResetPlayerCharacterCommandAsync();
        }

        private async void SelectedPlayerCharacter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedPlayerCharacter.Race):
                    SelectedPlayerCharacter.Abilities.Unlisten();

                    SelectedPlayerCharacter.Abilities.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.DispatchedBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Strength.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedStrengthBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Dexterity.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedDexterityBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Constitution.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedConstitutionBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Intelligence.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedIntelligenceBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Wisdom.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedWisdomBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Charisma.MaxRacialDispatchedBonus = int.Parse(SelectedPlayerCharacter.Race.Race.MaxDispatchedCharismaBonus ?? "0");

                    SelectedPlayerCharacter.Abilities.Strength.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.StrengthBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.StrengthBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Dexterity.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.DexterityBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.DexterityBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Constitution.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.ConstitutionBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.ConstitutionBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Intelligence.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.IntelligenceBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.IntelligenceBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Wisdom.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.WisdomBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.WisdomBonus ?? "0");
                    SelectedPlayerCharacter.Abilities.Charisma.RacialBonus =
                        int.Parse(SelectedPlayerCharacter.Race.Race?.CharismaBonus ?? "0") +
                        int.Parse(SelectedPlayerCharacter.Race.SubRace?.CharismaBonus ?? "0");

                    SelectedPlayerCharacter.Abilities.Listen();
                    break;
            }
        }

        #region Selected PC
        private PlayerCharacterViewModel _SelectedPlayerCharacter = null;
        public PlayerCharacterViewModel SelectedPlayerCharacter
        {
            get
            {
                return _SelectedPlayerCharacter;
            }
            set
            {
                if (_SelectedPlayerCharacter != null)
                {
                    _SelectedPlayerCharacter.PropertyChanged -= _SelectedPlayerCharacter_PropertyChanged;
                }
                SetProperty(ref _SelectedPlayerCharacter, value);
                if (_SelectedPlayerCharacter != null)
                {
                    _SelectedPlayerCharacter.PropertyChanged += _SelectedPlayerCharacter_PropertyChanged;
                }
            }
        }

        private void _SelectedPlayerCharacter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Race")
            {
                OnSelectedPlayerCharacterRaceChanged();
            }
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Class")
            {
                OnSelectedPlayerCharacterClassChanged();
            }
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Background")
            {
                OnSelectedPlayerCharacterBackgroundChnaged();
            }
            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Alignment")
            {
                OnSelectedPlayerCharacterAlignmentChanged();
            }
        }

        private void OnSelectedPlayerCharacterAlignmentChanged()
        {

        }

        private void OnSelectedPlayerCharacterBackgroundChnaged()
        {

        }

        private void OnSelectedPlayerCharacterClassChanged()
        {

        }

        private void OnSelectedPlayerCharacterRaceChanged()
        {

        }
        #endregion Selected PC

        #region Alignment
        private List<AlignmentItem> _Alignments = null;
        public List<AlignmentItem> Alignments
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

        private async Task ResetAlignments()
        {
            Alignments = await LoadAlignmentsAsync();
            if (!string.IsNullOrEmpty(SelectedPlayerCharacter.Background.PersonalityIdeal))
            {
                var regex = new Regex(".*\\((?<alignment>.*?)\\)$");
                var match = regex.Match(SelectedPlayerCharacter.Background.PersonalityIdeal);
                var alignment = match.Groups["alignment"].Value;
                if (!string.IsNullOrEmpty(alignment) && alignment.ToLower() != "tous")
                {
                    Alignments = await LoadAlignmentsAsync(alignment);
                    SelectedPlayerCharacter.Alignment = null;
                }
                else
                {
                    Alignments = await LoadAlignmentsAsync();
                }
            }
            else
            {
                Alignments = await LoadAlignmentsAsync();
            }
        }
        #endregion Alignment

        #region Race

        private List<RaceViewModel> _Races = null;
        public List<RaceViewModel> Races { get { return _Races; } private set { SetProperty(ref _Races, value); } }

        public async Task<List<RaceViewModel>> LoadRacesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                var expandedRaces = new List<RaceViewModel>();
                var races = context.Races.Where(r => r.GetType() == typeof(RaceItem));
                foreach (var race in races)
                {
                    if (race.HasSubRaces)
                    {
                        var subraces = context.SubRaces.Where(sr => sr.ParentLink == race.Id);
                        foreach (var subrace in subraces)
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
        private List<ClassViewModel> _Classes = null;
        public List<ClassViewModel> Classes { get { return _Classes; } private set { SetProperty(ref _Classes, value); } }

        public async Task<List<ClassViewModel>> LoadClassesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Classes.Where(c => !(c is SubClassItem)).OrderBy(c => Tools.Helpers.RemoveDiacritics(c.Name)).Select(c => new ClassViewModel() { Class = c }).ToListAsync().ConfigureAwait(false);
            }
        }
        #endregion Class

        #region Background
        private List<BackgroundItem> _Backgrounds = null;
        public List<BackgroundItem> Backgrounds { get { return _Backgrounds; } private set { SetProperty(ref _Backgrounds, value); } }

        //private int _BackgroundSelectedIndex = -1;
        //public int BackgroundSelectedIndex
        //{
        //    get
        //    {
        //        return _BackgroundSelectedIndex;
        //    }
        //    set
        //    {
        //        SetProperty(ref _BackgroundSelectedIndex, value);
        //        SelectedBackground = Backgrounds.Result[_BackgroundSelectedIndex];
        //    }
        //}

        //private BackgroundItem _SelectedBackground = null;
        //public BackgroundItem SelectedBackground
        //{
        //    get
        //    {
        //        return _SelectedBackground;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedBackground, value);
        //        OnPropertyChanged(nameof(BackgroundOrSubBackground));
        //        SelectedPlayerCharacter.Background.Background = value;
        //        //NotifySelectedBackground = new NotifyTaskCompletion<BackgroundItem>(Task.Run(() => LoadBackgroundAsync(_SelectedBackground)));
        //    }
        //}
        //private NotifyTaskCompletion<BackgroundItem> _NotifySelectedBackground = null;
        //public NotifyTaskCompletion<BackgroundItem> NotifySelectedBackground
        //{
        //    get
        //    {
        //        return _NotifySelectedBackground;
        //    }
        //    private set
        //    {
        //        SetProperty(ref _NotifySelectedBackground, value);
        //    }
        //}

        private async Task<BackgroundItem> LoadBackgroundAsync(BackgroundItem background)
        {
            SelectedPlayerCharacter.Background.SubBackground = null;
            SelectedPlayerCharacter.Background.PersonalityTrait = null;
            SelectedPlayerCharacter.Background.PersonalityIdeal = null;
            SelectedPlayerCharacter.Background.PersonalityLink = null;
            SelectedPlayerCharacter.Background.PersonalityDefect = null;
            SelectedPlayerCharacter.Background.BackgroundSpecialty = null;
            SelectedPlayerCharacter.Background.Background = background;

            if (background != null)
            {
                SubBackgrounds = await LoadSubBackgroundsAsync(background);
                //SelectedSubBackground = null;
                //NotifySelectedSubBackground = new NotifyTaskCompletion<SubBackgroundItem>(null);
                PersonalityTraits = await LoadPersonalityTraitsAsync(background);
                PersonalityIdeals = await LoadPersonalityIdealsAsync(background);
                PersonalityLinks = await LoadPersonalityLinksAsync(background);
                PersonalityDefects = await LoadPersonalityDefectsAsync(background);
                //SelectedPersonalityTrait = null;
                //SelectedPersonalityIdeal = null;
                //SelectedPersonalityLink = null;
                //SelectedPersonalityDefect = null;
                BackgroundSpecialties = await LoadBackgroundsSpecialtiesAsync(background);
                //BackgroundSpecialty = null;
                //SubBackgroundSpecialties = null;
                SelectedPlayerCharacter.Background.BackgroundSkill = await LoadSkillAsync(background);
                //SubBackgroundSkill = null;
                await ResetAlignments();
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

        //private int _SubBackgroundSelectedIndex = -1;
        //public int SubBackgroundSelectedIndex
        //{
        //    get
        //    {
        //        return _SubBackgroundSelectedIndex;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SubBackgroundSelectedIndex, value);
        //        if (_SubBackgroundSelectedIndex == 0)
        //        {
        //            //SelectedPlayerCharacter.SubBackground = null;
        //            SubBackgroundSelectedIndex = -1;
        //            SelectedSubBackground = null;
        //        }
        //        else if (_SubBackgroundSelectedIndex > 0)
        //        {
        //            SelectedSubBackground = SubBackgrounds[_SubBackgroundSelectedIndex];
        //        }
        //    }
        //}

        //private SubBackgroundItem _SelectedSubBackground = null;
        //public SubBackgroundItem SelectedSubBackground
        //{
        //    get
        //    {
        //        return _SelectedSubBackground;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedSubBackground, value);
        //        OnPropertyChanged(nameof(BackgroundOrSubBackground));
        //        //NotifySelectedSubBackground = new NotifyTaskCompletion<SubBackgroundItem>(Task.Run(() => LoadSubBackgroundAsync(SelectedSubBackground)));
        //    }
        //}
        //private NotifyTaskCompletion<SubBackgroundItem> _NotifySelectedSubBackground = null;
        //public NotifyTaskCompletion<SubBackgroundItem> NotifySelectedSubBackground
        //{
        //    get
        //    {
        //        return _NotifySelectedSubBackground;
        //    }
        //    private set
        //    {
        //        SetProperty(ref _NotifySelectedSubBackground, value);
        //    }
        //}

        private async Task<SubBackgroundItem> LoadSubBackgroundAsync(SubBackgroundItem subbackground)
        {
            SelectedPlayerCharacter.Background.SubBackground = subbackground;
            if (subbackground == null)
            {
                SubBackgroundSpecialties = null;
                SelectedPlayerCharacter.Background.SubBackgroundSkill = null;
                //SubBackgroundSpecialty = null;
            }
            else
            {
                SubBackgroundSpecialties = await LoadBackgroundsSpecialtiesAsync(subbackground);
                SelectedPlayerCharacter.Background.SubBackgroundSkill = await LoadSkillAsync(subbackground);
            }
            return subbackground;
        }

        //public BackgroundItem BackgroundOrSubBackground
        //{
        //    get
        //    {
        //        return SelectedSubBackground ?? SelectedBackground;
        //    }
        //}

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
        //private string _SelectedPersonalityTrait = null;
        //public string SelectedPersonalityTrait
        //{
        //    get
        //    {
        //        return _SelectedPersonalityTrait;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedPersonalityTrait, value);
        //        SelectedPlayerCharacter.Background.PersonalityTrait = value;
        //    }
        //}

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
        //private string _SelectedPersonalityIdeal = null;
        //public string SelectedPersonalityIdeal
        //{
        //    get
        //    {
        //        return _SelectedPersonalityIdeal;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedPersonalityIdeal, value);
        //        SelectedPlayerCharacter.Background.PersonalityIdeal = value;
        //    }
        //}

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
        //private string _SelectedPersonalityLink = null;
        //public string SelectedPersonalityLink
        //{
        //    get
        //    {
        //        return _SelectedPersonalityLink;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedPersonalityLink, value);
        //        SelectedPlayerCharacter.Background.PersonalityLink = value;
        //    }
        //}

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
        //private string _SelectedPersonalityDefect = null;
        //public string SelectedPersonalityDefect
        //{
        //    get
        //    {
        //        return _SelectedPersonalityDefect;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SelectedPersonalityDefect, value);
        //        SelectedPlayerCharacter.Background.PersonalityDefect = value;
        //    }
        //}

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
                OnPropertyChanged(nameof(BackgroundOrSubBackgroundSpecialties));
                //OnPropertyChanged(nameof(HasBackgroundSpecialties));
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
                OnPropertyChanged(nameof(BackgroundOrSubBackgroundSpecialties));
                //OnPropertyChanged(nameof(HasBackgroundSpecialties));
            }
        }
        public BackgroundSpecialtyItem BackgroundOrSubBackgroundSpecialties
        {
            get
            {
                return _SubBackgroundSpecialties ?? _BackgroundSpecialties;
            }
        }
        //public bool HasBackgroundSpecialties
        //{
        //    get
        //    {
        //        return PreferedBackgroundSpecialties != null;
        //    }
        //}

        //private string _BackgroundSpecialty = null;
        //public string BackgroundSpecialty
        //{
        //    get
        //    {
        //        return _BackgroundSpecialty;
        //    }
        //    set
        //    {
        //        SetProperty(ref _BackgroundSpecialty, value);
        //        SelectedPlayerCharacter.Background.BackgroundSpecialty = BackgroundSpecialty;
        //    }
        //}
        //private string _SubBackgroundSpecialty = null;
        //public string SubBackgroundSpecialty
        //{
        //    get
        //    {
        //        return _SubBackgroundSpecialty;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SubBackgroundSpecialty, value);
        //        SelectedPlayerCharacter.Background.SubBackgroundSpecialty = SubBackgroundSpecialty;
        //    }
        //}

        //private SkillItem _BackgroundSkill = null;
        //public SkillItem BackgroundSkill
        //{
        //    get
        //    {
        //        return _BackgroundSkill;
        //    }
        //    set
        //    {
        //        SetProperty(ref _BackgroundSkill, value);
        //        OnPropertyChanged(nameof(BackgroundOrSubBackgroundSkill));
        //        //OnPropertyChanged(nameof(HasBackgroundSkill));
        //    }
        //}
        //private SkillItem _SubBackgroundSkill = null;
        //public SkillItem SubBackgroundSkill
        //{
        //    get
        //    {
        //        return _SubBackgroundSkill;
        //    }
        //    set
        //    {
        //        SetProperty(ref _SubBackgroundSkill, value);
        //        OnPropertyChanged(nameof(BackgroundOrSubBackgroundSkill));
        //        //OnPropertyChanged(nameof(HasBackgroundSkill));
        //    }
        //}
        //public SkillItem BackgroundOrSubBackgroundSkill
        //{
        //    get
        //    {
        //        return _SubBackgroundSkill ?? _BackgroundSkill;
        //    }
        //}
        //public bool HasBackgroundSkill
        //{
        //    get
        //    {
        //        return PreferedBackgroundSkill != null;
        //    }
        //}
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
                    var list = await context.Features.Where(it => it.ParentLink == background.Id).ToListAsync().ConfigureAwait(false);
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

        /*        private int? _Strength = null;
                public int? Strength
                {
                    get
                    {
                        return _Strength;
                    }
                    set
                    {
                        SetProperty(ref _Strength, value);
                    }
                }

                private int? _Dexterity = null;
                public int? Dexterity
                {
                    get
                    {
                        return _Dexterity;
                    }
                    set
                    {
                        SetProperty(ref _Dexterity, value);
                    }
                }

                private int? _Constitution = null;
                public int? Constitution
                {
                    get
                    {
                        return _Constitution;
                    }
                    set
                    {
                        SetProperty(ref _Constitution, value);
                    }
                }

                private int? _Intelligence = null;
                public int? Intelligence
                {
                    get
                    {
                        return _Intelligence;
                    }
                    set
                    {
                        SetProperty(ref _Intelligence, value);
                    }
                }

                private int? _Wisdom = null;
                public int? Wisdom
                {
                    get
                    {
                        return _Wisdom;
                    }
                    set
                    {
                        SetProperty(ref _Wisdom, value);
                    }
                }

                private int? _Charisma = null;
                public int? Charisma
                {
                    get
                    {
                        return _Charisma;
                    }
                    set
                    {
                        SetProperty(ref _Charisma, value);
                    }
                }
                */
        public ICommand RollDicesMRickCommand
        {
            get
            {
                return new Command(() => PrefillDices(RollMRick()));
            }
        }
        public ICommand RollDices2d6plus6Command
        {
            get
            {
                return new Command(() => PrefillDices(Roll6x2d6plus6()));
            }
        }
        public ICommand ResetDicesCommand
        {
            get
            {
                return new Command(() => ExecuteResetDicesCommand());
            }
        }
        private void ExecuteResetDicesCommand()
        {
            SelectedPlayerCharacter.Abilities.Unlisten();
            SelectedPlayerCharacter.Abilities.Strength.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Strength.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Dexterity.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Dexterity.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Constitution.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Constitution.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Intelligence.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Intelligence.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Wisdom.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Wisdom.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Charisma.BaseValue = null;
            SelectedPlayerCharacter.Abilities.Charisma.RacialDispatchedBonus = 0;
            SelectedPlayerCharacter.Abilities.Listen();
        }
        private void PrefillDices(List<int> values)
        {
            values.Sort();
            List<int> mins;
            List<int> maxs;
            if (SelectedPlayerCharacter.Class?.Proficiencies?.SavingThrows != null)
            {
                mins = values.Take(4).ToList();
                maxs = values.Skip(4).ToList();
            }
            else
            {
                mins = values;
                maxs = null;
            }

            SelectedPlayerCharacter.Abilities.Unlisten();
            SelectedPlayerCharacter.Abilities.Strength.BaseValue = PickAbility(ref mins, ref maxs, "Force");
            SelectedPlayerCharacter.Abilities.Dexterity.BaseValue = PickAbility(ref mins, ref maxs, "Dextérité");
            SelectedPlayerCharacter.Abilities.Constitution.BaseValue = PickAbility(ref mins, ref maxs, "Constitution");
            SelectedPlayerCharacter.Abilities.Intelligence.BaseValue = PickAbility(ref mins, ref maxs, "Intelligence");
            SelectedPlayerCharacter.Abilities.Wisdom.BaseValue = PickAbility(ref mins, ref maxs, "Sagesse");
            SelectedPlayerCharacter.Abilities.Charisma.BaseValue = PickAbility(ref mins, ref maxs, "Charisme");
            SelectedPlayerCharacter.Abilities.Listen();
        }

        public BaseFont findFontInForm(PdfReader reader, PdfName fontname)
        {

            PdfDictionary root = reader.Catalog;

            PdfDictionary acroform = root.GetAsDict(PdfName.ACROFORM);

            if (acroform == null) return null;

            PdfDictionary dr = acroform.GetAsDict(PdfName.DR);

            if (dr == null) return null;

            PdfDictionary font = dr.GetAsDict(PdfName.FONT);

            if (font == null) return null;

            foreach (PdfName key in font.Keys)
            {

                if (key.Equals(fontname))
                {

                    return BaseFont.CreateFont((PRIndirectReference)font.GetAsIndirectObject(key));

                }

            }

            return null;

        }


        public PRIndirectReference findNamedFont(PdfReader myReader, String desiredFontName)
        {
            int objNum = 0;
            PdfObject curObj;
            do
            {
                //The "Release" version doesn't keep a reference 
                //to the object so it can be GC'd later.  Quite Handy 
                //when dealing with Really Big PDFs.
                curObj = myReader.GetPdfObjectRelease(objNum++);
                if (curObj is PRStream)
                {
                    PRStream stream = (PRStream)curObj;
                    PdfName type = stream.GetAsName(PdfName.TYPE);
                    if (PdfName.FONT.Equals(type))
                    {
                        PdfString fontName = stream.GetAsString(PdfName.BASEFONT);
                        if (desiredFontName.Equals(fontName.ToString()))
                        {
                            return curObj.IndRef;
                        }
                    }
                }
            } while (curObj != null);
            return null;
        }

        public PRIndirectReference findFontInPage(PdfReader reader, String desiredName, int i)
        {

            PdfDictionary page = reader.GetPageN(i);
            return findFontInResources(page.GetAsDict(PdfName.RESOURCES), desiredName);
        }

        public PRIndirectReference findFontInResources(PdfDictionary resources, String desiredName)
        {
            if (resources != null)
            {
                foreach (DictionaryEntry res in resources)
                {
                    Debug.WriteLine(res.Key);
                }
                PdfDictionary fonts = resources.GetAsDict(PdfName.BASEFONT);
                if (fonts != null)
                {
                    foreach (DictionaryEntry ccurFont in fonts)
                    {
                        PdfName curFontName = ccurFont.Key as PdfName;
                        Debug.WriteLine($"curFontName.IsArray = {curFontName.IsArray()}");
                        Debug.WriteLine($"curFontName.IsBoolean = {curFontName.IsBoolean()}");
                        Debug.WriteLine($"curFontName.IsDictionary = {curFontName.IsDictionary()}");
                        Debug.WriteLine($"curFontName.IsIndirect = {curFontName.IsIndirect()}");
                        Debug.WriteLine($"curFontName.IsName = {curFontName.IsName()}");
                        Debug.WriteLine($"curFontName.IsNull = {curFontName.IsNull()}");
                        Debug.WriteLine($"curFontName.IsNumber = {curFontName.IsNumber()}");
                        Debug.WriteLine($"curFontName.IsStream = {curFontName.IsStream()}");
                        Debug.WriteLine($"curFontName.IsString = {curFontName.IsString()}");
                        PRStream curFont = (PRStream)fonts.GetAsStream(curFontName);
                        if (curFont != null)
                        {
                            if (desiredName.Equals(curFont.GetAsString(PdfName.BASEFONT).ToString()))
                            {
                                return (PRIndirectReference)curFont.IndRef;
                            }
                        }
                    }
                }
                PdfDictionary xobjs = resources.GetAsDict(PdfName.XOBJECT);
                if (xobjs != null)
                {
                    foreach (DictionaryEntry ccurXObj in xobjs)
                    {
                        PdfName curXObjName = ccurXObj.Key as PdfName;
                        var curXObjVal = ccurXObj.Value as PRIndirectReference;
                        Debug.WriteLine($"curXObjVal.IsArray = {curXObjVal.IsArray()}");
                        Debug.WriteLine($"curXObjVal.IsBoolean = {curXObjVal.IsBoolean()}");
                        Debug.WriteLine($"curXObjVal.IsDictionary = {curXObjVal.IsDictionary()}");
                        Debug.WriteLine($"curXObjVal.IsIndirect = {curXObjVal.IsIndirect()}");
                        Debug.WriteLine($"curXObjVal.IsName = {curXObjVal.IsName()}");
                        Debug.WriteLine($"curXObjVal.IsNull = {curXObjVal.IsNull()}");
                        Debug.WriteLine($"curXObjVal.IsNumber = {curXObjVal.IsNumber()}");
                        Debug.WriteLine($"curXObjVal.IsStream = {curXObjVal.IsStream()}");
                        Debug.WriteLine($"curXObjVal.IsString = {curXObjVal.IsString()}");


                        PRStream curXObj = (PRStream)xobjs.GetAsStream(curXObjName);
                        var name = curXObj.GetAsName(PdfName.SUBTYPE);
                        if (curXObj != null && PdfName.FORM.Equals(name))
                        {
                            PdfDictionary resources2 = curXObj.GetAsDict(PdfName.RESOURCES);
                            PRIndirectReference reff = findFontInResources(resources2, desiredName);
                            if (reff != null)
                            {
                                return reff;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static Dictionary<String, PRIndirectReference> listFonts(PdfReader reader)
        {
            Dictionary<String, PRIndirectReference> set = new Dictionary<String, PRIndirectReference>();
            //PdfReader reader = new PdfReader(src);
            PdfDictionary resources;
            for (int k = 1; k <= reader.NumberOfPages; ++k)
            {
                resources = reader.GetPageN(k).GetAsDict(PdfName.RESOURCES);
                processResource(set, resources);
            }
            return set;
        }

        public static void processResource(Dictionary<String, PRIndirectReference> set, PdfDictionary resource)
        {
            if (resource == null)
                return;
            PdfDictionary xobjects = resource.GetAsDict(PdfName.XOBJECT);
            if (xobjects != null)
            {
                foreach (PdfName key in xobjects.Keys)
                {
                    processResource(set, xobjects.GetAsDict(key));
                }
            }
            PdfDictionary fonts = resource.GetAsDict(PdfName.FONT);
            if (fonts == null)
                return;
            PdfDictionary font;
            foreach (PdfName key in fonts.Keys)
            {
                font = fonts.GetAsDict(key);
                String name = font.GetAsName(PdfName.BASEFONT).ToString();
                var flat = resource.Keys; //.GetAsDict(PdfName.FILTER);
                var dic = font.GetAsStream(PdfName.BASEFONT);
                if (name.Length > 8 && name[7] == '+')
                {
                    name = String.Format("{0} subset ({1})", name.Substring(8), name.Substring(1, 7));
                }
                else
                {
                    name = name.Substring(1);
                    PdfDictionary desc = font.GetAsDict(PdfName.FONTDESCRIPTOR);
                    if (desc == null)
                        name += " nofontdescriptor";
                    else if (desc.Get(PdfName.FONTFILE) != null)
                        name += " (Type 1) embedded";
                    else if (desc.Get(PdfName.FONTFILE2) != null)
                        name += " (TrueType) embedded";
                    else if (desc.Get(PdfName.FONTFILE3) != null)
                        name += " (" + font.GetAsName(PdfName.SUBTYPE).ToString().Substring(1) + ") embedded";
                }
                set[name] = font.IndRef;
            }
        }

        void DrawText(PdfContentByte cb, string text, iTextSharp.text.Font font, float x, float y, float width, float height, int alignment)
        {
            cb.SetRGBColorFill(127, 127, 127);
            //cb.Rectangle(x, y, width, height);
            //cb.Stroke();
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(x, y, x + width, y + height);
            var p = new Paragraph(text, font);
            p.Alignment = alignment;
            ct.AddElement(p);
            ct.Go();

        }


        public ICommand GenerateAndOpenPdfCommand
        {
            get
            {
                return new Command(async () => await ExecuteGenerateAndOpenPdfCommandAsync());
            }
        }

        public async Task ExecuteGenerateAndOpenPdfCommandAsync()
        {
            var filename = await GeneratePdfAsync(SelectedPlayerCharacter);
            await OpenPdfAsync(filename);
        }

        public string BasePdfDirectory
        {
            get
            {
                return Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "pdf");
            }
        }
        public async Task<string> GeneratePdfAsync(PlayerCharacterViewModel playerCharacter)
        {
            return await Task.Run(() =>
            {
                var basePath = BasePdfDirectory;
                Directory.CreateDirectory(basePath);
                var now = DateTime.Now;
                var fileName = string.Format("PJ_{0:yyyyMMddHHmmss}.pdf", now);
                var filePath = Path.Combine(basePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    PdfReader reader = null;
                    try
                    {
                        reader = new PdfReader(AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.feuille_de_personnage_editable.pdf"));
                        PdfStamper stamper = null;
                        try
                        {
                            stamper = new PdfStamper(reader, stream);
                            var form = stamper.AcroFields;
                            var fields = form.Fields;
#if DEBUG
                            foreach (DictionaryEntry field in fields)
                            {
                                var item = field.Value as AcroFields.Item;
                                Debug.WriteLine(field.Key);
                                if(field.Key.ToString().Equals("Portrait"))
                                {
                                    Debug.WriteLine("Portrait");
                                }
                                form.SetField(field.Key.ToString(), field.Key.ToString());
                            }
#endif // DEBUG

                            form.SetField("Nom", SelectedPlayerCharacter?.Name ?? string.Empty);
                            form.SetField("Niveau", "1");
                            form.SetField("Race", SelectedPlayerCharacter?.Race?.Name ?? string.Empty);
                            form.SetField("Classe", SelectedPlayerCharacter?.Class?.Name ?? string.Empty);
                            form.SetField("Alignement", SelectedPlayerCharacter?.Alignment?.Name ?? string.Empty);
                            form.SetField("Historique", SelectedPlayerCharacter?.Background?.Background?.Name ?? string.Empty);
                            form.SetField("Trait de personnalité",
                                (SelectedPlayerCharacter.Background.PersonalityTrait ?? string.Empty) + "\n\n" +
                                (SelectedPlayerCharacter.Background.PersonalityIdeal ?? string.Empty) + "\n\n" +
                                (SelectedPlayerCharacter.Background.PersonalityLink ?? string.Empty) + "\n\n" +
                                (SelectedPlayerCharacter.Background.PersonalityDefect ?? string.Empty)
                                );
                            form.SetField("For Valeur", SelectedPlayerCharacter?.Abilities?.Strength?.Value?.ToString());
                            form.SetField("For MOD", SelectedPlayerCharacter?.Abilities?.Strength?.ModString);
                            form.SetField("Dex Valeur", SelectedPlayerCharacter?.Abilities?.Dexterity?.Value?.ToString());
                            form.SetField("Dex MOD", SelectedPlayerCharacter?.Abilities?.Dexterity?.ModString);
                            form.SetField("Con Valeur", SelectedPlayerCharacter?.Abilities?.Constitution?.Value?.ToString());
                            form.SetField("Con MOD", SelectedPlayerCharacter?.Abilities?.Constitution?.ModString);
                            form.SetField("Int Valeur", SelectedPlayerCharacter?.Abilities?.Intelligence?.Value?.ToString());
                            form.SetField("Int MOD", SelectedPlayerCharacter?.Abilities?.Intelligence?.ModString);
                            form.SetField("Sag Valeur", SelectedPlayerCharacter?.Abilities?.Wisdom?.Value?.ToString());
                            form.SetField("Sag MOD", SelectedPlayerCharacter?.Abilities?.Wisdom?.ModString);
                            form.SetField("Cha Valeur", SelectedPlayerCharacter?.Abilities?.Charisma?.Value?.ToString());
                            form.SetField("Cha MOD", SelectedPlayerCharacter?.Abilities?.Charisma?.ModString);
                        }
                        finally
                        {
                            stamper?.Close();
                        }
                    }
                    finally
                    { 
                        reader?.Close();
                    }

                    return fileName;
                }
            });
        }

        public async Task OpenPdfAsync(string filename)
        {
            var filepath = Path.Combine(BasePdfDirectory, filename);
            await DependencyService.Get<INativeAPI>().LaunchFileAsync("hophop", "coucou", filepath);
        }

        private int PickAbility(ref List<int> mins, ref List<int> maxs, string name)
        {
            var value = SelectedPlayerCharacter.Class?.Proficiencies?.SavingThrows?.Contains(name);
            if (value == true)
            {
                return PickOne(ref maxs);
            }
            else
            {
                return PickOne(ref mins);
            }
        }

        private int PickOne(ref List<int> values)
        {
            var index = _Random.Next(values.Count);
            var pick = values[index];
            values.RemoveAt(index);
            return pick;
        }

        private List<int> RollMRick()
        {
            var dices = new List<int>();
            var roll = Roll2d6();
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = Roll2d6();
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = Roll2d6();
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            return dices;
        }
        private List<int> Roll6x2d6plus6()
        {
            var dices = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                dices.Add(Roll2d6() + 6);
            }
            return dices;
        }
        private int Roll2d6()
        {
            return _Random.Next(6) + _Random.Next(6) + 2;
        }
        #endregion Abilities

        #region Level
        public List<string> Levels { get; set; } = new List<string>()
        {
            "1", //"2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
        };
        #endregion Level

    }

}
