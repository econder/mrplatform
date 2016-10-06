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
		public MRAlarmEventLog(MRDbConnection mrDbConnection)
		{
			
		}
		
		~MRAlarmEventLog()
		{
			
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
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this.DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}
		
		
		private DataSet DoGetTopOccurrences(string tableName, int topCount, DateTime endDate, int numDays)
		{
			DataSet ds = new DataSet();
			string sQuery = "SELECT TOP(" + topCount + ") TagName, Count(*) FROM " + tableName + " WHERE EventStamp >= DATEADD(day, " + numDays.ToString() + ", EventStamp) AND EventStamp <= '" + endDate.ToShortDateString() + " 23:59:59.999' GROUP BY TagName ORDER BY Count(*) DESC";
			
			SqlDataAdapter dbAdapt = new SqlDataAdapter(sQuery, this.DbConnection.DbConnection);
			dbAdapt.Fill(ds);
			
			return ds;
		}


        private MRDbConnection DbConnection { get; set; }
	}
}
