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
        public string Text { get; set; }

        public override string Markdown
        {
            get
            {
                return 
                    $"# {Name}\n\n" +
                    $"{AltName}\n\n" +
                    Text;
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block is HeadingBlock)
                {
                    var headingBlock = block as HeadingBlock;
                    if (headingBlock.HeaderChar == '#' && (headingBlock.Level == 1 || headingBlock.Level == 2))
                    {
                        if (this.Name == null)
                        {
                            this.Name = headingBlock.Inline.ToMarkdownString();
                        }
                        else
                        {
                            this.Text += block.ToMarkdownString();
                        }
                    }
                    else
                    {
                        this.Text += block.ToMarkdownString();
                    }
                }
                else if (block is ParagraphBlock)
                {
                    if (block.IsNewItem())
                    {
                        return;
                    }
                    //var paragraphBlock = block as ParagraphBlock;
                    //this.Text += paragraphBlock.ToMarkdownString() + "\n";
                    this.Text += block.ToMarkdownString();
                }
                else if (block is ListBlock)
                {
                    var listBlock = block as ListBlock;
                    if (listBlock.BulletType == '-')
                    {
                        foreach (var inblock in listBlock)
                        {
                            var regex = new Regex("(?<key>.*?): (?<value>.*)");
                            if (inblock is ListItemBlock)
                            {
                                var listItemBlock = inblock as ListItemBlock;
                                foreach (var ininblock in listItemBlock)
                                {
                                    if (ininblock is ParagraphBlock)
                                    {
                                        var paragraphBlock = ininblock as ParagraphBlock;
                                        var str = paragraphBlock.Inline.ToMarkdownString();

                                        var properties = new List<Tuple<string, Action<Generic, string>>>()
                                        {
                                            new Tuple<string, Action<Generic, string>>("AltName: ", (m, s) => m.AltName = s),
                                        };

                                        foreach (var property in properties)
                                        {
                                            if (str.StartsWith(property.Item1))
                                            {
                                                property.Item2.Invoke(this, str.Substring(property.Item1.Length));
                                                break;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Text += block.ToMarkdownString();
                    }
                }
                else if (block is Markdig.Extensions.Tables.Table)
                {
                    this.Text += block.ToMarkdownString();
                    //var tableBlock = block as Markdig.Extensions.Tables.Table;
                    //this.Text += "\n\n" + tableBlock.ToMarkdownString() + "\n\n";
                }
                else
                {
                    this.Text += block.ToMarkdownString();
                }

                enumerator.MoveNext();

            }
        }
    }
}
