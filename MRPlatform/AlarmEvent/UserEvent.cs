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
            if (eventMessage == null) { throw new ArgumentNullException("eventMessage", "eventMessage must not be null or empty."); }
            if (eventMessage == "") { throw new ArgumentNullException("eventMessage", "eventMessage must not be null or empty."); }
            if (eventMessage.Length > 8000) { throw new ArgumentOutOfRangeException("eventMessage", "eventMessage must be 8000 characters or less."); }
            if (eventSource == null) { throw new ArgumentNullException("eventSource", "eventSource must not be null or empty."); }
            if (eventSource == "") { throw new ArgumentNullException("eventSource", "eventSource must not be null or empty."); }
            if (eventSource.Length > 50) { throw new ArgumentOutOfRangeException("eventSource", "eventSource must be 50 characters or less."); }

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
            string sQuery = "INSERT INTO UserEventLog(userName, nodeName, evtMessage, evtType, evtSource)" +
                            " VALUES(?, ?, ?, ?, ?)";

            return sQuery;
        }
        
		
        [ComVisible(false)]
		public DataSet GetHistoryDataSet(int pageNumber, int resultsPerPage, bool sortAscending = true)
		{
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetHistoryQuery(sortAscending), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@offset", (pageNumber - 1) * resultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", resultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();
                    return ds;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetHistoryDataSet(int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }


        public Recordset GetHistoryRecordset(int pageNumber, int resultsPerPage, bool sortAscending = true)
        {
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetHistoryQuery(sortAscending);
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, (pageNumber - 1) * resultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, resultsPerPage);
            dbCmd.Parameters.Append(dbParam);

            Recordset rs = new Recordset();
            rs.CursorType = CursorTypeEnum.adOpenStatic;

            try
            {
                object recAffected;
                rs = dbCmd.Execute(out recAffected);
                return rs;
            }
            catch (COMException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetHistoryRecordset(int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();

                return rs;
            }
        }


        [ComVisible(false)]
        private string GetHistoryQuery(bool sortAscending)
        {
            string sortOrder = null;
            if (sortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, userName, nodeName, evtMessage, evtType, evtSource" +
                                          " FROM UserEventLog ORDER BY evtDateTime {0}" + 
                                          " OFFSET ? ROWS" + 
                                          " FETCH NEXT ? ROWS ONLY", sortOrder);

            return sQuery;
        }


        /// <summary>GetEventHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="startDateTime">DateTime object of the date of the recordset.</param>
        /// <returns>System.Data.DataSet</returns>
        [ComVisible(false)]
        public DataSet GetHistoryDataSet(DateTime eventDate, int pageNumber, int resultsPerPage, bool sortAscending = true)
		{
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetHistoryStartDateQuery(sortAscending), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@eventStartDate", eventDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@eventEndDate", eventDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@offset", (pageNumber - 1) * resultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", resultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();
                    return ds;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetHistoryDataSet(DateTime eventDate, int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }


        public Recordset GetHistoryRecordset(double eventDate, int pageNumber, int resultsPerPage, bool sortAscending = true)
        {
            DateTime dtEventDate = DateTime.FromOADate(eventDate); // Test for ArgumentException
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetHistoryStartDateQuery(sortAscending);
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("eventStartDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtEventDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("eventEndDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtEventDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, (pageNumber - 1) * resultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, resultsPerPage);
            dbCmd.Parameters.Append(dbParam);

            Recordset rs = new Recordset();
            rs.CursorType = CursorTypeEnum.adOpenStatic;

            try
            {
                object recAffected;
                rs = dbCmd.Execute(out recAffected);
                return rs;
            }
            catch (COMException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetHistoryRecordset(double eventDate, int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }


        [ComVisible(false)]
        private string GetHistoryStartDateQuery(bool sortAscending)
        {
            string sortOrder = null;
            if (sortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, userName, nodeName, evtMessage, evtType, evtSource" +
                                          " FROM UserEventLog" +
                                          " WHERE evtDateTime >= ?" +
                                          " AND evtDateTime < DATEADD(dd, 1, ?)" +
                                          " ORDER BY evtDateTime {0}" +
                                          " OFFSET ? ROWS" +
                                          " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        /// <summary>GetEventHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
        /// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
        /// <returns>System.Data.DataSet</returns>
        [ComVisible(false)]
        public DataSet GetHistoryDataSet(DateTime eventStartDate, DateTime eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending = true)
		{
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetHistoryStartEndDateQuery(sortAscending), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@eventStartDate", eventStartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@eventEndDate", eventEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@offset", (pageNumber - 1) * resultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", resultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();
                    return ds;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetHistoryDataSet(DateTime eventStartDate, DateTime eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }


        public Recordset GetHistoryRecordset(double eventStartDate, double eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending = true)
        {
            string sEventStartDate = DateTime.FromOADate(eventStartDate).ToString("yyyy-MM-dd HH:mm:ss"); // Test for ArgumentException
            string sEventEndDate = DateTime.FromOADate(eventEndDate).ToString("yyyy-MM-dd HH:mm:ss"); // Test for ArgumentException
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetHistoryStartEndDateQuery(sortAscending);
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("eventStartDate", DataTypeEnum.adDate, ParameterDirectionEnum.adParamInput, 0, sEventStartDate);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("eventEndDate", DataTypeEnum.adDate, ParameterDirectionEnum.adParamInput, 0, sEventEndDate);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, (pageNumber - 1) * resultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, resultsPerPage);
            dbCmd.Parameters.Append(dbParam);

            Recordset rs = new Recordset();
            rs.CursorType = CursorTypeEnum.adOpenStatic;

            try
            {
                object recAffected;
                rs = dbCmd.Execute(out recAffected);
                return rs;
            }
            catch (COMException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetHistoryRecordset(double eventStartDate, double eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending = true)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }


        [ComVisible(false)]
        private string GetHistoryStartEndDateQuery(bool sortAscending)
        {
            string sortOrder = null;
            if (sortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, userName, nodeName, evtMessage, evtType, evtSource" +
                                          " FROM UserEventLog" +
                                          " WHERE evtDateTime >= ?" +
                                          " AND evtDateTime <= ?" +
                                          " ORDER BY evtDateTime {0}" +
                                          " OFFSET ? ROWS" +
                                          " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }
	}
}
