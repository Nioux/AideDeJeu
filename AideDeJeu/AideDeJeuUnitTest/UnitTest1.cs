using AideDeJeu.ViewModels;
using AideDeJeuLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AideDeJeuUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var store = new StoreViewModel();
            var item = store.ToItem(null, AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.sandbox.md"));
            var md = item.Markdown;
            if(item is Items)
            {
                foreach(var iitem in item as Items)
                {
                    md += iitem.Markdown;
                }
            }
            Assert.IsNotNull(md);
        }
    }
}
