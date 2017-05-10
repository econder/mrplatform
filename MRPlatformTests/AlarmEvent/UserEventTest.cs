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
        private MRDbConnection _mrdb;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private UserEvent _ue;
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
        private int _resultsPerPage = 5000;
        private bool _sortAscending = false;

        private DataSet _ds;
        private Recordset _rs;
        private long _msgId;


        [TestInitialize]
        public void Initialize()
        {
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _ue = new UserEvent(_mrdb);
        }

        // Send(string sender, string recipient, string message, int priority = 2)
        //[TestMethod]
        public void LogEvent()
        {
            int count = 0;
            int countPrev = 0;

            // Get original row count
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumber, _resultsPerPage, _sortAscending);
            countPrev = _ds.Tables[0].Rows.Count;

            // Log event
            _ue.LogEvent(_userName, _nodeName, _eventMessage, _eventType, _eventSource);

            // Get original row count
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumber, _resultsPerPage, _sortAscending);
            count = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(count > countPrev);
        }

        #region " GetHistory "

        // GetHistory(DateTime startDate)
        //[TestMethod]
        public void GetHistoryWithValidStartDate()
        {
            _ds = new DataSet();
            _ds = _ue.GetHistoryDataSet(DateTime.Now, _pageNumber, _resultsPerPage, _sortAscending);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            _rs = new Recordset();
            _rs = _ue.GetHistoryRecordset(DateTime.Now.ToOADate(), _pageNumber, _resultsPerPage, _sortAscending);

            Assert.IsTrue(_rs.RecordCount >= 1);
        }


        #endregion
    }
}
