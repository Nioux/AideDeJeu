using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace AideDeJeuLib.Spells
{
    public class Spell : Item
    {
        public string LevelType
        {
            get
            {
                return string.Format("{0} - {1}", this.Level, this.Type);
            }
            set
            {
                var re = new Regex("(?<type>.*) de niveau (?<level>\\d).?(?<rituel>\\(rituel\\))?");
                var match = re.Match(value);
                this.Type = match.Groups["type"].Value;
                this.Level = match.Groups["level"].Value;
                this.Rituel = match.Groups["rituel"].Value;
                if (string.IsNullOrEmpty(this.Type))
                {
                    re = new Regex("(?<type>.*), (?<level>tour de magie)");
                    match = re.Match(value);
                    if (match.Groups["level"].Value == "tour de magie")
                    {
                        this.Type = match.Groups["type"].Value;
                        this.Level = "0"; // match.Groups["level"].Value;
                        this.Rituel = match.Groups["rituel"].Value;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(value);
                        //re = new Regex("level (?<level>\\d) - (?<type>.*?)\\w?(?<rituel>\\(ritual\\))?");
                        re = new Regex("^(?<level>\\d) - (?<type>.*?)\\s?(?<rituel>\\(ritual\\))?$");
                        match = re.Match(value);
                        this.Type = match.Groups["type"].Value;
                        this.Level = match.Groups["level"].Value;
                        this.Rituel = match.Groups["rituel"].Value;
                    }
                }
            }
        }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Concentration { get; set; }
        public string Rituel { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string DescriptionHtml { get; set; }
        public string DescriptionText
        {
            get
            {
                return DescriptionDiv?.InnerText?.Replace("\n", "\n\n");
            }
        }
        [IgnoreDataMember]
        public XmlNode DescriptionDiv
        {
            get
            {
                if(DescriptionHtml != null)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(DescriptionHtml);
                    return xdoc.DocumentElement;
                    //HtmlDocument doc = new HtmlDocument() { OptionOutputAsXml = true };
                    //doc.LoadHtml(DescriptionHtml);
                    //return doc.DocumentNode;
                }
                return null;
            }
            set
            {
                DescriptionHtml = value?.OuterXml;
            }
        }

        public string Overflow { get; set; }
        public string NoOverflow { get; set; }
        public string Source { get; set; }

    }
}
