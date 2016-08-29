/***************************************************************************************************
 * Class:    	MRAlarmEventLog.cs
 * Created By:  Eric Conder
 * Created On:  2014-03-28
 * 
 * Changes:
 * 2014-04-01	Renamed MRAlarm to MRAlarmEventLog to be more descriptive. Added
 * 				GetTopAlarmOccurrences() classes.
 * 
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform.Data.Sql;


namespace MRPlatform.AlarmEvent
{
	/// <summary>
	/// Description of MRAlarmEvent.
	/// </summary>
	public class MRAlarmEventLog
	{
		private SqlConnection _dbConn;
		
		public MRAlarmEventLog(string serverName, string dbInstanceName, string userName, string password)
		{
			MRDbConnection mrdb = new MRDbConnection();
			mrdb.ConnectionString = "Server=" + serverName + "; Database=" + dbInstanceName + "; Uid=" + userName + "; Pwd=" + password;
			this._dbConn = mrdb.Open(serverName, dbInstanceName, userName, password);
		}
		
		~MRAlarmEventLog()
		{
			if(this._dbConn.State == ConnectionState.Open)
			{
				this._dbConn.Close();
			}
		}
		
		
		public DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("AlarmHistory2", topCount, startDate, startDate);
			
			return ds;
		}
		
		
		public DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate, DateTime endDate)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("AlarmHistory2", topCount, startDate, endDate);
			
			return ds;
		}
		
		
		public DataSet GetTopAlarmOccurrences(int topCount, DateTime endDate, int numDays)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("AlarmHistory2", topCount, endDate, numDays);
			
			return ds;
		}
		
		
		public DataSet GetTopEventOccurrences(int topCount, DateTime startDate)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("EventHistory2", topCount, startDate, startDate);
			
			return ds;
		}
		
		
		public DataSet GetTopEventOccurrences(int topCount, DateTime startDate, DateTime endDate)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("EventHistory2", topCount, startDate, endDate);
			
			return ds;
		}
		
		
		public DataSet GetTopEventOccurrences(int topCount, DateTime endDate, int numDays)
		{
			DataSet ds = new DataSet();
			ds = DoGetTopOccurrences("EventHistory2", topCount, endDate, numDays);
			
			return ds;
		}
		
		
		private DataSet DoGetTopOccurrences(string tableName, int topCount, DateTime startDate, DateTime endDate)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT TOP(" + topCount + ") TagName, Count(*) FROM " + tableName + " WHERE EventStamp >= '" + startDate.ToShortDateString() + "' AND EventStamp < '" + endDate.ToShortDateString() + " 23:59:59.999' GROUP BY TagName ORDER BY Count(*) DESC";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet DoGetTopOccurrences(string tableName, int topCount, DateTime endDate, int numDays)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT TOP(" + topCount + ") TagName, Count(*) FROM " + tableName + " WHERE EventStamp >= DATEADD(day, " + numDays.ToString() + ", EventStamp) AND EventStamp <= '" + endDate.ToShortDateString() + " 23:59:59.999' GROUP BY TagName ORDER BY Count(*) DESC";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this._dbConn);
			dbAdapt.Fill(ds);
			
			return ds;
		}
	}
}
