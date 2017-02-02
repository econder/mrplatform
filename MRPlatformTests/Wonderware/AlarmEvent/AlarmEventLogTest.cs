using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.DB.Sql;
using MRPlatform.Wonderware.AlarmEvent;

namespace MRPlatformTests.Wonderware.AlarmEvent
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AlarmEventLogTest
    {
        private MRDbConnection _mrdb;
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "WWALMDB";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private AlarmEventLog _ae;

        private int _topCount = 500;
        private DateTime _startDate = Convert.ToDateTime("2016-09-15 00:00:00.000");
        private DateTime _endDate = Convert.ToDateTime("2016-09-15 23:59:59.999");
        private string _startDateStr = "2016-09-15 00:00:00.000";
        private string _endDateStr = "2016-09-15 23:59:59.999";
        private int _numDays = -7;

        private string _startDateStrInvalid = "2016-09-2016 00:00:00.000";
        private string _endDateStrInvalid = "2016-09-2016 23:59:59.999";

        private DataSet _ds = new DataSet();


        [TestInitialize]
        public void Initialize()
        {
            _mrdb = new MRDbConnection(_dbServer, _dbName, _dbUser, _dbPass);
            Assert.IsTrue(_mrdb.DbConnected);

            _ae = new AlarmEventLog(_mrdb);
        }

        #region " GetTopAlarmOccurrences(topCount, startDate) "

        // GetTopAlarmOccurrences(int topCount, DateTime startDate)
        [TestMethod]
        public void GetTopAlarmsWithValidStartDate()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDate);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetTopAlarmOccurrences(int topCount, string startDate)
        [TestMethod]
        public void GetTopAlarmsWithValidStartDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDateStr);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetTopAlarmOccurrences(int topCount, string startDate)
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetTopAlarmsWithInvalidStartDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDateStrInvalid);
        }

        // GetTopAlarmOccurrences(int topCount, string startDate)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTopAlarmsWithNullStartDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, "");
        }

        #endregion

        #region " GetTopAlarmOccurrences(topCount, startDate, endDate) "

        [TestMethod]
        public void GetTopAlarmsWithValidStartAndEndDate()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDate, _endDate);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        [TestMethod]
        public void GetTopAlarmsWithValidStartAndEndDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDateStr, _endDateStr);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetTopAlarmsWithInvalidStartAndEndDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _startDateStrInvalid, _endDateStrInvalid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTopAlarmsWithNullStartAndEndDateString()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, "", "");
        }

        #endregion

        #region " GetTopAlarmOccurrences(topCount, startDate, numDays) "

        [TestMethod]
        public void GetTopAlarmsWithValidEndDateNumDays()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _endDate, _numDays);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        [TestMethod]
        public void GetTopAlarmsWithValidEndDateStringNumDays()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _endDateStr, _numDays);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetTopAlarmsWithInvalidEndDateStringNumDays()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, _endDateStrInvalid, _numDays);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTopAlarmsWithNullEndDateStringNumDays()
        {
            _ds = _ae.GetTopAlarmOccurrences(_topCount, "", _numDays);
        }

        #endregion
    }
}