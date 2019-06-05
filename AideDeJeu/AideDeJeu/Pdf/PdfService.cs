using AideDeJeu.Tools;
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
            var p = new Paragraph(text); //, font);
            p.Alignment = alignment;
            ct.AddElement(p);
            ct.Go();

        }

        public void MarkdownToPdf(string md, Stream stream)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();
            var parsed = Markdig.Markdown.Parse(md, pipeline);

            PdfReader reader = null;
            reader = new PdfReader(AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.poker_size.pdf"));
            PdfStamper stamper = null;
            stamper = new PdfStamper(reader, stream);

            Render(parsed.AsEnumerable(), stamper);

            stamper.Close();
            reader.Close();
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
            var cb = stamper.GetOverContent(1);
            //ColumnText.ShowTextAligned(cb, iTextSharp.text.Element.ALIGN_LEFT, new Phrase("Galefrin"), 40, 40, 0);


            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(10f, 48f, 200f, 600f);
            Font f = new Font();
            Paragraph pz = new Paragraph(new Phrase(20, "Hello World!", f));
            ct.AddElement(pz);
            ct.Go();
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1252", BaseFont.EMBEDDED);
            f = new Font(bf, 13);
            ct = new ColumnText(cb);
            ct.SetSimpleColumn(10f, 48f, 200f, 700f);
            pz = new Paragraph("Hello World!", f);
            ct.AddElement(pz);
            ct.Go();



            return;
            /*
            var text = block.ToMarkdownString();
            //DrawText(content, md, null, 100, 100, 300, 300, 0);

            float x = 10;
            float y = 10;
            float width = 300;
            float height = 300;
            cb.SetRGBColorFill(127, 127, 127);
            //cb.Rectangle(x, y, width, height);
            //cb.Stroke();
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(x, y, x + width, y + height);

            Font font = new Font(BaseFont.CreateFont());
            //int rectWidth = 80;
            //float maxFontSize = getMaxFontSize(BaseFont.CreateFont(), "text", rectWidth);
            font.Size = 20;


            var p = new Paragraph(text, font);
            //p.Alignment = alignment;
            ct.AddElement(p);
            ct.Go();



            //var style = this.Theme.Paragraph;
            //var foregroundColor = isQuoted ? this.Theme.Quote.ForegroundColor : style.ForegroundColor;
            //var label = new Label
            //{
            //    FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, foregroundColor, style.BackgroundColor, style.FontSize),
            //};
            //AttachLinks(label);
            //this.stack.Children.Add(label); 
            */
        }
    }
}
