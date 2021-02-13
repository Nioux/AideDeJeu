using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AideDeJeu.Tools
{
    public class MonsterMarkdownTheme : Xam.Forms.Markdown.MarkdownTheme
    {
        public MonsterMarkdownTheme()
        {
            //this.Margin = 0;
            var fdParagraph = FormatedTextHelpers.FontData.FromResource("paragraph");
            var fdHeading1 = FormatedTextHelpers.FontData.FromResource("heading1");
            var fdHeading2 = FormatedTextHelpers.FontData.FromResource("heading2");
            var fdHeading3 = FormatedTextHelpers.FontData.FromResource("heading3");
            var fdHeading4 = FormatedTextHelpers.FontData.FromResource("heading4");
            var fdHeading5 = FormatedTextHelpers.FontData.FromResource("heading5");
            var fdHeading6 = FormatedTextHelpers.FontData.FromResource("heading6");
            var fdLink = FormatedTextHelpers.FontData.FromResource("link");
            var fdTableHeader = FormatedTextHelpers.FontData.FromResource("tableheader");
            var fdTableCell = FormatedTextHelpers.FontData.FromResource("tablecell");
            var fdTableCellAlt = FormatedTextHelpers.FontData.FromResource("tablecellalt");

            this.Paragraph.FontFamily = fdParagraph.FontFamily;
            this.Paragraph.FontSize = (float)fdParagraph.FontSize;
            this.Paragraph.Attributes = fdParagraph.FontAttributes;
            this.Paragraph.ForegroundColor = fdParagraph.TextColor;

            this.Paragraph.BackgroundColor = DefaultBackgroundColor;
            this.BackgroundColor = DefaultBackgroundColor;

            this.Heading1.ForegroundColor = fdHeading1.TextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading1.FontFamily = fdHeading1.FontFamily;

            this.Heading2.ForegroundColor = fdHeading2.TextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading2.FontFamily = fdHeading2.FontFamily;

            this.Heading3.ForegroundColor = fdHeading3.TextColor;
            this.Heading3.BorderColor = DefaultSeparatorColor;
            this.Heading3.FontFamily = fdHeading3.FontFamily;

            this.Heading4.ForegroundColor = fdHeading4.TextColor;
            this.Heading4.BorderColor = DefaultSeparatorColor;
            this.Heading4.FontFamily = fdHeading4.FontFamily;

            this.Heading5.ForegroundColor = fdHeading5.TextColor;
            this.Heading5.BorderColor = DefaultSeparatorColor;
            this.Heading5.FontFamily = fdHeading5.FontFamily;

            this.Heading6.ForegroundColor = fdHeading6.TextColor;
            this.Heading6.BorderColor = DefaultSeparatorColor;
            this.Heading6.FontFamily = fdHeading6.FontFamily;

            //this.Link.FontFamily = fdLink.FontFamily;
            //this.Link.FontSize = (float)fdLink.FontSize;
            this.Link.Attributes = fdLink.FontAttributes;
            this.Link.ForegroundColor = fdLink.TextColor;

            this.TableHeader.FontFamily = fdTableHeader.FontFamily;
            this.TableHeader.FontSize = (float)fdTableHeader.FontSize;
            this.TableHeader.Attributes = fdTableHeader.FontAttributes;
            this.TableHeader.ForegroundColor = fdTableHeader.TextColor;

            this.TableCell.FontFamily = fdTableCell.FontFamily;
            this.TableCell.FontSize = (float)fdTableCell.FontSize;
            this.TableCell.Attributes = fdTableCell.FontAttributes;
            this.TableCell.ForegroundColor = fdTableCell.TextColor;

            this.TableCellAlt.FontFamily = fdTableCellAlt.FontFamily;
            this.TableCellAlt.FontSize = (float)fdTableCellAlt.FontSize;
            this.TableCellAlt.Attributes = fdTableCellAlt.FontAttributes;
            this.TableCellAlt.ForegroundColor = fdTableCellAlt.TextColor;

            //this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;


        }

        public static readonly Color DefaultBackgroundColor = Color.FromHex("#FFFFFF");

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }

    public class DarkMonsterMarkdownTheme : Xam.Forms.Markdown.MarkdownTheme
    {
        public DarkMonsterMarkdownTheme()
        {
            //this.Margin = 0;
            var fdParagraph = FormatedTextHelpers.FontData.FromResource("paragraph");
            var fdHeading1 = FormatedTextHelpers.FontData.FromResource("heading1");
            var fdHeading2 = FormatedTextHelpers.FontData.FromResource("heading2");
            var fdHeading3 = FormatedTextHelpers.FontData.FromResource("heading3");
            var fdHeading4 = FormatedTextHelpers.FontData.FromResource("heading4");
            var fdHeading5 = FormatedTextHelpers.FontData.FromResource("heading5");
            var fdHeading6 = FormatedTextHelpers.FontData.FromResource("heading6");
            var fdLink = FormatedTextHelpers.FontData.FromResource("link");
            var fdTableHeader = FormatedTextHelpers.FontData.FromResource("tableheader");
            var fdTableCell = FormatedTextHelpers.FontData.FromResource("tablecell");
            var fdTableCellAlt = FormatedTextHelpers.FontData.FromResource("tablecellalt");

            this.Paragraph.FontFamily = fdParagraph.FontFamily;
            this.Paragraph.FontSize = (float)fdParagraph.FontSize;
            this.Paragraph.Attributes = fdParagraph.FontAttributes;
            this.Paragraph.ForegroundColor = ParagraphColor; // fdParagraph.TextColor;

            this.Paragraph.BackgroundColor = DefaultBackgroundColor;
            this.BackgroundColor = DefaultBackgroundColor;

            this.Heading1.ForegroundColor = Heading1Color; // fdHeading1.TextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading1.FontFamily = fdHeading1.FontFamily;

            this.Heading2.ForegroundColor = Heading2Color; // fdHeading2.TextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading2.FontFamily = fdHeading2.FontFamily;

            this.Heading3.ForegroundColor = Heading3Color; // fdHeading3.TextColor;
            this.Heading3.BorderColor = DefaultSeparatorColor;
            this.Heading3.FontFamily = fdHeading3.FontFamily;

            this.Heading4.ForegroundColor = Heading4Color; // fdHeading4.TextColor;
            this.Heading4.BorderColor = DefaultSeparatorColor;
            this.Heading4.FontFamily = fdHeading4.FontFamily;

            this.Heading5.ForegroundColor = Heading5Color; // fdHeading5.TextColor;
            this.Heading5.BorderColor = DefaultSeparatorColor;
            this.Heading5.FontFamily = fdHeading5.FontFamily;

            this.Heading6.ForegroundColor = Heading6Color; // fdHeading6.TextColor;
            this.Heading6.BorderColor = DefaultSeparatorColor;
            this.Heading6.FontFamily = fdHeading6.FontFamily;

            //this.Link.FontFamily = fdLink.FontFamily;
            //this.Link.FontSize = (float)fdLink.FontSize;
            this.Link.Attributes = fdLink.FontAttributes;
            this.Link.ForegroundColor = LinkColor; // fdLink.TextColor;

            this.TableHeader.FontFamily = fdTableHeader.FontFamily;
            this.TableHeader.FontSize = (float)fdTableHeader.FontSize;
            this.TableHeader.Attributes = fdTableHeader.FontAttributes;
            this.TableHeader.ForegroundColor = TableHeaderColor; // fdTableHeader.TextColor;

            this.TableCell.FontFamily = fdTableCell.FontFamily;
            this.TableCell.FontSize = (float)fdTableCell.FontSize;
            this.TableCell.Attributes = fdTableCell.FontAttributes;
            this.TableCell.ForegroundColor = TableCellColor; // fdTableCell.TextColor;

            this.TableCellAlt.FontFamily = fdTableCellAlt.FontFamily;
            this.TableCellAlt.FontSize = (float)fdTableCellAlt.FontSize;
            this.TableCellAlt.Attributes = fdTableCellAlt.FontAttributes;
            this.TableCellAlt.ForegroundColor = TableCellAltColor; // fdTableCellAlt.TextColor;

            //this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;


        }

        public static readonly Color DarkHDRed = Color.FromHex("#9B1C47");
        public static readonly Color DarkHDBlue = Color.FromHex("#c38dcc");
        public static readonly Color DarkHDGrey = Color.FromHex("#563F5A");
        public static readonly Color DarkHDMidGrey = Color.FromHex("#6F5B73");
        public static readonly Color DarkHDLightGrey = Color.FromHex("#7C7B7B");
        public static readonly Color DarkHDLightBlack = Color.FromHex("#3A213C");
        public static readonly Color DarkHDBackMidGrey = Color.FromHex("#B5AAB9");
        public static readonly Color DarkHDBackLightGrey = Color.FromHex("#EDEDED");
        public static readonly Color DarkHDWhite = Color.FromHex("#333");
        public static readonly Color DarkHDBlack = Color.FromHex("#ddd");

        public static readonly Color ParagraphColor = DarkHDBlack;
        public static readonly Color Heading1Color = DarkHDRed;
        public static readonly Color Heading2Color = DarkHDBlack;
        public static readonly Color Heading3Color = DarkHDBlack;
        public static readonly Color Heading4Color = DarkHDBlack;
        public static readonly Color Heading5Color = DarkHDBlack;
        public static readonly Color Heading6Color = DarkHDBlack;
        public static readonly Color LinkColor = DarkHDBlue;
        public static readonly Color TableHeaderColor = DarkHDWhite;
        public static readonly Color TableHeaderBackgroundColor = DarkHDGrey;
        public static readonly Color TableCellColor = DarkHDBlack;
        public static readonly Color TableCellBackgroundColor = DarkHDWhite;
        public static readonly Color TableCellAltColor = DarkHDBlack;
        public static readonly Color TableCellAltBackgroundColor = DarkHDLightGrey;

        public static readonly Color DefaultBackgroundColor = DarkHDWhite; // = Color.FromHex("#333");

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = DarkHDBlack; // Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = DarkHDWhite; // Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }
}
