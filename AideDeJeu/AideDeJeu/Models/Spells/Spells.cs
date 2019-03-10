using AideDeJeu.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using YamlDotNet.Serialization;
using System.Linq;

namespace AideDeJeuLib
{
    public class Spells : FilteredItems
    {
        public string Classes { get; set; }
        public string Levels { get; set; }
        public string Schools { get; set; }
        public string Rituals { get; set; }
        public string Sources { get; set; }

        public List<KeyValuePair<string, string>> Split(string collapsed)
        {
            if (collapsed == null) return new List<KeyValuePair<string, string>>();
            var split = collapsed.Split(new string[] { ", " }, StringSplitOptions.None).Select(s => new KeyValuePair<string, string>(s, s)).ToList();
            split.Insert(0, new KeyValuePair<string, string>("", "-"));
            return split;
        }
        public override FilterViewModel GetNewFilterViewModel()
        {
            return new SpellFilterViewModel(Family,
                Split(Classes),
                Split(Levels),
                Split(Schools),
                Split(Rituals),
                Split(Sources)
            );
        }
    }
    }
