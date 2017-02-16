using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.DB.Sql;
using MRPlatform.Message;


namespace MRPlatformTests.Message
{
    [TestClass]
    public class UserMessageTest
    {
        private MRDbConnection _mrdb;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private UserMessage _userMsg;
        private string _sender = "Sender Name";
        private string _senderInvalid = "John Doe";
        private string _recipient = "Recipient Name";
        private string _recipientInvalid = "Jane Doe";
        private List<string> _recipients;
        private string _message = "This is a unit test message";
        private int _priority = 1;
        private long _msgId;

        private DataSet _ds;


        [TestInitialize]
        public void Initialize()
        {
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _userMsg = new UserMessage(_mrdb);
            _recipients = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                _recipients.Add(string.Format("John Doe {0}", i));
            }
        }

        // CREATE MESSAGE
        // Send(string sender, string recipient, string message, int priority = 2)
        [TestMethod]
        public void SendAllValidWithRecipientString()
        {
            _userMsg.Send(_sender, _recipient, _message, _priority);

            // Make sure the sent message exists
            GetMessagesWithValidSender();
        }

        // VERIFY MESSAGE EXISTS
        // GetMessages(string sender)
        [TestMethod]
        public void GetMessagesWithValidSender()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetMessages(string recipient, int priority)
        [TestMethod]
        public void GetMessagesWithValidSenderPriority()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient, _priority);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        #region " GetUnreadMessages(string recipient) "

        // GetUnreadMessages(string recipient)
        [TestMethod]
        public void GetUnreadMessagesWithValidRecipient()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetUnreadMessages(string recipient)
        [TestMethod]
        public void GetUnreadMessagesWithInvalidRecipient()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages("John Doe");
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetUnreadMessages(string recipient, int priority)
        [TestMethod]
        public void GetUnreadMessagesWithValidRecipientPriority()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient, _priority);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        #endregion
    }
}
