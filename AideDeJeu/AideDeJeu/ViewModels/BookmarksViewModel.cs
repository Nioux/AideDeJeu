using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeu.ViewModels
{
    public class BookmarksViewModel : BaseViewModel
    {
        public List<KeyValuePair<string, IEnumerable<Item>>> BookmarksKeyValues { get; set; } = new List<KeyValuePair<string, IEnumerable<Item>>>()
        {
            new KeyValuePair<string, IEnumerable<Item>>("Général", new List<Item>()),
            new KeyValuePair<string, IEnumerable<Item>>("Grimoire", new List<Item>()),
            new KeyValuePair<string, IEnumerable<Item>>("Bestiaire", new List<Item>()),
            new KeyValuePair<string, IEnumerable<Item>>("Sac", new List<Item>()),
        };
        public int BookmarksIndex { get; set; } = 0;
        public IEnumerable<Item> Bookmarks { get; set; }
        public int BookmarksCount { get; set; } = 0;
    }
}
