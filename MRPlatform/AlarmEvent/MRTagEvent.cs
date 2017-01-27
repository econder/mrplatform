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
using System.Runtime.InteropServices;

using MRPlatform.AlarmEvent;
using MRPlatform.Data.Sql;
	

namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// MRPlatform.AlarmEvent.MRTagEvent class.
    /// </summary>
    [ComVisible(true)]
    [Guid("832C3EAF-D79D-42A0-989E-D1514F630668"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMRTagEvent))]
    public class MRTagEvent : IMRTagEvent
	{
        //Properties
        public MRDbConnection DbConnection { get; set; }
        public DataSet EventHistory { get; set; }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <remarks>Creates a new instance of MRTagEvent.</remarks>
        public MRTagEvent(MRDbConnection mrDbConnection)
		{
            DbConnection = mrDbConnection;
		}


        /// <summary>
        /// Class destructor
        /// </summary>
        ~MRTagEvent()
		{
            if (DbConnection.DbConnection.State == ConnectionState.Open)
            {
                DbConnection.DbConnection.Close();
            }
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

            SqlCommand dbCmd = new SqlCommand(sQuery, DbConnection.DbConnection);

            dbCmd.ExecuteNonQuery();
			
			dbCmd.CommandText = sQuery;
            dbCmd.Connection = DbConnection.DbConnection;

            dbCmd.ExecuteNonQuery();
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

            EventHistory = ds;
			
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
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, DbConnection.DbConnection);
			dbAdapt.Fill(ds);

            EventHistory = ds;
			
			return ds;
		}
	}
}
