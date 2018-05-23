using AideDeJeu.Services;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace AideDeJeuCmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var documentsDirectoryPath = @"database.db"; // Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            //ItemDatabaseHelper helper = new ItemDatabaseHelper(documentsDirectoryPath);
            //var spells = await helper.GetSpellsAsync(classe: "", niveauMin: "0", niveauMax: "9", ecole: "", rituel: "", source: "(SRD)");
            //var monsters = await helper.GetMonstersAsync(category: "", type: "", minPower: " 0 (0 PX)", maxPower: " 30 (155000 PX)", size: "", legendary: "", source: "(SRD)");
            var pack = new HtmlDocument();
            var client = new HttpClient();

            var spells = LoadJSon<IEnumerable<Spell>>("spells.json");
            var monsters = LoadJSon<IEnumerable<Monster>>("monsters.json");
            var spellsVO = new List<Spell>();
            foreach(var spell in spells)
            {
                spell.ParseHtml();
                var htmlVO = await client.GetStringAsync(string.Format("https://www.aidedd.org/dnd/sorts.php?vo={0}", spell.IdVO));
                pack.LoadHtml(htmlVO);
                var spellVO = Spell.FromHtml(pack.DocumentNode.SelectSingleNode("//div[contains(@class,'bloc')]"));
                spellVO.IdVO = spell.IdVO;
                spell.IdVF = spellVO.IdVF;
                spellsVO.Add(spellVO);

                Console.WriteLine(string.Format("{0} : {1} / {2} : {3}", spell.IdVF, spell.NamePHB, spellVO.IdVO, spellVO.NameVO));
            }

            foreach(var monster in monsters)
            {
                monster.ParseHtml();
            }
            SaveJSon<IEnumerable<Monster>>("monsters_vf.json", monsters);
            SaveJSon<IEnumerable<Spell>>("spells_vf.json", spells);
            //SaveJSon<IEnumerable<Monster>>("monsters_fr.json", monsters);
            SaveJSon<IEnumerable<Spell>>("spells_vo.json", spellsVO);
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(items.GetType());
            //MemoryStream stream = new MemoryStream();
            //serializer.WriteObject(stream, items);
            //stream.Seek(0, SeekOrigin.Begin);
            //string text = await new StreamReader(stream).ReadToEndAsync();
            Console.WriteLine("Hello World!");
        }

        private static T LoadJSon<T>(string filename) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                return serializer.ReadObject(stream) as T;
            }
        }

        private static void SaveJSon<T>(string filename, T objT) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                serializer.WriteObject(stream, objT);
            }
        }
    }
}
