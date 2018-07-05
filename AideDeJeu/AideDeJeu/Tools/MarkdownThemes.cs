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
            //this.Paragraph.FontFamily = fd.FontFamily;
            //this.Paragraph.FontFamily = "Droid Serif";
            this.Paragraph.FontFamily = fdParagraph.FontFamily;
            this.Paragraph.FontSize = (float)fdParagraph.FontSize;
            this.Paragraph.Attributes = fdParagraph.FontAttributes;
            this.Paragraph.ForegroundColor = fdParagraph.TextColor;

            this.Paragraph.BackgroundColor = DefaultBackgroundColor;
            this.BackgroundColor = DefaultBackgroundColor;
            //this.Paragraph.ForegroundColor = DefaultTextColor;
            this.Heading1.ForegroundColor = DefaultTextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading1.FontFamily = fdHeading1.FontFamily;
            this.Heading2.ForegroundColor = DefaultTextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading2.FontFamily = fdHeading2.FontFamily;
            this.Heading3.ForegroundColor = DefaultTextColor;
            this.Heading3.FontFamily = fdHeading3.FontFamily;
            this.Heading4.ForegroundColor = DefaultTextColor;
            this.Heading5.ForegroundColor = DefaultTextColor;
            this.Heading6.ForegroundColor = DefaultTextColor;
            this.Link.ForegroundColor = DefaultAccentColor;
            this.Code.ForegroundColor = DefaultTextColor;
            this.Code.BackgroundColor = DefaultCodeBackground;
            this.Quote.ForegroundColor = DefaultQuoteTextColor;
            this.Quote.BorderColor = DefaultQuoteBorderColor;
            this.Separator.BorderColor = DefaultSeparatorColor;
        }

        public static readonly Color DefaultBackgroundColor = Color.FromHex("#F7F2E5");

        public static readonly Color DefaultAccentColor = Color.FromHex("#0366d6");

        public static readonly Color DefaultTextColor = Color.FromHex("#24292e");

        public static readonly Color DefaultCodeBackground = Color.FromHex("#f6f8fa");

        public static readonly Color DefaultSeparatorColor = Color.FromHex("#eaecef");

        public static readonly Color DefaultQuoteTextColor = Color.FromHex("#6a737d");

        public static readonly Color DefaultQuoteBorderColor = Color.FromHex("#dfe2e5");
    }
}
