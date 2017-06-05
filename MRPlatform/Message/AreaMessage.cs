using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
    /// <summary>
    /// Description of MRAreaMessage.
    /// </summary>
    [ComVisible(true)]
    [Guid("832C3EAF-D79D-42A0-989E-D1514F630668"),
    ClassInterface(ClassInterfaceType.None),
<<<<<<< HEAD
    ComSourceInterfaces(typeof(IAreaMessageEvent))]
=======
    ComSourceInterfaces(typeof(IAreaMessage))]
>>>>>>> feature/ConvertToADO
    public class AreaMessage : IAreaMessage
	{
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;

        // Global message type = 1 for Area Messages
        private const int MESSAGETYPE = 1;

        public AreaMessage()
        {
<<<<<<< HEAD

=======
            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            SortAscending = true;
>>>>>>> feature/ConvertToADO
        }

        public AreaMessage(MRDbConnection mrDbConnection)
		{
            _dbConnection = mrDbConnection;
            
            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            SortAscending = true;
        }

        #region " Properties "

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

        public int ResultsPageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public bool SortAscending { get; set; }

        #endregion

<<<<<<< HEAD

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

        
        public void Send(string sender, string area, string message, int priority = 2)
=======
        #region " Send "

        public int Send(string sender, string area, string message, int priority = 2)
>>>>>>> feature/ConvertToADO
		{
            // Parameter exceptions check
            if (sender == null) { throw new ArgumentNullException("sender", "sender must not be null or empty."); }
            if (sender == "") { throw new ArgumentNullException("sender", "sender must not be null or empty."); }
            if (sender.Length > 50) { throw new ArgumentOutOfRangeException("sender", "sender must be 50 characters or less."); }
            if (area == null) { throw new ArgumentNullException("area", "area must not be null or empty."); }
            if (area == "") { throw new ArgumentNullException("area", "area must not be null or empty."); }
            if (area.Length > 50) { throw new ArgumentOutOfRangeException("area", "area must be 50 characters or less."); }
            if (message == null) { throw new ArgumentNullException("message", "message must not be null or empty."); }
            if (message == "") { throw new ArgumentNullException("message", "message must not be null or empty."); }
            if (message.Length > 5000) { throw new ArgumentOutOfRangeException("message", "message must be 50 characters or less."); }
            if (priority < 1) { throw new ArgumentOutOfRangeException("priority", "priority must be greater than zero."); }

            if (!_dbConnection.UseADODB)
            {
                using (IDbConnection dbConnection = _dbConnection.Connection)
<<<<<<< HEAD
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetSendQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@sender", sender);
                    sqlCmd.Parameters.AddWithValue("@recipient", area);
                    sqlCmd.Parameters.AddWithValue("@message", message);
                    sqlCmd.Parameters.AddWithValue("@msgTypeId", MESSAGETYPE);
                    sqlCmd.Parameters.AddWithValue("@priorityId", priority);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "Send(string sender, string recipient, string message, int priority = 2)", ex.Message);
                    }
=======
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetSendQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@sender", sender);
                    sqlCmd.Parameters.AddWithValue("@recipient", area);
                    sqlCmd.Parameters.AddWithValue("@message", message);
                    sqlCmd.Parameters.AddWithValue("@msgTypeId", MESSAGETYPE);
                    sqlCmd.Parameters.AddWithValue("@priorityId", priority);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "Send(string sender, string recipient, string message, int priority = 2)", ex.Message);
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
                dbCmd.CommandText = GetSendQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;

                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("sender", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, sender);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("message", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 8000, message);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("msgTypeId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, MESSAGETYPE);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("priorityId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
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
                    _errorLog.LogMessage(this.GetType().Name, "Send(string sender, string recipient, string message, int priority = 2)", ex.Message);
                    return -1;
>>>>>>> feature/ConvertToADO
                }
            }
            else
            {

            }
		}


        [ComVisible(false)]
        private string GetSendQuery()
        {
            string sQuery = "INSERT INTO Messages(sender, recipient, message, msgTypeId, priorityId)" +
                            " VALUES(?, ?, ?, ?, ?)";
            return sQuery;
        }
<<<<<<< HEAD
		

	    public DataSet GetMessages(string area)
=======

        #endregion

        #region " GetMessages "

        [ComVisible(false)]
	    public DataSet GetMessagesDataSet(string area)
>>>>>>> feature/ConvertToADO
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetMessagesQuery_Area(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();
                    return ds;
                }
                catch(OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetMessages(string area)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
		}

        public Recordset GetMessagesRecordset(string area)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetMessagesQuery_Area();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetMessagesRecordset(string area)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetMessagesQuery_Area()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessages WHERE recipient = ?" +
                            " ORDER BY msgDateTime {0}" + 
                            " OFFSET ? ROWS" + 
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }

		
		[ComVisible(false)]
		public DataSet GetMessagesDataSet(string area, int priority)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetMessagesQuery_AreaPriority(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetMessages(string area, int priority)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetMessagesRecordset(string area, int priority)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetMessagesQuery_AreaPriority();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetMessagesRecordset(string area, int priority)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetMessagesQuery_AreaPriority()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessages" + 
                            " WHERE recipient = ?" +
                            " AND priorityId = ?" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        [ComVisible(false)]
        private DataSet GetMessagesDataSet(string area, int priority, DateTime messageDate)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetMessagesQuery_AreaPriorityStart(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@startDate", messageDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@endDate", messageDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetMessages(string area, int priority, DateTime dtDate)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetMessagesRecordset(string area, int priority, double messageDate)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            DateTime dtMessageDate = DateTime.FromOADate(messageDate); // Test for ArgumentException

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetMessagesQuery_AreaPriorityStart();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("startDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("endDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetMessagesRecordset(string area, int priority, double messageDate)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetMessagesQuery_AreaPriorityStart()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessages" +
                            " WHERE recipient = ?" +
                            " AND priorityId = ?" +
                            " AND msgDateTime >= ?" +
                            " AND msgDateTime < DATEADD(dd, 1, ?)" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        [ComVisible(false)]
        private DataSet GetMessagesDataSet(string area, int priority, DateTime messageStartDate, DateTime messageEndDate)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetMessagesQuery_AreaPriorityStartEnd(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@startDate", messageStartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@endDate", messageEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetMessages(string area, int priority, DateTime dtDate)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetMessagesRecordset(string area, int priority, double messageStartDate, double messageEndDate)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("priority", (object)priority, "Priority parameter cannot be less than zero."); }

            DateTime dtMessageStartDate = DateTime.FromOADate(messageStartDate); // Test for ArgumentException
            DateTime dtMessageEndDate = DateTime.FromOADate(messageStartDate); // Test for ArgumentException

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetMessagesQuery_AreaPriorityStartEnd();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("startDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageStartDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("endDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageEndDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetMessagesRecordset(string area, int priority, double messageStartDate, double messageEndDate)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetMessagesQuery_AreaPriorityStartEnd()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessages" +
                            " WHERE recipient = ?" +
                            " AND priorityId = ?" +
                            " AND msgDateTime >= ?" +
                            " AND msgDateTime <= ?" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }

#endregion

        #region " GetUnreadMessages "

        [ComVisible(false)]
        public DataSet GetUnreadMessagesDataSet(string userName, string area)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetUnreadMessagesQuery_UsernameArea(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessages(string userName, string area)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetUnreadMessagesRecordset(string userName, string area)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetUnreadMessagesQuery_UsernameArea();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("sender", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessages(string userName, string area)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetUnreadMessagesQuery_UsernameArea()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessagesUnread" +
                            " WHERE recipient = ?" +
                            " AND sender = ?" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        [ComVisible(false)]
        public DataSet GetUnreadMessagesDataSet(string userName, string area, int priority)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetUnreadMessagesQuery_UsernameAreaPriority(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessages(string userName, string area)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetUnreadMessagesRecordset(string userName, string area, int priority)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetUnreadMessagesQuery_UsernameAreaPriority();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("sender", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessagesRecordset(string userName, string area, int priority)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetUnreadMessagesQuery_UsernameAreaPriority()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessagesUnread" +
                            " WHERE recipient = ?" +
                            " AND sender = ?" +
                            " AND priority= ?" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        [ComVisible(false)]
        private DataSet GetUnreadMessagesDataSet(string userName, string area, int priority, DateTime messageDate)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetUnreadMessagesQuery_UsernameAreaPriorityStart(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@startDate", messageDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@endDate", messageDate.ToShortDateString());
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessages(string userName, string area, int priority, DateTime messageDate)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double messageDate)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            DateTime dtMessageDate = DateTime.FromOADate(messageDate); // Test for ArgumentException

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetUnreadMessagesQuery_UsernameAreaPriorityStart();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("sender", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("startDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("endDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessagesRecordset(string userName, string area, int priority, double messageDate)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetUnreadMessagesQuery_UsernameAreaPriorityStart()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessagesUnread" +
                            " WHERE recipient = ?" +
                            " AND sender = ?" +
                            " AND priority= ?" +
                            " AND msgDateTime >= ?" +
                            " AND msgDateTime < DATEADD(dd, 1, ?)" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }


        [ComVisible(false)]
        private DataSet GetUnreadMessagesDataSet(string userName, string area, int priority, DateTime messageStartDate, DateTime messageEndDate)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetUnreadMessagesQuery_UsernameAreaPriorityStartEnd(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@startDate", messageStartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@endDate", messageEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

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
                    _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessagesDataSet(string userName, string area, int priority, DateTime messageStartDate, DateTime messageEndDate)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return ds;
                }
            }
        }

        public Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double messageStartDate, double messageEndDate)
        {
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }
            if (userName == "" | userName == null) { throw new ArgumentNullException("userName", "User name parameter cannot be empty or null."); }
            if (priority < 0) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)priority, "Priority cannot be less than zero."); }

            DateTime dtMessageStartDate = DateTime.FromOADate(messageStartDate); // Test for ArgumentException
            DateTime dtMessageEndDate = DateTime.FromOADate(messageEndDate); // Test for ArgumentException

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetUnreadMessagesQuery_UsernameAreaPriorityStartEnd();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("sender", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("priority", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, priority);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("startDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageStartDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("endDate", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput, 0, dtMessageEndDate.ToShortDateString());
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, (ResultsPageNumber - 1) * ResultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, ResultsPerPage);
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
                _errorLog.LogMessage(this.GetType().Name, "GetUnreadMessagesRecordset(string userName, string area, int priority, double messageStartDate, double messageEndDate)", ex.Message);
                if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                    dbConnection.Close();
                return rs;
            }
        }

        [ComVisible(false)]
        private string GetUnreadMessagesQuery_UsernameAreaPriorityStartEnd()
        {
            string sortOrder = null;
            if (SortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT id, msgDateTime, recipient, message, priority" +
                            " FROM vMessagesUnread" +
                            " WHERE recipient = ?" +
                            " AND sender = ?" +
                            " AND priority= ?" +
                            " AND msgDateTime >= ?" +
                            " AND msgDateTime <= ?" +
                            " ORDER BY msgDateTime {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);
            return sQuery;
        }

        #endregion

        #region " Count "

        public int Count(string area)
		{
            if (area == "" | area == null) { throw new ArgumentNullException("area", "Area parameter cannot be empty or null."); }

            if (!_dbConnection.UseADODB)
            {
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(CountQuery_Area(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@recipient", area);

                    OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                    DataSet ds = new DataSet();

                    try
                    {
                        dbAdapt.Fill(ds);
                        dbConnection.Close();
                        if (ds.Tables.Count > 0)
                        {
                            return ds.Tables[0].Rows.Count;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "Count(string area)", ex.Message);
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();
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
                dbCmd.CommandText = CountQuery_Area();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;

                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("recipient", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, area);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    int nCount = rs.RecordCount;

                    dbConnection.Close();
                    return nCount;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area)", ex.Message);
                    return -1;
                }
            }
        }

        [ComVisible(false)]
        private string CountQuery_Area()
        {
            string sQuery = "SELECT COUNT(*) FROM vMessages" +
                            " WHERE recipient = ?";
            return sQuery;
        }
		
		
		public int Count(string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@prioprity", priority);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority)", ex.Message);
                    return -1;
                }
            }
		}

        [ComVisible(false)]
        private string CountQuery_AreaPriority()
        {
            string sQuery = "SELECT COUNT(*) FROM vMessages" +
                            " WHERE recipient = ?" +
                            " AND priorityId = ?";
            return sQuery;
        }
		
		
		private int Count(string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtDate.Date.ToString() + " 00:00:00.000");

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority, DateTime dtDate)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessages" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtEndDate.Date.ToString() + " 00:00:00.000");

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate)", ex.Message);
                    return -1;
                }
            }
		}

        #endregion

        #region " UnreadCount "

        public int UnreadCount(string userName, string area)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		public int UnreadCount(string userName, string area, int priority)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area, int priority)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string startDate)", ex.Message);
                    return -1;
                }
            }
		}
		
		
		private int UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();
                string sQuery = "SELECT COUNT(*) FROM vMessagesUnread" +
                                " WHERE recipient = ?" +
                                " AND priorityId = ?" +
                                " AND msgDateTime >= ?" +
                                " AND msgDateTime <	?" +
                                " AND sender = ?";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@recipient", area);
                sqlCmd.Parameters.AddWithValue("@priority", priority);
                sqlCmd.Parameters.AddWithValue("@dtStartDate", dtStartDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@dtEndDate", dtEndDate.Date.ToString() + " 00:00:00.000");
                sqlCmd.Parameters.AddWithValue("@sender", userName);

                try
                {
                    int rowCount = (int)sqlCmd.ExecuteScalar();
                    return rowCount;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "UnreadCount(string sender, string area, int priority, DateTime dtStartDate, DateTime dtEndDate)", ex.Message);
                    return -1;
                }
            }
		}

        #endregion
    }
}
