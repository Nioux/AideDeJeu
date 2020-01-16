using AideDeJeu.Pdf;
using AideDeJeu.Tools;
using AideDeJeu.ViewModels;
using AideDeJeuLib;
using Markdig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AideDeJeuCmd
{
    class Program
    {

        //static async Task<IEnumerable<Spell>> TestMarkdown(string filename)
        //{
        //    using (var sr = new StreamReader(filename))
        //    {
        //        var md = await sr.ReadToEndAsync();
        //        var document = Markdig.Parsers.MarkdownParser.Parse(md);
        //        //DumpMarkdownDocument(document);

        //        var spellss = document.ToSpells<SpellHD>();
        //        Console.WriteLine("ok");
        //        var md2 = spellss.ToMarkdownString();
        //        if (md.CompareTo(md2) != 0)
        //        {
        //            Debug.WriteLine("failed");
        //        }
        //        return spellss;
        //    }
        //}

        static async Task<IEnumerable<MonsterItem>> TestMarkdownMonsters(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var md = await sr.ReadToEndAsync();
                var pipeline = new MarkdownPipelineBuilder()
                    .UseYamlFrontMatter()
                    .UsePipeTables()
                    .Build();
                //var document = Markdig.Parsers.MarkdownParser.Parse(md, pipeline);
                //DumpMarkdownDocument(document);
                var monsters = DependencyService.Get<StoreViewModel>().ToItem(filename, md, null) as IEnumerable<MonsterItem>; // document.ToMonsters<MonsterHD>();
                //document.Dump();
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
            var items = DependencyService.Get<StoreViewModel>().ToItem("spells_hd", md, null) as IEnumerable<SpellItem>;

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

        static async Task<IEnumerable<string>> GetAllAnchorsAsync()
        {
            var anchors = new List<string>();
            var allitems = new Dictionary<string, Item>();
            var names = Helpers.GetResourceNames();
            foreach (var name in names)
            {
                //if (name.Contains("_hd."))
                //{
                var md = await Helpers.GetResourceStringAsync(name);
                var item = DependencyService.Get<StoreViewModel>().ToItem(name, md, null);
                allitems.Add(name, item);
                //}
            }
            foreach (var allitem in allitems)
            {
                if (allitem.Value is Items)
                {
                    var items = allitem.Value as Items;
                    foreach (var item in await items.GetChildrenAsync())
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name))
                        {
                            //Console.WriteLine(item.Name);
                            anchors.Add(item.Name);
                        }
                    }
                }
                else if (allitem.Value != null)
                {
                    if (!string.IsNullOrWhiteSpace(allitem.Value.Name))
                    {
                        //Console.WriteLine(allitem.Value.Name);
                        anchors.Add(allitem.Value.Name);
                    }
                }
            }
            return anchors;
        }

        static async Task SearchAsync(string anchor)
        {
            var first = true;
            var names = Helpers.GetResourceNames();
            foreach (var name in names)
            {
                if (name.EndsWith("_hd.md"))
                {
                    var md = await Helpers.GetResourceStringAsync(name);
                    using (var reader = new StringReader(md))
                    {
                        var line = await reader.ReadLineAsync();
                        while (line != null)
                        {
                            if (line.FirstOrDefault() != '#' &&
                                !line.StartsWith("- AltName") &&
                                line.Contains(anchor) &&
                                !line.Contains($"[{anchor}") &&
                                !line.Contains($"{anchor}]")
                                )
                            {
                                if (first)
                                {
                                    first = false;
                                    Console.WriteLine();
                                    Console.WriteLine(anchor);
                                    Console.WriteLine();
                                }
                                Console.WriteLine(line);
                                Console.WriteLine();
                            }
                            line = await reader.ReadLineAsync();
                        }
                    }
                }
            }
            //Console.WriteLine();
        }

        static async Task ReorderSpellsAsync()
        {
            string dataDir = @"..\..\..\..\..\Data\";
            var mdVF = await LoadStringAsync(dataDir + "spells_hd.md");
            var mdVO = await LoadStringAsync(dataDir + "spells_vo.md");
            var md = mdVO;

            StringBuilder mdOut = new StringBuilder();
            using (var writer = new StringWriter(mdOut) { NewLine = "\n" })
            {
                using (var reader = new StringReader(md))
                {
                    var line = await reader.ReadLineAsync();
                    string levelType = null;
                    string castingTime = null;
                    string range = null;
                    string components = null;
                    string duration = null;
                    string classes = null;
                    string source = null;
                    while (line != null)
                    {
                        if (line.StartsWith("- ") && !line.StartsWith("- AltName:"))
                        {
                            if (line.StartsWith("- LevelType:"))
                            {
                                levelType = line;
                            }
                            else if (line.StartsWith("- **Temps d'incantation :**") || line.StartsWith("- **Casting Time :**"))
                            {
                                castingTime = line;
                            }
                            else if (line.StartsWith("- **Portée :**") || line.StartsWith("- **Range :**"))
                            {
                                range = line;
                            }
                            else if (line.StartsWith("- **Composantes :**") || line.StartsWith("- **Components :**"))
                            {
                                components = line;
                            }
                            else if (line.StartsWith("- **Durée :**") || line.StartsWith("- **Duration :**"))
                            {
                                duration = line;
                            }
                            else if (line.StartsWith("- Classes:"))
                            {
                                classes = line;
                            }
                            else if (line.StartsWith("- Source:"))
                            {
                                source = line;
                            }
                            else
                            {
                                Console.WriteLine(line);
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            if (levelType != null)
                            {
                                await writer.WriteLineAsync(levelType);
                                if (castingTime != null) await writer.WriteLineAsync(castingTime);
                                if (range != null) await writer.WriteLineAsync(range);
                                if (components != null) await writer.WriteLineAsync(components);
                                if (duration != null) await writer.WriteLineAsync(duration);
                                if (classes != null) await writer.WriteLineAsync(classes);
                                if (source != null) await writer.WriteLineAsync(source);
                                levelType = null;
                                castingTime = null;
                                range = null;
                                components = null;
                                duration = null;
                                classes = null;
                                source = null;
                            }
                            await writer.WriteLineAsync(line);
                        }
                        line = await reader.ReadLineAsync();
                    }
                }
            }
            await SaveStringAsync(dataDir + "spells_vo_rev.md", mdOut.ToString());
            Console.Write(mdOut);
            Console.ReadLine();
        }


        static string inDir = @"..\..\..\..\..\Data\";

        public static async Task PreloadAllItemsFromFilesAsync(StoreViewModel store)
        {
            foreach (var fileName in Directory.GetFiles(inDir, "*.md", new EnumerationOptions() { MatchType = MatchType.Simple, RecurseSubdirectories = false }))
            {
                //foreach (var resourceName in Tools.Helpers.GetResourceNames())
                //{
                var shortName = fileName.Substring(inDir.Length);
                var regex = new Regex(@"(?<name>.*?)\.md");
                var match = regex.Match(shortName);
                var source = match.Groups["name"].Value;
                if (!string.IsNullOrEmpty(source))
                {
                    if (!store._AllItems.ContainsKey(source))
                    {
                        //ar md = await AideDeJeu.Tools.Helpers.GetResourceStringAsync(resourceName);
                        var md = await File.ReadAllTextAsync(fileName);
                        if (md != null)
                        {
                            var item = store.ToItem(source, md, store._AllItems);
                            if (item != null)
                            {
                                if (item.NewId == "hd_aasimar_aasimar.md")
                                {
                                    Debug.WriteLine("");
                                }
                                var anchors = new Dictionary<string, Item>();
                                //MakeAnchors(source, anchors, item);
                                item.RootId = $"{source}.md";
                                store._AllItems[source] = item;
                            }
                        }
                    }
                }
            }
        }

        public static async Task<Dictionary<string, string>> LoadMDsFromFilesAsync()
        {
            var dico = new Dictionary<string, string>();
            foreach (var fileName in Directory.GetFiles(inDir, "*.md", new EnumerationOptions() { MatchType = MatchType.Simple, RecurseSubdirectories = false }))
            {
                var md = await File.ReadAllTextAsync(fileName);
                if (md != null)
                {
                    dico[fileName] = md;
                }
            }
            return dico;
        }
        static string outDir = @"..\..\..\..\..\Data\HD\";

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("l : build library");
                Console.WriteLine("o : check orphan links");
                Console.WriteLine("p : test pdf");
                Console.WriteLine("h : extract html");
                Console.WriteLine("y : extract yaml");
                Console.WriteLine("m : convert maps");
                Console.WriteLine("q : quitter");
                var key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'l':
                        Console.WriteLine("> build library");
                        await BuildLibraryAsync();
                        Console.WriteLine("/ build library");
                        break;
                    case 'o':
                        Console.WriteLine("> check orphan links");
                        await CheckOrphanLinksAsync();
                        Console.WriteLine("/ check orphan links");
                        break;
                    case 'p':
                        Console.WriteLine("> test pdf");
                        await TestPdfAsync();
                        Console.WriteLine("/ test pdf");
                        break;
                    case 'h':
                        Console.WriteLine("> extract html");
                        await ExtractHtmlAsync();
                        Console.WriteLine("/ extract html");
                        break;
                    case 'y':
                        Console.WriteLine("> extract yaml");
                        await ExtractYamlAsync();
                        Console.WriteLine("/ extract yaml");
                        break;
                    case 'm':
                        Console.WriteLine("> convert maps");
                        await ConvertMapsAsync();
                        Console.WriteLine("/ convert maps");
                        break;
                    case 'q':
                        return;
                }
            }
        }

        static string nsSvg = "http://www.w3.org/2000/svg";
        static async Task ConvertMapsAsync()
        {
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\osgild");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\ferrance");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\fourche");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\hauterive");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\port-sable");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\vercelise");
            await ConvertMapAsync(@"..\..\..\..\..\Docs\Osgild\xelys");
        }
        static async Task ConvertMapAsync(string basename)
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.Load($"{basename}.map.html");
            var svg = new XmlDocument();
            var svgElt = svg.CreateElement("svg", nsSvg);
            svgElt.SetAttribute("style", "fill: transparent");
            svg.AppendChild(svgElt);

            var img = document.DocumentNode.SelectSingleNode("img");
            var image = svg.CreateElement("image", nsSvg);
            image.SetAttribute("href", img.GetAttributeValue("src", ""));
            var width = img.GetAttributeValue("width","");
            var height = img.GetAttributeValue("height", "");
            svgElt.SetAttribute("viewBox", $"0 0 {width} {height}");
            svgElt.AppendChild(image);

            var areas = document.DocumentNode.SelectNodes("//area");
            foreach(var area in areas)
            {
                var coords = area.GetAttributeValue("coords", "");
                var coordsSplit = coords.Split(",");

                var a = svg.CreateElement("a", nsSvg);
                a.SetAttribute("href", area.GetAttributeValue("href", ""));
                a.SetAttribute("target", area.GetAttributeValue("target", ""));
                var shapeAttr = area.GetAttributeValue("shape", "");
                XmlElement shape = null;
                if (shapeAttr == "rect")
                {
                    shape = svg.CreateElement("rect", nsSvg);
                    shape.SetAttribute("x", coordsSplit[0]);
                    shape.SetAttribute("y", coordsSplit[1]);
                    shape.SetAttribute("width", (int.Parse(coordsSplit[2]) - int.Parse(coordsSplit[0])).ToString());
                    shape.SetAttribute("height", (int.Parse(coordsSplit[3]) - int.Parse(coordsSplit[1])).ToString());
                }
                if (shapeAttr == "circle")
                {
                    shape = svg.CreateElement("circle", nsSvg);
                    shape.SetAttribute("cx", coordsSplit[0]);
                    shape.SetAttribute("cy", coordsSplit[1]);
                    shape.SetAttribute("r", coordsSplit[2]);
                }
                var title = svg.CreateElement("title", nsSvg);
                title.InnerText = area.GetAttributeValue("alt", "");
                shape.AppendChild(title);
                a.AppendChild(shape);
                svgElt.AppendChild(a);
            }
            svg.Save($"{basename}.svg");
        }

        static async Task ExtractYamlAsync()
        {
            var tomeOfBeasts = await LoadStringAsync(@"..\..\..\..\..\Data\tome_of_beasts.md");
            var monstersHD = await LoadStringAsync(@"..\..\..\..\..\Data\monsters_hd.md");
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            var terrainLines = new Dictionary<string, string>();
            using (var reader = new StreamReader(@"..\..\..\..\..\Ignore\Index Bestiaires H&D.yaml"))
            {
                var terrains = deserializer.Deserialize(reader) as Dictionary<object, object>;
                foreach(var terrain in terrains)
                {
                    var terrainName = terrain.Key as string;
                    var monsters = terrain.Value as List<object>;
                    foreach(var monster in monsters)
                    {
                        var monsterName = monster as string;
                        if (tomeOfBeasts.Contains($"<!--Name-->{monsterName}<!--/Name-->", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Debug.WriteLine($"TOB : {terrainName} => {monsterName}");
                            if(terrainLines.ContainsKey(monsterName))
                            {
                                terrainLines[monsterName] += ", ";
                            }
                            else
                            {
                                terrainLines[monsterName] = "";
                            }
                            terrainLines[monsterName] += terrainName;
                            
                        }
                        else if (monstersHD.Contains($"<!--Name-->{monsterName}<!--/Name-->", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Debug.WriteLine($"CEO : {terrainName} => {monsterName}");
                        }
                        else
                        {
                            Console.WriteLine($"{terrainName} => {monsterName}");
                        }
                    }

                }
                Debug.WriteLine(true);
                foreach(var terrainLine in terrainLines)
                {
                    var monsterName = terrainLine.Key;
                    var terrainName = terrainLine.Value;
                    tomeOfBeasts = Regex.Replace(
                        tomeOfBeasts,
                        Regex.Escape($"<!--Name-->{monsterName}<!--/Name-->") + "\n\n- Source:.*?\n- TOB:.*?\n-  <!--Type-->.*?\n",
                        delegate (Match match)
                        {
                            //var ret = $"<!--Name-->{monsterName}<!--/Name-->" + $"\n\n{match.Groups[1]}\n{match.Groups[2]}\n" + terrainName;
                            var ret = $"{match.Groups[0]}- **Terrain** <!--Terrain-->{terrainName}<!--/Terrain-->\n";
                            return ret;
                        },
                        //terrainName, 
                        RegexOptions.IgnoreCase);

                }
            }
            await SaveStringAsync(@"..\..\..\..\..\Data\tome_of_beasts_bis.md", tomeOfBeasts);
        }

        static async Task ExtractHtmlAsync()
        {
            using (var output = new StreamWriter(@"..\..\..\..\..\Data\tome_of_beasts.md", false, Encoding.UTF8))
            {
                var parser = new HtmlParser();
                for (int i = 10; i <= 428; i++)
                //for (int i = 256; i <= 256; i++)
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    doc.Load($@"..\..\..\..\..\Ignore\tome_of_beasts\page{i}.html");
                    parser.OutputMarkdown(parser.Parse(doc), output, Console.Error);
                    //parser.OutputMarkdown(parser.Parse(doc), Console.Out, Console.Error);
                }
                output.Write("\n<!--/MonsterItem-->\n\n<!--/MonsterItems-->\n");
            }
        }

        class HtmlParser
        {
            string key = "";
            string value = "";
            enum State
            {
                Before,
                Name,
                Type,
                TopKeyValues,
                Abilities,
                BottomKeyValues,
                Competencies,
                Actions,
                Reactions,

            }

            public class ParsedSpan
            {
                public string Text;
                public string Style;
                public string IdStyle;
                public string DivStyle;
            }
            public class FullLine : List<ParsedSpan>
            {
            }

            public class FullText : List<FullLine>
            {

            }
            public FullText Parse(HtmlAgilityPack.HtmlDocument doc)
            {
                var styles = doc.DocumentNode.SelectSingleNode("/html/head/style").InnerText.Split('\n');
                var txtDivs = doc.DocumentNode.SelectNodes("//div[@class='txt']");
                var fullText = new FullText();
                var fullLine = new FullLine();
                if (txtDivs != null)
                {
                    foreach (var txtDiv in txtDivs)
                    {
                        var spans = txtDiv.Elements("span");
                        for (var i = 0; i < spans.Count(); i++)
                        {
                            var span = spans.ToArray()[i];
                            var spanId = span.GetAttributeValue("id", "");
                            var spanStyle = span.GetAttributeValue("style", "");
                            var spanIdStyle = new string(styles.SingleOrDefault(s => s.StartsWith($"#{spanId} ")).SkipWhile(c => c != '{').ToArray());
                            var divStyle = txtDiv.GetAttributeValue("style", "");
                            var parsedSpan = new ParsedSpan()
                            {
                                Text = span.InnerText.Replace(" ",""),
                                Style = spanStyle,
                                IdStyle = spanIdStyle,
                                DivStyle = divStyle,
                            };
                            if (i == 0)
                            {
                                var previousParsedSpan = fullLine.LastOrDefault();
                                if (previousParsedSpan == null)
                                {
                                    var previousFullLine = fullText.LastOrDefault();
                                    if (previousFullLine != null)
                                    {
                                        previousParsedSpan = previousFullLine.LastOrDefault();
                                    }
                                }

                                if (previousParsedSpan != null)
                                {
                                    if (previousParsedSpan.Style != parsedSpan.Style || previousParsedSpan.IdStyle != parsedSpan.IdStyle)
                                    {
                                        fullText.Add(fullLine);
                                        fullLine = new FullLine();
                                    }
                                }
                            }
                            fullLine.Add(parsedSpan);
                        }
                    }
                    fullText.Add(fullLine);
                }

                return fullText;
            }

            string idssnn = "{ font-family:sans-serif; font-weight:normal; font-style:normal; }";
            string idssni = "{ font-family:sans-serif; font-weight:normal; font-style:italic; }";
            string idssbn = "{ font-family:sans-serif; font-weight:bold; font-style:normal; }";
            string idssbi = "{ font-family:sans-serif; font-weight:bold; font-style:italic; }";
            string idsbn = "{ font-family:serif; font-weight:bold; font-style:normal; }";
            string idsnn = "{ font-family:serif; font-weight:normal; font-style:normal; }";
            string idsni = "{ font-family:serif; font-weight:normal; font-style:italic; }";

            string st23_255 = "font-size:23px;vertical-align:baseline;color:rgba(255,207,52,1);";
            string st16_255 = "font-size:16px;vertical-align:baseline;color:rgba(255,207,52,1);";
            string st11_255 = "font-size:11px;vertical-align:baseline;color:rgba(255,207,52,1);";
            string st48_0 = "font-size:48px;vertical-align:baseline;color:rgba(0,0,0,1);";
            string st14_137 = "font-size:14px;vertical-align:baseline;color:rgba(137,23,26,1);";
            string st8_0 = "font-size:8px;vertical-align:baseline;color:rgba(0,0,0,1);";
            string st8_121 = "font-size:8px;vertical-align:baseline;color:rgba(121,27,16,1);";
            string st8_137 = "font-size:8px;vertical-align:baseline;color:rgba(137,23,26,1);";
            string st9_203 = "font-size:9px;vertical-align:baseline;color:rgba(203,0,0,1);";

            bool started = false;

            public string MDStyle(string text, string style)
            {
                text = text.Trim();
                if (style.Contains("italic"))
                {
                    text = $"_{text}_";
                }
                if (style.Contains("bold"))
                {
                    text = $"**{text}**";
                }
                return text;
            }

            List<KeyValuePair<string, string>> MDSizes = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Petite/Très Grande", "P/TG"),
                new KeyValuePair<string, string>("Très Petite", "TP"),
                new KeyValuePair<string, string>("Très Grande", "TG"),
                new KeyValuePair<string, string>("Grande", "G"),
                new KeyValuePair<string, string>("Moyenne", "M"),
                new KeyValuePair<string, string>("Gigantesque", "Gig"),
                //new KeyValuePair<string, string>("Moyenne (métamorphe)", "M"),
                //new KeyValuePair<string, string>("Très Petite taille ", "TP"),
                new KeyValuePair<string, string>("Petite", "P"),
                //new KeyValuePair<string, string>("Petite taille (cynome)", "P"),
                //new KeyValuePair<string, string>("Grande taille d’élémentaires de taille Minuscule", "G"),
            };

            string ToMDSize(string size)
            {
                foreach(var mdsize in MDSizes)
                {
                    if(size.Contains(mdsize.Key))
                    {
                        return mdsize.Value;
                    }
                }
                return size;
            }
            public void OutputMarkdown(FullText fullText, TextWriter output, TextWriter error)
            {
                var page = fullText.Where(l => l.FirstOrDefault().Style.Contains(st16_255))?.FirstOrDefault()?.FirstOrDefault()?.Text;
                Console.ForegroundColor = ConsoleColor.Yellow;
                error.WriteLine($"Page : {page}");
                Console.ForegroundColor = ConsoleColor.White;
                string abilities = null;
                foreach (var line in fullText)
                {
                    var keySpan = line.FirstOrDefault();
                    string value = "";
                    if (line.Count > 1)
                    {
                        value = line.Skip(1).Select(p => MDStyle(p.Text, p.Style)).Aggregate((p1, p2) => p1.Trim() + " " + p2.Trim());
                    }
                    string text = MDStyle(keySpan.Text, keySpan.Style);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    error.Write($"{text}");
                    error.WriteLine($" {value}");

                    if (keySpan.Style.Contains(st48_0) && keySpan.IdStyle.Contains(idsnn))
                    {   // titre de page
                        Console.ForegroundColor = ConsoleColor.Blue;
                        error.Write($"{text} {value}\n");
                    }
                    else if (keySpan.Style.Contains(st9_203) && keySpan.IdStyle.Contains(idssnn))
                    {   // bloodmark
                        Console.ForegroundColor = ConsoleColor.Blue;
                        error.Write($"{text} {value}\n");
                    }
                    else if (keySpan.Style.Contains(st16_255) && keySpan.IdStyle.Contains(idsbn))
                    {   // page
                        Console.ForegroundColor = ConsoleColor.Blue;
                        error.Write($"{text} {value}\n");
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idsnn))
                    {   // encadré
                        Console.ForegroundColor = ConsoleColor.Blue;
                        error.Write($"{text} {value}\n");
                    }
                    else if (keySpan.Style.Contains(st11_255) && keySpan.IdStyle.Contains(idssnn))
                    {   // nom
                        Console.ForegroundColor = ConsoleColor.White;
                        if(!started)
                        {
                            started = true;
                            output.Write("\n<!--MonsterItems Family=\"TomeOfBeasts\" Types=\"Humanoïde|Aberration|Bête|Céleste|Créature artificielle|Créature monstrueuse|Dragon|Élémentaire|Fée|Fiélon|Géant|Mort-vivant|Plante|Vase\" Challenges=\"0 (0 PX)|1/8 (25 PX)|1/4 (50 PX)|1/2 (100 PX)|1 (200 PX)|2 (450 PX)|3 (700 PX)|4 (1100 PX)|5 (1800 PX)|6 (2300 PX)|7 (2900 PX)|8 (3900 PX)|9 (5000 PX)|10 (5900 PX)|11 (7200 PX)|12 (8400 PX)|13 (10000 PX)|14 (11500 PX)|15 (13000 PX)|16 (15000 PX)|17 (18000 PX)|18 (20000 PX)|19 (22000 PX)|20 (25000 PX)|21 (33000 PX)|22 (41000 PX)|23 (50000 PX)|24 (62000 PX)|30 (155000 PX)\" Sizes=\"TP|P|M|G|TG|Gig\" Sources=\"CEO|SRD\" Terrains=\"Arctique / Subarctique|Bois / Forêt|Collines / Vallées|Désert chaud|Jungle|Littoral|Mangrove / Marécage|Mer / Océan|Montagnes|Plaine / Champs / Prairie / Savane|Plans élémentaires|Caverne aménagée|Caverne naturelle|Caverne sous-marine|Donjon maçonné|Ruines extérieures|Ruines souterraines|Ruines sous-marines\"-->\n\n");
                            output.Write("# <!--Name-->Livre des monstres<!--/Name-->\n\n");
                        }
                        else
                        {
                            output.Write("\n<!--/MonsterItem-->\n\n");
                        }
                        output.Write("<!--MonsterItem Family=\"TomeOfBeasts\"-->\n\n");
                        output.Write($"# <!--Name-->{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower())}<!--/Name-->\n\n- Source: <!--Source-->(LDM p{page})<!--/Source-->\n");
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idssni) && text.Contains("taille"))
                    {   // type / size / alignment
                        // todo : découper type / size / alignment
                        var regex = new Regex("^(?<type>.*?) de (taille )?(?<size>.*?)( taille)?, (?<alignment>.*?)$");
                        var match = regex.Match(text);
                        var type = match.Groups["type"].Value;
                        var size = match.Groups["size"].Value;
                        var alignment = match.Groups["alignment"].Value;
                        if (type.Length > 0)
                        {
                            text = text.Replace(type, $"<!--Type-->{type}<!--/Type-->");
                            text = text.Replace(",", $" (<!--Size-->{ToMDSize(size)}<!--/Size-->),");
                            text = text.Replace(alignment, $"<!--Alignment-->{alignment}<!--/Alignment-->");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        output.Write($"-  {text}\n");
                    }
                    else if (keySpan.Style.Contains(st8_121) && keySpan.IdStyle.Contains(idssbn))
                    {   // key / ...
                        string tag = "";
                        if (KeyTags.ContainsKey(text.Trim()))
                        {
                            tag = KeyTags[text.Trim()];

                            Console.ForegroundColor = ConsoleColor.White;
                            output.Write($"- **{text.Trim()}** <!--{tag}-->{value}<!--/{tag}-->\n");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            //error.WriteLine($"ABILITIES");
                            if (abilities == null)
                            {
                                abilities = "";
                            }
                        }
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idssnn))
                    {   // ... / value
                        if (abilities != null)
                        {
                            abilities += text;
                            if(value.Length > 0)
                            {
                                abilities += $" {value}";
                            }
                            if (abilities.Count(c => c == '(') == 6)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                output.Write($"\n|FOR|DEX|CON|INT|SAG|CHA|\n|---|---|---|---|---|---|\n|{abilities.Replace(") ", ")").Replace(")", ")|")}\n\n");
                                abilities = null;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            output.Write($"{text} {value}\n");
                        }
                    }
                    else if (keySpan.Style.Contains(st14_137) && keySpan.IdStyle.Contains(idsnn))
                    {   // actions / réactions
                        Console.ForegroundColor = ConsoleColor.White;
                        output.Write($"\n## {text}\n{value}\n");
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idssnn))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        output.Write($"{text.Trim()} {value}\n");
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idssbi))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        output.Write($"\n**_{text.Trim()}_** {value}\n");
                    }
                    else if (keySpan.Style.Contains(st8_0) && keySpan.IdStyle.Contains(idssbn))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        output.Write($"\n**{text.Trim()}** {value}\n");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        error.Write($"{text} {value}\n");
                        error.Write($"{keySpan.Style}\n");
                        error.Write($"{keySpan.IdStyle}\n");
                    }
                }
            }

            void StripLine()
            {

            }

            Dictionary<string, string> KeyTags = new Dictionary<string, string>()
            {
                { "Jets de sauvegarde", "SavingThrows" },
                { "Classe d’armure" , "ArmorClass" },
                { "Points de vie", "HitPoints" },
                { "Vitesse", "Speed" },
                { "Compétences", "Skills" },
                { "Sens", "Senses" },
                { "Langues", "Languages" },
                { "Dangerosité", "Challenge" },
                { "Résistance aux dégâts", "DamageResistances" },
                { "Immunité contre les dégâts", "DamageImmunities" },
                { "Immunité contre les états", "ConditionImmunities" },
                { "Immunité contre l’état", "ConditionImmunities" },
                { "Vulnérabilité aux dégâts", "DamageVulnerabilities" },
                //{ "", "" },
                //{ "", "" },
                //{ "", "" },
                //{ "", "" },
                //{ "", "" },
            };
            List<string> KeyCaracs = new List<string>()
            {
                "FOR", "DEX", "CON", "INT", "SAG", "CHA"
            };

            string caracs = null;
            bool CloseKeyValue()
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (KeyTags.ContainsKey(key.Trim()))
                    {
                        var tag = KeyTags[key.Trim()];
                        Console.WriteLine($"- **{key.Trim()}** <!--{tag}-->{value.Trim()}<!--/{tag}-->");
                    }
                    else if (KeyCaracs.Contains(key.Trim()))
                    {
                        if (key.Trim() == "FOR")
                        {
                            Console.WriteLine("|FOR|DEX|CON|INT|SAG|CHA|\n|---|---|---|---|---|---|");
                        }
                    }
                    else
                    {
                        Console.WriteLine(value);
                    }
                    key = "";
                    value = "";
                    return true;
                }
                return false;
            }
        }
        static async Task TestPdfAsync()
        {
            Tests.Xamarin.Forms.Mocks.MockForms.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            DependencyService.Register<INativeAPI, AideDeJeu.Cmd.Version_CMD>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Xamarin.Essentials.Platform.Init(this, bundle);
            //Xamarin.Essentials.ExperimentalFeatures.Enable(Xamarin.Essentials.ExperimentalFeatures.ShareFileRequest);

            var store = new StoreViewModel();

            var item = await store.GetItemFromDataAsync("spells_hd", "aide");

            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                var spells = await context.Spells.ToListAsync();

                var pdfService = new PdfService();
                //var pc = new AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterViewModel();
                //var pce = new AideDeJeu.ViewModels.PlayerCharacter.PlayerCharacterEditorViewModel();
                using (var stream = new FileStream("test.pdf", FileMode.Create))
                {
                    //pdfService.MarkdownToPdf("# mon titre\n\nhop", stream);
                    await pdfService.MarkdownToPdf(spells.Select(s => s.Markdown).Take(3).ToList(), stream);
                    //pdfService.MarkdownToPdf(new List<string>() { item.Markdown }, stream);
                    //var stream = new MemoryStream();
                    //pce.GeneratePdfToStream(pc, stream);
                }
            }
        }

        static async Task BuildLibraryAsync()
        {
            Tests.Xamarin.Forms.Mocks.MockForms.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            DependencyService.Register<INativeAPI, AideDeJeu.Cmd.Version_CMD>();
            //var store = new StoreViewModel();
            //await store.GetItemFromDataAsync("test", "truc");

            var store = new StoreViewModel();
            //await store.PreloadAllItemsAsync();
            await PreloadAllItemsFromFilesAsync(store);

            var index = store._AllItems.Where(it => it.Value.RootId == "index.md").FirstOrDefault();
            index.Value.Id = index.Value.RootId;
            index.Value.Name = "Bibliothèque";

            using (var context = await StoreViewModel.GetLibraryContextAsync())
            {
                try
                {
                    await context.Database.EnsureDeletedAsync();
                }
                catch
                {

                }
                await context.Database.EnsureCreatedAsync();

                var flags = new Dictionary<string, bool>();
                foreach (var it in store._AllItems.Values)
                {
                    if (flags.ContainsKey(it.Id))
                    {
                        Debug.WriteLine(it);
                    }
                    flags[it.Id] = true;
                }
                await context.Items.AddRangeAsync(store._AllItems.Values);
                await context.SaveChangesAsync();

                var itemsSRD = await context.Items.Where(item => (item.Source != null && item.Source.Contains("SRD"))).ToListAsync();
                var monsters = await context.Monsters.ToListAsync();
                //var monstersHD = await context.MonstersHD.ToListAsync();
                //var monstersVO = await context.MonstersVO.ToListAsync();
                var spells = await context.Spells.ToListAsync();
                var classes = await context.Classes.ToListAsync();
                var races = await context.Races.ToListAsync();
                var backgrounds = await context.Backgrounds.ToListAsync();
                var items = await context.Items.ToListAsync();

                foreach (ClassItem c in classes)
                {
                    var parent = items.Where(it => it.Id == c.ParentLink).FirstOrDefault();
                    if (parent != null)
                    {
                        var pparent = items.Where(iit => iit.Id == parent.ParentLink).FirstOrDefault();
                        if (pparent != null && pparent is ClassItem)
                        {
                            var sc = c as SubClassItem;
                            sc.ParentClassId = pparent.NewId;
                            Console.WriteLine($"{pparent.Name} - {c.Name}");
                        }
                        else
                        {
                        }
                    }
                }

                foreach (var r in races)
                {
                    r.HasSubRaces = false;
                }
                foreach (var r in races)
                {
                    var parent = items.Where(it => it.Id == r.ParentLink).FirstOrDefault();
                    if (parent != null && parent is RaceItem)
                    {
                        var sr = r as SubRaceItem;
                        sr.FullName = $"{parent.Name} - {r.Name}";
                        sr.ParentRaceId = parent.NewId;
                        (parent as RaceItem).HasSubRaces = true;
                        Console.WriteLine(sr.FullName);
                    }
                    else
                    {
                        r.FullName = r.Name;
                        Console.WriteLine($"{r.Name}");
                    }
                }

                //var item1 = monsters.FirstOrDefault();

                //var test1y = item1.Yaml;
                //var test1m = item1.Markdown;
                //var test1ym = item1.YamlMarkdown;

                //var item2 = Item.ParseYamlMarkdown(test1ym);

                //var test2y = item2.Yaml;
                //var test2m = item2.Markdown;
                //var test2ym = item2.YamlMarkdown;


                var matchids = new Dictionary<string, string>();

                foreach (var item in await context.Items.ToListAsync())
                {
                    matchids[item.Id] = item.NewId;
                    if (!string.IsNullOrEmpty(item.RootId))
                    {
                        matchids[item.RootId] = item.NewId;
                    }
                    // Helpers.RemoveDiacritics(item.Id).Replace(".md#", "_") + ".md";
                }

                foreach (var item in await context.Items.ToListAsync())
                {
                    //await item.LoadFilteredItemsAsync();
                    if (item is SpellItems)
                    {
                        int iii = 1;
                    }
                    var yaml = item.YamlMarkdown;
                    //var rx = new Regex(@"\(.*?\.md.*?\)");
                    //var matchess = rx.Matches(yaml);
                    //foreach (Match match in matchess)
                    //{
                    //    yaml = yaml.Replace(match.Value, matchids[match.Value]);
                    //}
                    foreach (var matchid in matchids)
                    {
                        yaml = yaml.Replace($"({matchid.Key})", $"({matchid.Value})");
                    }
                    var filename = Path.Combine(outDir, WebUtility.UrlEncode(item.NewId));
                    if (filename.Contains("%"))
                    {
                        Console.WriteLine(filename);
                    }
                    await SaveStringAsync(filename, yaml);

                    var filtervm = item.GetNewFilterViewModel();
                    if (filtervm != null)
                    {
                        foreach (var filter in filtervm.Filters)
                        {
                            foreach (var kv in filter.KeyValues)
                            {
                                var key = kv.Key;
                                var val = kv.Value;
                            }
                        }
                    }
                }
                int i = 1;
                await context.SaveChangesAsync();

                //var serializer = new SerializerBuilder()
                //    .WithTagMapping("!MonsterHD", typeof(MonsterHD))
                //    .WithTagMapping("!MonsterVO", typeof(MonsterVO))
                //    .WithTagMapping("!Monsters", typeof(List<Monster>))
                //    .EnsureRoundtrip()
                //    .WithNamingConvention(new PascalCaseNamingConvention())
                //    .Build();
                //var deserializer = new DeserializerBuilder()
                //    .WithTagMapping("!MonsterHD", typeof(MonsterHD))
                //    .WithTagMapping("!MonsterVO", typeof(MonsterVO))
                //    .WithTagMapping("!Monsters", typeof(List<Monster>))
                //    .WithNamingConvention(new PascalCaseNamingConvention())
                //    .Build();
                //var yaml = serializer.Serialize(monsters);
                //var sr = new StringReader(yaml);
                //var deser = deserializer.Deserialize(sr);

                //var truc = Item.ParseYamlMarkdown(monsters.FirstOrDefault().YamlMarkdown);
                //Console.WriteLine(yaml);
            }


            return;
        }

        static async Task CheckOrphanLinksAsync()
        {
            Tests.Xamarin.Forms.Mocks.MockForms.Init();
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            DependencyService.Register<INativeAPI, AideDeJeu.Cmd.Version_CMD>();
            //var store = new StoreViewModel();
            //await store.GetItemFromDataAsync("test", "truc");

            //var store = new StoreViewModel();
            //await store.PreloadAllItemsAsync();
            //await PreloadAllItemsFromFilesAsync(store);
            var mds = await LoadMDsFromFilesAsync();
            //await ReorderSpellsAsync();
            //return;
            //string dataDir = @"..\..\..\..\..\Data\";
            await CheckAllLinks(mds);
            //var anchors = await GetAllAnchorsAsync();
            //foreach (var anchor in anchors)
            //{
            //    await SearchAsync(anchor);
            //}
            return;
        }

        async Task test()
        {
            var dataDir = "";
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
            foreach (var name in names)
            {
                Console.WriteLine($"[{name}]: spells_hd.md#{Helpers.IdFromName(Helpers.Capitalize(name))}");
            }

            Console.WriteLine(mdVF);
            //await SaveStringAsync(dataDir + "monsters_hd_tmp.md", mdVF);

            return;

        }

        public static async Task CheckAllLinks(Dictionary<string, string> mds)
        {
            // string dataDir = @"..\..\..\..\..\Data\";

            var allmds = new Dictionary<string, string>();
            var allanchors = new Dictionary<string, IEnumerable<string>>();
            var alllinks = new Dictionary<string, IEnumerable<Tuple<string, string>>>();
            var allnames = new Dictionary<string, IEnumerable<string>>();
            //var resnames = Helpers.GetResourceNames();
            foreach (var mdkv in mds)
            {
                var name = mdkv.Key;
                var md = mdkv.Value;
                allmds.Add(name, md);
                var anchors = GetMarkdownAnchors(md).ToList();
                allanchors.Add(name, anchors);
                var links = GetMarkdownLinks(md).ToList();
                alllinks.Add(name, links);
                var names = GetMarkdownAnchorNames(md).ToList();
                allnames.Add(name, names);
            }
            foreach (var mdkv in mds)
            {
                var name = mdkv.Key;
                var md = mdkv.Value;
                var unlinkedrefs = GetMarkdownUnlinkedRefs(md).ToList();
                if (unlinkedrefs.Count > 0)
                {
                    Console.WriteLine($"{name} :");
                    Console.WriteLine();
                    foreach (var unlinkedref in unlinkedrefs.Distinct().OrderBy(i => i))
                    {
                        //var file = "";
                        var files = new Dictionary<string, string>();
                        foreach (var aalinks in alllinks)
                        {
                            var found = aalinks.Value.FirstOrDefault(t => t.Item2 == Helpers.IdFromName(unlinkedref));
                            if (found != null)
                            {
                                files[found.Item1] = $"{found.Item1}.md";
                                //file = $"{found.Item1}.md";
                                //Console.WriteLine($"[{unlinkedref}]: {file}#{Helpers.IdFromName(unlinkedref)}");
                            }
                        }
                        foreach (var aanchors in allanchors)
                        {
                            if (aanchors.Value.Contains(Helpers.IdFromName(unlinkedref)))
                            {
                                files[aanchors.Key] = $"{aanchors.Key}.md";
                                //file = $"{aanchors.Key}.md";
                                //Console.WriteLine($"[{unlinkedref}]: {file}#{Helpers.IdFromName(unlinkedref)}");
                                //break;
                            }
                        }
                        if (files.Count == 0)
                        {
                            files[""] = "";
                        }
                        foreach (var file in files)
                        {
                            Console.WriteLine($"[{unlinkedref}]: {file.Value}#{Helpers.IdFromName(unlinkedref)}");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }

            foreach (var links in alllinks)
            {
                foreach (var link in links.Value)
                {
                    var file = link.Item1;
                    var anchor = link.Item2;
                    if (allanchors.ContainsKey(file))
                    {
                        if (!allanchors[file].Contains(anchor))
                        {
                            Console.WriteLine($"{links.Key} => {file} {anchor}");
                        }
                    }
                }
            }

            //foreach (var names in allnames)
            //{
            //    foreach (var name in names.Value)
            //    {
            //        foreach(var mdkv in allmds)
            //        {
            //            FindName(mdkv.Value, name);
            //        }
            //    }
            //}
        }

        public static void FindName(string md, string name)
        {
            var index = md.IndexOf(name);
            while (index >= 0)
            {
                if ((md.Substring(index - 1, 1) != "[" ||
                    md.Substring(index + name.Length, 1) != "]") &&
                    md.Substring(index - 1, 1) != "#" &&
                    md.Substring(index - 2, 2) != "# ")
                {
                    Console.WriteLine(name);
                    Console.WriteLine(md.Substring(index - 10, name.Length + 20).Replace("\n", "\\n"));
                    Console.WriteLine();
                }
                index = md.IndexOf(name, index + 1);
            }
        }

        public static IEnumerable<Tuple<string, string>> GetMarkdownLinks(string md)
        {
            var regex = new Regex("[ \\(](?<file>[a-z_]*?)\\.md#(?<anchor>.*?)[\\n\\)]");
            var matches = regex.Matches(md);
            foreach (Match match in matches)
            {
                var file = match.Groups["file"].Value;
                var anchor = match.Groups["anchor"].Value;
                yield return new Tuple<string, string>(file, anchor);
            }
        }

        public static IEnumerable<string> GetMarkdownUnlinkedRefs(string md)
        {
            var regex = new Regex("\\[(?<ref>.+?)\\]", RegexOptions.IgnoreCase);
            var matches = regex.Matches(md);
            md = md.ToLower();
            foreach (Match match in matches)
            {
                var rref = match.Groups["ref"].Value;
                var lref = rref.ToLower();
                if (!md.Contains($"[{lref}]:") &&
                    !md.Contains($"[{lref}](") &&
                    !lref.Contains("]["))
                {
                    yield return rref;
                }
            }
        }

        public static IEnumerable<string> GetMarkdownAnchors(string md)
        {
            foreach (var name in GetMarkdownAnchorNames(md))
            {
                yield return Helpers.IdFromName(name);
            }
        }

        public static IEnumerable<string> GetMarkdownAnchorNames(string md)
        {
            var regex = new Regex($"\\n##* (?<name>.*?)\\s*?\\n");
            var matches = regex.Matches(md);
            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                yield return name;
            }
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
