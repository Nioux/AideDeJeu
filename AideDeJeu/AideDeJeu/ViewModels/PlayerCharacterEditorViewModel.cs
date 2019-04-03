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
        private List<RaceItem> _Races = new List<RaceItem>();
        public List<RaceItem> Races
        {
            get
            {
                return _Races;
            }
            set
            {
                SetProperty(ref _Races, value);
            }
        }
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
                SelectedPlayerCharacter.Race = Races[_RaceSelectedIndex];
            }
        }
        private List<ClassItem> _Classes = new List<ClassItem>();
        public List<ClassItem> Classes
        {
            get
            {
                return _Classes;
            }
            set
            {
                SetProperty(ref _Classes, value);
            }
        }
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
                SelectedPlayerCharacter.Class = Classes[_ClassSelectedIndex];
            }
        }
        private List<BackgroundItem> _Backgrounds = new List<BackgroundItem>();
        public List<BackgroundItem> Backgrounds
        {
            get
            {
                return _Backgrounds;
            }
            set
            {
                SetProperty(ref _Backgrounds, value);
            }
        }
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
                SelectedPlayerCharacter.Background = Backgrounds[_BackgroundSelectedIndex];
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

        public async Task InitAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                Races = await context.Races.Where(r => !r.HasSubRaces).OrderBy(r => Tools.Helpers.RemoveDiacritics(r.Name)).ToListAsync();
                Classes = await context.Classes.Where(c => !(c is SubClassItem)).OrderBy(c => Tools.Helpers.RemoveDiacritics(c.Name)).ToListAsync();
                Backgrounds = await context.Backgrounds.Where(b => b.GetType() == typeof(BackgroundItem)).OrderBy(b => Tools.Helpers.RemoveDiacritics(b.Name)).ToListAsync();
            }
        }
    }
}
