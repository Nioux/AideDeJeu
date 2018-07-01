using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AideDeJeuLib.Monsters
{
    public class MonsterVO : Monster
    {
        public override string Markdown
        {
            get
            {
                return
                    $"# {Name}\n" +
                    $"{NameVO}\n" +
                    $"{Size} {Type}, {Alignment}\n" +
                    $"**Armor Class** {ArmorClass}\n" +
                    $"**Hit Points** {HitPoints}\n" +
                    $"**Speed** {Speed}\n\n" +
                    $"|STR|DEX|CON|INT|WIS|CHA|\n" +
                    $"|---|---|---|---|---|---|\n" +
                    $"|{Strength}|{Dexterity}|{Constitution}|{Intelligence}|{Wisdom}|{Charisma}|\n\n" +
                    $"**Skills** {Skills}\n" +
                    $"**Senses** {Senses}\n" +
                    $"**Languages** {Languages}\n" +
                    $"**Challenge** {Challenge}\n\n" +
                    (SpecialFeatures != null ? $"## Special Features\n\n" + SpecialFeatures.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Actions != null ? $"## Actions\n\n" + Actions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Reactions != null ? $"## Reactions\n\n" + Reactions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (LegendaryActions != null ? $"## Legendary Actions\n\n" + LegendaryActions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "");

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
