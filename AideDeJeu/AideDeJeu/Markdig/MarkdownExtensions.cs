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
using Markdig.Renderers.Normalize.Inlines;
using System.Reflection;

namespace AideDeJeu.Tools
{
    public static class MarkdownExtensions
    {
        /*
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

                    if (block is HtmlBlock)
                    {
                        if (block.IsNewItem())
                        {
                            var item = ParseItem(ref enumerator);
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

        public static Item ParseItem(ref ContainerBlock.Enumerator enumerator)
        {
            var currentItem = enumerator.Current.GetNewItem();

            if (currentItem != null)
            {
                enumerator.MoveNext();
                while (enumerator.Current != null)
                {
                    var block = enumerator.Current;

                    if (block is HtmlBlock)
                    {
                        if (block.IsClosingItem())
                        {
                            return currentItem;
                        }
                        else if (block.IsNewItem())
                        {
                            var subItem = ParseItem(ref enumerator);

                            var propertyName = subItem.GetType().Name;

                            if (currentItem.GetType().GetProperty(propertyName) != null)
                            {
                                PropertyInfo prop = currentItem.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                                if (null != prop && prop.CanWrite)
                                {
                                    prop.SetValue(currentItem, subItem, null);
                                }
                            }
                            else if (currentItem is Items)
                            {
                                var items = currentItem as Items;
                                items.Add(subItem);
                            }
                        }
                    }

                    else // if (block is ContainerBlock)
                    {
                        ParseItemProperties(currentItem, block);
                    }

                    currentItem.Markdown += enumerator.Current.ToMarkdownString();

                    enumerator.MoveNext();
                }
            }

            return currentItem;
        }

        public static void ParseItemProperties(Item item, Block block)
        {
            switch(block)
            {
                case Markdig.Extensions.Tables.Table table:
                    ParseItemProperties(item, table);
                    break;
                case ContainerBlock blocks:
                    ParseItemProperties(item, blocks);
                    break;
                case LeafBlock leaf:
                    ParseItemProperties(item, leaf.Inline);
                    break;
            }
        }

        public static void ParseItemProperties(Item item, ContainerBlock blocks)
        {
            foreach(var block in blocks)
            {
                ParseItemProperties(item, block);
            }
        }

        public static void ParseItemProperties(Item item, ContainerInline inlines)
        {
            if(inlines == null)
            {
                return;
            }
            PropertyInfo prop = null;
            foreach (var inline in inlines)
            {
                if(inline is HtmlInline)
                {
                    var tag = (inline as HtmlInline).Tag;
                    if(tag == "<!--br-->" || tag =="<br>")
                    {

                    }
                    else if (tag.StartsWith("<!--/"))
                    {
                        prop = null;
                    }
                    else if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        var propertyName = tag.Substring(4, tag.Length - 7);
                        prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    }
                }
                else
                {
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(item, inline.ToMarkdownString(), null);
                    }
                }
            }
        }



        public static bool IsNewItem(this Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsClosingItem(this Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--/"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Item GetNewItem(this Block block)
        {
            var htmlBlock = block as HtmlBlock;
            if (htmlBlock.Type == HtmlBlockType.Comment)
            {
                var tag = htmlBlock.Lines.Lines.FirstOrDefault().Slice.ToString();
                if (!string.IsNullOrEmpty(tag) && tag != "<!--br-->" && tag != "<br>")
                {
                    if (tag.StartsWith("<!--") && !tag.StartsWith("<!--/"))
                    {
                        var name = $"AideDeJeuLib.{tag.Substring(4, tag.Length - 7)}, AideDeJeu";
                        var type = Type.GetType(name);
                        var instance = Activator.CreateInstance(type) as Item;
                        return instance;
                    }
                }
            }
            return null;
        }
        */

        /*
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
                var label = linkInline.Label;
                var title = linkInline.Title;
                if (title == string.Empty && label != string.Empty)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsClosingItem(this Block block)
        {
            var paragraphBlock = block as ParagraphBlock;
            var linkInline = paragraphBlock?.Inline?.FirstChild as LinkInline;
            if (linkInline != null)
            {
                var label = linkInline.Label;
                var title = linkInline.Title;
                if (title == string.Empty && label == string.Empty)
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
        */


        public static string ToMarkdownString(this Block block)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UsePipeTables()
                .Build();

            using (var writer = new StringWriter())
            {
                var renderer = new NormalizeRenderer(writer);
                renderer.ObjectRenderers.Remove(renderer.ObjectRenderers.FirstOrDefault(i => i is LinkInlineRenderer));
                renderer.ObjectRenderers.Add(new LinkInlineRendererEx());
                renderer.ObjectRenderers.Add(new TableRenderer());
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
            else if (inline is Markdig.Syntax.Inlines.HtmlInline)
            {
                var htmlInline = inline as Markdig.Syntax.Inlines.HtmlInline;
                add = htmlInline.Tag;
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
            foreach (Markdig.Extensions.Tables.TableRow row in tableBlock)
            {
                var line = "|";
                foreach (Markdig.Extensions.Tables.TableCell cell in row)
                {
                    foreach (Markdig.Syntax.ParagraphBlock block in cell)
                    {
                        line += block.ToMarkdownString().Replace("\n", "");
                    }
                    line += "|";
                }
                if (row.IsHeader)
                {
                    line += "\n|";
                    for (int i = 0; i < row.Count; i++)
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
