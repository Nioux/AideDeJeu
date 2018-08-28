using AideDeJeuLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AideDeJeu.ViewModels
{
    public class BookmarksViewModel //: BaseViewModel
    {
        public BookmarksViewModel()
        {
            LoadBookmarks();
        }

        public List<KeyValuePair<string, List<Item>>> BookmarksKeyValues { get; set; } = new List<KeyValuePair<string, List<Item>>>()
        {
            new KeyValuePair<string, List<Item>>("Général", new List<Item>()),
            new KeyValuePair<string, List<Item>>("Grimoire", new List<Item>()),
            new KeyValuePair<string, List<Item>>("Bestiaire", new List<Item>()),
            new KeyValuePair<string, List<Item>>("Sac", new List<Item>()),
        };
        public int BookmarksIndex { get; set; } = 0;
        public List<Item> Bookmarks { get; set; }
        public int BookmarksCount { get; set; } = 0;

        public void LoadBookmarks()
        {
            foreach(var key in App.Current.Properties.Keys)
            {
                var property = App.Current.Properties[key] as string;
                if(property != null)
                {
                    BookmarksKeyValues.Add(new KeyValuePair<string, List<Item>>(key, ToItems(property)));
                }
            }
        }

        public async Task SaveBookmarksAsync()
        {
            foreach(var keyValues in BookmarksKeyValues)
            {
                App.Current.Properties[keyValues.Key] = ToString(keyValues.Value);
            }
            await App.Current.SavePropertiesAsync();
        }

        public string ToString(List<Item> items)
        {
            var serializer = ItemJsonSerializer;
            using(var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, items);
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public List<Item> ToItems(string str)
        {
            var serializer = ItemJsonSerializer;
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            using (var stream = new MemoryStream(byteArray))
            {
                return serializer.ReadObject(stream) as List<Item>;
            }
        }

        public DataContractJsonSerializer ItemJsonSerializer
        {
            get
            {
                var settings = new DataContractJsonSerializerSettings();
                settings.KnownTypes = new List<Type>()
                {
                    typeof(HomeItem),
                    typeof(Spell),
                    typeof(Monster),
                    //typeof(Items),
                    typeof(LinkItem),
                    typeof(Equipment),
                    //typeof(Spells),
                    //typeof(Monsters),
                    //typeof(Equipments),
                    typeof(PageItem),
                };
                return new DataContractJsonSerializer(typeof(List<Item>), settings);
            }
        }
    }
}
