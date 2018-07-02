using System;
using System.Collections.Generic;
using System.Text;

namespace AideDeJeuLib.Conditions
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
    }
}
