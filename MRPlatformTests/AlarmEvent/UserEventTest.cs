using System;
using System.Data;
using ADODB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.DB.Sql;
using MRPlatform.AlarmEvent;


namespace MRPlatformTests.Message
{
    [TestClass]
    public class UserEventTest
    {
        private MRDbConnection _mrdb, _mrdbADO;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private UserEvent _ue, _ueADO;
        private string _userName = "mrsystems";
        private string _userNameInvalid = "";
        private string _nodeName = "WTP-WS1";
        private string _nodeNameInvalid = "";
        private string _eventMessage = "This is a user event message.";
        private string _eventMessageInvalid = "";
        private int _eventType = 1;
        private string _eventSource = "FS - Plant Overview";
        private string _eventSourceInvalid = "";

        private int _pageNumber = 1;
        private int _pageNumberInvalid = 0;
        private int _resultsPerPage = 5000;
        private int _resultsPerPageInvalid = 0;
        private bool _sortAscending = false;

        private DataSet _ds;
        private Recordset _rs;


        [TestInitialize]
        public void Initialize()
        {
            // OleDbConnection
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _ue = new UserEvent(_mrdb);

            // ADODB Connection
            _mrdbADO = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass, true);
            _ue = new UserEvent(_mrdbADO);
        }

        #region " LogEvent() "

        // LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)
        [TestMethod]
        public void LogEvent()
        {
            int count = 0;
            int countPrev = 0;

            // Get original row count
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(_pageNumber, _resultsPerPage, _sortAscending);
            if(_ds.Tables.Count == 0)
            {
                countPrev = 0;
            }
            else
            {
                countPrev = _ds.Tables[0].Rows.Count;
            }

            // Log event
            _ue.LogEvent(_userName, _nodeName, _eventMessage, _eventType, _eventSource);

            // Get original row count
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(_pageNumber, _resultsPerPage, _sortAscending);
            if (_ds.Tables.Count == 0)
            {
                count = 0;
            }
            else
            {
                count = _ds.Tables[0].Rows.Count;
            }

            Assert.IsTrue(count > countPrev);
        }

        // LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)
        [TestMethod]
        public void LogEventADO()
        {
            int count = 0;
            int countPrev = 0;

            // Get original row count
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(_pageNumber, _resultsPerPage, _sortAscending);
            countPrev = _rs.RecordCount;
            _rs.Close();

            // Log event
            _ueADO.LogEvent(_userName, _nodeName, _eventMessage, _eventType, _eventSource);

            // Get original row count
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(_pageNumber, _resultsPerPage, _sortAscending);
            count = _rs.RecordCount;
            _rs.Close();

            Assert.IsTrue(count > countPrev);
        }

        #endregion

        #region " GetHistory(int pageNumber, int resultsPerPage, bool sortAscending) "

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        public void GetHistory()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(_pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(_pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_rs.RecordCount >= 1);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryInvalidPageNumberDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(_pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryInvalidPageNumberRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(_pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryInvalidResultsPerPageDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(_pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryInvalidResultsPerPageRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(_pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        #endregion

        #region " GetHistory(eventDate, int pageNumber, int resultsPerPage, bool sortAscending) "

        // GetHistory(eventDate, int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        public void GetHistoryWithStartDate()
        {
            // Log event
            _ue.LogEvent(_userName, _nodeName, _eventMessage, _eventType, _eventSource);

            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.ToOADate(), _pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_rs.RecordCount >= 1);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventDateInvalidPageNumberDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventDateInvalidPageNumberRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.ToOADate(), _pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventDateInvalidResultsPerPageDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventDateInvalidResultsPerPageRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.ToOADate(), _pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        #endregion

        #region " GetHistory(eventStartDate, eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending) "

        // GetHistory(eventStartDate, eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        public void GetHistoryWithStartEndDate()
        {
            // Log event
            _ue.LogEvent(_userName, _nodeName, _eventMessage, _eventType, _eventSource);

            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now.AddDays(-1), DateTime.Now, _pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_ds.Tables.Count >= 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.AddDays(-1).ToOADate(), DateTime.Now.ToOADate(), _pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_rs.RecordCount >= 1);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventStartEndDateInvalidPageNumberDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, DateTime.Now, _pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventStartEndDateInvalidPageNumberRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.ToOADate(), DateTime.Now.ToOADate(), _pageNumberInvalid, _resultsPerPage, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventStartEndDateInvalidResultsPerPageDataSet()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, DateTime.Now, _pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        // GetHistory(int pageNumber, int resultsPerPage, bool sortAscending)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetHistoryEventStartEndDateInvalidResultsPerPageRecordset()
        {
            _rs = new Recordset();
            _rs = _ueADO.GetHistoryRecordset(DateTime.Now.ToOADate(), DateTime.Now.ToOADate(), _pageNumber, _resultsPerPageInvalid, _sortAscending);
        }

        #endregion
    }
}
