using System.Collections.Generic;
using System.Linq;

namespace AideDeJeuLib
{
    public class ClassEvolutionItem : Item
    {
        public string Table { get; set; }
        public string[,] BindableTable
        {
            get
            {
                return ExtractSimpleTable(Table);
            }
        }
        public string[,] ExtractSimpleTable(string table)
        {
            var rows = table.Split('\n');
            var countRows = rows.Count(r => r.StartsWith("|"));
            var countCols = rows.FirstOrDefault().Count(c => c == '|') - 1;
            var matrix = new string[countCols, countRows];
            var y = 0;
            foreach (var row in rows)
            {
                var x = 0;
                if (row.StartsWith("|"))
                {
                    var allcols = row.Split('|');
                    var cols = allcols.Skip(1).Take(allcols.Length - 2);
                    foreach (var col in cols)
                    {
                        var text = col.Replace("<!--br-->", " ").Replace("  ", " ");
                        matrix[x, y] = text;
                        x++;
                    }
                }
                y++;
            }
            return matrix;
        }

    }
}
