using AideDeJeu.ViewModels;
using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AideDeJeu.Repositories
{
    public class BookmarksRepository : BaseViewModel
    {
        public ObservableCollection<string> BookmarkCollectionNames { get; set; } = new ObservableCollection<string>()
        {
            "Général",
            "Grimoire",
            "Bestiaire",
            "Sac",
            "Nouvelle liste",
        };

        private int _BookmarkCollectionIndex = 0;
        public int BookmarkCollectionIndex
        {
            get
            {
                return _BookmarkCollectionIndex;
            }
            set
            {
                SetProperty(ref _BookmarkCollectionIndex, value);
                //LoadBookmarkCollection(BookmarkCollectionNames[BookmarkCollectionIndex]);
            }
        }

        public Dictionary<string, ObservableCollection<Item>> _BookmarkCollections = new Dictionary<string, ObservableCollection<Item>>();

        public  ObservableCollection<Item> _BookmarkCollection = new ObservableCollection<Item>();
        public ObservableCollection<Item> BookmarkCollection
        {
            get
            {
                return _BookmarkCollection;
            }
            set
            {
                SetProperty(ref _BookmarkCollection, value);
            }
        }

    }
}
