using AideDeJeu.ViewModels;
using AideDeJeuLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AideDeJeuUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var store = new StoreViewModel();
            var item = store.ToItem(null, AideDeJeu.Tools.Helpers.GetResourceString($"AideDeJeu.Data.sandbox.md"));
            var md = item.Markdown;
            var children = await item.GetChildrenAsync();
            foreach(var iitem in children)
            {
                md += iitem.Markdown;
            }
            Assert.IsNotNull(md);
        }
    }
}
