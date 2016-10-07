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
using MRPlatform.Data.Sql;


namespace MRPlatform.Message
{
	/// <summary>
	/// Description of MRAreaMessage.
	/// </summary>
	public class MRAreaMessage
	{
		public MRAreaMessage(MRDbConnection mrDbConnection)
		{
            DbConnection = mrDbConnection;
		}


        /// <summary>
        /// Class destructor
        /// </summary>
        ~MRAreaMessage()
		{
            //Any destructor code goes here
        }


        public enum Priority : int
        {
            Low = 1,
            Medium,
            High,
            Critical
        }


        //
        // TODO: Change nType to nPriority (-1=All; 0=Low; 1=Medium; 2=High; 3=Critical).
        //

        
        public void Send(string userName, string nodeName, string recipient, string message, int priority = 0)
		{
			string sQuery = "INSERT INTO Messages(userName, nodeName, recipient, message, priority, msgType) VALUES('" + userName + "', '" + nodeName + "', '" + recipient + "', '" + message + "', " + priority + ", " + 1 + ")";
			SqlCommand dbCmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
			try
			{
				dbCmd.ExecuteNonQuery();

                //Sync databases
                // TODO: Change so that based on where code is called from, the direction is automatically determined.
                DbConnection.Sync(MRDbConnection.SyncDirection.UploadAndDownload);
            }
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
			}
		}
		
		
		public DataSet GetMessages(string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetMessages(string sArea, int nPriority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetMessages(string sArea, int nPriority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetMessages(string sArea, int nPriority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetUnreadMessages(string userName, string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1" + 
							" AND userName='" + userName + "'";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public DataSet GetUnreadMessages(string userName, string sArea, int nPriority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND userName='" + userName + "'";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetUnreadMessages(string userName, string sArea, int nPriority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet GetUnreadMessages(string userName, string sArea, int nPriority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public int Count(string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		public int Count(string sArea, int nPriority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		private int Count(string sArea, int nPriority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		private int Count(string sArea, int nPriority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		public int UnreadCount(string userName, string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1" + 
							" AND userName='" + userName + "'";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		public int UnreadCount(string userName, string sArea, int nPriority)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND userName='" + userName + "'";
			
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		private int UnreadCount(string userName, string sArea, int nPriority, DateTime dtDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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
		
		
		private int UnreadCount(string userName, string sArea, int nPriority, DateTime dtStartDate, DateTime dtEndDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND priority=" + nPriority + 
							" AND msgType=1" + 
							" AND msgDateTime >= '" + dtStartDate.Date.ToString() + " 00:00:00.000'" + 
							" AND msgDateTime <	'" + dtEndDate.Date.ToString() + " 23:59:59.999'" + 
							" AND userName='" + userName + "'";
				
			SqlCommand cmd = new SqlCommand(sQuery, DbConnection.DbConnection);
			
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


        private MRDbConnection DbConnection { get; set; }
	}
}
