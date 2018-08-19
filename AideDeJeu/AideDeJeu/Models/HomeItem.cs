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
                return AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.index.md");
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {

        }
    }
}
