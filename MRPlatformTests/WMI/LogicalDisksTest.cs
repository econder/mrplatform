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
            int driveIndex = 0;
            var result = _logicalDisks[driveIndex];
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetDiskWithInvalidIndex()
        {
            int driveIndex = 20;
            var result = _logicalDisks[driveIndex];
            Assert.IsNull(result);
        }

        #endregion

        #region " public LogicalDisk.Disk(string driveLetter) "

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDiskWithInvalidDriveLetter()
        {
            string driveLetter = null;
            var result = _logicalDisks.Disk(driveLetter);
        }
        
        [TestMethod]
        public void GetDiskWithValidDriveLetter()
        {
            string driveLetter = "C";
            var result = _logicalDisks.Disk(driveLetter);
            Assert.IsInstanceOfType(result, result.GetType());
            Assert.IsNotNull(result);
        }

        #endregion

        #region " Enumerator Methods "

        [TestMethod]
        public void GetDiskCount()
        {
            var result = _logicalDisks.Disks.Count;
            Assert.IsTrue(result >= 1);
        }

        #endregion
    }
}