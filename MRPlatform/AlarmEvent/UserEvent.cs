using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// Description of MREvent.
    /// </summary>
    [ComVisible(true)]
    [Guid("F3C0CE97-4E39-47A6-9856-F3A4E112C932"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IUserEvent))]
    public class UserEvent : IUserEvent
	{
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;


        public UserEvent()
        {

        }


        public UserEvent(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        public MRDbConnection DbConnection
        {
            get
            {
                return _dbConnection;
            }
            set
            {
                _dbConnection = value;
                _useADODB = _dbConnection.UseADODB;
                
            }
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
        public int LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)
		{
            // Parameter exceptions check
            if (userName == null) { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName == "") { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName.Length > 50) { throw new ArgumentOutOfRangeException("userName", "userName must be 50 characters or less."); }
            if (nodeName == null) { throw new ArgumentNullException("nodeName", "nodeName must not be null or empty."); }
            if (nodeName == "") { throw new ArgumentNullException("nodeName", "nodeName must not be null or empty."); }
            if (nodeName.Length > 50) { throw new ArgumentOutOfRangeException("nodeName", "nodeName must be 50 characters or less."); }
            if (userName == null) { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName == "") { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName.Length > 8000) { throw new ArgumentOutOfRangeException("userName", "userName must be 8000 characters or less."); }
            if (userName == null) { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName == "") { throw new ArgumentNullException("userName", "userName must not be null or empty."); }
            if (userName.Length > 50) { throw new ArgumentOutOfRangeException("userName", "userName must be 50 characters or less."); }

            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetLogEventQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@userName", userName);
                    sqlCmd.Parameters.AddWithValue("@nodeName", nodeName);
                    sqlCmd.Parameters.AddWithValue("@eventMessage", eventMessage);
                    sqlCmd.Parameters.AddWithValue("@eventType", eventType);
                    sqlCmd.Parameters.AddWithValue("@eventSource", eventSource);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)", ex.Message);
                        return -1;
                    }
                }
            }
            else
            {
                // Use ADODB Connection
                Connection dbConnection = _dbConnection.ADODBConnection;
                dbConnection.Open();

                Command dbCmd = new Command();
                dbCmd.ActiveConnection = dbConnection;
                dbCmd.CommandText = GetLogEventQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;

                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("userName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("nodeName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, nodeName);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("eventMessage", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 8000, eventMessage);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("eventType", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, eventType);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("eventSource", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, eventSource);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    rs = null;
                    dbConnection.Close();

                    return (int)recAffected;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource)", ex.Message);
                    return -1;
                }
            }
		}


        [ComVisible(false)]
        private string GetLogEventQuery()
        {
            string sQuery = "INSERT INTO EventLog(userName, nodeName, evtMessage, evtType, evtSource)" +
                            " VALUES(?, ?, ?, ?, ?)";

            return sQuery;
        }
        
		
		/// <summary>GetEventHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
        [ComVisible(false)]
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
        [ComVisible(false)]
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
