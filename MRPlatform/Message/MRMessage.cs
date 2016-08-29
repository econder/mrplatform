/***************************************************************************************************
 * Class: 		MRMessage.cs
 * Created By:	Eric Conder
 * Created On:	2014-03-04
 * 
 * Changes:
 * 
 * 2014-03-04	Created Send & Retrieve methods.
 * 2014-03-05	Created MarkAs... methods.
 * 2014-03-06	Created GetMessages methods.
 * 2014-04-03	Added error handling to some of the functions.
 * 
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform2014.AlarmEvent;
using MRPlatform2014.Data.Sql;


namespace MRPlatform2014.Message
{
	/// <summary>
	/// Description of MRMessage.
	/// </summary>
	public class MRMessage
	{
		private SqlConnection _dbConn;
		
		public MRMessage(string dbServerName, string dbInstanceName, string userName, string password)
		{
			//Connect to MRDbConnection
			MRDbConnection mrdb = new MRDbConnection();
			mrdb.ConnectionString = "Server=" + dbServerName + "; Database=" + dbInstanceName + "; Uid=" + userName + "; Pwd=" + password;
			this._dbConn = mrdb.Open(dbServerName, dbInstanceName, userName, password);
		}
		
		~MRMessage()
		{
			if(this._dbConn.State == ConnectionState.Open)
			{
				this._dbConn.Close();
			}
		}
		
		// TODO: Change nType to nPriority (-1=All; 0=Low; 1=Medium; 2=High; 3=Critical).
		// TODO: Change type column in mrsystems database Messages table to priority.
		
		public void Send(string sUsername, string sNodeName, string sRecipient, string sMessage, int nType)
		{
			this.DoSend(sUsername, sNodeName, sRecipient, sMessage, nType);
		}
		
		
		public void Send(string sUsername, string sNodeName, Array arRecipients, string sMessage, int nType)
		{
			for(int i = 0; i <= arRecipients.Length; i++)
			{
				this.DoSend(sUsername, sNodeName, arRecipients.GetValue(i).ToString(), sMessage, nType);
			}
		} 
		
		
		private void DoSend(string sUsername, string sNodeName, string sRecipient, string sMessage, int nType)
		{
			string sQuery = "INSERT INTO Messages(userName, nodeName, recipient, message, type) VALUES('" + sUsername + "', '" + sNodeName + "', '" + sRecipient + "', '" + sMessage + "', " + nType + ")";
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
		
		
		public DataSet Retrieve(string sUsername, int nType, bool bUnread, bool bArchived)
		{
			DataSet ds = new DataSet();
			ds = this.DoRetrieve(sUsername, nType, bUnread, bArchived);
			
			return ds;
		}
		
		
		private DataSet DoRetrieve(string sUsername, int nType, bool bUnread, bool bArchived)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgId, userName, message, type FROM Messages WHERE recipient='" + sUsername + "' AND type=" + nType + " AND unread=" + bUnread + " AND archived=" + bArchived;
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public void MarkAsUnread(string sUsername, int nMsgId)
		{
			this.DoMark(sUsername, nMsgId, true);
		}
		
		
		public void MarkAsRead(string sUsername, int nMsgId)
		{
			this.DoMark(sUsername, nMsgId, false);
		}
		
		
		private void DoMark(string sUsername, int nMsgId, bool bUnread)
		{
			string sQuery = "UPDATE Messages SET unread=" + bUnread+ " WHERE recipient='" + sUsername + "' AND msgId=" + nMsgId;
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
		
		
		public void Archive(string sUsername, int nMsgId)
		{
			this.DoArchive(sUsername, nMsgId, true);
		}
		
		
		public void UnArchive(string sUsername, int nMsgId)
		{
			this.DoArchive(sUsername, nMsgId, false);
		}
		
		
		private void DoArchive(string sUsername, int nMsgId, bool bArchived)
		{
			string sQuery = "UPDATE Messages SET archived=" + bArchived + " WHERE recipient='" + sUsername + "' AND msgId=" + nMsgId;
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
		
		
		public DataSet GetMessages(string sUsername)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + sUsername + "' ORDER BY msgDateTime DESC";
			return this.DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string sUsername, int nType)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + sUsername + "' AND type=" + nType + " ORDER BY msgDateTime DESC";
			return this.DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string sUsername, int nType, bool bUnread)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + sUsername + "' AND type=" + nType + " AND unread=" + bUnread + " ORDER BY msgDateTime DESC";
			return this.DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string sUsername, int nType, bool bUnread, bool bArchived)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + sUsername + "' AND type=" + nType + " AND unread=" + bUnread + " AND archived=" + bArchived + " ORDER BY msgDateTime DESC";
			return this.DoGetMessages(sQuery);
		}
		
		
		private DataSet DoGetMessages(string sQuery)
		{
			this._dbConn = new SqlConnection();
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			DataSet ds = new DataSet();
			dbAdapt.Fill(ds);
			
			return ds;
		}
	}
}
