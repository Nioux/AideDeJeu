using AideDeJeu.Tools;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
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

        Document _Document = null;
        PdfWriter _Writer = null;
        public void MarkdownToPdf(List<string> mds, Stream stream)
        {
            var pipeline = new Markdig.MarkdownPipelineBuilder().UseYamlFrontMatter().UsePipeTables().Build();

            _Document = new Document(new Rectangle(822, 1122));
            _Writer = PdfWriter.GetInstance(_Document, stream);
            _Document.Open();
            //PdfReader reader = null;
            //reader = new PdfReader(AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.poker_size.pdf"));
            //PdfStamper stamper = null;
            //stamper = new PdfStamper(reader, stream);

            foreach (var md in mds)
            {
                var parsed = Markdig.Markdown.Parse(md, pipeline);
                Render(parsed.AsEnumerable(), _Document);
            }

            _Document.Close();
            _Writer.Close();
            //stamper.Close();
            //reader.Close();
        }

        Font ParagraphFont = null;
        private void Render(IEnumerable<Block> blocks, Document document)
        {
            if(ParagraphFont == null)
            {
                //var fontPath = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "Cinzel-Regular.otf");
                var fontPath = @"C:\Users\yanma\Documents\Visual Studio 2017\Projects\AideDeJeu\AideDeJeu\AideDeJeuCmd\bin\Debug\netcoreapp2.1\Cinzel-Regular.otf";
                using (var inFont = AideDeJeu.Tools.Helpers.GetResourceStream("AideDeJeu.Pdf.Cinzel-Regular.otf"))
                {
                    using (var outFont = new FileStream(fontPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        inFont.CopyTo(outFont);
                    }
                }
                FontFactory.Register(fontPath);

                //var mafont = FontFactory.GetFont("cinzel", 20, iTextSharp.text.Font.NORMAL);
                var mafont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                //var font = mafont.BaseFont;
                var bigFont = new iTextSharp.text.Font(mafont, 20);

                var fonts = FontFactory.RegisteredFonts;

                ParagraphFont = bigFont;
            }
            var phrases = Render(blocks);


            ColumnText ct = new ColumnText(_Writer.DirectContent);

            int column = 0;
            ct.SetSimpleColumn(10, 10 + 200 * column, 200, 200 + 200 * column);
            int status = 0;
            Phrase p = null;
            float y = 0;
            foreach (var phrase in phrases)
            {
                //var pphrase = new Phrase("test", bigFont);
                phrase.Font = ParagraphFont;
                y = ct.YLine;

                document.Add(phrase);
                //ct.AddText(phrase);
                //status = ct.Go(true);
                //if(ColumnText.HasMoreText(status))
                //{

                //    column++;
                //    ct.SetSimpleColumn(10, 10 + 200 * column, 200, 200 + 200 * column);
                //    y += 200;
                //}
                //ct.YLine = y;
                //ct.SetText(phrase);
                //status = ct.Go(false);



                //ColumnText ct = new ColumnText(_Writer.DirectContent);
                //ct.AddText(CreateFormatted(block.Inline, Font.HELVETICA, 0, new Color(0, 0, 0), 12));
                //ct.Alignment = Element.ALIGN_JUSTIFIED;
                //ct.SetSimpleColumn(10, 10, 200, 200);
                //ct.Go();
            }
        }
        private IEnumerable<Phrase> Render(IEnumerable<Block> blocks)
        {
            var phrases = new List<Phrase>();
            foreach (var block in blocks)
            {
                var phrase = this.Render(block);
                if(phrase != null)
                {
                    phrases.Add(phrase);
                }
                if (block.IsBreakable)
                {
                    phrases.Add(new Phrase(Chunk.NEWLINE));
                }
            }
            return phrases;
        }
        private Phrase Render(Block block)
        {
            switch (block)
            {
                case HeadingBlock heading:
                    return Render(heading);

                case ParagraphBlock paragraph:
                    return Render(paragraph);

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
                    return null;
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

        private Phrase Render(HeadingBlock block)
        {
            return CreateFormatted(block.Inline, Font.HELVETICA, 0, new Color(0x9B1C47), 14 + (7 - block.Level) * 2);
        }

        private Phrase Render(ParagraphBlock block)
        {
            return CreateFormatted(block.Inline, Font.HELVETICA, 0, new Color(0, 0, 0), 12);
            //_Document.Add(CreateFormatted(block.Inline, Font.HELVETICA, 0, new Color(0, 0, 0), 20));
        }

        private Phrase CreateFormatted(ContainerInline inlines, int fontFamily, int fontStyle, Color fontColor, float fontSize)
        {
            var phrase = new Phrase();
            foreach (var inline in inlines)
            {
                var spans = CreateChunks(inline, fontFamily, fontStyle, fontColor, fontSize);
                if (spans != null)
                {
                    foreach (var span in spans)
                    {
                        phrase.Add(span);
                    }
                }
            }
            return phrase;
        }
        private Chunk[] CreateChunks(Inline inline, int fontFamily, int fontStyle, Color fontColor, float fontSize)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    return new Chunk[]
                    {
                        new Chunk()
                        {
                            Content = literal.Content.Text.Substring(literal.Content.Start, literal.Content.Length),
                            Font = ParagraphFont, //new Font(fontFamily, fontSize, fontStyle, fontColor)
                        }
                    };
                case EmphasisInline emphasis:
                    var childStyle = fontStyle | (emphasis.DelimiterCount == 2 ? Font.BOLD : Font.ITALIC);
                    var espans = emphasis.SelectMany(x => CreateChunks(x, fontFamily, childStyle, fontColor, fontSize));
                    return espans.ToArray();

                case LineBreakInline breakline:
                    return new Chunk[] { Chunk.NEWLINE };

                case LinkInline link:
                case CodeInline code:

                case HtmlInline html:

                default:
                    return new Chunk[] { };
            }
            //var cb = stamper.GetOverContent(1);
            ////ColumnText.ShowTextAligned(cb, iTextSharp.text.Element.ALIGN_LEFT, new Phrase("Galefrin"), 40, 40, 0);


            //ColumnText ct = new ColumnText(cb);
            //ct.SetSimpleColumn(10f, 48f, 200f, 600f);
            //Font f = new Font();
            //Paragraph pz = new Paragraph(new Phrase(20, "Hello World!", f));
            //ct.AddElement(pz);
            //ct.Go();
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1252", BaseFont.EMBEDDED);
            //f = new Font(bf, 13);
            //ct = new ColumnText(cb);
            //ct.SetSimpleColumn(10f, 48f, 200f, 700f);
            //pz = new Paragraph("Hello World!", f);
            //ct.AddElement(pz);
            //ct.Go();




            //return;
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
