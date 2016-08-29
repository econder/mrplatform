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
****************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform2014.AlarmEvent;
using MRPlatform2014.Data.Sql;


namespace MRPlatform2014.Message
{
	/// <summary>
	/// Description of MRAreaMessage.
	/// </summary>
	public class MRAreaMessage
	{
		private SqlConnection _dbConn;
		
		public MRAreaMessage(string dbServerName, string dbInstanceName, string userName, string password)
		{
			//Connect to MRDbConnection
			MRDbConnection mrdb = new MRDbConnection();
			mrdb.ConnectionString = "Server=" + dbServerName + "; Database=" + dbInstanceName + "; Uid=" + userName + "; Pwd=" + password;
			this._dbConn = mrdb.Open(dbServerName, dbInstanceName, userName, password);
		}
		
		~MRAreaMessage()
		{
			if(this._dbConn.State == ConnectionState.Open)
			{
				this._dbConn.Close();
			}
		}
		
		//
		// TODO: Change nType to nPriority (-1=All; 0=Low; 1=Medium; 2=High; 3=Critical).
		//
		
		public void Send(string userName, string nodeName, string recipient, string message, int priority = 0)
		{
			string sQuery = "INSERT INTO Messages(userName, nodeName, recipient, message, priority, msgType) VALUES('" + userName + "', '" + nodeName + "', '" + recipient + "', '" + message + "', " + priority + ", " + 1 + ")";
			SqlCommand dbCmd = new SqlCommand(sQuery, this._dbConn);
			
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
		
		
		public DataSet GetMessages(string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgDateTime, recipient, message, priority FROM vMessages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
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
				
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public int Count(string sArea)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT COUNT(*) FROM Messages" + 
							" WHERE recipient='" + sArea + "'" + 
							" AND msgType=1";
			
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
			
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
				
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
				
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
			
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
			
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
				
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
				
			SqlCommand cmd = new SqlCommand(sQuery, this._dbConn);
			
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
