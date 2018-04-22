using System;

using AideDeJeu.Models;
using AideDeJeuLib;

namespace AideDeJeu.ViewModels
{
    public class SpellDetailViewModel : BaseViewModel
    {
        public Spell Item { get; set; }
        public SpellDetailViewModel(Spell item = null)
        {
            Title = item?.Title;
            Item = item;
        }
    }
}
