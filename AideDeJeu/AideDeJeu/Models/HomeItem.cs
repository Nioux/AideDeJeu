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
                    "# [Sorts](spells_hd.md)\n\n" +
                    "# [Créatures](monsters_hd.md)\n\n" +
                    "# [Etats spéciaux](conditions_hd.md)\n\n" +
                    "# [Spells](spells_vo.md)\n\n" +
                    "# [Monsters](monsters_vo.md)\n\n" +
                    "# [Conditions](conditions_vo.md)\n\n";
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            throw new NotImplementedException();
        }
    }
}
