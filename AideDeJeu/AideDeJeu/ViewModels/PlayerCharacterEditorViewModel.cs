using AideDeJeu.Tools;
using AideDeJeuLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class PlayerCharacterEditorViewModel : BaseViewModel
    {
        public PlayerCharacterEditorViewModel()
        {
            Races = new NotifyTaskCompletion<List<RaceItem>>(Task.Run(() => LoadRacesAsync()));
            Classes = new NotifyTaskCompletion<List<ClassItem>>(Task.Run(() => LoadClassesAsync()));
            Backgrounds = new NotifyTaskCompletion<List<BackgroundItem>>(Task.Run(() => LoadBackgroundsAsync()));
            SubBackgrounds = new NotifyTaskCompletion<List<SubBackgroundItem>>(null);
        }

        public NotifyTaskCompletion<List<RaceItem>> Races { get; private set; }
        private int _RaceSelectedIndex = 0;
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
        public NotifyTaskCompletion<List<ClassItem>> Classes { get; private set; }

        private int _ClassSelectedIndex = 0;
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

        public NotifyTaskCompletion<List<BackgroundItem>> Backgrounds { get; private set; }

        private int _BackgroundSelectedIndex = 0;
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
                SelectedPlayerCharacter.SubBackground = null;
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

        private int _SubBackgroundSelectedIndex = 0;
        public int SubBackgroundSelectedIndex
        {
            get
            {
                return _SubBackgroundSelectedIndex;
            }
            set
            {
                SetProperty(ref _SubBackgroundSelectedIndex, value);
                if(_SubBackgroundSelectedIndex == 0)
                {
                    SelectedPlayerCharacter.SubBackground = null;
                    SubBackgroundSelectedIndex = -1;
                }
                else if(_SubBackgroundSelectedIndex > 0)
                {
                    SelectedPlayerCharacter.SubBackground = SubBackgrounds.Result[_SubBackgroundSelectedIndex];
                }
            }
        }

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

        public List<string> Abilities { get; set; } = new List<string>()
        {
            "2 (-4)", "3 (-4)", "4 (-3)", "5 (-3)", "6 (-2)", "7 (-2)", "8 (-1)", "9 (-1)", "10 (+0)", "11 (+0)", "12 (+1)", "13 (+1)", "14 (+2)", "15 (+2)", "16 (+3)", "17 (+3)", "18 (+4)", "19 (+4)", "20 (+5)", "21 (+5)"
        };
        public List<string> Levels { get; set; } = new List<string>()
        {
            "1", //"2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
        };

        public async Task<List<RaceItem>> LoadRacesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Races.Where(r => !r.HasSubRaces).OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync().ConfigureAwait(false);
            }
        }
        public async Task<List<ClassItem>> LoadClassesAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Classes.Where(c => !(c is SubClassItem)).OrderBy(c => Tools.Helpers.RemoveDiacritics(c.Name)).ToListAsync().ConfigureAwait(false);
            }
        }
        public async Task<List<BackgroundItem>> LoadBackgroundsAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                return await context.Backgrounds.Where(b => b.GetType() == typeof(BackgroundItem)).OrderBy(b => Tools.Helpers.RemoveDiacritics(b.Name)).ToListAsync().ConfigureAwait(false);
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
    }
}
