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

        public async Task InitAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                Races = await context.Races.Where(r => r.GetType() == typeof(RaceItem)).OrderBy(r => r.Name).ToListAsync();
                Classes = await context.Classes.OrderBy(c => c.Name).ToListAsync();
                Backgrounds = await context.Backgrounds.Where(b => b.GetType() == typeof(BackgroundItem)).OrderBy(b => b.Name).ToListAsync();
            }
        }
    }
}
