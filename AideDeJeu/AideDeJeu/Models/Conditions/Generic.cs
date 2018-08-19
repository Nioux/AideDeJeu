using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class Generic : Item
    {
        //public string Text { get; set; }

        //public override string Markdown
        //{
        //    get
        //    {
        //        return 
        //            //$"# {Name}\n\n" +
        //            //$"{AltName}\n\n" +
        //            Text;
        //    }
        //}

        //public void ParseBlock(Block block)
        //{
        //    if (block is HeadingBlock)
        //    {
        //        var headingBlock = block as HeadingBlock;
        //        if (this.Name == null)
        //        {
        //            this.Name = headingBlock.Inline.ToMarkdownString();
        //            this.NameLevel = headingBlock.Level - 1;
        //        }
        //        this.Markdown += block.ToMarkdownString();
        //    }
        //    else if (block is ListBlock)
        //    {
        //        var listBlock = block as ListBlock;
        //        if (listBlock.BulletType == '-')
        //        {
        //            foreach (var inblock in listBlock)
        //            {
        //                if (inblock is Markdig.Syntax.ListItemBlock)
        //                {
        //                    var listItemBlock = inblock as Markdig.Syntax.ListItemBlock;
        //                    foreach (var ininblock in listItemBlock)
        //                    {
        //                        if (ininblock is Markdig.Syntax.ParagraphBlock)
        //                        {
        //                            var paragraphBlock = ininblock as Markdig.Syntax.ParagraphBlock;
        //                            var str = paragraphBlock.ToMarkdownString();
        //                            var regex = new Regex("(?<key>.*?): (?<value>.*)");
        //                            var properties = new List<Tuple<string, Action<Generic, string>>>()
        //                                {
        //                                    new Tuple<string, Action<Generic, string>>("AltName: ", (m, s) =>
        //                                    {
        //                                        this.Markdown += "- " + s; m.AltName = s;
        //                                    }),
        //                                    new Tuple<string, Action<Generic, string>>("", (m, s) =>
        //                                    {
        //                                        this.Markdown += "- " + str;
        //                                    }),
        //                                };

        //                            foreach (var property in properties)
        //                            {
        //                                if (str.StartsWith(property.Item1))
        //                                {
        //                                    property.Item2.Invoke(this, str.Substring(property.Item1.Length));
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            this.Markdown += "\n";
        //        }
        //        else
        //        {
        //            this.Markdown += block.ToMarkdownString();
        //        }
        //    }
        //    else
        //    {
        //        this.Markdown += block.ToMarkdownString();
        //    }
        //}
        //public override void Parse(ref ContainerBlock.Enumerator enumerator)
        //{
        //    enumerator.MoveNext();
        //    while (enumerator.Current != null)
        //    {
        //        var block = enumerator.Current;
        //        if (block.IsNewItem())
        //        {
        //            return;
        //        }
        //        ParseBlock(block);
        //        enumerator.MoveNext();

        //    }
        //}
    }
}
