using AideDeJeu.Services;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

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
            var spellsVF = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vf_full.json");
            var spellsVO = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vo_full.json");
            var monstersVF = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vf_full.json");
            var monstersVO = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vo_full.json");

            spellsVF.ForEach(sp => sp.Html = null);
            spellsVO.ForEach(sp => sp.Html = null);
            monstersVF.ForEach(it => it.Html = null);
            monstersVO.ForEach(it => it.Html = null);

            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vf.json", spellsVF);
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vo.json", spellsVO);
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vf.json", monstersVF);
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vo.json", monstersVO);
            return;

            var spellLists = new Dictionary<string, IEnumerable<string>>()
            {
                { "Barde", await LoadList(dataDir + "spell_barde.txt") },
                { "Clerc", await LoadList(dataDir + "spell_clerc.txt") },
                { "Druide", await LoadList(dataDir + "spell_druide.txt") },
                { "Ensorceleur", await LoadList(dataDir + "spell_ensorceleur.txt") },
                { "Magicien", await LoadList(dataDir + "spell_magicien.txt") },
                { "Ombrelame", await LoadList(dataDir + "spell_ombrelame.txt") },
                { "Paladin", await LoadList(dataDir + "spell_paladin.txt") },
                { "Rôdeur", await LoadList(dataDir + "spell_rodeur.txt") },
                { "Sorcier", await LoadList(dataDir + "spell_sorcier.txt") },
            };

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
                        spell.Source = "(HD)";
                        spell.Id = spell.IdVF = IdFromName(spell.NamePHB);
                        var spVF = spellsVF.SingleOrDefault(sp => IdFromName(sp.NamePHB) == spell.Id);
                        if(spVF != null)
                        {
                            if(spVF.Source.Contains("(SRD)"))
                            {
                                spell.Source += "(SRD)";
                                var spVO = spellsVO.SingleOrDefault(sp => sp.IdVO == spVF.IdVO);
                                if (spVO != null)
                                {
                                    spell.NameVO = spVO.Name;
                                    spell.IdVO = spVO.IdVO;
                                }
                            }
                        }
                        foreach (var spellList in spellLists)
                        {
                            if(spellList.Value.Contains(spell.NamePHB.ToLower()))
                            {
                                spell.Source += " " + spellList.Key + " ;";
                            }
                        }


                        spellsHD.Add(spell);
                        spell = new Spell();
                    }
                    else
                    {
                        if (spell.NamePHB == null)
                        {
                            spell.NamePHB = Capitalize(line);
                        }
                        else if (spell.LevelType == null)
                        {
                            spell.LevelType = line;
                            var re = new Regex("(?<type>.*) de niveau (?<level>\\d*).?(?<rituel>\\(rituel\\))?");
                            var match = re.Match(line);
                            spell.Type = match.Groups["type"].Value;
                            spell.Level = match.Groups["level"].Value;
                            spell.Rituel = match.Groups["rituel"].Value;
                            if (string.IsNullOrEmpty(spell.Type))
                            {
                                re = new Regex("(?<type>.*), (?<level>tour de magie)");
                                match = re.Match(line);
                                spell.Type = match.Groups["type"].Value;
                                spell.Level = "0"; // match.Groups["level"].Value;
                                spell.Rituel = match.Groups["rituel"].Value;
                            }
                        }
                        else
                        {
                            if (line.StartsWith("Temps d’incantation : "))
                            {
                                spell.CastingTime = line.Substring(22);
                            }
                            else if (line.StartsWith("Portée : "))
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

        public static string Capitalize(string text)
        {
            return string.Concat(text.Take(1)).ToUpper() + string.Concat(text.Skip(1)).ToString().ToLower();
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        static string IdFromName(string name)
        {
            return RemoveDiacritics(name.ToLower().Replace(" ", "-"));
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
                    else
                    {
                        var titles = new List<string>()
                        {
                            "À plus haut niveau.",
                            "Agrandir.",
                            "Rétrécir.",
                            "Endurance de l’ours.",
                            "Force du taureau.",
                            "Grâce du chat.",
                            "Splendeur de l’aigle.",
                            "Ruse du renard.",
                            "Sagesse du hibou.",
                            "Aura factice.",
                            "Masque.",
                            "Effets visant une cible.",
                            "Zones de magie.",
                            "Sorts.",
                            "Objets magiques.",
                            "Déplacement magique.",
                            "Créatures et objets.",
                            "Dissipation de la magie.",
                            "Bouille-crâne.",
                            "Convulsions.",
                            "Fièvre répugnante.",
                            "Mal aveuglant.",
                            "Mort poisseuse.",
                            "Pourriture.",
                            "Écarter les eaux.",
                            "Inondation.",
                            "Modifier le cours de l’eau.",
                            "Tourbillon.",
                            "Création d’eau.",
                            "Destruction d’eau.",
                            "Si vous lancez ce sort en une action, choisissez un point à portée.",
                            "Si vous lancez le sort sur une période de huit heures, vous enrichissez la terre.",
                            "Annulation d’enchantement.",
                            "Renvoi.",
                            "Confinement minimal.",
                            "Enchaîné.",
                            "Enseveli.",
                            "Prison isolée.",
                            "Sommeil.",
                            "Mettre fin au sort.",
                            "Glyphe à sort.",
                            "Runes explosives.",
                            "Approche.",
                            "Arrête.",
                            "Fuis.",
                            "Lâche.",
                            "Rampe.",
                            "Main agrippeuse.",
                            "Main interposée.",
                            "Main percutante.",
                            "Poing serré.",
                            "Nauséeux.",
                            "Endormi.",
                            "Paniqué.",
                            "Créature en créature.",
                            "Objet en créature.",
                            "Créature en objet.",
                            "Adaptation aquatique.",
                            "Armes naturelles.",
                            "Changer d’apparence.",
                            "1. Rouge.",
                            "2. Orange.",
                            "3. Jaune.",
                            "4. Vert.",
                            "5. Bleu.",
                            "6. Indigo.",
                            "7. Violet.",
                            "Couloirs.",
                            "Escaliers.",
                            "Portes.",
                            "Autres effets de sort.",
                            "Attirance.",
                            "Répulsion.",
                            "Mettre un terme à l’effet.",
                            "Courage.",
                            "Interférence extradimensionnelle.",
                            "Langues.",
                            "Lumière du jour.",
                            "Protection contres les énergies.",
                            "Repos éternel.",
                            "Ténèbres.",
                            "Vulnérabilité à l’énergie.",
                            "Silence.",
                            "Terreur.",
                            "Démence.",
                            "Désespoir.",
                            "Discorde.",
                            "Douleur.",
                            "Étourdissement.",
                            "Frayeur.",
                            "Mort.",
                            "Sommeil.",
                            "Créatures.",
                            "Objets.",
                            "Familiarité.",
                            "Sur place.",
                            "À proximité.",
                            "Zone similaire.",
                            "Incident.",
                            "Round 2.",
                            "Round 3.",
                            "Round 4.",
                            "Rounds 5 à 10.",
                            "Fissures.",
                            "Structures.",
                        };
                        foreach (var title in titles)
                        {
                            if (line.StartsWith(title))
                            {
                                line = "<strong><em>" + title + "</em></strong>" + line.Substring(title.Length);
                                break;
                            }
                        }
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

        private static async Task<IEnumerable<string>> LoadList(string filename)
        {
            using (var stream = new StreamReader(filename))
            {
                var lines = new List<string>();
                var line = await stream.ReadLineAsync();
                while (line != null)
                {
                    if(!string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                    line = await stream.ReadLineAsync();
                }
                return lines;
            }
        }
    }
}
