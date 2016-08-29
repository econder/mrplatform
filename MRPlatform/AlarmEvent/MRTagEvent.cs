/***************************************************************************************************
 * Class:    	MRTagEvent.cs
 * Created By:  Eric Conder
 * Created On:  2014-03-26
 * 
 * Changes:
 * 
 * 2014-04-01	Changed namespace from MRPlatform2014.Event to MRPlatform2014.AlarmEvent
 * 
 * 
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform2014.AlarmEvent;
using MRPlatform2014.Data.Sql;
	

namespace MRPlatform2014.AlarmEvent
{
	/// <summary>
	/// Description of MRTagEvent
	/// </summary>
	public class MRTagEvent
	{
		private SqlConnection _dbConn;
		
		/// <summary>
		/// Class constructor
		/// </summary>
		/// <remarks>Creates a new instance of MRTagEvent.</remarks>
		/// <example>Example:<code>
		/// MRTagEvent mrte = new MRTagEvent();
		///</code></example>
		public MRTagEvent(string dbServerName, string dbInstanceName, string userName, string password)
		{
			MRDbConnection mrdb = new MRDbConnection();
			mrdb.ConnectionString = "Server=" + dbServerName + "; Database=" + dbInstanceName + "; Uid=" + userName + "; Pwd=" + password;
			this._dbConn = mrdb.Open(dbServerName, dbInstanceName, userName, password);
		}
		
		~MRTagEvent()
		{
			if(this._dbConn.State == ConnectionState.Open)
			{
				this._dbConn.Close();
			}
		}
		
		
		/// <summary>
		/// LogEvent Method
		/// </summary>
		/// <remarks>Method to log events to the mrsystems SQL Server database using an 
		/// existing, open SqlConnection object to connect to the database.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="userName">String object of the HMI username of the user currently logged in.</param>
		/// <param name="nodeName">String object of the HMI client node name the event is generated from.</param>
		/// <param name="eventMessage">String object of the event description.</param>
		/// <param name="eventType">Integer object defining the event type.</param>
		/// <param name="eventSource">String object of the event source. (e.g. Flow Setpoint, Alarm Summary, etc. 
		/// Just use a descriptive source name that tells the user where the event is coming from.</param>
		/// <returns>Integer value representing the number of records affected by the query.</returns>
		/// <example>Example:<code>
		/// MREvent mrel = new MREvent();
		/// int numRecords = mrel.LogEvent(dbConn, userName, nodeName, eventMessage, eventType, eventSource);
		/// </code></example>
		public void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew)
		{
			SqlCommand dbCmd = new SqlCommand();
			string sQuery = "";
			
			sQuery = "INSERT INTO TagEventLog(userName, nodeName, tagName, tagValueOrig, tagValueNew VALUES('" + userName + "', '" + nodeName + "', '" + tagName + "', " + tagValueOrig + ", " + tagValueNew + ")";
			
			dbCmd.CommandText = sQuery;
			dbCmd.Connection = this._dbConn;
			
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
		
		
		/// <summary>GetHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(string tagName, int nRecordCount)
		{
			string sQuery = "SELECT TOP " + nRecordCount.ToString() + " * FROM TagEventLog ORDER BY evtDateTime DESC";
			
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(sQuery);
			
			this.EventHistory = ds;
			
			return ds;
		}
		
		
		/// <summary>GetHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="startDateTime">DateTime object of the date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDate)
		{
			string sQuery = "SELECT * FROM TagEventLog WHERE evtDateTime >= '" + startDate + " 00:00:00.000' AND evtDateTime <= '" + startDate + "23:59:59.999' ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		/// <summary>GetHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
		/// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDateTime, DateTime endDateTime)
		{
			string sQuery = "SELECT * FROM TagEventLog WHERE evtDateTime >= '" + startDateTime + "' AND evtDateTime <= '" + endDateTime + "' ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		private DataSet GetDataSetFromQuery(string sQuery)
		{
			DataSet ds = new DataSet();
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			dbAdapt.Fill(ds);
			
			this.EventHistory = ds;
			
			return ds;
		}
		
		
		public DataSet EventHistory { get; set; }
	}
}
