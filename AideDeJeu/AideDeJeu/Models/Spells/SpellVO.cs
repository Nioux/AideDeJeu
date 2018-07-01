using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AideDeJeuLib.Spells
{
    public class SpellVO : Spell
    {
        public override string LevelType
        {
            get
            {
                if (string.IsNullOrEmpty(Rituel))
                {
                    return $"Level {Level} - {Type}";
                }
                else
                {
                    return $"Level {Level} - {Type} {Rituel}";
                }
            }
            set
            {
                var re = new Regex("^(?<level>\\d) - (?<type>.*?)\\s?(?<rituel>\\(ritual\\))?$");
                var match = re.Match(value);
                this.Type = match.Groups["type"].Value;
                this.Level = match.Groups["level"].Value;
                this.Rituel = match.Groups["rituel"].Value;
            }
        }

        public override string Markdown
        {
            get
            {
                return
                    $"# {Name}\n" +
                    $"{NameVO}\n" +
                    $"_{LevelType}_\n" +
                    $"**Casting Time :** {CastingTime}\n" +
                    $"**Range :** {Range}\n" +
                    $"**Components :** {Components}\n" +
                    $"**Duration :** {Duration}\n\n" +
                    $"{DescriptionHtml}\n\n" +
                    $"**Source :** {Source}";

            }
        }
    }
}
