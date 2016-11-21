using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.WMI;

namespace MRPlatformTests
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

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void GetDiskWithInvalidDriveLetter()
        {
            string driveLetter = null;
            var result = _logicalDisks.Disk(driveLetter);
        }
    }
}