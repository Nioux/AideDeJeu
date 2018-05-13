using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AideDeJeuLib
{
    public class Item
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameVO { get; set; }
        public string NamePHB { get; set; }
        public string Html { get; set; }
    }
}
