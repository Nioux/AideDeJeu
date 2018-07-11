using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdig.Syntax;

namespace AideDeJeuLib
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
                    (Skills != null ? $"**Compétences** {Skills}\n" : "") +
                    (SavingThrows != null ? $"**Jets de sauvegarde** {SavingThrows}\n" : "") +
                    (DamageVulnerabilities != null ? $"**Vulnérabilité aux dégâts** {DamageVulnerabilities}\n" : "") +
                    (DamageImmunities != null ? $"**Immunité contre les dégâts** {DamageImmunities}\n" : "") +
                    (ConditionImmunities != null ? $"**Immunité contre les états** {ConditionImmunities}\n" : "") +
                    (DamageResistances != null ? $"**Résistance aux dégâts** {DamageResistances}\n" : "") +
                    $"**Sens** {Senses}\n" +
                    $"**Langues** {Languages}\n" +
                    $"**Dangerosité** {Challenge}\n\n" +
                    (SpecialFeatures != null ? $"## Capacités\n\n" + SpecialFeatures.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Actions != null ? $"## Actions\n\n" + Actions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (Reactions != null ? $"## Réactions\n\n" + Reactions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "") +
                    (LegendaryActions != null ? $"## Actions Légendaires\n\n" + LegendaryActions.Aggregate((s1, s2) => s1 + "\n\n" + s2) : "");
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            throw new NotImplementedException();
        }
    }
}