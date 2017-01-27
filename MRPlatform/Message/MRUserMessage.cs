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


        public void Send(string userName, string recipient, string message, int priority = 2)
		{
            DoSend(userName, recipient, message, priority);
		}
		
		
		public void Send(string userName, Array recipients, string message, int priority = 2)
		{
			for(int i = 0; i <= recipients.Length; i++)
			{
                DoSend(userName, recipients.GetValue(i).ToString(), message, priority);
			}
		} 
		
		
		private void DoSend(string userName, string recipient, string message, int priority)
		{
			string sQuery = "INSERT INTO Messages(userName, nodeName, recipient, message, type) VALUES('" + userName + "', '" + recipient + "', '" + message + "', " + priority + ")";
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
		
		
		public DataSet Retrieve(string userName, int priority, bool unread, bool archived)
		{
			DataSet ds = new DataSet();
			ds = DoRetrieve(userName, priority, unread, archived);
			
			return ds;
		}
		
		
		private DataSet DoRetrieve(string userName, int priority, bool unread, bool archived)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT msgId, userName, message, type FROM Messages WHERE recipient='" + userName + "' AND priorityId=" + priority + " AND unread=" + unread + " AND archived=" + archived;
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		public void MarkAsUnread(string userName, int nMsgId)
		{
            DoMark(userName, nMsgId, true);
		}
		
		
		public void MarkAsRead(string userName, int nMsgId)
		{
            DoMark(userName, nMsgId, false);
		}
		
		
		private void DoMark(string userName, int nMsgId, bool unread)
		{
			string sQuery = "UPDATE Messages SET unread=" + unread+ " WHERE recipient='" + userName + "' AND msgId=" + nMsgId;
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
		
		
		public void Archive(string userName, int nMsgId)
		{
            DoArchive(userName, nMsgId, true);
        }
		
		
		public void UnArchive(string userName, int nMsgId)
		{
            DoArchive(userName, nMsgId, false);
        }
		
		
		private void DoArchive(string userName, int nMsgId, bool archived)
		{
			string sQuery = "UPDATE Messages SET archived=" + archived + " WHERE recipient='" + userName + "' AND msgId=" + nMsgId;
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
		
		
		public DataSet GetMessages(string userName)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + userName + "' ORDER BY msgDateTime DESC";
			return DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string userName, int priority)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + userName + "' AND priorityId=" + priority + " ORDER BY msgDateTime DESC";
			return DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string userName, int priority, bool unread)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + userName + "' AND priorityId=" + priority + " AND unread=" + unread + " ORDER BY msgDateTime DESC";
			return DoGetMessages(sQuery);
		}
		
		
		public DataSet GetMessages(string userName, int priority, bool unread, bool archived)
		{
			string sQuery = "SELECT * FROM Messages WHERE recipient='" + userName + "' AND priorityId=" + priority + " AND unread=" + unread + " AND archived=" + archived + " ORDER BY msgDateTime DESC";
			return DoGetMessages(sQuery);
		}
		
		
		private DataSet DoGetMessages(string sQuery)
		{
            DbConnection.DbConnection = new SqlConnection();
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			DataSet ds = new DataSet();
			dbAdapt.Fill(ds);
			
			return ds;
		}
    }
}
