//using AideDeJeu.ViewModels;
//using AideDeJeuLib;
//using Microsoft.VisualStudio.T.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Threading.Tasks;

//namespace AideDeJeuUnitTest
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        [TestMethod]
//        public async Task TestMethod1()
//        {
//            var diceRoller = new DiceRollerViewModel();
//            var diceRolls = diceRoller.DicesValues(6, 3);
//            foreach(var diceRoll in diceRolls)
//            {
//                Debug.WriteLine($"{diceRoll.Key} => {diceRoll.Value / 3}");
//            }
//            Assert.IsNotNull(diceRolls);
//            //var allItems = new Dictionary<string, Item>();
//            //var store = new StoreViewModel();
//            //var item = store.ToItem(null, AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.sandbox.md"), allItems);
//            //var md = item.Markdown;
//            //var children = await item.GetChildrenAsync();
//            //foreach(var iitem in children)
//            //{
//            //    md += iitem.Markdown;
//            //}
//            //Assert.IsNotNull(md);
//        }
//    }
//}
