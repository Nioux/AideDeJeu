using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AideDeJeu.Tools;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace AideDeJeuLib
{
    public class SpellHD : Spell
    {
        public override string LevelType
        {
            get
            {
                if (int.Parse(Level) > 0)
                {
                    if (string.IsNullOrEmpty(Rituel))
                    {
                        return $"{Type} de niveau {Level}";
                    }
                    else
                    {
                        return $"{Type} de niveau {Level} {Rituel}";
                    }
                }
                else
                {
                    return $"{Type}, tour de magie";
                }
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
                }
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
                    $"**Temps d'incantation :** {CastingTime}\n" +
                    $"**Portée :** {Range}\n" +
                    $"**Composantes :** {Components}\n" +
                    $"**Durée :** {Duration}\n\n" +
                    $"{DescriptionHtml}\n\n" +
                    $"**Source :** {Source}";

            }
        }


    }
}
