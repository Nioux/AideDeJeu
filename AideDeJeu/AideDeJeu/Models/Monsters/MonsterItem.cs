using AideDeJeu;
using AideDeJeu.Tools;
using Markdig.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;

namespace AideDeJeuLib
{
    public class MonsterItem : Item
    {
        public string Family { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Alignment { get; set; }
        public string Terrain { get; set; }
        public string Legendary { get; set; }
        //public string Source { get; set; }
        public string ArmorClass { get; set; }
        public string HitPoints { get; set; }
        public string Speed { get; set; }
        public string Strength { get; set; }
        public string Dexterity { get; set; }
        public string Constitution { get; set; }
        public string Intelligence { get; set; }
        public string Wisdom { get; set; }
        public string Charisma { get; set; }
        public string SavingThrows { get; set; }
        public string Skills { get; set; }
        public string DamageVulnerabilities { get; set; }
        public string DamageImmunities { get; set; }
        public string ConditionImmunities { get; set; }
        public string DamageResistances { get; set; }
        public string Senses { get; set; }
        public string Languages { get; set; }
        public string Challenge { get; set; }
    }
}
