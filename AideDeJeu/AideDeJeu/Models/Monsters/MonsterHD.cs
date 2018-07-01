using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AideDeJeuLib.Monsters
{
    public class MonsterHD : Monster
    {
        public override string Markdown
        {
            get
            {
                return
                    $"# {Name}\n" +
                    $"{NameVO}\n" +
                    $"{Type} de taille {Size}, {Alignment}\n" +
                    $"**Classe d'armure** {ArmorClass}\n" +
                    $"**Points de vie** {HitPoints}\n" +
                    $"**Vitesse** {Speed}\n\n" +
                    $"|FOR|DEX|CON|INT|SAG|CHA|\n" +
                    $"|---|---|---|---|---|---|\n" +
                    $"|{Strength}|{Dexterity}|{Constitution}|{Intelligence}|{Wisdom}|{Charisma}|\n\n" +
                    $"**Compétences** {Skills}\n" +
                    $"**Sens** {Senses}\n" +
                    $"**Langues** {Languages}\n" +
                    $"**Dangerosité** {Challenge}\n\n" +
                    (SpecialFeatures != null ? $"## Capacités\n\n" + SpecialFeatures.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Actions != null ? $"## Actions\n\n" + Actions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Reactions != null ? $"## Réactions\n\n" + Reactions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (LegendaryActions != null ? $"## Actions Légendaires\n\n" + LegendaryActions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "");

                //public string Type { get; set; }
                //public string Size { get; set; }
                //public string Alignment { get; set; }
                //public string Legendary { get; set; }
                //public string Source { get; set; }
                //public string ArmorClass { get; set; }
                //public string HitPoints { get; set; }
                //public string Speed { get; set; }
                //public string Strength { get; set; }
                //public string Dexterity { get; set; }
                //public string Constitution { get; set; }
                //public string Intelligence { get; set; }
                //public string Wisdom { get; set; }
                //public string Charisma { get; set; }
                //public string SavingThrows { get; set; }
                //public string Skills { get; set; }
                //public string DamageVulnerabilities { get; set; }
                //public string DamageImmunities { get; set; }
                //public string ConditionImmunities { get; set; }
                //public string DamageResistances { get; set; }
                //public string Senses { get; set; }
                //public string Languages { get; set; }
                //public string Challenge { get; set; }
                //public string Description { get; set; }
                //        return "";
            }
        }
    }
}