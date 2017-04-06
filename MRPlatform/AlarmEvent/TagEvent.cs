using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;
	

namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// MRPlatform.AlarmEvent.MRTagEvent class.
    /// </summary>
    [Guid("8F141A9D-EB47-4FF5-9FFE-9C507625EAAF")]
    public class TagEvent : ITagEvent
	{
        public MRDbConnection _dbConnection;
        private bool _useADODB = false;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <remarks>Creates a new instance of MRTagEvent.</remarks>
        public TagEvent(MRDbConnection mrDbConnection)
		{
            _dbConnection = mrDbConnection;
            _useADODB = mrDbConnection.UseADODB;
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
        [ComVisible(true)]
        public void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew)
		{
            if(_useADODB)
            {
                ADODB.Connection dbConnection = _dbConnection.ADODBConnection;
                dbConnection.Open();

                string sQuery = "INSERT INTO TagEventLog(userName, nodeName, tagName, tagValueOrig, tagValueNew) " +
                                "VALUES('" + userName + "', '" + nodeName + "', '" + tagName + "', " + tagValueOrig + ", " + tagValueNew + ")";

                object recAffected = 0;
                dbConnection.Execute(sQuery, out recAffected);
            }
            else
            {
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    string sQuery = "INSERT INTO TagEventLog(userName, nodeName, tagName, tagValueOrig, tagValueNew) " +
                                    "VALUES(?, ?, ?, ?, ?)";

                    OleDbCommand dbCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                    dbCmd.Parameters.AddWithValue("@userName", userName);
                    dbCmd.Parameters.AddWithValue("@nodeName", nodeName);
                    dbCmd.Parameters.AddWithValue("@tagName", tagName);
                    dbCmd.Parameters.AddWithValue("@tagValueOrig", tagValueOrig);
                    dbCmd.Parameters.AddWithValue("@tagValueNew", tagValueNew);

                    dbCmd.ExecuteNonQuery();
                }
            }
        }
		
		
		/// <summary>GetHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">OleDbConnection object of the database connection.</param>
		/// <param name="nRecordCount">Last 'n' number of records to return.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistoryDataSet(string tagName, int nRecordCount)
		{
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(GetHistoryQuery(tagName, nRecordCount));
			
			return ds;
		}

        [ComVisible(true)]
        public Recordset GetHistoryRecordset(string tagName, int nRecordCount)
        {
            Recordset rs = new Recordset();
            rs = GetRecordsetFromQuery(GetHistoryQuery(tagName, nRecordCount));

            return rs;
        }

        private string GetHistoryQuery(string tagName, int nRecordCount)
        {
            string sQuery = "SELECT TOP " + nRecordCount.ToString() + " *" +
                            " FROM TagEventLog" +
                            " ORDER BY evtDateTime DESC";

            return sQuery;
        }
		
		
		/// <summary>GetHistory Method</summary>
		/// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
		/// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
		/// <param name="dbConn">OleDbConnection object of the database connection.</param>
		/// <param name="startDateTime">DateTime object of the date of the recordset.</param>
		/// <returns>System.Data.DataSet</returns>
		public DataSet GetHistoryDataSet(DateTime startDate)
		{
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(GetHistoryQuery(startDate));
			
			return ds;
		}

        [ComVisible(true)]
        public Recordset GetHistoryRecordset(DateTime startDate)
        {
            Recordset rs = new Recordset();
            rs = GetRecordsetFromQuery(GetHistoryQuery(startDate));

            return rs;
        }

        private string GetHistoryQuery(DateTime startDate)
        {
            string sQuery = "SELECT *" +
                            " FROM TagEventLog" +
                            " WHERE evtDateTime >= '" + startDate.Date + "'" +
                            " AND evtDateTime <= '" + startDate.Date.Add(TimeSpan.FromSeconds(86399)) + "'" +
                            " ORDER BY evtDateTime";

            return sQuery;
        }


        /// <summary>GetHistory Method</summary>
        /// <remarks>Overloaded method to retrieve data from the mrsystems SQL Server database in the
        /// form of a System.Data.DataSet object that can be used to fill a DataGrid object.</remarks>
        /// <param name="dbConn">OleDbConnection object of the database connection.</param>
        /// <param name="startDateTime">DateTime object of the start date of the recordset.</param>
        /// <param name="endDateTime">DateTime object of the end date of the recordset.</param>
        /// <returns>System.Data.DataSet</returns>
        public DataSet GetHistoryDataSet(DateTime startDateTime, DateTime endDateTime)
		{
			DataSet ds = new DataSet();
			ds = GetDataSetFromQuery(GetHistoryQuery(startDateTime, endDateTime));
			
			return ds;
		}

        [ComVisible(true)]
        public Recordset GetHistoryRecordset(DateTime startDateTime, DateTime endDateTime)
        {
            Recordset rs = new Recordset();
            rs = GetRecordsetFromQuery(GetHistoryQuery(startDateTime, endDateTime));

            return rs;
        }

        private string GetHistoryQuery(DateTime startDateTime, DateTime endDateTime)
        {
            string sQuery = "SELECT *" +
                            " FROM TagEventLog" +
                            " WHERE evtDateTime >= '" + startDateTime + "'" +
                            " AND evtDateTime <= '" + endDateTime + "'" +
                            " ORDER BY evtDateTime";

            return sQuery;
        }


        private DataSet GetDataSetFromQuery(string sQuery)
		{
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                DataSet ds = new DataSet();

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sQuery, dbConnection.ConnectionString);
                dbAdapt.Fill(ds);

                return ds;
            }
		}

        
        private ADODB.Recordset GetRecordsetFromQuery(string sQuery)
        {
            ADODB.Connection dbConnection = _dbConnection.ADODBConnection;
            ADODB.Recordset rs = new ADODB.Recordset();
            object recAffected = 0;
            rs = dbConnection.Execute(sQuery, out recAffected);

            return rs;
        }
    }
}
