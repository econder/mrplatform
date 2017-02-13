/***************************************************************************************************
* Class:    	MRAreaMessage.cs
* Created By:	Eric Conder
* Created On:	2014-03-07
* 
* Changes:
* 
* 2014-03-07	Created class.
*				
* 2014-04-03	Added msgType to Send function query.
* 
* 2016-10-06    Changed class constructor to accept only an MRDbConnection object parameter to 
*               simplify usage for the end user. Added sync method call as well to sync primary and 
*               secondary databases.
*               
****************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform.AlarmEvent;
using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
	/// <summary>
	/// Description of MRAreaMessage.
	/// </summary>
	public class AreaMessage
	{
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;

        // Global message type = 1 for Area Messages
        private const int MESSAGETYPE = 1;


        public AreaMessage(MRDbConnection mrDbConnection)
		{
            _dbConnection = mrDbConnection;
		}

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_dbConnection.ConnectionString);
            }
        }


        public void Send(string sender, string recipient, string message, int priority = 2)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "INSERT INTO Messages(sender, recipient, message, msgTypeId, priorityId)" +
                                " VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + MESSAGETYPE + ", " + priority + ")";
                SqlCommand dbCmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    dbCmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Send(string sender, string recipient, string message, int priority = 2)", ex.Message);
                }
            }
		}
		
		
		public DataSet GetMessages(string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient='" + area + "'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetMessages(string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" + 
                                " FROM vMessages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority;

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" + 
                                " FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND userName='" + userName + "'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority" +
                                " FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND userName='" + userName + "'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" +
                                " AND userName='" + userName + "'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" +
                                " AND userName='" + userName + "'";

                SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public int Count(string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM Messages" +
                                " WHERE recipient='" + area + "'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		public int Count(string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM Messages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority;

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int Count(string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM Messages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority, DateTime dtDate)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM Messages" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		public int UnreadCount(string userName, string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND userName='" + userName + "'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string userName, string area)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		public int UnreadCount(string userName, string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND userName='" + userName + "'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string userName, string area, int priority)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" +
                                " AND userName='" + userName + "'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string startDate)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient='" + area + "'" +
                                " AND priority=" + priority +
                                " AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" +
                                " AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" +
                                " AND userName='" + userName + "'";

                SqlCommand cmd = new SqlCommand(sQuery, (SqlConnection)dbConnection);

                try
                {
                    int rowCount = (int)cmd.ExecuteScalar();
                    return rowCount;
                }
                catch (SqlException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)", ex.Message);
                    return -1;
                }
            }
		}
	}
}
