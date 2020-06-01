using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace AideDeJeuLib
{
    public class ClassEvolutionItem : Item
    {
        [YamlIgnore]
        public override Dictionary<string, Dictionary<string, Dictionary<string, string>>> MapTable
        {
            get
            {
                return null;
            }
        }
        [YamlMember(Alias = "evolution_table")]
        public Dictionary<string, Dictionary<string, Dictionary<string, string[]>>> MapEvolutionTable
        {
            get
            {
                return Table != null ? ExtractMapEvolutionTable(Table) : null;
            }
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, string[]>>> ExtractMapEvolutionTable(string table)
        {
            var map = new Dictionary<string, Dictionary<string, string[]>>();
            var matrix = ExtractMatrixTable(table);
            for (int y = 2; y < matrix.GetLength(1); y++)
            {
                map[matrix[0, y]] = new Dictionary<string, string[]>();
            }
            for (int x = 1; x < matrix.GetLength(0); x++)
            {
                for (int y = 2; y < matrix.GetLength(1); y++)
                {
                    map[matrix[0, y]][matrix[x, 0]] = matrix[x, y].Split(new string[] { ", " }, System.StringSplitOptions.None);
                }
            }
            var supermap = new Dictionary<string, Dictionary<string, Dictionary<string, string[]>>>();
            supermap[matrix[0, 0]] = map;
            return supermap;
        }


        [YamlIgnore]
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
