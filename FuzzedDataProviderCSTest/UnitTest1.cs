using Microsoft.VisualStudio.TestTools.UnitTesting;

using FuzzedDataProviderCSLibrary;

namespace FuzzedDataProviderCSTest
{

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReturnedString()
        {
            Assert.AreEqual(FuzzedDataProviderCSLibrary.FuzzedDataProviderCSLibrary.WriteLine(), "HelloWorld");
            Assert.AreEqual((new FuzzedDataProviderCSLibrary.FuzzedDataProviderCSLibrary()).RetOne(), 1);
        }
    }
}