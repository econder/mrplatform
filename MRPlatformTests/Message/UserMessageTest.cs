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
            GetMessagesWithValidRecip();
        }

        #region " GetMessages "

        // VERIFY MESSAGE EXISTS
        // GetMessages(string sender)
        [TestMethod]
        public void GetMessagesWithValidRecip()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetMessages(string sender)
        [TestMethod]
        public void GetMessagesInvalidRecip()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages("");

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
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

        // GetMessages(string recipient, int priority)
        [TestMethod]
        public void GetMessagesWithInvalidSenderPriority()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages("", -1);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
        }

        #endregion

        #region " GetUnreadMessages "

        // GetUnreadMessages(string recipient)
        [TestMethod]
        public void GetUnreadMessagesWithValidRecipient()
        {
            // Make sure a message exists
            SendAllValidWithRecipientString();

            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetUnreadMessages(string recipient)
        [TestMethod]
        public void GetUnreadMessagesWithInvalidRecipient()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages("");

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
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

        #region " GetArchivedMessages "

        // GetArchivedMessages(string recipient)
        [TestMethod]
        public void GetArchivedMessagesWithValidRecipient()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            _msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetArchivedMessages(string recipient)
        [TestMethod]
        public void GetArchivedMessagesWithInvalidRecipient()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages("");

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsFalse(_ds.Tables[0].Rows.Count >= 1);
        }

        // GetArchivedMessages(string recipient, int priority)
        [TestMethod]
        public void GetArchivedMessagesWithValidRecipientPriority()
        {
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient, _priority);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }

        #endregion

        #region " MarkAsRead "

        // MarkAsRead(string recipient, long msgId)
        [TestMethod]
        public void MarkAsReadWithValidRecipMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead(_recipient, msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(unreadCount == unreadCountPrev - 1);
        }

        // MarkAsRead(string recipient, long msgId)
        [TestMethod]
        public void MarkAsReadWithInvalidRecipValidMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead("", msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(unreadCount == unreadCountPrev);
        }

        // MarkAsRead(string recipient, long msgId)
        [TestMethod]
        public void MarkAsReadWithValidRecipInvalidMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead(_recipient, -1);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(unreadCount == unreadCountPrev);
        }

        #endregion

        #region " MarkAsUnread "

        // MarkAsUnread(string recipient, long msgId)
        [TestMethod]
        public void MarkAsUnReadWithValidRecipMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead(_recipient, msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(unreadCount == unreadCountPrev - 1);

            _userMsg.MarkAsUnread(_recipient, _msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(unreadCount == unreadCountPrev);
        }

        // MarkAsUnread(string recipient, long msgId)
        [TestMethod]
        public void MarkAsUnReadWithInvalidRecipValidMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead(_recipient, msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(unreadCount == unreadCountPrev - 1);

            _userMsg.MarkAsUnread("", _msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(unreadCount == unreadCountPrev);
        }

        // MarkAsUnread(string recipient, long msgId)
        [TestMethod]
        public void MarkAsUnReadWithValidRecipInvalidMsgId()
        {
            int unreadCount = 0;
            int unreadCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.MarkAsRead(_recipient, _msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(unreadCount == unreadCountPrev - 1);

            _userMsg.MarkAsUnread(_recipient, -1);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetUnreadMessages(_recipient);
            unreadCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(unreadCount == unreadCountPrev);
        }

        #endregion

        #region " Archive "

        // Archive(string recipient, long msgId)
        [TestMethod]
        public void MarkAsArchivedWithValidRecipMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.Archive(_recipient, msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(archivedCount == archivedCountPrev + 1);
        }

        // Archive(string recipient, long msgId)
        [TestMethod]
        public void ArchiveWithInvalidRecipValidMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.Archive("", msgId);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(archivedCount == archivedCountPrev + 1);
        }

        // Archive(string recipient, long msgId)
        [TestMethod]
        public void ArchiveWithValidRecipInvalidMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Mark a message as read
            _userMsg.Archive(_recipient, -1);

            // Get unread messages count again to check
            // that message is now marked as read
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(archivedCount == archivedCountPrev);
        }

        #endregion

        #region " UnArchive "

        // UnArchive(string recipient, long msgId)
        [TestMethod]
        public void UnArchiveWithValidRecipMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient);
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Archive a message
            _userMsg.Archive(_recipient, msgId);

            // Get archived messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]) == msgId);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Unarchive a message
            _userMsg.UnArchive(_recipient, _msgId);

            // Get messages count again to check
            // that message is now marked as archived
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(archivedCount == archivedCountPrev - 1);
        }

        // UnArchive(string recipient, long msgId)
        [TestMethod]
        public void UnArchiveWithInvalidRecipValidMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient);
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Archive a message
            _userMsg.Archive(_recipient, msgId);

            // Get archived messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]) == msgId);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Unarchive a message
            _userMsg.UnArchive("", _msgId);

            // Get messages count again to check
            // that message is now marked as archived
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsFalse(archivedCount == archivedCountPrev - 1);
        }

        // UnArchive(string recipient, long msgId)
        [TestMethod]
        public void UnArchiveWithValidRecipInvalidMsgId()
        {
            int archivedCount = 0;
            int archivedCountPrev = 0;
            long msgId;

            // Send a message first
            SendAllValidWithRecipientString();

            // Get unread messages count
            _ds = new DataSet();
            _ds = _userMsg.GetMessages(_recipient);
            msgId = Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Archive a message
            _userMsg.Archive(_recipient, 0);

            // Get archived messages count
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCountPrev = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(Convert.ToInt64(_ds.Tables[0].Rows[0]["id"]) == msgId);
            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);

            // Unarchive a message
            _userMsg.UnArchive(_recipient, _msgId);

            // Get messages count again to check
            // that message is now marked as archived
            _ds = new DataSet();
            _ds = _userMsg.GetArchivedMessages(_recipient);
            archivedCount = _ds.Tables[0].Rows.Count;

            Assert.IsTrue(archivedCount == archivedCountPrev - 1);
        }

        #endregion
    }
}
