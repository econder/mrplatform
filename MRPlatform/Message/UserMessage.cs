/***************************************************************************************************
 * Class: 		MRUserMessage.cs
 * Created By:	Eric Conder
 * Created On:	2014-03-04
 * 
 * Changes:
 * 
 * 2014-03-04	Created Send & Retrieve methods.
 * 
 * 2014-03-05	Created MarkAs... methods.
 * 
 * 2014-03-06	Created GetMessages methods.
 * 
 * 2014-04-03	Added error handling to some of the functions.
 * 
 * 2016-10-06   Changed class constructor to accept only an MRDbConnection object parameter to 
 *              simplify usage for the end user. Added sync method call as well to sync primary and 
 *              secondary databases.
 * *************************************************************************************************/
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
    ComSourceInterfaces(typeof(IUserMessageEvents))]
    public class UserMessage : IUserMessage
	{
        private ErrorLog _errorLog;
        private MRDbConnection _dbConnection;

        // Global message type = 2 for User Messages
        private const int MESSAGETYPE = 2;


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
                    throw;
                }
            }
		}

        
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
                    throw;
                }
            }
        }
		
		
		public void MarkAsRead(string recipient, long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO MessagesRead(msgId, recipient) VALUES(?, ?)";

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
                    throw;
                }
            }
        }
		
		
		public void Archive(string recipient, long msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO MessagesArchived(msgId, recipient) VALUES(?, ?)";

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
                    throw;
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
                    throw;
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
                    throw;
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
                    throw;
                }
            }
        }
    }
}
