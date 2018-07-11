using System;
using System.Collections.Generic;
using System.Text;
using Markdig.Syntax;

namespace AideDeJeuLib
{
    public class Condition : Item
    {
        public string Text { get; set; }

        public override string Markdown
        {
            get
            {
                return 
                    $"# {Name}\n\n" +
                    $"{NameVO}\n\n" +
                    Text;
            }
        }

        public override void Parse(ref ContainerBlock.Enumerator enumerator)
        {
            throw new NotImplementedException();
        }
    }
}
