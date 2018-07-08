using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib.Models
{
    public class HomeItem : Item
    {
        public override string Markdown
        {
            get
            {
                return 
                    "# [Sorts](spells_hd.md)\n\n" +
                    "# [Créatures](monsters_hd.md)\n\n" +
                    "# [Etats spéciaux](conditions_hd.md)\n\n" +
                    "# [Spells](spells_vo.md)\n\n" +
                    "# [Monsters](monsters_vo.md)\n\n" +
                    "# [Conditions](conditions_vo.md)\n\n";
            }
        }
    }
}
