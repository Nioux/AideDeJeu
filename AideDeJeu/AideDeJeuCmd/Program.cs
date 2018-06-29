using AideDeJeu.Services;
using AideDeJeu.Tools;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
using Markdig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        static async Task<IEnumerable<Spell>> TestMarkdown(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var md = await sr.ReadToEndAsync();
                var document = Markdig.Parsers.MarkdownParser.Parse(md);
                //DumpMarkdownDocument(document);

                var spellss = document.ToSpells();
                Console.WriteLine("ok");
                var md2 = spellss.ToMarkdownString();
                if (md.CompareTo(md2) != 0)
                {
                    Debug.WriteLine("failed");
                }
                return spellss;
            }
        }

        static async Task<IEnumerable<Monster>> TestMarkdownMonsters(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var md = await sr.ReadToEndAsync();
                var pipeline = new MarkdownPipelineBuilder()
                    .UsePipeTables()
                    .Build();
                var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
                //DumpMarkdownDocument(document);

                var monsters = document.ToMonsters();
                document.Dump();
                Console.WriteLine("ok");
                //var md2 = monsters.ToMarkdownString();
                //if (md.CompareTo(md2) != 0)
                //{
                //    Debug.WriteLine("failed");
                //}
                return monsters;
            }
        }

        static async Task CreateIndexes()
        {
            string dataDir = @"..\..\..\..\..\Data\";

            var result = string.Empty;
            var md = await LoadStringAsync(dataDir + "spells_hd.md");
            var items = AideDeJeu.Tools.MarkdownExtensions.MarkdownToSpells(md);

            var classes = new string[]
            {
                "Barde",
                "Clerc",
                "Druide",
                "Ensorceleur",
                "Magicien",
                "Paladin",
                "Rôdeur",
                "Sorcier"
            };
            var levels = new string[]
            {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                //"tour de magie",
                //"niveau 1",
                //"niveau 2",
                //"niveau 3",
                //"niveau 4",
                //"niveau 5",
                //"niveau 6",
                //"niveau 7",
                //"niveau 8",
                //"niveau 9"
            };
            foreach (var classe in classes)
            {
                //Console.WriteLine(classe);
                result += string.Format("## {0}\n\n", classe);
                foreach (var level in levels)
                {
                    //Console.WriteLine(level);
                    var spells = items.Where(s => s.Level == level && s.Source.Contains(classe)).OrderBy(s => s.Name).Select(s => string.Format("* [{0}](spells_hd.md#{1})", s.Name, Helpers.IdFromName(s.Name))).ToList();
                    if (spells.Count > 0)
                    {
                        result += string.Format("### {0}\n\n", level == "0" ? "Tours de magie" : "Niveau " + level);
                        result += spells.Aggregate((s1, s2) => s1 + "\n" + s2);
                        result += "\n\n";
                    }
                }
            }
            Console.WriteLine(result);
            await SaveStringAsync(dataDir + "spells_hd_by_class_level.md", result);
        }

        static async Task Main(string[] args)
        {
            string dataDir = @"..\..\..\..\..\Data\";

            var mdVO = await LoadStringAsync(dataDir + "monsters_vo.md");
            var mdVF = await LoadStringAsync(dataDir + "monsters_hd.md");

            var regex = new Regex("# (?<namevo>.*?)\n- NameVO: \\[(?<namevf>.*?)\\]\n");
            var matches = regex.Matches(mdVO);
            foreach(Match match in matches)
            {
                var nameVF = match.Groups["namevf"].Value;
                var nameVO = match.Groups["namevo"].Value;
                var replaceOld = string.Format("# {0}\n", nameVF);
                var replaceNew = string.Format("# {0}\n- NameVO: [{1}](monsters_vo.md#{2})\n", nameVF, nameVO, Helpers.IdFromName(nameVO));
                mdVF = mdVF.Replace(replaceOld, replaceNew);
            }
            Console.WriteLine(mdVF);
            await SaveStringAsync(dataDir + "monsters_hd_tmp.md", mdVF);

            //var md = await LoadStringAsync(dataDir + "spells_vo.md");
            //var items = AideDeJeu.Tools.MarkdownExtensions.MarkdownToSpells(md);

            //var mdOut = string.Empty;
            //foreach (var item in items)
            //{
            //    mdOut += item.ToMarkdownString();
            //}

            //Console.WriteLine(mdOut);

            //await CreateIndexes();
            //var spellsVF = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vf_full.json");
            //var spellsVO = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vo_full.json");
            //var spellsHD = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_hd_full.json");
            //var monstersVF = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vf_full.json");
            //var monstersVO = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vo_full.json");

            //var result = string.Empty;
            //var md = await LoadStringAsync(dataDir + "spells_vo.md");

            //var regex = new Regex("\\[(?<name>.*?)\\]\\: spells_hd\\.md\\#(?<id>.*?)\n");
            //var matches = regex.Matches(md);
            //foreach(Match match in matches)
            //{
            //    Debug.WriteLine(match.Value);
            //    var oldMatch = match.Value;
            //    var name = match.Groups["name"].Value;
            //    var newMatch = string.Format("[{0}]: spells_hd.md#{1}\n", name, Helpers.IdFromName(name));

            //    md = md.Replace(oldMatch, newMatch);
            //}
            //await SaveStringAsync(dataDir + "spells_vo_tmp.md", md);
            //foreach(var spell in spellsVF)
            //{
            //    var nameAideDD = spell.Name;
            //    var nameHD = spell.NamePHB;
            //    if(!string.IsNullOrWhiteSpace(nameAideDD) && !string.IsNullOrWhiteSpace(nameHD))
            //    {
            //        if (nameAideDD != nameHD)
            //        {
            //            Debug.WriteLine(string.Format("{0} => {1}", nameAideDD, nameHD));

            //            md = md.Replace(
            //                string.Format("- NameVO: [{0}]", nameAideDD),
            //                string.Format("- NameVO: [{0}]", nameHD));

            //            var tmpmd = md.Replace(
            //                string.Format("[{0}]: spells_hd.md#{1}", nameAideDD, Helpers.OldIdFromName(nameAideDD)),
            //                string.Format("[{0}]: spells_hd.md#{1}", nameHD, Helpers.IdFromName(nameHD))
            //                );

            //            if(tmpmd == md)
            //            {
            //                Debug.WriteLine("ko");
            //            }
            //            md = tmpmd;
            //        }
            //    }
            //}
            /*var regex = new Regex("- NameVO: (?<name>.*?)\n");
            var matches = regex.Matches(md);
            foreach(Match match in matches)
            {
                var name = match.Groups["name"].Value;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Debug.WriteLine(name);
                    var oldNameVO = string.Format("- NameVO: {0}", name);
                    var newNameVO = string.Format("- NameVO: [{0}](spells_vo.md#{1})", name, Helpers.IdFromName(name));
                    md = md.Replace(oldNameVO, newNameVO);
                }
            }*/

            //var items = AideDeJeu.Tools.MarkdownExtensions.MarkdownToSpells(md);

            //await SaveStringAsync(dataDir + "spells_vo_tmp.md", md);
            //var regex = new Regex("(\\[[a-z].*?\\])");
            //var matches = regex.Matches(monstersVOmd);
            //var links = matches.OrderBy(m => m.Value).Select(m => m.Value + string.Format(": spells_vo.md#{0}", m.Value.Replace("[", "").Replace("]","").Replace(" ","-"))).Distinct().ToList().Aggregate((a, b) => a + "\n" + b);

            return;
            //var mdhd = spellsHD.ToMarkdownString();
            //var spellsMDHD = spellsHD.ToMarkdownString();
            //var spellsMDVO = spellsVO.ToMarkdownString();
            //var monstersMDVO = monstersVO.ToMarkdownString();
            //await SaveStringAsync(dataDir + "spells_vo.md", spellsMDVO);
            //await SaveStringAsync(dataDir + "monsters_vo.md", monstersMDVO);

            //using (var instream = new StreamReader(dataDir + "monsters_hd.md", Encoding.UTF8))
            //{
            //    using (var outstream = new StreamWriter(dataDir + "monsters_hd_modif.md", false, Encoding.UTF8))
            //    {
            //        var line = await instream.ReadLineAsync();
            //        while (line != null)
            //        {
            //            if (line.StartsWith("# "))
            //            {
            //                await outstream.WriteLineAsync(line);
            //                line = await instream.ReadLineAsync();
            //                await outstream.WriteLineAsync("- " + line);
            //            }
            //            else if(line.StartsWith("| ---   | ---   | ---   | ---   | ---   | ---   |"))
            //            {
            //                await outstream.WriteLineAsync(line);
            //                line = await instream.ReadLineAsync();
            //                var caracs = line.Substring(1).Split(' ');
            //                //var rx = new Regex("|(?<for>.*?) (?<bfor>\\(.*?)\\) (?<dex>.*?) (?<bdex>\\(.*?)\\) (?<con>.*?) (?<bcon>\\(.*?)\\) (?<int>.*?) (?<bint>\\(.*?)\\) (?<sag>.*?) (?<bsag>\\(.*?)\\) (?<cha>.*?) (?<bcha>\\(.*?)\\)");
            //                //var match = rx.Match(line);
            //                var outline = string.Format("|{0,2} {1,4}|{2,2} {3,4}|{4,2} {5,4}|{6,2} {7,4}|{8,2} {9,4}|{10,2} {11,4}|",
            //                    caracs
            //                    );
            //                await outstream.WriteLineAsync(outline);
            //                await outstream.WriteLineAsync(string.Empty);
            //            }
            //            else
            //            {
            //                await outstream.WriteLineAsync(line);
            //            }
            //            line = await instream.ReadLineAsync();
            //        }
            //    }
            //}
            return;
            //var spellss = await TestMarkdown(@"..\..\..\..\..\Data\spells_hd.md");
            //var monsterss = await TestMarkdownMonsters(@"..\..\..\..\..\Data\monsters_hd.md");
            //return;
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
            //var spellsVF = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vf_full.json");
            //var spellsVO = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_vo_full.json");
            //var spellsHD = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_hd_full.json");
            //var monstersVF = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vf_full.json");
            //var monstersVO = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vo_full.json");

            //var mdhd = spellsHD.ToMarkdownString();
            //await SaveStringAsync(dataDir + "spells_hd.md", mdhd);

            //spellsVF.ForEach(sp => sp.Html = null);
            //spellsVO.ForEach(sp => sp.Html = null);
            //spellsVF.ForEach(sp => sp.DescriptionDiv = sp.DescriptionDiv);
            //spellsVO.ForEach(sp => sp.DescriptionDiv = sp.DescriptionDiv);
            //monstersVF.ForEach(it => it.Html = null);
            //monstersVO.ForEach(it => it.Html = null);

            //SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vf.json", spellsVF);
            //SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vo.json", spellsVO);
            //SaveJSon<IEnumerable<Spell>>(dataDir + "spells_hd.json", spellsHD);
            //SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vf.json", monstersVF);
            //SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vo.json", monstersVO);
            //return;

            /*
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
            */
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
            var settings = new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true, "  "))
                {
                    serializer.WriteObject(writer, objT);
                }
            }
        }

        private static async Task SaveStringAsync(string filename, string text)
        {
            using (var sw = new StreamWriter(path: filename, append: false, encoding: Encoding.UTF8))
            {
                await sw.WriteAsync(text);
            }
        }

        private static async Task<string> LoadStringAsync(string filename)
        {
            using (var sr = new StreamReader(filename, Encoding.UTF8))
            {
                return await sr.ReadToEndAsync();
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
                    if (!string.IsNullOrEmpty(line))
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
