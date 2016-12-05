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
 * 
 * 2016-09-29   Added overloaded constructors to accept SqlConnection objects to be more flexible
 *              and compatible with the newly added MRDbSync library for syncing 2 databases.
 *              
 * 2016-10-05   Removed overloaded constructors and changed the only constructor to only accept
 *              a MRDbConnection object as its parameter. This object contains one or both database
 *              connections to also allow syncing while keeping the code straighforward.
 *              
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

        public MREvent(MRDbConnection mrDbConnection)
        {
            DbConnection = mrDbConnection;
        }

		
        /// <summary>
        /// Class destructor
        /// </summary>
		~MREvent()
		{
            if (this.DbConnection.DbConnection.State == ConnectionState.Open)
            {
                this.DbConnection.DbConnection.Close();
            }
        }
		
		
		/// <summary>
		/// LogEvent Method
		/// </summary>
		/// <remarks>Method to log events to the mrsystems SQL Server database using an 
		/// existing, open SqlConnection object to connect to the database.</remarks>
		/// <param name="userName">String object of the HMI username of the user currently logged in.</param>
		/// <param name="nodeName">String object of the HMI client node name the event is generated from.</param>
		/// <param name="eventMessage">String object of the event description.</param>
		/// <param name="eventType">Integer object defining the event type.</param>
		/// <param name="eventSource">String object of the event source. (e.g. Flow Setpoint, Alarm Summary, etc. 
		/// Just use a descriptive source name that tells the user where the event is coming from.</param>
		/// <returns>Integer value representing the number of records affected by the query.</returns>
		/// <example>Example:<code>
		/// MREvent mrel = new MREvent();
		/// mrel.LogEvent(userName, nodeName, eventMessage, eventType, eventSource);
		/// </code></example>
		public void LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)
		{
			SqlCommand dbCmd = new SqlCommand();
			string sQuery = "";
			
			sQuery = "INSERT INTO EventLog(userName, nodeName, evtMessage, evtType, evtSource VALUES('" + userName + "', '" + nodeName + "', '" + eventMessage + "', " + eventType + ", '" + eventSource + "')";
			
			dbCmd.CommandText = sQuery;
            dbCmd.Connection = DbConnection.DbConnection;
            dbCmd.ExecuteNonQuery();

            //Sync databases
            // TODO: Change so that based on where code is called from, the direction is automatically determined.
            DbConnection.Sync(MRDbConnection.SyncDirection.UploadAndDownload);
		}
        
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(int nRecordCount)
		{
			string sQuery = "SELECT TOP " + nRecordCount.ToString() + " * FROM EventLog ORDER BY evtDateTime DESC";
			
			DataSet ds = new DataSet();
			ds = DoGetDataSetFromQuery(sQuery);

            EventHistory = ds;
			
			return ds;
		}
		
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="startDateTime">DateTime object of the date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDate)
		{
			string sQuery = "SELECT * FROM EventLog WHERE evtDateTime >= '" + startDate + " 00:00:00.000' AND evtDateTime <= '" + startDate + "23:59:59.999' ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = DoGetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
		/// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDateTime, DateTime endDateTime)
		{
			string sQuery = "SELECT * FROM EventLog WHERE evtDateTime >= '" + startDateTime + "' AND evtDateTime <= '" + endDateTime + "' ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = DoGetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		private DataSet DoGetDataSetFromQuery(string sQuery)
		{
			DataSet ds = new DataSet();
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);

            EventHistory = ds;
			
			return ds;
		}
		
		
        //Class Properties
        private MRDbConnection DbConnection { get; set; }
		public DataSet EventHistory { get; set; }
	}
}
