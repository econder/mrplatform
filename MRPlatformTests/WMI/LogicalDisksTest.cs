using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.WMI;

namespace MRPlatformTests.WMI
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LogicalDisksTest
    {
        private LogicalDisks _logicalDisks;

        [TestInitialize]
        public void Initialize()
        {
            _logicalDisks = new LogicalDisks();
        }


        #region " public LogicalDisk.Disks[int index] "

        [TestMethod]
        public void GetDiskWithValidIndex()
        {
            int driveIndex = 1;
            var result = _logicalDisks[driveIndex];
            Assert.IsNotNull(result);
        }

        #endregion

        #region " Enumerator Methods "

        [TestMethod]
        public void GetDiskCount()
        {
            var result = _logicalDisks.Count;
            Assert.IsTrue(result >= 1);
        }

        #endregion
    }
}