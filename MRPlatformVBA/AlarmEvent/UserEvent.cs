using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatformVBA.DB.Sql;


namespace MRPlatformVBA.AlarmEvent
{
    /// <summary>
    /// Description of MREvent.
    /// </summary>
    [ComVisible(true)]
    [Guid("64DEF310-1D25-438E-834B-F58A22418CC3"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IUserEventEvents))]
    public class UserEvent : IUserEvent
	{
        private MRDbConnection _dbConnection;

        public UserEvent(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        /// <summary>
        /// LogEvent Method
        /// </summary>
        /// <remarks>Method to log events to the mrsystems SQL Server database using an 
        /// existing, open OleDbConnection object to connect to the database.</remarks>
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
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "INSERT INTO EventLog(userName, nodeName, evtMessage, evtType, evtSource)" +
                                " VALUES('" + userName + "', '" + nodeName + "', '" + eventMessage + "', " + eventType + ", '" + eventSource + "')";

                OleDbCommand dbCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                dbCmd.ExecuteNonQuery();
            }
		}
        
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(int nRecordCount)
		{
            string sQuery = "SELECT TOP " + nRecordCount.ToString() + " * " +
                            " FROM EventLog" +
                            " ORDER BY evtDateTime DESC";

            DataSet ds = new DataSet();
            ds = DoGetDataSetFromQuery(sQuery);

            return ds;
        }
		
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="startDateTime">DateTime object of the date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistory(DateTime startDate)
		{
			string sQuery = "SELECT * " +
                            " FROM EventLog" +
                            " WHERE evtDateTime >= '" + startDate + " 00:00:00.000'" +
                            " AND evtDateTime <= '" + startDate + "23:59:59.999'" + 
                            " ORDER BY evtDateTime";
			
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
			string sQuery = "SELECT * " + 
                            " FROM EventLog" + 
                            " WHERE evtDateTime >= '" + startDateTime + "'" +
                            " AND evtDateTime <= '" + endDateTime + "'" +
                            " ORDER BY evtDateTime";
			
			DataSet ds = new DataSet();
			ds = DoGetDataSetFromQuery(sQuery);
			
			return ds;
		}
		
		
		private DataSet DoGetDataSetFromQuery(string sQuery)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
	}
}
