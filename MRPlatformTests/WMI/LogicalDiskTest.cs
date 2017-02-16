using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.WMI;

namespace MRPlatformTests.WMI
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LogicalDiskTest
    {
        private LogicalDisk _logicalDisk;

        [TestInitialize]
        public void Initialize()
        {
            _logicalDisk = new LogicalDisk();
        }


        #region " Test Properties "

        [TestMethod]
        public void TestCaption()
        {
            string s = "Test caption";
            _logicalDisk.Caption = s;
            Assert.AreEqual(s, _logicalDisk.Caption);
        }

        [TestMethod]
        public void TestDescription()
        {
            string s = "Test description";
            _logicalDisk.Description = s;
            Assert.AreEqual(s, _logicalDisk.Description);
        }

        [TestMethod]
        public void TestDeviceId()
        {
            string s = "Test device id";
            _logicalDisk.DeviceId = s;
            Assert.AreEqual(s, _logicalDisk.DeviceId);
        }

        [TestMethod]
        public void TestErrorDescription()
        {
            string s = "Test error description";
            _logicalDisk.ErrorDescription = s;
            Assert.AreEqual(s, _logicalDisk.ErrorDescription);
        }

        [TestMethod]
        public void TestFreeSpace()
        {
            double d = 1.7 * (10^308);
            _logicalDisk.FreeSpace = d;
            Assert.AreEqual(d, _logicalDisk.FreeSpace);
        }

        [TestMethod]
        public void TestInstallDate()
        {
            DateTime dt = Convert.ToDateTime("2017-01-01 00:00:00.000");
            _logicalDisk.InstallDate = dt;
            Assert.AreEqual(dt, _logicalDisk.InstallDate);
        }

        [TestMethod]
        public void TestLastErrorCode()
        {
            int i = 32767;
            _logicalDisk.LastErrorCode = i;
            Assert.AreEqual(i, _logicalDisk.LastErrorCode);
        }

        [TestMethod]
        public void TestName()
        {
            string s = "Test name";
            _logicalDisk.Name = s;
            Assert.AreEqual(s, _logicalDisk.Name);
        }

        [TestMethod]
        public void TestSize()
        {
            double d = 1.7 * (10 ^ 308);
            _logicalDisk.Size = d;
            Assert.AreEqual(d, _logicalDisk.Size);
        }

        [TestMethod]
        public void TestStatus()
        {
            string s = "Test status";
            _logicalDisk.Status = s;
            Assert.AreEqual(s, _logicalDisk.Status);
        }

        [TestMethod]
        public void TestSystemName()
        {
            string s = "Test system name";
            _logicalDisk.SystemName = s;
            Assert.AreEqual(s, _logicalDisk.SystemName);
        }

        #endregion
    }
}