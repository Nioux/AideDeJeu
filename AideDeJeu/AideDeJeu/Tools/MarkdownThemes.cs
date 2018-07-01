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
            var fd = FormatedTextHelpers.FontData.FromResource("content");
            //this.Paragraph.FontFamily = fd.FontFamily;
            //this.Paragraph.FontFamily = "Droid Serif";
            this.Paragraph.FontFamily = "serif";
            this.Paragraph.FontSize = (float)fd.FontSize;
            this.Paragraph.Attributes = fd.FontAttributes;
            this.Paragraph.ForegroundColor = fd.TextColor;

            this.Paragraph.BackgroundColor = DefaultBackgroundColor;
            this.BackgroundColor = DefaultBackgroundColor;
            //this.Paragraph.ForegroundColor = DefaultTextColor;
            this.Heading1.ForegroundColor = DefaultTextColor;
            this.Heading1.BorderColor = DefaultSeparatorColor;
            this.Heading2.ForegroundColor = DefaultTextColor;
            this.Heading2.BorderColor = DefaultSeparatorColor;
            this.Heading3.ForegroundColor = DefaultTextColor;
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
