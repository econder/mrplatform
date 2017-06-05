using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;

namespace MRPlatform.Message
{
    /// <summary>
    /// Description of MRMessage.
    /// </summary>
    [ComVisible(true)]
    [Guid("389711FE-7AAB-452B-A14E-78EED94A23DB"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IUserMessage))]
    public class UserMessage : IUserMessage
	{
        private ErrorLog _errorLog;
        private MRDbConnection _dbConnection;

        // Global message type = 2 for User Messages
        private const int MESSAGETYPE = 2;

        public UserMessage()
        {

        }

        public UserMessage(MRDbConnection mrDbConnection)
		{
            _errorLog = new ErrorLog();
            _dbConnection = mrDbConnection;
        }

        public IDbConnection Connection
        {
            get
            {
                return new OleDbConnection(_dbConnection.ConnectionString);
            }
        }


        #region " Properties "

        public MRDbConnection DbConnection
        {
            get
            {
                return _dbConnection;
            }
            set
            {
                _dbConnection = value;
            }
        }

        public int ResultsPageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public bool SortAscending { get; set; }

        #endregion


        #region " Send "


        public void Send(string sender, string recipient, string message, int priority = 2)
		{
            DoSend(sender, recipient, message, priority);
		}
		
		
		public void Send(string sender, List<string> recipients, string message, int priority = 2)
		{
			for(int i = 0; i <= recipients.Capacity - 1; i++)
			{
                DoSend(sender, recipients[i].ToString(), message, priority);
			}
		} 
		
		
		private void DoSend(string sender, string recipient, string message, int priority = 2)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO Messages(sender, recipient, message, priorityId, msgTypeId)" + 
                                " VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + priority + ", " + MESSAGETYPE + ")";

                OleDbCommand dbCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);

                try
                {
                    dbCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DoSend(string sender, string recipient, string message, int priority = 2)", ex.Message);
                }
            }
		}

        #endregion


        public DataSet GetMessages(string recipient)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessages" + 
                            " WHERE recipient='" + recipient + "'" + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetMessages(string recipient, int priority)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessages" + 
                            " WHERE recipient='" + recipient + "'" + 
                            " AND priorityId=" + priority + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetUnreadMessages(string recipient)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesUnread" +
                            " WHERE recipient='" + recipient + "'" +
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetUnreadMessages(string recipient, int priority)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
                            " WHERE recipient='" + recipient + "'" + 
                            " AND priorityId=" + priority + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetArchivedMessages(string recipient)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesArchived" +
                            " WHERE recipient='" + recipient + "'" +
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetArchivedMessages(string recipient, int priority)
        {
            string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesArchived" + 
                            " WHERE recipient='" + recipient + "'" + 
                            " AND priorityId=" + priority + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        private DataSet DoGetMessages(string sQuery)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        public void MarkAsUnread(string recipient, long msgId)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "DELETE FROM MessagesRead WHERE msgId = ? AND recipient = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@recipient", recipient);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MarkAsUnread(string recipient, int msgId)", ex.Message);
                }
            }
        }
		
		
		public void MarkAsRead(string recipient, long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO MessagesRead(msgId, recipient)" +
                                " SELECT id, recipient" +
                                " FROM vMessages" +
                                " WHERE id = ? AND recipient = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@recipient", recipient);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MarkAsRead(string recipient, int msgId)", ex.Message);
                }
            }
        }
		
		
		public void Archive(string recipient, long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO MessagesArchived(msgId, recipient)" + 
                                " SELECT id, recipient" + 
                                " FROM vMessages" + 
                                " WHERE id = ? AND recipient = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@recipient", recipient);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Archive(string recipient, int msgId)", ex.Message);
                }
            }
        }
		
		
		public void UnArchive(string recipient, long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "DELETE FROM MessagesArchived WHERE msgId = ? AND recipient = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@recipient", recipient);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnArchive(string recipient, int msgId)", ex.Message);
                }
            }
        }


        public void DeleteMessage(long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "DELETE FROM Messages WHERE msgId = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteMessage(int msgId)", ex.Message);
                }
            }
        }


        public void DeleteArchivedMessage(long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "DELETE FROM MessagesArchived WHERE msgId = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteArchivedMessage(int msgId)", ex.Message);
                }
            }
        }
    }
}
