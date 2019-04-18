using System.Collections.Generic;
using System.Linq;

namespace AideDeJeuLib
{
    public class ClassEvolutionItem : Item
    {
        public string Table { get; set; }
        public List<string> BindableTable
        {
            get
            {
                return ExtractSimpleTable(Table);
            }
        }
        public List<string> ExtractSimpleTable(string table)
        {
            var lines = table.Split('\n');
            var result = new List<string>();
            foreach (var line in lines.Skip(2))
            {
                if (line.StartsWith("|"))
                {
                    var cols = line.Split('|');
                    var text = cols[2].Replace("<!--br-->", " ").Replace("  ", " ");
                    result.Add(text);
                }
            }
            return result;
        }

    }
}
