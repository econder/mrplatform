using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatformVBA.DB.Sql;


namespace MRPlatformVBA.Message
{
    /// <summary>
    /// Description of MRAreaMessage.
    /// </summary>
    [ComVisible(true)]
    [Guid("F0EC291F-F17C-4CC2-A741-E464BAFB12B4"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IAreaMessageEvents))]
    public class AreaMessage : IAreaMessage
	{
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;

        // Global message type = 1 for Area Messages
        private const int MESSAGETYPE = 1;


        public AreaMessage(MRDbConnection mrDbConnection)
		{
            _dbConnection = mrDbConnection;
		}

        
        public void Send(string sender, string area, string message, int priority = 2)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "INSERT INTO Messages(sender, recipient, message, msgTypeId, priorityId)" +
                                " VALUES(?, ?, ?, ?, ?)";
                                //" VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + MESSAGETYPE + ", " + priority + ")";
                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@sender", sender);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@message", message);
                sqlCmd.Parameters.AddWithValue("@msgTypeId", MESSAGETYPE);
                sqlCmd.Parameters.AddWithValue("@priorityId", priority);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Send(string sender, string recipient, string message, int priority = 2)", ex.Message);
                }
            }
		}
		
		
		public DataSet GetMessages(string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetMessages(string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" + 
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@msgDateTimeStart", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@msgDateTimeEnd", dtDate.Date.ToString() + " 00:00:00.000");

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@msgDateTimeStart", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@msgDateTimeEnd", dtEndDate.Date.ToString() + " 00:00:00.000");

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND sender = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority" +
                                " FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priority= ?" +
                                " AND sender = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT id, msgDateTime, recipient, message, priority FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?" +
                                " ORDER BY msgDateTime DESC";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtEndDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		public int Count(string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
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
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@prioprity", priority);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
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
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtDate.Date.ToString() + " 00:00:00.000");

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
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
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtEndDate.Date.ToString() + " 00:00:00.000");

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
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
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		public int UnreadCount(string userName, string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area, int priority)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
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
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtEndDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)", ex.Message);
                    return -1;
                }
            }
		}
	}
}
