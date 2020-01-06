using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagmaWorksToolbar;

namespace ToolbarTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var toolbar = new Toolbar();
            Assert.AreEqual(true, CheckDynamo.checkDynamoPackages());
        }
    }
}
