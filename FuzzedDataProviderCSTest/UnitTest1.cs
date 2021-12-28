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
            var fdp = new FuzzedDataProviderCSLibrary.FuzzedDataProviderCS(x,3,s);
            fdp.InsufficientData
            
        }
    }
}