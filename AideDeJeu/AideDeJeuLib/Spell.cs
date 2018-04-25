using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeuLib
{
    public class Spell
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TitleUS { get; set; }
        public string LevelType { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Rituel { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml
        {
            get
            {
                return DescriptionDiv?.InnerHtml;
            }
            set
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);
                DescriptionDiv = doc.DocumentNode;
            }
        }
        public string DescriptionText
        {
            get
            {
                string html = DescriptionDiv?.InnerText?.Replace("\n", "\r\n\r\n");
                //string html = DescriptionDiv?.InnerHtml;
                //html = html?.Replace("<br>", "\r\n");
                //html = html?.Replace("<strong>", "");
                //html = html?.Replace("</strong>", "");
                //html = html?.Replace("<em>", "");
                //html = html?.Replace("</em>", "");
                //if (html != null)
                //{
                //    var regex = new Regex("<a href=.*?>");
                //    html = regex.Replace(html, "");
                //}
                //html = html?.Replace("</a>", "");
                return html;
            }
        }
        [IgnoreDataMember]
        public HtmlNode DescriptionDiv { get; set; }
        public string Overflow { get; set; }
        public string NoOverflow { get; set; }
        public string Source { get; set; }
    }
}
