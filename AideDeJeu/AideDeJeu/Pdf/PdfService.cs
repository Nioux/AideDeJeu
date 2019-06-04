using iTextSharp.text;
using iTextSharp.text.pdf;
using Markdig;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AideDeJeu.Pdf
{
    public class PdfService
    {
        public static void DrawText(PdfContentByte cb, string text, iTextSharp.text.Font font, float x, float y, float width, float height, int alignment)
        {
            cb.SetRGBColorFill(127, 127, 127);
            //cb.Rectangle(x, y, width, height);
            //cb.Stroke();
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(x, y, x + width, y + height);
            var p = new Paragraph(text, font);
            p.Alignment = alignment;
            ct.AddElement(p);
            ct.Go();

        }

        public void MarkdownToPdf(string md, Stream stream)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();
            var parsed = Markdig.Markdown.Parse(md, pipeline);

            PdfReader reader = null;
            reader = new PdfReader(AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.feuille_de_personnage_editable.pdf"));
            PdfStamper stamper = null;
            stamper = new PdfStamper(reader, stream);

            Render(parsed.AsEnumerable(), stamper);
        }
        private void Render(IEnumerable<Block> blocks, PdfStamper stamper)
        {
            foreach (var block in blocks)
            {
                this.Render(block, stamper);
            }
        }
        private void Render(Block block, PdfStamper stamper)
        {
            switch (block)
            {
                //case HeadingBlock heading:
                //    Render(heading);
                //    break;

                case ParagraphBlock paragraph:
                    Render(paragraph, stamper);
                    break;

                //case QuoteBlock quote:
                //    Render(quote);
                //    break;

                //case CodeBlock code:
                //    Render(code);
                //    break;

                //case ListBlock list:
                //    Render(list);
                //    break;

                //case ThematicBreakBlock thematicBreak:
                //    Render(thematicBreak);
                //    break;

                //case HtmlBlock html:
                //    Render(html);
                //    break;

                //case Markdig.Extensions.Tables.Table table:
                //    Render(table);
                //    break;

                default:
                    Debug.WriteLine($"Can't render {block.GetType()} blocks.");
                    break;
            }

            //if (queuedViews.Any())
            //{
            //    foreach (var view in queuedViews)
            //    {
            //        this.stack.Children.Add(view);
            //    }
            //    queuedViews.Clear();
            //}
        }

        private void Render(ParagraphBlock block, PdfStamper stamper)
        {
            //DrawText(stamper.GetOverContent(0),  )
            //var style = this.Theme.Paragraph;
            //var foregroundColor = isQuoted ? this.Theme.Quote.ForegroundColor : style.ForegroundColor;
            //var label = new Label
            //{
            //    FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            //};
            //AttachLinks(label);
            //this.stack.Children.Add(label); 
        }
    }
}
