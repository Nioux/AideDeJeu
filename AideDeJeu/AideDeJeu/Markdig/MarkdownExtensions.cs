using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Markdig;
using AideDeJeuLib;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdig.Parsers;
using System.IO;
using Markdig.Renderers.Normalize;

namespace AideDeJeu.Tools
{
    public static class MarkdownExtensions
    {
        public static Item ToItem(string md)
        {
            var pipeline = new MarkdownPipelineBuilder().UsePipeTables().Build();
            var document = MarkdownParser.Parse(md, pipeline);

            var enumerator = document.GetEnumerator();
            try
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (enumerator.Current is ParagraphBlock)
                    {
                        if(block.IsNewItem())
                        {
                            var item = block.GetNewItem();
                            item.Parse(ref enumerator);
                            return item;
                        }
                    }
                    enumerator.MoveNext();
                }
                
            }
            finally
            {
                enumerator.Dispose();
            }
            return null;
        }

        public static bool IsNewItem(this Block block)
        {
            var paragraphBlock = block as ParagraphBlock;
            var linkInline = paragraphBlock?.Inline?.FirstChild as LinkInline;
            if (linkInline != null)
            {
                var title = linkInline.Title;
                if (title == string.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static Item GetNewItem(this Block block)
        {
            var paragraphBlock = block as ParagraphBlock;
            var linkInline = paragraphBlock?.Inline?.FirstChild as LinkInline;
            if (linkInline != null)
            {
                var label = linkInline.Label;
                var title = linkInline.Title;
                var url = linkInline.Url;
                if (title == string.Empty)
                {
                    var name = $"AideDeJeuLib.{label}, AideDeJeu";
                    var type = Type.GetType(name);
                    var instance = Activator.CreateInstance(type) as Item;
                    return instance;
                }
            }
            return null;
        }



        public static string ToMarkdownString(this Block block)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UsePipeTables()
                .Build();

            using (var writer = new StringWriter())
            {
                var renderer = new NormalizeRenderer(writer);
                pipeline.Setup(renderer);

                renderer.Render(block);
                return writer.ToString();
            }
        }

        public static string ToMarkdownString(this Markdig.Syntax.Inlines.ContainerInline inlines)
        {
            var str = string.Empty;
            foreach (var inline in inlines)
            {
                str += inline.ToMarkdownString();
            }
            return str;
        }

        public static string ToMarkdownString(this Markdig.Syntax.Inlines.Inline inline)
        {
            //Debug.WriteLine(inline.GetType());
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
                add = delimiterChar + emphasisInline.ToMarkdownString() + delimiterChar;
            }
            else if (inline is Markdig.Syntax.Inlines.LinkInline)
            {
                var linkInline = inline as Markdig.Syntax.Inlines.LinkInline;
                add = string.Empty;
                if (linkInline.IsImage)
                {
                    add = "!";
                }
                var label = linkInline.ToMarkdownString();
                var url = linkInline.Url;
                var title = linkInline.Title;
                if (!string.IsNullOrEmpty(title))
                {
                    add += $"[{label}]({url} \"{title}\")";
                }
                else
                {
                    add += $"[{label}]({url})";
                }
            }
            else if (inline is Markdig.Syntax.Inlines.ContainerInline)
            {
                var containerInline = inline as Markdig.Syntax.Inlines.ContainerInline;
                add = containerInline.ToMarkdownString();
            }
            else
            {
                add = inline.ToString();
            }
            //Debug.WriteLine(add);
            return add;
        }

        public static string ToMarkdownString(this Markdig.Syntax.ParagraphBlock paragraphBlock)
        {
            var str = string.Empty;
            str += paragraphBlock.Inline.ToMarkdownString();
            if (paragraphBlock.IsBreakable)
            {
                str += "\n";
            }
            return str;
        }
        public static string ToMarkdownString(this Markdig.Extensions.Tables.Table tableBlock)
        {
            var ret = string.Empty;
            foreach(Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                var line = "|";
                foreach(Markdig.Extensions.Tables.TableCell cell in row)
                {
                    foreach(Markdig.Syntax.ParagraphBlock block in cell)
                    {
                        line += block.ToMarkdownString().Replace("\n", "");
                    }
                    line += "|";
                }
                if(row.IsHeader)
                {
                    line += "\n|";
                    for(int i = 0; i < row.Count; i++)
                    {
                        line += "---|";
                    }
                }
                ret += line + "\n";
            }
            return ret;
        }

        public static Dictionary<string, List<string>> ToTable(this Markdig.Extensions.Tables.Table tableBlock)
        {
            var table = new Dictionary<string, List<string>>();
            var indexes = new Dictionary<int, string>();
            foreach (var blockrow in tableBlock)
            {
                var row = blockrow as Markdig.Extensions.Tables.TableRow;
                int indexCol = 0;
                foreach (var blockcell in row)
                {
                    var cell = blockcell as Markdig.Extensions.Tables.TableCell;
                    foreach (var blockpar in cell)
                    {
                        var par = blockpar as Markdig.Syntax.ParagraphBlock;
                        var name = par.ToMarkdownString().Trim();
                        if (row.IsHeader)
                        {
                            indexes[indexCol] = name;
                            table[name] = new List<string>();
                        }
                        else
                        {
                            table[indexes[indexCol]].Add(name);
                        }
                    }
                    indexCol++;
                }
            }
            return table;
        }
    }
}
