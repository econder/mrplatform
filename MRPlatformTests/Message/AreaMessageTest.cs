using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADODB;

using MRPlatform.DB.Sql;
using MRPlatform.Message;


namespace MRPlatformTests.Message
{
    [TestClass]
    public class AreaMessageTest
    {
        private MRDbConnection _mrdb, _mrdbADO;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private AreaMessage _msg, _msgADO;
        private string _sender = "Sender Name";
        private string _senderInvalid = "";
        private string _area = "Influent PS";
        private string _areaInvalid = "";
        private string _message = "This is a unit test area message";
        private string _messageInvalid = "";
        private int _priority = 1;
        private int _priorityInvalid = 0;

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
            _mrdbADO = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass, true);

            // ADODB Connection
            _msg = new AreaMessage(_mrdb);
            _msgADO = new AreaMessage(_mrdbADO);
        }


        #region " Send "

        // Send(string sender, string area, string message, int priority = 2)
        [TestMethod]
        public void Send()
        {
            int count = 0;
            int countPrev = 0;

            //Get original row count
            _ds = new DataSet();
            _ds = _msg.GetMessages(_area, 0);
            if (_ds.Tables.Count == 0)
            {
                countPrev = 0;
            }
            else
            {
                countPrev = _ds.Tables[0].Rows.Count;
            }

            _msg.Send(_sender, _area, _message, _priority);

            // Make sure the sent message exists
            GetMessagesWithValidArea();
        }

        #endregion

        #region " GetMessages "

        // GetMessages(string area)
        [TestMethod]
        public void GetMessagesWithValidArea()
        {
            _ds = new DataSet();
            _ds = _msg.GetMessages(_area);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetMessages(string area)
        [TestMethod]
        public void GetMessagesInvalidArea()
        {
            _ds = new DataSet();
            _ds = _msg.GetMessages(_areaInvalid);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetMessages(string area, int priority)
        [TestMethod]
        public void GetMessagesWithValidAreaPriority()
        {
            _ds = new DataSet();
            _ds = _msg.GetMessages(_area, _priority);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetMessages(string area, int priority)
        [TestMethod]
        public void GetMessagesWithInvalidAreaPriority()
        {
            _ds = new DataSet();
            _ds = _msg.GetMessages("", -1);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
        }

        #endregion

        #region " GetUnreadMessages "

        // GetUnreadMessages(string area)
        [TestMethod]
        public void GetUnreadMessagesWithValidArea()
        {
            // Make sure a message exists
            SendAllWithValidAreaString();

            _ds = new DataSet();
            _ds = _msg.GetUnreadMessages(_sender, _area);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetUnreadMessages(string area)
        [TestMethod]
        public void GetUnreadMessagesWithInvalidArea()
        {
            _ds = new DataSet();
            _ds = _msg.GetUnreadMessages("", _area);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetUnreadMessages(string area, int priority)
        [TestMethod]
        public void GetUnreadMessagesWithValidAreaPriority()
        {
            _ds = new DataSet();
            _ds = _msg.GetMessages(_area, _priority);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        #endregion
    }
}
