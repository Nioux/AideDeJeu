using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig.Syntax;

namespace AideDeJeuLib
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
                    (Skills != null ? $"**Skills** {Skills}\n" : "") +
                    (SavingThrows != null ? $"**Saving Throws** {SavingThrows}\n" : "") +
                    (DamageVulnerabilities != null ? $"**Damage Vulnerabilities** {DamageVulnerabilities}\n" : "") +
                    (DamageImmunities != null ? $"**Damage Immunities** {DamageImmunities}\n" : "") +
                    (ConditionImmunities != null ? $"**Condition Immunities** {ConditionImmunities}\n" : "") +
                    (DamageResistances != null ? $"**Damage Resistances** {DamageResistances}\n" : "") +
                    $"**Senses** {Senses}\n" +
                    $"**Languages** {Languages}\n" +
                    $"**Challenge** {Challenge}\n\n" +
                    (SpecialFeatures != null ? $"## Special Features\n\n" + SpecialFeatures.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Actions != null ? $"## Actions\n\n" + Actions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Reactions != null ? $"## Reactions\n\n" + Reactions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (LegendaryActions != null ? $"## Legendary Actions\n\n" + LegendaryActions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "");
            }
        }
    }
}
