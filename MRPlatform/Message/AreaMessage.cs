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
        // Global message type = 1 for Area Messages
        private const int MESSAGETYPE = 1;

        // Properties
        private MRDbConnection DbConnection { get; set; }


        public AreaMessage(MRDbConnection mrDbConnection)
		{
            DbConnection = mrDbConnection;
		}


        /// <summary>
        /// Class destructor
        /// </summary>
        ~AreaMessage()
		{
            if (DbConnection.DatabaseConnection.State == ConnectionState.Open)
            {
                DbConnection.DatabaseConnection.Close();
            }
        }

        
        public void Send(string sender, string recipient, string message, int priority = 2)
		{
			string sQuery = "INSERT INTO Messages(sender, recipient, message, msgTypeId, priorityId) VALUES('" + sender + "', '" + recipient + "', '" + message + "', " + MESSAGETYPE + ", " + priority + ")";
			SqlCommand dbCmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
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
		
		
		public DataSet GetMessages(string area)
		{
			DataSet ds = new DataSet();
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" +
                            " WHERE recipient='" + area + "'";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetMessages(string area, int priority)
		{
			DataSet ds = new DataSet();
            string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" +
                            " WHERE recipient='" + area + "'" +
                            " AND priority=" + priority;

            SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetMessages(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND userName='" + userName + "'";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetUnreadMessages(string userName, string area, int priority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND userName='" + userName + "'";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetUnreadMessages(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DatabaseConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public int Count(string area)
		{
			DataSet ds = new DataSet();
            string sQuery = "SELECT COUNT(*) FROM Messages" +
                            " WHERE recipient='" + area + "'";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		public int Count(string area, int priority)
		{
			DataSet ds = new DataSet();
            string sQuery = "SELECT COUNT(*) FROM Messages" +
                            " WHERE recipient='" + area + "'" +
                            " AND priority=" + priority;
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		private int Count(string area, int priority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		private int Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		public int UnreadCount(string userName, string area)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND userName='" + userName + "'";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		public int UnreadCount(string userName, string area, int priority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND userName='" + userName + "'";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + area + "'" + 
							" AND priority=" + priority + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DatabaseConnection);
			
			try
			{
				int rowCount = (int)cmd.ExecuteScalar();
				return rowCount;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return -1;
			}
		}
	}
}
