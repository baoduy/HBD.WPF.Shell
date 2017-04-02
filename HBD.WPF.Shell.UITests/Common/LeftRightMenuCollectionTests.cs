#region

using HBD.Mef.Shell;
using HBD.Mef.Shell.Navigation;
using HBD.WPF.Shell.UI.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace WPF.Shell.UITests.Common
{
    [TestClass]
    public class LeftRightMenuCollectionTests
    {
        [TestMethod]
        public void AddTest()
        {
            var coll = new LeftRightMenuCollection();
            coll.Menu("AA").Alignment = MenuAlignment.Left;
            coll.Menu("BB").Alignment = MenuAlignment.Right;

            Assert.IsTrue(coll.LeftMenuItems.Count == 1);
            Assert.IsTrue(coll.RightMenuItems.Count == 1);

            coll.Menu("AA").Alignment = MenuAlignment.Right;

            Assert.IsTrue(coll.LeftMenuItems.Count == 0);
            Assert.IsTrue(coll.RightMenuItems.Count == 2);

            coll.Menu("AA").Alignment = MenuAlignment.Left;
            coll.Menu("BB").Alignment = MenuAlignment.Left;

            Assert.IsTrue(coll.LeftMenuItems.Count == 2);
            Assert.IsTrue(coll.RightMenuItems.Count == 0);

            coll.Remove("AA");
            coll.Remove("BB");

            Assert.IsTrue(coll.LeftMenuItems.Count == 0);
            Assert.IsTrue(coll.RightMenuItems.Count == 0);
            Assert.IsTrue(coll.Count == 0);
        }

        [TestMethod]
        public void ClearTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void ContainsTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void CopyToTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void GetEnumeratorTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void RemoveTest()
        {
            Assert.Fail();
        }
    }
}