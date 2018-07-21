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
                    //$"# {Name}\n\n" +
                    //$"{AltName}\n\n" +
                    Text;
            }
        }

        public void ParseBlock(Block block)
        {
            if (block is HeadingBlock)
            {
                var headingBlock = block as HeadingBlock;
                if (this.Name == null)
                {
                    this.Name = headingBlock.Inline.ToMarkdownString();
                }
                this.Text += block.ToMarkdownString();
            }
            else if (block is ListBlock)
            {
                var listBlock = block as ListBlock;
                if (listBlock.BulletType == '-')
                {
                    var regex = new Regex("(?<key>.*?): (?<value>.*)");
                    var str = block.ToMarkdownString();
                    var properties = new List<Tuple<string, Action<Generic, string>>>()
                        {
                            new Tuple<string, Action<Generic, string>>("- AltName: ", (m, s) =>
                            {
                                this.Text += "- " + s; m.AltName = s;
                            }),
                            new Tuple<string, Action<Generic, string>>("", (m, s) =>
                            {
                                this.Text += str;
                            }),
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
        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            enumerator.MoveNext();
            while (enumerator.Current != null)
            {
                var block = enumerator.Current;
                if (block.IsNewItem())
                {
                    return;
                }
                ParseBlock(block);
                enumerator.MoveNext();

            }
        }
    }
}
