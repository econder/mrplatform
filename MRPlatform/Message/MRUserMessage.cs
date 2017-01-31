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
using System.Data.SqlClient;

using MRPlatform.AlarmEvent;
using MRPlatform.Data.Sql;


namespace MRPlatform.Message
{
	/// <summary>
	/// Description of MRMessage.
	/// </summary>
	public class MRUserMessage
	{
        // Global message type = 2 for User Messages
        private const int MESSAGETYPE = 2;

        //Properties
        private MRDbConnection DbConnection { get; set; }


        public MRUserMessage(MRDbConnection mrDbConnection)
		{
            DbConnection = mrDbConnection;
        }


        /// <summary>
        /// Class destructor
        /// </summary>
        ~MRUserMessage()
		{
            if (DbConnection.DbConnection.State == ConnectionState.Open)
            {
                DbConnection.DbConnection.Close();
            }
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
			string sQuery = "INSERT INTO Messages(sender, nodeName, recipient, message, type) VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + priority + ")";
			SqlCommand dbCmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
			try
			{
				dbCmd.ExecuteNonQuery();
            }
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
			}
		}

        /*
		public DataSet Retrieve(string sender, int priority, bool unread, bool archived)
		{
			DataSet ds = new DataSet();
			ds = DoRetrieve(sender, priority, unread, archived);
			
			return ds;
		}
		
		
		private DataSet DoRetrieve(string sender, int priority, bool unread, bool archived)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgId, sender, message, type FROM Messages WHERE recipient='" + sender + "' AND priorityId=" + priority + " AND unread=" + unread + " AND archived=" + archived;
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		*/

        
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
            if (DbConnection.DbConnection.State != ConnectionState.Open)
                DbConnection.DbConnection = new SqlConnection();

            SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
            DataSet ds = new DataSet();
            dbAdapt.Fill(ds);

            return ds;
        }


        public void MarkAsUnread(string hmiUserName, int msgId)
		{
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "DELETE FROM MessagesRead WHERE msgId = @msgId AND userName = @userName";

            sqlCmd.Parameters.AddWithValue("@msgId", msgId);
            sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

            try
            {
                sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("SqlException: " + e.Message);
            }
        }
		
		
		public void MarkAsRead(string hmiUserName, int msgId)
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "INSERT INTO MessagesRead(msgId, userName) VALUES(@msgId,@userName)";

            sqlCmd.Parameters.AddWithValue("@msgId", msgId);
            sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

            try
            {
                sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("SqlException: " + e.Message);
            }
        }
		
		
		public void Archive(string hmiUserName, int msgId)
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "INSERT INTO MessagesArchived(msgId, userName) VALUES(@msgId,@userName)";

            sqlCmd.Parameters.AddWithValue("@msgId", msgId);
            sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

            try
            {
                sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("SqlException: " + e.Message);
            }
        }
		
		
		public void UnArchive(string hmiUserName, int msgId)
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "DELETE FROM MessagesArchived WHERE msgId = @msgId AND userName = @userName";

            sqlCmd.Parameters.AddWithValue("@msgId", msgId);
            sqlCmd.Parameters.AddWithValue("@userName", hmiUserName);

            try
            {
                sqlCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("SqlException: " + e.Message);
            }
        }
    }
}
