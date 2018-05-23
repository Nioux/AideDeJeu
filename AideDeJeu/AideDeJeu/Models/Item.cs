using HtmlAgilityPack;
using System.Collections.Generic;

namespace AideDeJeuLib
{
    public class Item
    {
        public string Id { get; set; }
        public string IdVO { get; set; }
        public string IdVF { get; set; }
        public string Name { get; set; }
        public string NameVO { get; set; }
        public string NamePHB { get; set; }
        public string Html { get; set; }

        public static IEnumerable<string> NodeListToStringList(IEnumerable<HtmlNode> nodes)
        {
            if (nodes == null) return null;
            var strings = new List<string>();
            foreach (var node in nodes)
            {
                strings.Add(node.OuterHtml);
            }
            return strings;
        }

        public static HtmlNode StringToNode(string str)
        {
            if (str == null) return null;
            var doc = new HtmlDocument();
            doc.LoadHtml(str);
            return doc.DocumentNode;
        }

        //public static IEnumerable<HtmlNode> StringListToNodeList(IEnumerable<string> strings)
        //{
        //    if (strings == null) return null;
        //    var nodes = new List<HtmlNode>();
        //    foreach (var str in strings)
        //    {
        //        var doc = new HtmlDocument();
        //        doc.LoadHtml(str);
        //        nodes.Add(doc.DocumentNode);
        //    }
        //    return nodes;
        //}

    }
}
