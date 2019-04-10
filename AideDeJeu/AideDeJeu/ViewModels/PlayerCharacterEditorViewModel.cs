using AideDeJeu.Tools;
using AideDeJeuLib;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class PlayerCharacterEditorViewModel : BaseViewModel
    {
        public PlayerCharacterEditorViewModel()
        {
            ResetAlignments();
            Races = new NotifyTaskCompletion<List<RaceItem>>(Task.Run(() => LoadRacesAsync()));
            Classes = new NotifyTaskCompletion<List<ClassItem>>(Task.Run(() => LoadClassesAsync()));

            Backgrounds = new NotifyTaskCompletion<List<BackgroundItem>>(Task.Run(() => LoadBackgroundsAsync()));
            SubBackgrounds = new NotifyTaskCompletion<List<SubBackgroundItem>>(null);
            PersonalityTraits = new NotifyTaskCompletion<List<string>>(null);
            PersonalityIdeals = new NotifyTaskCompletion<List<string>>(null);
            PersonalityLinks = new NotifyTaskCompletion<List<string>>(null);
            PersonalityDefects = new NotifyTaskCompletion<List<string>>(null);
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
        /*private List<string> _AllAllignments = new List<string>()
            {
                "Loyal Bon (LB)",
                "Neutre Bon (NB)",
                "Chaotique Bon (CB)",
                "Loyal Neutre (LN)",
                "Neutre (N)",
                "Chaotique Neutre (CN)",
                "Loyal Mauvais (LM)",
                "Neutre Mauvais (NM)",
                "Chaotique Mauvais (CM)"
            };
        
        private List<string> _Alignments = null;
        public List<string> Alignments
        {
            get
            {
                return _Alignments;
            }
            set
            {
                SetProperty(ref _Alignments, value);
            }
        }*/

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
                    SelectedPlayerCharacter.Alignment = Alignments.Result[_AlignmentSelectedIndex];
                }
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
        public NotifyTaskCompletion<List<RaceItem>> Races { get; private set; }
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
                    SelectedPlayerCharacter.Race = Races.Result[_RaceSelectedIndex];
                }
            }
        }

        public async Task<List<RaceItem>> LoadRacesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Races.Where(r => !r.HasSubRaces).OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync().ConfigureAwait(false);
            }
        }
        #endregion Race

        #region Class
        public NotifyTaskCompletion<List<ClassItem>> Classes { get; private set; }

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
                SelectedPlayerCharacter.Class = Classes.Result[_ClassSelectedIndex];
            }
        }

        public async Task<List<ClassItem>> LoadClassesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Classes.Where(c => !(c is SubClassItem)).OrderBy(c => Tools.Helpers.RemoveDiacritics(c.Name)).ToListAsync().ConfigureAwait(false);
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
                SelectedPlayerCharacter.Background = Backgrounds.Result[_BackgroundSelectedIndex];
                SubBackgrounds = new NotifyTaskCompletion<List<SubBackgroundItem>>(Task.Run(() => LoadSubBackgroundsAsync(SelectedPlayerCharacter.Background)));
                PersonalityTraits = new NotifyTaskCompletion<List<string>>(Task.Run(() => LoadPersonalityTraitsAsync(SelectedPlayerCharacter.Background)));
                PersonalityIdeals = new NotifyTaskCompletion<List<string>>(Task.Run(() => LoadPersonalityIdealsAsync(SelectedPlayerCharacter.Background)));
                PersonalityLinks = new NotifyTaskCompletion<List<string>>(Task.Run(() => LoadPersonalityLinksAsync(SelectedPlayerCharacter.Background)));
                PersonalityDefects = new NotifyTaskCompletion<List<string>>(Task.Run(() => LoadPersonalityDefectsAsync(SelectedPlayerCharacter.Background)));
                SelectedPlayerCharacter.SubBackground = null;
                SelectedPlayerCharacter.PersonalityTrait = null;
                SelectedPlayerCharacter.PersonalityIdeal = null;
                SelectedPlayerCharacter.PersonalityLink = null;
                SelectedPlayerCharacter.PersonalityDefect = null;
                ResetAlignments();
            }
        }

        private NotifyTaskCompletion<List<SubBackgroundItem>> _SubBackgrounds = null;
        public NotifyTaskCompletion<List<SubBackgroundItem>> SubBackgrounds
        {
            get
            {
                return _SubBackgrounds;
            }
            private set
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
                    SelectedPlayerCharacter.SubBackground = null;
                    SubBackgroundSelectedIndex = -1;
                }
                else if (_SubBackgroundSelectedIndex > 0)
                {
                    SelectedPlayerCharacter.SubBackground = SubBackgrounds.Result[_SubBackgroundSelectedIndex];
                }
            }
        }

        private NotifyTaskCompletion<List<string>> _PersonalityTraits = null;
        public NotifyTaskCompletion<List<string>> PersonalityTraits
        {
            get
            {
                return _PersonalityTraits;
            }
            private set
            {
                SetProperty(ref _PersonalityTraits, value);
            }
        }
        private NotifyTaskCompletion<List<string>> _PersonalityIdeals = null;
        public NotifyTaskCompletion<List<string>> PersonalityIdeals
        {
            get
            {
                return _PersonalityIdeals;
            }
            private set
            {
                SetProperty(ref _PersonalityIdeals, value);
            }
        }
        private NotifyTaskCompletion<List<string>> _PersonalityLinks = null;
        public NotifyTaskCompletion<List<string>> PersonalityLinks
        {
            get
            {
                return _PersonalityLinks;
            }
            private set
            {
                SetProperty(ref _PersonalityLinks, value);
            }
        }
        private NotifyTaskCompletion<List<string>> _PersonalityDefects = null;
        public NotifyTaskCompletion<List<string>> PersonalityDefects
        {
            get
            {
                return _PersonalityDefects;
            }
            private set
            {
                SetProperty(ref _PersonalityDefects, value);
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

        public ICommand PersonalityTraitPickerCommand
        {
            get
            {
                return new Command<List<string>>(async (strings) => SelectedPlayerCharacter.PersonalityTrait = await ExecuteStringPickerCommandAsync(strings));
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
