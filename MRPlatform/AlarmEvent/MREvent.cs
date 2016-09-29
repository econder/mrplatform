/***************************************************************************************************
 * Class: 		MREvent.cs
 * Created By:	Eric Conder
 * Created On:	2014-01-10
 * 
 * Changes:
 * 2014-02-28 	Added documentation to the class constructor, properties, functions, & methods.
 * 				Fixed GetEventHistory methods, so they are operational now.
 *
 * 2014-03-26	Changed name from MREventLog to MREvent. Added constructor code to open database
 * 				connection using MRDbConnection on class consctruction. Added desctructor to close
 * 				the database connection on class destruction.
 * 
 * 2014-04-01	Changed namespace from MRPlatform2014.Event to MRPlatform2014.AlarmEvent
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform.Data.Sql;


namespace MRPlatform.AlarmEvent
{
	/// <summary>
	/// Description of MREvent.
	/// </summary>
	public class MREvent
	{
		private SqlConnection _dbConn;
        private SqlConnection _dbSyncConn;
        private bool _syncTables;

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <remarks>Creates a new instance of MREvent.</remarks>
		/// <example>Example:<code>
		/// MREvent mrel = new MREvent();
		///</code></example>
		public MREvent(string dbServerName, string dbInstanceName, string dbUserName, string dbPassword, string dbProvider)
		{
			MRDbConnection mrdb = new MRDbConnection();
			mrdb.ConnectionString = "Server=" + dbServerName + "; Database=" + dbInstanceName + "; Uid=" + dbUserName + "; Pwd=" + dbPassword;
			this._dbConn = mrdb.OpenDatabase(dbServerName, dbInstanceName, dbUserName, dbPassword);
		}


        public MREvent(SqlConnection dbConnection)
        {
            
        }


        public MREvent(string dbServerName, string dbInstanceName, string dbUserName, string dbPassword,
                       string dbSyncServerName, string dbSyncInstanceName, string dbSyncUserName, string dbSyncPassword)
        {
            MRDbConnection mrdb = new MRDbConnection();

            //Get primary SQL server connection
            mrdb.ConnectionString = "Server=" + dbServerName + "; Database=" + dbInstanceName + "; Uid=" + dbUserName + "; Pwd=" + dbPassword;
            this._dbConn = mrdb.OpenDatabase(dbServerName, dbInstanceName, dbUserName, dbPassword);

            //Get sync'd SQL server connection
            mrdb.SyncConnectionString = "Server=" + dbSyncServerName + "; Database=" + dbSyncInstanceName + "; Uid=" + dbSyncUserName + "; Pwd=" + dbSyncPassword;
            this._dbSyncConn = mrdb.OpenDatabase(dbSyncServerName, dbSyncInstanceName, dbSyncUserName, dbSyncPassword);

            //Check if sync database connection is open and update SyncTables property
            if(this._dbSyncConn.State == ConnectionState.Open)
            {
                SyncTables = true;
            }
        }

		
		~MREvent()
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
		public void LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)
		{
			SqlCommand dbCmd = new SqlCommand();
			string sQuery = "";
			
			sQuery = "INSERT INTO EventLog(userName, nodeName, evtMessage, evtType, evtSource VALUES('" + userName + "', '" + nodeName + "', '" + eventMessage + "', " + eventType + ", '" + eventSource + "')";
			
			dbCmd.CommandText = sQuery;
			dbCmd.Connection = this._dbConn;
            int recCount = dbCmd.ExecuteNonQuery();
            
            
            if (recCount > 0 && SyncTables == true)
            {
                MRDbSync.CProvisionSync ps = new MRDbSync.CProvisionSync(_dbConn, _dbSyncConn);
                
            }
		}
        
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(int nRecordCount)
		{
			string sQuery = "SELECT TOP " + nRecordCount.ToString() + " * FROM EventLog ORDER BY evtDateTime DESC";
			
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(sQuery);
			
			this.EventHistory = ds;
			
			return ds;
		}
		
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="startDateTime">DateTime object of the date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDate)
		{
			string sQuery = "SELECT * FROM EventLog WHERE evtDateTime >= '" + startDate + " 00:00:00.000' AND evtDateTime <= '" + startDate + "23:59:59.999' ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">SqlConnection object of the database connection.</param>
		/// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
		/// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDateTime, DateTime endDateTime)
		{
			string sQuery = "SELECT * FROM EventLog WHERE evtDateTime >= '" + startDateTime + "' AND evtDateTime <= '" + endDateTime + "' ORDER BY evtDateTime";
			
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
		
		
        //Class Properties
		public DataSet EventHistory { get; set; }
        private bool SyncTables { get; set; }
	}
}
