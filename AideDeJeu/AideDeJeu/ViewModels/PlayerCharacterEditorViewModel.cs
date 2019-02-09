using AideDeJeuLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public PlayerCharacterViewModel SelectedPlayerCharacter { get; set; }

        public async Task InitAsync()
        {
            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                Races = await context.Races.ToListAsync();
                Classes = await context.Classes.ToListAsync();
                Backgrounds = await context.Backgrounds.ToListAsync();
            }
        }
    }
}
