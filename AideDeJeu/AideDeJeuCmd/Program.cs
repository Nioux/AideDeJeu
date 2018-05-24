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
            string dataDir = @"..\..\..\..\..\Data\";
            //string ignoreDir = @"..\..\..\..\..\Ignore\";
            //var documentsDirectoryPath = @"database.db"; // Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            //ItemDatabaseHelper helper = new ItemDatabaseHelper(documentsDirectoryPath);
            //var spells = await helper.GetSpellsAsync(classe: "", niveauMin: "0", niveauMax: "9", ecole: "", rituel: "", source: "(SRD)");
            //var monsters = await helper.GetMonstersAsync(category: "", type: "", minPower: " 0 (0 PX)", maxPower: " 30 (155000 PX)", size: "", legendary: "", source: "(SRD)");

            /*
            var pack = new HtmlDocument();
            var client = new HttpClient();

            var spells = LoadJSon<IEnumerable<Spell>>(dataDir + "spells.json");
            var spellsVO = new List<Spell>();
            foreach (var spell in spells)
            {
                spell.ParseHtml();
                var htmlVO = await client.GetStringAsync(string.Format("https://www.aidedd.org/dnd/sorts.php?vo={0}", spell.IdVO));
                pack.LoadHtml(htmlVO);
                var spellVO = Spell.FromHtml(pack.DocumentNode.SelectSingleNode("//div[contains(@class,'bloc')]"));
                spellVO.IdVO = spell.IdVO;
                spell.IdVF = spellVO.IdVF;
                spellsVO.Add(spellVO);

                Console.WriteLine(string.Format("{0} : {1} / {2} : {3}", spell.IdVF, spell.NamePHB, spellVO.IdVO, spellVO.NamePHB));
            }
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vf.json", spells);
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vo.json", spellsVO);

            var monsters = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters.json");
            var monstersVO = new List<Monster>();
            foreach (var monster in monsters)
            {
                monster.ParseHtml();
                var htmlVO = await client.GetStringAsync(string.Format("https://www.aidedd.org/dnd/monstres.php?vo={0}", monster.IdVO));
                pack.LoadHtml(htmlVO);
                var monsterVO = Monster.FromHtml(pack.DocumentNode.SelectSingleNode("//div[contains(@class,'bloc')]"));
                monsterVO.IdVO = monster.IdVO;
                monster.IdVF = monsterVO.IdVF;
                monstersVO.Add(monsterVO);

                Console.WriteLine(string.Format("{0} : {1} / {2} : {3}", monster.IdVF, monster.NamePHB, monsterVO.IdVO, monsterVO.NamePHB));
            }
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vf.json", monsters);
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vo.json", monstersVO);
            */
            var spellsHD = new List<Spell>();
            var spell = new Spell();
            using (var reader = new StreamReader(dataDir + "spells_hd.txt"))
            {
                var line = await reader.ReadLineAsync();
                while (line != null)
                {
                    if (line.Length == 0)
                    {
                        Console.WriteLine(spell.NamePHB);

                        spell.DescriptionHtml = await FormatDescriptionAsync(spell.DescriptionHtml);
                        spellsHD.Add(spell);
                        spell = new Spell();
                    }
                    else
                    {
                        if (spell.NamePHB == null)
                        {
                            spell.NamePHB = line;
                        }
                        else if (spell.LevelType == null)
                        {
                            spell.LevelType = line;
                        }
                        else
                        { 
                            if(line.StartsWith("Temps d’incantation : "))
                            {
                                spell.CastingTime = line.Substring(23);
                            }
                            else if(line.StartsWith("Portée : "))
                            {
                                spell.Range = line.Substring(9);
                            }
                            else if (line.StartsWith("Composantes : "))
                            {
                                spell.Components = line.Substring(14);
                            }
                            else if (line.StartsWith("Durée : "))
                            {
                                spell.Duration = line.Substring(8);
                            }
                            else
                            {
                                spell.DescriptionHtml += line + "\n";
                                //if (line.EndsWith("."))
                                //{
                                //    spell.DescriptionHtml += line + "\n";
                                //}
                                //else if (line.EndsWith("-"))
                                //{
                                //    spell.DescriptionHtml += line.Substring(0, line.Length - 1);
                                //}
                                //else
                                //{
                                //    spell.DescriptionHtml += line + " ";
                                //}
                            }
                        }
                    }
                    line = await reader.ReadLineAsync();
                }
            }
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_hd.json", spellsHD);

            Console.WriteLine("Hello World!");
        }

        static async Task<string> FormatDescriptionAsync(string description)
        {
            string formattedDescription = "";
            using (var reader = new StringReader(description))
            {
                var line = await reader.ReadLineAsync();
                var li = false;
                while (line != null)
                {
                    if (line.StartsWith("»» "))
                    {
                        li = true;
                        formattedDescription += "<li>" + line.Substring(3);
                    }

                    if (line.EndsWith("."))
                    {
                        formattedDescription += line;
                        if (li)
                        {
                            formattedDescription += "</li>";
                            li = false;
                        }
                        formattedDescription += "\n";
                    }
                    else
                    {
                        formattedDescription += line + " ";
                    }

                    line = await reader.ReadLineAsync();
                }
                if (li)
                {
                    formattedDescription += "</li>";
                }
            }
            return formattedDescription;
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
