/***************************************************************************************************
 * Class:    	MRAlarmEventLog.cs
 * Created By:  Eric Conder
 * Created On:  2014-03-28
 * 
 * Changes:
 * 2014-04-01	Renamed MRAlarm to MRAlarmEventLog to be more descriptive. Added
 * 				GetTopAlarmOccurrences() classes.
 * 				
 * 2016-08-29   Updated MRDbConnection methods to match method name changes in MRDbConnection. Added
 *              comments to all methods and properties.
 * 
 * 2016-10-06   Changed class constructor to accept only an MRDbConnection object parameter to 
 *              simplify usage for the end user. Added sync method call as well to sync primary and 
 *              secondary databases.
 * 
 * 2016-10-07   Merged in newer codebase from MRPlatform2014 SharpDevelop project. Merged changes include:
 * 
 *       2014-04-07	Added function documentation.
 * 		    		Added overloaded functions to allow passing dates as strings, not just System.DateTime
 * 			    	objects.
 * 
 *       2014-04-09	Fixed date issues in private function queries.
 * 
 *       2014-05-21	Changed sql query strings to OleDbCommand objects to prevent connection initialization
 * 		    		errors from happening.
 * 
 *       2014-05-24	Issues using MRDbConnection class. Now initiating SQLDB connection in class
 * 			    	constructor.
 * 
 *       2014-07-01	Added GetAlarmsEvents methods to return alarms & events from a date or date range.
 *  
 *   
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.Wonderware.AlarmEvent
{
    /// <summary>
    /// MRPlatform.AlarmEvent.MRAlarmEventLog class.
    /// </summary>
    [ComVisible(false)]
    [Guid("96E0CD61-EC8D-428F-BAF7-0A0910A6432F"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IAlarmEventLogEvents))]
    public class AlarmEventLog : IAlarmEventLog
	{
        private ErrorLog _errorLog = new ErrorLog();

        //Properties
        private MRDbConnection _dbConnection;

        /// <summary>Initializes a new instance of MRAlarmEventLog.</summary>
        /// <param name="mrDbConnection"></param>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// </code></example>
		public AlarmEventLog(MRDbConnection mrDbConnection)
		{
            _dbConnection = mrDbConnection;
		}

        public IDbConnection Connection
        {
            get
            {
                return new OleDbConnection(_dbConnection.ConnectionString);
            }
        }


        #region " GetTopAlarmOccurrences "

        /// <summary>
		/// Gets top alarm occurrences from the System Platform alarm database for a date.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="startDate">The System.DateTime date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 alarm occurrences for 3/1/2014
		/// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, DateTime(2014, 3, 1));
		/// </code></example>
        public DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate)
		{
			return DoGetTopOccurrences("v_AlarmHistory2", topCount, startDate, startDate);
		}


        /// <summary>
		/// Gets top alarm occurrences from the System Platform alarm database for a date.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="startDate">The System.String date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 alarm occurrences for 3/1/2014
		/// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, "3/1/2014");
		/// </code></example>
		public DataSet GetTopAlarmOccurrences(int topCount, string startDate)
        {
            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");
            
            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            DateTime dtStart = new DateTime();

            try {
                dtStart = DateTime.Parse(startDate);
            }
            catch(FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string startDate)", ex.Message);
                throw;
            }

            return DoGetTopOccurrences("v_AlarmHistory2", topCount, DateTime.Parse(startDate), DateTime.Parse(startDate));
        }


        /// <summary>
        /// Gets top alarm occurrences from the System Platform alarm database for a date range.
        /// </summary>
        /// <param name="topCount">The number of top occurrences to return.</param>
        /// <param name="startDate">The start of the date range to query.</param>
        /// <param name="endDate">The end of the date range to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve top 10 alarm occurrences for 3/1/2014 - 3/31/2014
        /// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate, DateTime endDate)
		{
			return DoGetTopOccurrences("v_AlarmHistory2", topCount, startDate, endDate);
		}


        /// <summary>
        /// Gets top alarm occurrences from the System Platform alarm database for a date range.
        /// </summary>
        /// <param name="topCount">The number of top occurrences to return.</param>
        /// <param name="startDate">The start of the date range to query.</param>
        /// <param name="endDate">The end of the date range to query.</param>
        /// <returns>System.Data.DataSet object or null if exception occurrs.</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve top 10 alarm occurrences from 3/1/2014 - 3/31/2014
        /// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetTopAlarmOccurrences(int topCount, string startDate, string endDate)
        {
            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string startDate, string endDate)", ex.Message);
                throw;
            }

            return DoGetTopOccurrences("v_AlarmHistory2", topCount, DateTime.Parse(startDate), DateTime.Parse(endDate));
        }


        /// <summary>
		/// Gets top alarm occurrences from the System Platform alarm database for an end date and a number of preceding days.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="endDate">The System.DateTime end date to query.</param>
		/// <param name="numDays">The number of days prior to the end date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 event occurrences for 3/15/2014 - 3/31/2014
		/// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, DateTime(2014, 3, 31), 15);
		/// </code></example>
        public DataSet GetTopAlarmOccurrences(int topCount, DateTime endDate, int numDays)
		{
			return DoGetTopOccurrences("v_AlarmHistory2", topCount, endDate, numDays);
		}


        /// <summary>
		/// Gets top alarm occurrences from the System Platform alarm database for an end date and a number of preceding days.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="endDate">The end of the date range to query.</param>
		/// <param name="numDays">The number of days before the end date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 alarm occurrences from 3/15/2014 - 3/31/2014
		/// DataGrid1.DataSource = mrae.GetTopAlarmOccurrences(10, "3/31/2014", 15);
		/// </code></example>
		public DataSet GetTopAlarmOccurrences(int topCount, string endDate, int numDays)
        {
            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");
            
            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            if (numDays == 0)
                throw new ArgumentOutOfRangeException("numDays", "The number of days to search must be less than or greater than zero.");
            
            DateTime dtEnd = new DateTime();

            try
            {
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string endDate, int numDays)", ex.Message);
                throw;
            }

            return DoGetTopOccurrences("v_AlarmHistory2", topCount, DateTime.Parse(endDate), numDays);
        }

        #endregion


        #region " GetTopEventOccurrences "

        /// <summary>
		/// Gets top event occurrences from the System Platform alarm database for a date.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="startDate">The System.DateTime date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 event occurrences for 3/1/2014
		/// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, DateTime(2014, 3, 1));
		/// </code></example>
        public DataSet GetTopEventOccurrences(int topCount, DateTime startDate)
		{
			return DoGetTopOccurrences("v_EventHistory", topCount, startDate, startDate);
		}


        /// <summary>
        /// Gets top event occurrences from the System Platform alarm database for a date.
        /// </summary>
        /// <param name="topCount">The number of top occurrences to return.</param>
        /// <param name="startDate">The System.String date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve top 10 event occurrences for 3/1/2014
        /// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, "3/1/2014");
        /// </code></example>
        public DataSet GetTopEventOccurrences(int topCount, string startDate)
        {
            if (topCount < 1)
            {
                string msg = "The number of occurrences to return must be greater than zero.";
                _errorLog.LogMessage(this.GetType().Name, "GetTopEventOccurrences(int topCount, string startDate)", msg);
                throw new ArgumentOutOfRangeException("topCount", msg);
            }

            if (startDate == null || startDate == "")
            {
                string msg = "Start date cannot be blank or null.";
                _errorLog.LogMessage(this.GetType().Name, "GetTopEventOccurrences(int topCount, string startDate)", msg);
                throw new ArgumentNullException("startDate", msg);
            }

            DateTime dtStart;

            if (!DateTime.TryParse(startDate, out dtStart))
            {
                string msg = "Start date is not a valid DateTime value.";
                _errorLog.LogMessage(this.GetType().Name, "GetTopEventOccurrences(int topCount, string startDate)", msg);
                throw new FormatException(msg);
            }

            return DoGetTopOccurrences("v_EventHistory", topCount, DateTime.Parse(startDate), DateTime.Parse(startDate));
        }


        /// <summary>
		/// Gets top event occurrences from the System Platform alarm database for a date range.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="startDate">The System.DateTime start date to query.</param>
		/// <param name="endDate">The System.DateTime end date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 event occurrences for 3/1/2014 - 3/31/2014
		/// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, DateTime(2014, 3, 1), DateTime(2014, 3, 31));
		/// </code></example>
        public DataSet GetTopEventOccurrences(int topCount, DateTime startDate, DateTime endDate)
		{
			return DoGetTopOccurrences("v_EventHistory", topCount, startDate, endDate);
		}


        /// <summary>
        /// Gets top event occurrences from the System Platform alarm database for a date range.
        /// </summary>
        /// <param name="topCount">The number of top occurrences to return.</param>
        /// <param name="startDate">The System.String start date to query.</param>
        /// <param name="endDate">The System.String end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve top 10 event occurrences for 3/1/2014 - 3/31/2014
        /// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetTopEventOccurrences(int topCount, string startDate, string endDate)
        {
            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTopEventOccurrences(int topCount, string startDate, string endDate)", ex.Message);
                throw;
            }

            return DoGetTopOccurrences("v_EventHistory", topCount, DateTime.Parse(startDate), DateTime.Parse(endDate));
        }


        /// <summary>
		/// Gets top event occurrences from the System Platform alarm database for an end date and a number of preceding days.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="endDate">The System.DateTime end date to query.</param>
		/// <param name="numDays">The number of days prior to the end date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 event occurrences for 3/15/2014 - 3/31/2014
		/// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, DateTime(2014, 3, 31), 15);
		/// </code></example>
        public DataSet GetTopEventOccurrences(int topCount, DateTime endDate, int numDays)
		{
			return DoGetTopOccurrences("v_EventHistory", topCount, endDate, numDays);
		}


        /// <summary>
		/// Gets top event occurrences from the System Platform alarm database for an end date and a number of preceding days.
		/// </summary>
		/// <param name="topCount">The number of top occurrences to return.</param>
		/// <param name="endDate">The end date to query.</param>
		/// <param name="numDays">The number of days prior to the end date to query.</param>
		/// <returns>System.Data.DataSet</returns>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
		/// 
		/// //Retrieve top 10 event occurrences for 3/15/2014 - 3/31/2014
		/// DataGrid1.DataSource = mrae.GetTopEventOccurrences(10, "3/31/2014", 15);
		/// </code></example>
		public DataSet GetTopEventOccurrences(int topCount, string endDate, int numDays)
        {
            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            if (numDays == 0)
                throw new ArgumentOutOfRangeException("numDays", "The number of days to search must be less than or greater than zero.");

            DateTime dtEnd = new DateTime();

            try
            {
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTopAlarmOccurrences(int topCount, string endDate, int numDays)", ex.Message);
                throw ex;
            }

            return DoGetTopOccurrences("v_EventHistory", topCount, DateTime.Parse(endDate), numDays);
        }

        #endregion


        #region " GetAlarmsEvents "

        /// <summary>
        /// Gets alarm and event occurrences from the System Platform alarm database for a date.
        /// </summary>
        /// <param name="startDate">The System.DateTime date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve the alarms and events for 3/1/2014
        /// DataGrid1.DataSource = mrae.GetAlarmsEvents(DateTime(2014, 3, 1));
        /// </code></example>
        public DataSet GetAlarmsEvents(DateTime startDate)
        {
            return DoGetHistory(startDate, startDate);
        }


        /// <summary>
        /// Gets alarm and event occurrences from the System Platform alarm database for a date range.
        /// </summary>
        /// <param name="startDate">The System.String date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve the alarms and events for 3/1/2014
        /// DataGrid1.DataSource = mrae.GetAlarmsEvents("3/1/2014");
        /// </code></example>
        public DataSet GetAlarmsEvents(string startDate)
        {
            if (startDate == null)
                throw new ArgumentNullException("startDate", "Start date cannot be null.");

            DateTime dtStart;

            if (!DateTime.TryParse(startDate, out dtStart))
                throw new ArgumentOutOfRangeException("startDate", "Start date is not a valid DateTime value.");

            return DoGetHistory(DateTime.Parse(startDate), DateTime.Parse(startDate));
        }


        /// <summary>
        /// Gets alarm and event occurrences from the System Platform alarm database for a date.
        /// </summary>
        /// <param name="startDate">The System.DateTime start date to query.</param>
        /// <param name="endDate">The System.DateTime end date to query</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve the alarms and events for 3/1/2014
        /// DataGrid1.DataSource = mrae.GetAlarmsEvents(DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetAlarmsEvents(DateTime startDate, DateTime endDate)
        {
            return DoGetHistory(startDate, endDate);
        }


        /// <summary>
        /// Gets alarm and event occurrences from the System Platform alarm database for a date range.
        /// </summary>
        /// <param name="startDate">The System.String start date to query.</param>
        /// <param name="endDate">The System.DateTime end date to query</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Retrieve the alarms and events for 3/1/2014
        /// DataGrid1.DataSource = mrae.GetAlarmsEvents("3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetAlarmsEvents(string startDate, string endDate)
        {
            if (startDate == null)
                throw new ArgumentNullException("startDate", "Start date cannot be null.");

            if (endDate == null)
                throw new ArgumentNullException("endDate", "End date cannot be null.");

            DateTime dtStart, dtEnd;

            if (!DateTime.TryParse(startDate, out dtStart))
                throw new ArgumentOutOfRangeException("startDate", "Start date is not a valid DateTime value.");

            if(!DateTime.TryParse(endDate, out dtEnd))
                throw new ArgumentOutOfRangeException("endDate", "End date is not a valid DateTime value.");

            return DoGetHistory(DateTime.Parse(startDate), DateTime.Parse(endDate));
        }

        #endregion


        #region " GetTagHistory "

        /// <summary>
        /// Gets all the alarm and event entries for a tagname.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName");
        /// </code></example>
        public DataSet GetTagHistory(string tagName)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            return DoGetTagHistory(tagName);
        }


        /// <summary>
        /// Gets all the alarm and event entries for a tagname that occurred on a specific date.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", DateTime(2014, 3, 1));
        /// </code></example>
        public DataSet GetTagHistory(string tagName, DateTime startDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            return DoGetTagHistory(tagName, startDate, startDate);
        }


        /// <summary>
        /// Gets all the alarm and event entries for a tagname that occurred on a specific date.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", "3/1/2014");
        /// </code></example>
        public DataSet GetTagHistory(string tagName, string startDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            DateTime dtStart = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTagHistory(string tagName, string startDate, string endDate)", ex.Message);
                throw;
            }

            return DoGetTagHistory(tagName, DateTime.Parse(startDate), DateTime.Parse(startDate));
        }


        /// <summary>
        /// Gets all the alarm and event entries for a tagname that occurred within a specific date range.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">The end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetTagHistory(string tagName, DateTime startDate, DateTime endDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            return DoGetTagHistory(tagName, startDate, endDate);
        }


        /// <summary>
        /// Gets all the alarm and event entries for a tagname that occurred within a specific date range.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">The end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetTagHistory(string tagName, string startDate, string endDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTagHistory(string tagName, string startDate, string endDate)", ex.Message);
                throw;
            }

            return DoGetTagHistory(tagName, DateTime.Parse(startDate), DateTime.Parse(endDate));
        }


        /// <summary>
        /// Gets a specified number of alarm and event entries for a tagname.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", 10);
        /// </code></example>
        public DataSet GetTagHistory(string tagName, int topCount)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            return DoGetTagHistory(tagName, topCount);
        }


        /// <summary>
        /// Gets a specified number of alarm and event entries for a tagname that occurred on a specific date.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", 10, DateTime(2014, 3, 1));
        /// </code></example>
        public DataSet GetTagHistory(string tagName, int topCount, DateTime startDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            return DoGetTagHistory(tagName, topCount, startDate, startDate);
        }


        /// <summary>
        /// Gets a specified number of alarm and event entries for a tagname that occurred on a specific date.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", 10, "3/1/2014");
        /// </code></example>
        public DataSet GetTagHistory(string tagName, int topCount, string startDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            DateTime dtStart = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTagHistory(string tagName, int topCount, string startDate)", ex.Message);
                throw;
            }

            return DoGetTagHistory(tagName, topCount, DateTime.Parse(startDate), DateTime.Parse(startDate));
        }


        /// <summary>
        /// Gets a specified number of alarm and event entries for a tagname that occurred within a specific date range.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">the end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", 10, DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetTagHistory(string tagName, int topCount, DateTime startDate, DateTime endDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            return DoGetTagHistory(tagName, topCount, startDate, endDate);
        }


        /// <summary>
        /// Gets a specified number of alarm and event entries for a tagname that occurred within a specific date range.
        /// </summary>
        /// <param name="tagName">The tagname to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">the end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("tagName", 10, "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetTagHistory(string tagName, int topCount, string startDate, string endDate)
        {
            if (tagName == null || tagName == "")
                throw new ArgumentNullException("tagName", "Tag name cannot be blank or null.");

            if (topCount < 1)
                throw new ArgumentOutOfRangeException("topCount", "The number of occurrences to return must be greater than zero.");

            if (startDate == null || startDate == "")
                throw new ArgumentNullException("startDate", "Start date cannot be blank or null.");

            if (endDate == null || endDate == "")
                throw new ArgumentNullException("endDate", "End date cannot be blank or null.");

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            try
            {
                dtStart = DateTime.Parse(startDate);
                dtEnd = DateTime.Parse(endDate);
            }
            catch (FormatException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "GetTagHistory(string tagName, int topCount, string startDate, string endDate)", ex.Message);
                throw;
            }

            return DoGetTagHistory(tagName, topCount, DateTime.Parse(startDate), DateTime.Parse(endDate));
        }

        #endregion


        #region " GetUserHistory "

        /// <summary>
        /// Gets all the alarm and event entries for a user name.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName");
        /// </code></example>
        public DataSet GetUserHistory(string userName)
		{
			return DoGetUserHistory(userName);
		}


        /// <summary>
        /// Gets all the alarm and event entries for a user name that occurred on a specific date.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", DateTime(2014, 3, 1));
        /// </code></example>
        public DataSet GetUserHistory(string userName, DateTime startDate)
		{
			return DoGetUserHistory(userName, startDate, startDate);
		}


        /// <summary>
        /// Gets all the alarm and event entries for a user name that occurred on a specific date.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", "3/1/2014");
        /// </code></example>
        public DataSet GetUserHistory(string userName, string startDate)
		{
			return DoGetUserHistory(userName, DateTime.Parse(startDate), DateTime.Parse(startDate));
		}


        /// <summary>
        /// Gets all the alarm and event entries for a user name that occurred within a specific date range.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">The end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetUserHistory(string userName, DateTime startDate, DateTime endDate)
		{
			return DoGetUserHistory(userName, startDate, endDate);
		}


        /// <summary>
        /// Gets all the alarm and event entries for a user name that occurred within a specific date range.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">The end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetUserHistory(string userName, string startDate, string endDate)
		{
			return DoGetUserHistory(userName, DateTime.Parse(startDate), DateTime.Parse(endDate));
		}


        /// <summary>
        /// Gets a specified number of alarm and event entries for a user name.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", 10);
        /// </code></example>
        public DataSet GetUserHistory(string userName, int topCount)
		{
			return DoGetUserHistory(userName, topCount);
		}


        /// <summary>
        /// Gets a specified number of alarm and event entries for a user name that occurred on a specific date.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", 10, DateTime(2014, 3, 1));
        /// </code></example>
        public DataSet GetUserHistory(string userName, int topCount, DateTime startDate)
		{
			return DoGetUserHistory(userName, topCount, startDate, startDate);
		}


        /// <summary>
        /// Gets a specified number of alarm and event entries for a user name that occurred on a specific date.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", 10, "3/1/2014");
        /// </code></example>
        public DataSet GetUserHistory(string userName, int topCount, string startDate)
		{
			return DoGetUserHistory(userName, topCount, DateTime.Parse(startDate), DateTime.Parse(startDate));
		}


        /// <summary>
        /// Gets a specified number of alarm and event entries for a user name that occurred within a specific date range.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">the end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", 10, DateTime(2014, 3, 1), DateTime(2014, 3, 31));
        /// </code></example>
        public DataSet GetUserHistory(string userName, int topCount, DateTime startDate, DateTime endDate)
		{
			return DoGetUserHistory(userName, topCount, startDate, endDate);
		}


        /// <summary>
        /// Gets a specified number of alarm and event entries for a user name that occurred within a specific date range.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <param name="topCount">The number of most recent occurrences to return.</param>
        /// <param name="startDate">The start date to query.</param>
        /// <param name="endDate">the end date to query.</param>
        /// <returns>System.Data.DataSet</returns>
        /// <example><code>
        /// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRAlarmEventLog mrae = new MRAlarmEventLog(mrdb);
        /// 
        /// //Get alarm and event log
        /// DataGrid1.DataSource = mrae.GetTagHistory("userName", 10, "3/1/2014", "3/31/2014");
        /// </code></example>
        public DataSet GetUserHistory(string userName, int topCount, string startDate, string endDate)
		{
			return DoGetUserHistory(userName, topCount, DateTime.Parse(startDate), DateTime.Parse(endDate));
		}

        #endregion


        #region " DoGetTopOccurrences "

        private DataSet DoGetTopOccurrences(string tableName, int topCount, DateTime startDate, DateTime endDate)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT TOP(" + topCount + ") TagName, Count(*)" + 
                                " FROM " + tableName + 
                                " WHERE EventStamp >= '" + startDate.ToShortDateString() + "'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " GROUP BY TagName" + 
                                " ORDER BY Count(*) DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}
		
		
		private DataSet DoGetTopOccurrences(string tableName, int topCount, DateTime endDate, int numDays)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                DataSet ds = new DataSet();
                string sQuery = "SELECT TOP(" + topCount + ") TagName, Count(*)" +
                                " FROM " + tableName +
                                " WHERE EventStamp >= DATEADD(day, " + numDays.ToString() + ", EventStamp)" +
                                " AND EventStamp <= '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " GROUP BY TagName" + 
                                " ORDER BY Count(*) DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}

        #endregion


        #region " DoGetTagHistory "

        /// <summary>
        /// DoGetHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetTagHistory(string tagName)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT EventStamp, Value, Operator" +
                            " FROM v_AlarmEventHistory2" +
                            " WHERE TagName = '" + tagName + "'" +
                            " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetTagHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetTagHistory(string tagName, int topCount)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT TOP " + topCount.ToString() + " EventStamp, Value, Operator" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE TagName = '" + tagName + "'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetTagHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetTagHistory(string tagName, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT EventStamp, Value, Operator" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE TagName = '" + tagName + "'" +
                                " AND EventStamp >= '" + startDate.ToShortDateString() + " 00:00:00.000'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetTagHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetTagHistory(string tagName, int topCount, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT TOP " + topCount.ToString() + " EventStamp, Value, Operator" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE TagName = '" + tagName + "'" +
                                " AND EventStamp >= '" + startDate.ToShortDateString() + " 00:00:00.000'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }

        #endregion


        #region " DoGetUserHistory "

        /// <summary>
        /// DoGetUserHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetUserHistory(string userName)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT EventStamp, AlarmState, TagName, Description, Area, Type, Value, CheckValue, Operator, AlarmDuration, OperatorNode" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE Operator = '" + userName + "'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetUserHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetUserHistory(string userName, int topCount)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT TOP " + topCount.ToString() + " EventStamp, AlarmState, TagName, Description, Area, Type, Value, CheckValue, Operator, AlarmDuration, OperatorNode" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE Operator = '" + userName + "'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetUserHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetUserHistory(string userName, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT EventStamp, AlarmState, TagName, Description, Area, Type, Value, CheckValue, Operator, AlarmDuration, OperatorNode" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE Operator = '" + userName + "'" +
                                " AND EventStamp >= '" + startDate.ToShortDateString() + " 00:00:00.000'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }


        /// <summary>
        /// DoGetUserHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetUserHistory(string userName, int topCount, DateTime startDate, DateTime endDate)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT TOP " + topCount.ToString() + " EventStamp, AlarmState, TagName, Description, Area, Type, Value, CheckValue, Operator, AlarmDuration, OperatorNode" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE Operator = '" + userName + "'" +
                                " AND EventStamp >= '" + startDate.ToShortDateString() + " 00:00:00.000'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }

        #endregion


        #region " DoGetHistory "

        /// <summary>
        /// DoGetHistory
        /// </summary>
        /// <returns>System.Data.DataSet</returns>
        private DataSet DoGetHistory(DateTime startDate, DateTime endDate)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                string sQuery = "SELECT EventStamp, Area, Description, Type, Value, CheckValue, AlarmState, AlarmDuration, Operator, TagName" +
                                " FROM v_AlarmEventHistory2" +
                                " WHERE EventStamp >= '" + startDate.ToShortDateString() + " 00:00:00.000'" +
                                " AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999'" +
                                " ORDER BY EventStamp DESC";

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                DataSet ds = new DataSet();
                dbAdapt.Fill(ds);

                return ds;
            }
        }

        #endregion
    }
}
