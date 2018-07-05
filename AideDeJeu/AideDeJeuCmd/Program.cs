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
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

                var spellss = document.ToSpells<SpellHD>();
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

                var monsters = document.ToMonsters<MonsterHD>();
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
            var items = AideDeJeu.Tools.MarkdownExtensions.MarkdownToSpells<SpellHD>(md);

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

            //var regex = new Regex("# (?<namevo>.*?)\n- NameVO: \\[(?<namevf>.*?)\\]\n");
            //var matches = regex.Matches(mdVO);
            //foreach(Match match in matches)
            //{
            //    var nameVF = match.Groups["namevf"].Value;
            //    var nameVO = match.Groups["namevo"].Value;
            //    var replaceOld = string.Format("# {0}\n", nameVF);
            //    var replaceNew = string.Format("# {0}\n- NameVO: [{1}](monsters_vo.md#{2})\n", nameVF, nameVO, Helpers.IdFromName(nameVO));
            //    mdVF = mdVF.Replace(replaceOld, replaceNew);
            //}

            var regex = new Regex("_\\[(?<name>.*?)\\]_");
            var matches = regex.Matches(mdVF);
            var names = new List<string>();
            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                if (!mdVF.Contains($"[{name}]:"))
                {
                    //Console.WriteLine(name);
                    names.Add(name);
                }
            }
            //names.Sort();
            names = names.OrderBy(n => n).Distinct().ToList();
            foreach(var name in names)
            {
                Console.WriteLine($"[{name}]: spells_hd.md#{Helpers.IdFromName(Helpers.Capitalize(name))}");
            }

            Console.WriteLine(mdVF);
            await SaveStringAsync(dataDir + "monsters_hd_tmp.md", mdVF);

            return;

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
