using AideDeJeu.Tools;
using AideDeJeuLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public NotifyTaskCompletion<List<ClassViewModel>> Classes { get; private set; }

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

        private int? _Strength = null;
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

        public ICommand RollDicesCommand
        {
            get
            {
                return new Command(async () => await ExecuteRollDicesCommandAsync());
            }
        }
        private async Task ExecuteRollDicesCommandAsync()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var values = RollMRick(random);
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


            Strength = PickAbility(random, ref mins, ref maxs, "Force");
            Dexterity = PickAbility(random, ref mins, ref maxs, "Dextérité");
            Constitution = PickAbility(random, ref mins, ref maxs, "Constitution");
            Intelligence = PickAbility(random, ref mins, ref maxs, "Intelligence");
            Wisdom = PickAbility(random, ref mins, ref maxs, "Sagesse");
            Charisma = PickAbility(random, ref mins, ref maxs, "Charisme");

            await GeneratePdfAsync();
            await OpenPdfAsync();
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
                foreach(DictionaryEntry res in resources)
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

        public static HashSet<String> listFonts(PdfReader reader)
        {
            HashSet<String> set = new HashSet<String>();
            //PdfReader reader = new PdfReader(src);
            PdfDictionary resources;
            for (int k = 1; k <= reader.NumberOfPages; ++k)
            {
                resources = reader.GetPageN(k).GetAsDict(PdfName.RESOURCES);
                processResource(set, resources);
            }
            return set;
            }

        public static void processResource(HashSet<String> set, PdfDictionary resource)
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
                set.Add(name);
            }
        }

        async Task GeneratePdfAsync()
        {
            //PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            //PdfDocument srcDoc;
            //PdfFormXObject page;
            //PdfCanvas canvas = new PdfCanvas(pdfDoc..FirstPage.newContentStreamBefore(),
            //        pdfDoc.getFirstPage().getResources(), pdfDoc);

            //for (String path : EXTRA)
            //{
            //    srcDoc = new PdfDocument(new PdfReader(path));
            //    page = srcDoc.getFirstPage().copyAsFormXObject(pdfDoc);
            //    canvas.addXObject(page, 0, 0);
            //    srcDoc.close();
            //}
            //pdfDoc.close();


            //PdfDocument pdfDoc = new PdfDocument(new PdfWriter());
            //var stream = DependencyService.Get<INativeAPI>().CreateStream("test.pdf");
            var stream = new FileStream(Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "test.pdf"), FileMode.Create, FileAccess.ReadWrite);

            PdfReader reader = new PdfReader(AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.178_hd_01_feuille_de_perso_v1.pdf"));

            var set = listFonts(reader);
            //var truc = findFontInPage(reader, "MinionPro-It", 1);
            //var fonts = BaseFont.GetDocumentFonts(reader);
            //var font = BaseFont.CreateFont("TMULFZ+MinionPro-It", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //var font = findFontInForm(reader, new PdfName("MinionPro-It"));
            var font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);
            //var font = BaseFont.CreateFont(PRIndirectReference.());
            //var font = findNamedFont(reader, "");

            var bigFont = new iTextSharp.text.Font(font, 20);

            // read the file
            //PdfReader fondo = new PdfReader("listaPrecios.pdf");
            PdfStamper stamper = new PdfStamper(reader, stream);
            var ct = new ColumnText(stamper.GetOverContent(1));
            ct.SetSimpleColumn(20, 685, 200, 35);
            //ct.Canvas.SetRGBColorFill(255, 0, 0);
            //ct.Canvas.
            //ct.Canvas.Rectangle(0, 0, 200f, 600f);
            var p = new Paragraph(new Phrase(20, "Hello World! gfdgfd gfdgfd gfdgfdg gfdgdg zrerezr ezrzerez rezrezrze zrezrez zrezrez ffdfdsz rezrzerez  fsffsdfs", bigFont));
            p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            ct.AddElement(p);
            ct.Go();

            PdfContentByte content = stamper.GetOverContent(1);
            // add text
            content.SetRGBColorFill(255, 0, 0);
            content.Rectangle(20, 685, 200, 35);
            content.Stroke();

            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase("Galefrin", bigFont), 40, 700, 0);

            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Strength.ToString(), bigFont), 40, 620, 0);
            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Dexterity.ToString(), bigFont), 40, 545, 0);
            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Constitution.ToString(), bigFont), 40, 470, 0);
            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Intelligence.ToString(), bigFont), 40, 395, 0);
            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Wisdom.ToString(), bigFont), 40, 320, 0);
            ColumnText.ShowTextAligned(content, iTextSharp.text.Element.ALIGN_LEFT, new Phrase(Charisma.ToString(), bigFont), 40, 245, 0);

            //ColumnText ct = new ColumnText(content);
            //// this are the coordinates where you want to add text
            //// if the text does not fit inside it will be cropped
            //ct.SetSimpleColumn(50, 500, 500, 50);
            //ct.SetText(new Phrase("Galefrin"));
            //ct.Go();
            stamper.Close();
            reader.Close();

            /*

            Document document = new Document(PageSize.LETTER);
            var stream = DependencyService.Get<INativeAPI>().CreateStream("test.pdf");
            var writer = PdfWriter.GetInstance(document, stream);
            
            
            document.Open();
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            document.NewPage();
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(page, 0, 0);
            //document.Add(new Paragraph(0, "Hello World!"));

            //document.Add(new iTextSharp.text.Jpeg(new Uri("https://www.w3.org/MarkUp/Test/xhtml-print/20050519/tests/jpeg444.jpg")));

            
            //document.Add(new Paragraph(1, "Hello World!"));
            PdfContentByte canvas = writer.DirectContentUnder;

            //var imageStream = AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.feuille_de_perso_1.jpg");
            //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("https://www.w3.org/MarkUp/Test/xhtml-print/20050519/tests/jpeg444.jpg");
            //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

            //image.ScaleAbsolute(document.PageSize.Width / 2, document.PageSize.Height / 2);// PageSize.LETTER);

            //image.SetAbsolutePosition(0, 0);
            
            //canvas.AddImage(image);

            ColumnText.ShowTextAligned(canvas, iTextSharp.text.Element.ALIGN_LEFT, new Phrase("Galefrin"), 40, document.PageSize.Height - 100, 0);
            
            document.Close();
            */

        }
        async Task OpenPdfAsync()
        {
            //DependencyService.Get<INativeAPI>().OpenFileByName("test.pdf");

            //var file = Path.Combine(FileSystem.CacheDirectory, fn);
            //File.WriteAllText(file, "Hello World");
            var testfile = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "test.pdf");
            var shareFile = new Xamarin.Essentials.ShareFile(testfile);
            //var truc = Platform.GetShareableFileUri(request.File.FullPath);
            //await Xamarin.Essentials.Browser.OpenAsync(testfile);
            await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest()
            {
                Title = "ou yeah",
                File = shareFile
            });

        }

        private int PickAbility(Random random, ref List<int> mins, ref List<int> maxs, string name)
        {
            var value = SelectedPlayerCharacter.Class?.Proficiencies?.SavingThrows?.Contains(name);
            if (value == true)
            {
                return PickOne(random, ref maxs);
            }
            else
            {
                return PickOne(random, ref mins);
            }
        }

        private int PickOne(Random random, ref List<int> values)
        {
            var index = random.Next(values.Count);
            var pick = values[index];
            values.RemoveAt(index);
            return pick;
        }

        private List<int> RollMRick(Random random)
        {
            var dices = new List<int>();
            var roll = Roll2d6(random);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = Roll2d6(random);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            roll = Roll2d6(random);
            dices.Add(6 + roll);
            dices.Add(19 - roll);
            return dices;
        }
        private int Roll2d6(Random random)
        {
            return random.Next(6) + random.Next(6) + 2;
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
