using System.Linq;
using System.Runtime.Serialization;

namespace AideDeJeuLib.Cards
{
    [DataContract]
    public class CardData
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "color")]
        public string Color { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "title_size")]
        public string TitleSize { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }
        [DataMember(Name = "icon_back")]
        public string IconBack { get; set; }
        [DataMember(Name = "contents")]
        public string[] ContentsToString { get { return Contents.Select(cc => cc.ToString()).ToArray(); } }
        [IgnoreDataMember]
        public CardContent[] Contents { get; set; }
        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }
        [DataMember(Name = "background_image")]
        public string BackgroundImage { get; set; }
    }

    public interface CardContent
    {
        int Height { get; }
    }

    public class SubtitleCardContent : CardContent
    {
        public string Subtitle { get; set; }

        public SubtitleCardContent(string subtitle)
        {
            Subtitle = subtitle;
        }

        public override string ToString()
        {
            return "subtitle | " + Subtitle;
        }

        public int Height { get { return 12; } }
    }

    public class RuleCardContent : CardContent
    {
        public override string ToString()
        {
            return "rule";
        }

        public int Height { get { return 15; } }
    }

    public class PropertyCardContent : CardContent
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public PropertyCardContent(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return "property | " + Name + " | " + Value;
        }

        public int Height { get { return ((Name.Length + Value.Length) / 35 + 1) * 15; } }
    }

    public class FillCardContent : CardContent
    {
        public int Fill { get; set; }

        public FillCardContent(int fill)
        {
            Fill = fill;
        }

        public override string ToString()
        {
            return "fill | " + Fill;
        }

        public int Height { get { return 1; } }
    }

    public class TextCardContent : CardContent
    {
        public string Text { get; set; }

        public TextCardContent(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return "text | " + Text;
        }

        public int Height { get { return (Text.Length / 35 + 1) * 15 + 6; } }
    }

    public class SectionCardContent : CardContent
    {
        public string Section { get; set; }

        public SectionCardContent(string section)
        {
            Section = section;
        }

        public override string ToString()
        {
            return "section | " + Section;
        }

        public int Height { get { return 20; } }
    }

    public class DescriptionCardContent : CardContent
    {
        public string Quality { get; set; }

        public string Text { get; set; }

        public DescriptionCardContent(string quality, string text)
        {
            Quality = quality;
            Text = text;
        }

        public override string ToString()
        {
            return "description | " + Quality + " | " + Text;
        }

        public int Height { get { return ((Quality.Length + Text.Length) / 35 + 1) * 15 + 6; } }
    }

    public class BulletCardContent : CardContent
    {
        public string Text { get; set; }

        public BulletCardContent(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            return "bullet | " + Text;
        }

        public int Height { get { return (Text.Length / 30 + 1) * 15; } }
    }

    public class BoxesCardContent : CardContent
    {
        public int Count { get; set; }

        public int Size { get; set; }

        public BoxesCardContent(int count, int size)
        {
            Count = count;
            Size = size;
        }

        public override string ToString()
        {
            return "boxes | " + Count + " | " + Size;
        }

        public int Height { get { return 16; } }
    }

    public class DndstatsCardContent : CardContent
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public override string ToString()
        {
            return "dndstats | " + Strength + " | " + Dexterity + " | " + Constitution + " | " + Intelligence + " | " + Wisdom + " | " + Charisma;
        }

        public int Height { get { return 20; } }
    }

    public class EmptyCardContent : CardContent
    {
        public override string ToString()
        {
            return "";
        }

        public int Height { get { return 0; } }
    }

}
