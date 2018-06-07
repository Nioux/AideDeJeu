using AideDeJeu.Services;
using AideDeJeuLib.Monsters;
using AideDeJeuLib.Spells;
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
    public static class MarkdownExtensions
    {
        public static string ToString(this Markdig.Syntax.SourceSpan span, string md)
        {
            return md.Substring(span.Start, span.Length);
        }
        public static string ToContainerString(this Markdig.Syntax.Inlines.ContainerInline inlines)
        {
            var str = string.Empty;
            foreach(var inline in inlines)
            {
                Console.WriteLine(inline.GetType());
                string add = string.Empty;
                if (inline is Markdig.Syntax.Inlines.LineBreakInline)
                {
                    add = "\n";
                }
                else if (inline is Markdig.Syntax.Inlines.LiteralInline)
                {
                    var literalInline = inline as Markdig.Syntax.Inlines.LiteralInline;
                    add = literalInline.Content.ToString();
                }
                else if (inline is Markdig.Syntax.Inlines.EmphasisInline)
                {
                    var emphasisInline = inline as Markdig.Syntax.Inlines.EmphasisInline;
                    var delimiterChar = emphasisInline.DelimiterChar.ToString();
                    if (emphasisInline.IsDouble)
                    {
                        delimiterChar += delimiterChar;
                    }
                    add = delimiterChar + emphasisInline.ToContainerString() + delimiterChar;
                }
                else if (inline is Markdig.Syntax.Inlines.ContainerInline)
                {
                    var containerInline = inline as Markdig.Syntax.Inlines.ContainerInline;
                    add = containerInline.ToContainerString();
                }
                else
                {
                    add = inline.ToString();
                }
                Console.WriteLine(add);
                str += add;
            }
            return str;
        }
        public static string ToParagraphString(this Markdig.Syntax.ParagraphBlock paragraphBlock)
        {
            var str = string.Empty;
            str += paragraphBlock.Inline.ToContainerString();
            if (paragraphBlock.IsBreakable)
            {
                str += "\n";
            }
            return str;
        }

        public static string ToMarkdownString(this IEnumerable<Spell> spells)
        {
            var md = string.Empty;
            foreach(var spell in spells)
            {
                md += spell.ToMarkdownString();
            }
            return md;
        }
        public static string ToMarkdownString(this Spell spell)
        {
            var md = string.Empty;
            md += string.Format("# {0}\n", spell.NamePHB);
            md += string.Format("- NameVO: {0}\n", spell.NameVO);
            md += string.Format("- CastingTime: {0}\n", spell.CastingTime);
            md += string.Format("- Components: {0}\n", spell.Components);
            md += string.Format("- Duration: {0}\n", spell.Duration);
            md += string.Format("- LevelType: {0}\n", spell.LevelType);
            md += string.Format("- Range: {0}\n", spell.Range);
            var regex = new Regex("(?<source>\\(.*\\)) (?<classes>.*)");
            var match = regex.Match(spell.Source);
            var source = match.Groups["source"].Value;
            var classes = match.Groups["classes"].Value;
            md += string.Format("- Source: {0}\n", source);
            md += string.Format("- Classes: {0}\n", classes.Replace(" ;", ",").Trim().Trim(','));
            md += "\n";
            md += "### Description\n\n";
            md += spell
                .DescriptionHtml
                .Replace("<strong>", "**")
                .Replace("</strong>", "**")
                .Replace("<em>", "_")
                .Replace("</em>", "_")
                .Replace("<li>", "* ")
                .Replace("</li>", "")
                .Replace("\n", "\r\n\r\n")
                .Replace("<br/>", "\r\n\r\n")
                ;
            md += "\n\n";
            return md;
        }
    }
    class Program
    {
        public class MarkdownConverter
        {
            public IEnumerable<Spell> MarkdownToSpells(string md)
            {
                var spells = new List<Spell>();
                var document = Markdig.Parsers.MarkdownParser.Parse(md);
                Spell spell = null;
                foreach (var block in document)
                {
                    //DumpBlock(block);
                    if (block is Markdig.Syntax.HeadingBlock)
                    {
                        var headingBlock = block as Markdig.Syntax.HeadingBlock;
                        //DumpHeadingBlock(headingBlock);
                        if (headingBlock.HeaderChar == '#' && headingBlock.Level == 1)
                        {
                            if (spell != null)
                            {
                                spells.Add(spell);
                            }
                            spell = new Spell();
                            spell.Name = spell.NamePHB = headingBlock.Inline.ToContainerString();
                            //Console.WriteLine(spell.Name);
                        }
                    }
                    if (block is Markdig.Syntax.ParagraphBlock)
                    {
                        var paragraphBlock = block as Markdig.Syntax.ParagraphBlock;
                        spell.DescriptionHtml += paragraphBlock.ToParagraphString();
                        ////DumpParagraphBlock(paragraphBlock);
                        //Console.WriteLine(paragraphBlock.IsBreakable);
                        //spell.DescriptionHtml += paragraphBlock.Inline.ToContainerString();
                        //if(paragraphBlock.IsBreakable)
                        //{
                        //    spell.DescriptionHtml += "\n";
                        //}
                    }
                    if (block is Markdig.Syntax.ListBlock)
                    {
                        var listBlock = block as Markdig.Syntax.ListBlock;
                        //DumpListBlock(listBlock);
                        if (listBlock.BulletType == '-')
                        {
                            spell.Source = "";
                            foreach (var inblock in listBlock)
                            {
                                //DumpBlock(inblock);
                                var regex = new Regex("(?<key>.*?): (?<value>.*)");
                                if (inblock is Markdig.Syntax.ListItemBlock)
                                {
                                    var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                    foreach (var ininblock in listItemBlock)
                                    {
                                        //DumpBlock(ininblock);
                                        if (ininblock is Markdig.Syntax.ParagraphBlock)
                                        {
                                            var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                            //DumpParagraphBlock(paragraphBlock);
                                            var str = paragraphBlock.Inline.ToContainerString();
                                            var match = regex.Match(str);
                                            var key = match.Groups["key"].Value;
                                            var value = match.Groups["value"].Value;
                                            switch (key)
                                            {
                                                case "NameVO":
                                                    spell.NameVO = value;
                                                    break;
                                                case "CastingTime":
                                                    spell.CastingTime = value;
                                                    break;
                                                case "Components":
                                                    spell.Components = value;
                                                    break;
                                                case "Duration":
                                                    spell.Duration = value;
                                                    break;
                                                case "LevelType":
                                                    spell.LevelType = value;
                                                    break;
                                                case "Range":
                                                    spell.Range = value;
                                                    break;
                                                case "Source":
                                                    spell.Source += value + " ";
                                                    break;
                                                case "Classes":
                                                    spell.Source += value;
                                                    break;
                                            }
                                        }
                                    }

                                    //DumpListItemBlock(inblock as Markdig.Syntax.ListItemBlock);
                                }
                            }
                        }
                        else
                        {
                            foreach (var inblock in listBlock)
                            {
                                if (inblock is Markdig.Syntax.ListItemBlock)
                                {
                                    var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
                                    foreach (var ininblock in listItemBlock)
                                    {
                                        //DumpBlock(ininblock);
                                        if (ininblock is Markdig.Syntax.ParagraphBlock)
                                        {
                                            var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
                                            spell.DescriptionHtml += listBlock.BulletType + " " + paragraphBlock.ToParagraphString();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (spell != null)
                {
                    spells.Add(spell);
                }
                return spells;
            }
        }

        static void DumpParagraphBlock(Markdig.Syntax.ParagraphBlock block)
        {
            //if (block.Lines != null)
            //{
            //    foreach (var line in block.Lines)
            //    {
            //        var stringline = line as Markdig.Helpers.StringLine?;
            //        Console.WriteLine(stringline.ToString());
            //    }
            //}
        }
        static void DumpListBlock(Markdig.Syntax.ListBlock block)
        {
            Console.WriteLine(block.BulletType);
            foreach(var inblock in block)
            {
                DumpBlock(inblock);
            }
        }
        static void DumpListItemBlock(Markdig.Syntax.ListItemBlock block)
        {
            foreach(var inblock in block)
            {
                DumpBlock(inblock);
            }
        }
        static void DumpHeadingBlock(Markdig.Syntax.HeadingBlock block)
        {
            Console.WriteLine(block.HeaderChar);
            Console.WriteLine(block.Level);
            //foreach(var line in block.Lines.Lines)
            //{
            //    DumpStringLine(line);
            //}
        }
        static void DumpStringLine(Markdig.Helpers.StringLine line)
        {
            Console.WriteLine(line.ToString());
        }
        static void DumpBlock(Markdig.Syntax.Block block)
        {
            Console.WriteLine(block.Column);
            Console.WriteLine(block.IsBreakable);
            Console.WriteLine(block.IsOpen);
            Console.WriteLine(block.Line);
            Console.WriteLine(block.RemoveAfterProcessInlines);
            Console.WriteLine(block.Span.ToString());
            //Console.WriteLine(block.Span.ToString(MD));
            Console.WriteLine(block.ToString());
            if(block is Markdig.Syntax.ParagraphBlock)
            {
                DumpParagraphBlock(block as Markdig.Syntax.ParagraphBlock);
            }
            if(block is Markdig.Syntax.ListBlock)
            {
                DumpListBlock(block as Markdig.Syntax.ListBlock);
            }
            if (block is Markdig.Syntax.HeadingBlock)
            {
                DumpHeadingBlock(block as Markdig.Syntax.HeadingBlock);
            }
            if (block is Markdig.Syntax.ListItemBlock)
            {
                DumpListItemBlock(block as Markdig.Syntax.ListItemBlock);
            }
        }
        static void DumpMarkdownDocument(Markdig.Syntax.MarkdownDocument document)
        {
            foreach (var block in document)
            {
                DumpBlock(block);
            }
        }

        static async Task<IEnumerable<Spell>> TestMarkdown(string filename)
        {
            using (var sr = new StreamReader(filename))
            { 
                var md = await sr.ReadToEndAsync();
                var document = Markdig.Parsers.MarkdownParser.Parse(md);
                //DumpMarkdownDocument(document);
                var converter = new MarkdownConverter();
                var spellss = converter.MarkdownToSpells(md);
                Console.WriteLine("ok");
                var md2 = spellss.ToMarkdownString();
                if(md.CompareTo(md2) != 0)
                {
                    Console.WriteLine("failed");
                }
                return spellss;
            }
        }

        static async Task Main(string[] args)
        {
            var spellss = await TestMarkdown(@"..\..\..\..\..\Data\spells_hd.md");
            return;
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
            var spellsHD = LoadJSon<IEnumerable<Spell>>(dataDir + "spells_hd_full.json");
            var monstersVF = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vf_full.json");
            var monstersVO = LoadJSon<IEnumerable<Monster>>(dataDir + "monsters_vo_full.json");

            var mdhd = spellsHD.ToMarkdownString();
            await SaveStringAsync(dataDir + "spells_hd.md", mdhd);

            spellsVF.ForEach(sp => sp.Html = null);
            spellsVO.ForEach(sp => sp.Html = null);
            spellsVF.ForEach(sp => sp.DescriptionDiv = sp.DescriptionDiv);
            spellsVO.ForEach(sp => sp.DescriptionDiv = sp.DescriptionDiv);
            monstersVF.ForEach(it => it.Html = null);
            monstersVO.ForEach(it => it.Html = null);

            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vf.json", spellsVF);
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_vo.json", spellsVO);
            SaveJSon<IEnumerable<Spell>>(dataDir + "spells_hd.json", spellsHD);
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vf.json", monstersVF);
            SaveJSon<IEnumerable<Monster>>(dataDir + "monsters_vo.json", monstersVO);
            return;

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
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                await stream.WriteAsync(buffer, 0, buffer.Length);
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
