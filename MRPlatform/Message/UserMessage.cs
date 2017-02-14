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


        public void Send(string sender, string recipient, string message, int priority = 2)
		{
            DoSend(sender, recipient, message, priority);
		}
		
		
		public void Send(string sender, Array recipients, string message, int priority = 2)
		{
			for(int i = 0; i <= recipients.Length; i++)
			{
                DoSend(sender, recipients.GetValue(i).ToString(), message, priority);
			}
		} 
		
		
		private void DoSend(string sender, string recipient, string message, int priority = 2)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "INSERT INTO Messages(sender, nodeName, recipient, message, type)" + 
                                " VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + priority + ")";

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

        
        public DataSet GetMessages(string sender)
        {
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
                            " WHERE recipient='" + sender + "'" + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetMessages(string sender, int priority)
        {
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM Messages" + 
                            " WHERE recipient='" + sender + "'" + 
                            " AND priorityId=" + priority + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetMessages(string sender, int priority, bool unread)
        {
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM Messages" + 
                            " WHERE recipient='" + sender + "'" + 
                            " AND priorityId=" + priority + 
                            " AND unread=" + unread + 
                            " ORDER BY msgDateTime DESC";

            return DoGetMessages(sQuery);
        }


        public DataSet GetMessages(string sender, int priority, bool unread, bool archived)
        {
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM Messages" + 
                            " WHERE recipient='" + sender + "'" + 
                            " AND priorityId=" + priority + 
                            " AND unread=" + unread + 
                            " AND archived=" + archived + 
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


        public void MarkAsUnread(string hmiUserName, int msgId)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "DELETE FROM MessagesRead WHERE msgId = @msgId AND userName = @userName";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MarkAsUnread(string hmiUserName, int msgId)", ex.Message);
                    throw;
                }
            }
        }
		
		
		public void MarkAsRead(string hmiUserName, int msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "INSERT INTO MessagesRead(msgId, userName) VALUES(@msgId,@userName)";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MarkAsRead(string hmiUserName, int msgId)", ex.Message);
                    throw;
                }
            }
        }
		
		
		public void Archive(string hmiUserName, int msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "INSERT INTO MessagesArchived(msgId, userName) VALUES(@msgId,@userName)";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Archive(string hmiUserName, int msgId)", ex.Message);
                    throw;
                }
            }
        }
		
		
		public void UnArchive(string hmiUserName, int msgId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {

                string sQuery = "DELETE FROM MessagesArchived WHERE msgId = @msgId AND userName = @userName";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@msgId", msgId);
                sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnArchive(string hmiUserName, int msgId)", ex.Message);
                    throw;
                }
            }
        }
    }
}
