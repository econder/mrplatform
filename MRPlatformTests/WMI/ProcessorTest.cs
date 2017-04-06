using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.WMI;

namespace MRPlatformTests.WMI
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProcessorTest
    {
        private Processor _processor;

        [TestInitialize]
        public void Initialize()
        {
            _processor = new Processor();
        }

        [TestMethod]
        public void TestLoadPercentage()
        {
            int i = 0;
            i = _processor.LoadPercentage;
            Assert.IsTrue(i > 0);
        }
    }
}