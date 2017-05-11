using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatformVBA.DB.Sql;


namespace MRPlatformVBA.AlarmEvent
{
    /// <summary>
    /// MRPlatform.AlarmEvent.MRTagEvent class.
    /// </summary>
    [ComVisible(true)]
    [Guid("10443FEB-A382-4AC5-A21E-BBBEDA7FA42C"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(ITagEventEvents))]
    public class TagEvent : ITagEvent
    {
        public MRDbConnection _dbConnection;


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <remarks>Creates a new instance of MRTagEvent.</remarks>
        public TagEvent(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        /// <summary>
		/// Logs a tag event.
		/// </summary>
		/// <param name="userName">String object of the HMI username of the user currently logged in.</param>
		/// <param name="nodeName">String object of the HMI client node name the event is generated from.</param>
		/// <param name="tagName">Tag name the event relates to.</param>
		/// <param name="tagValueOrig">Original value of the tag.</param>
		/// <param name="tagValueNew">New value of the tag.</param>
		/// <example><code>
		/// //Create instance of MRDbConnection first
        /// MRDbConnection mrdb = new MRDbConnection("ServerName", "DatabaseName", "Username", "Password")
        /// 
        /// //Create instance of MRAlarmEventLog
        /// MRTagEvent mrte = new MRTagEvent(mrdb);
		/// 
		/// //Log event
		/// mrte.LogEvent(InTouch:$Operator, InTouch:SCADA_NODE_NAME, "INF_PS_P1_SPD_SP", 53.5, 79.5); 
		/// </code></example>
        public void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew)
        {
            string sQuery = "INSERT INTO TagEventLog(userName, nodeName, tagName, tagValueOrig, tagValueNew) " +
                            "VALUES('" + userName + "', '" + nodeName + "', '" + tagName + "', " + tagValueOrig + ", " + tagValueNew + ")";

            ADODB.Connection dbConnection = _dbConnection.Connection;
            object recAffected = 0;
            dbConnection.Execute(sQuery, out recAffected);
        }


        /// <summary>GetHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="dbConn">OleDbConnection object of the database connection.</param>
        /// <param name="nRecordCount">Last 'n' number of records to return.</param>
        /// <returns>System.Data.DataSet</returns>
        public ADODB.Recordset GetHistory(string tagName, int nRecordCount)
        {
            string sQuery = "SELECT TOP " + nRecordCount.ToString() + " *" +
                            " FROM TagEventLog" +
                            " ORDER BY evtDateTime DESC";

            ADODB.Recordset rs = new ADODB.Recordset();
            rs = GetDataSetFromQuery(sQuery);

            return rs;
        }


        /// <summary>GetHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="dbConn">OleDbConnection object of the database connection.</param>
        /// <param name="startDateTime">DateTime object of the date of the recordset.</param>
        /// <returns>System.Data.DataSet</returns>
        public ADODB.Recordset GetHistory(DateTime startDate)
        {
            string sQuery = "SELECT *" +
                            " FROM TagEventLog" +
                            " WHERE evtDateTime >= '" + startDate + " 00:00:00.000'" +
                            " AND evtDateTime <= '" + startDate + "23:59:59.999'" +
                            " ORDER BY evtDateTime";

            ADODB.Recordset rs = new ADODB.Recordset();
            rs = GetDataSetFromQuery(sQuery);

            return rs;
        }


        /// <summary>GetHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="dbConn">OleDbConnection object of the database connection.</param>
        /// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
        /// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
        /// <returns>System.Data.DataSet</returns>
        public ADODB.Recordset GetHistory(DateTime startDateTime, DateTime endDateTime)
        {
            string sQuery = "SELECT *" +
                            " FROM TagEventLog" +
                            " WHERE evtDateTime >= '" + startDateTime + "'" +
                            " AND evtDateTime <= '" + endDateTime + "'" +
                            " ORDER BY evtDateTime";

            ADODB.Recordset rs = new ADODB.Recordset();
            rs = GetDataSetFromQuery(sQuery);

            return rs;
        }


        private ADODB.Recordset GetDataSetFromQuery(string sQuery)
        {
            ADODB.Connection dbConnection = _dbConnection.Connection;
            ADODB.Recordset rs = new ADODB.Recordset();
            object recAffected = 0;
            rs = dbConnection.Execute(sQuery, out recAffected);

            return rs;
        }
    }
}
