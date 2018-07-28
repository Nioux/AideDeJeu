using System;
using System.Collections.Generic;
using System.Text;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class HomeItem : Item
    {
        public override string Markdown
        {
            get
            {
                return
                    "# VF (H&D)\n\n" +
                    "## [Races](races_hd.md)\n\n" +
                    "## [Classes](classes_hd.md)\n\n" +
                    "## [Équipement](equipment_hd.md)\n\n" +
                    "## [Caractéristiques](abilities_hd.md)\n\n" +
                    "## [États spéciaux](conditions_hd.md)\n\n" +
                    "## [Sorts](spells_hd.md)\n\n" +
                    "## [Créatures](monsters_hd.md)\n\n" +
                    //"## [Mignons](baby_bestiary_hd.md)\n\n" +
                    "# VO (SRD)\n\n" +
                    "## [Conditions](conditions_vo.md)\n\n" +
                    "## [Spells](spells_vo.md)\n\n" +
                    "## [Monsters](monsters_vo.md)\n\n"
                    ;
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            throw new NotImplementedException();
        }
    }
}
