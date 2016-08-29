/***************************************************************************************************
 * Class: 			MRDbConnection.cs
 * Created By:		Eric Conder
 * Created On:		2014-01-06
 * 
 * Changes:
 * 
 * 2014-03-06	Recreated the MRDbConnection under new namespace MRPlatform2014.Data.Sql.
 * 
 * 2014-03-27	Writing MRSqlConnection.Open() functions to be used internally by the other classes
 * 				to access the SQL database.
 * 
 * 2014-04-03	Added deconstructor to close database if connection is still open when class is disposed.
 * 
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform2014.AlarmEvent;


namespace MRPlatform2014.Data.Sql
{
	/// <summary>
	/// Description of MRDbConnection.
	/// </summary>
	public class MRDbConnection
	{
		private SqlConnection _dbConn;
		
		public MRDbConnection()
		{
			this.DbConnected = false;
		}
		
		
		public SqlConnection Open(string dbServerName, string dbInstanceName, string userName, string password)
		{
			//Set property values
			this.ServerName = dbServerName;
			this.DatabaseName = dbInstanceName;
			this.UserName = userName;
			this.Password = password;
			
			//Connect to SQL Database
			string connStr = "Server=" + this.ServerName + "; Database=" + this.DatabaseName + "; User Id=" + this.UserName + "; Password=" + this.Password + ";";  
			this._dbConn = new SqlConnection(connStr);
			
			try
			{
				this._dbConn.Open();
				
				if(this._dbConn.State == ConnectionState.Open)
				{
					DbConnected = true;
					return this._dbConn;
				}
				return null;
			}
			catch(InvalidOperationException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("InvalidOperationException: " + e.Message);
				return null;
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
				return null;
			}
			catch(ArgumentException e)
			{
				WinEventLog winel	 = new WinEventLog();
				winel.WriteEvent("ArgumentException: " + e.Message);
				return null;
			}
		}
		
		
		public string ConnectionString { get; set; }
		public string ServerName { get; set; }
		public string DatabaseName { get; set; }
		public string UserName { get; set; }
		private  string Password { get; set; }
		public bool DbConnected { get; set; }
	}
}
