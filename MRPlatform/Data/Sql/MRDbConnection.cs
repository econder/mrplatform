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
 * 2016-08-29   Added new constructor parameters for the sync server's name, instance, username, & password.
 * 
 * *************************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;

using MRPlatform.AlarmEvent;


namespace MRPlatform.Data.Sql
{
    public class MRDbConnection
	{
		private SqlConnection _dbConn;
        private SqlConnection _dbSyncConn;

        /// <summary>
        /// MRDbConnection class constructor.
        /// </summary>
        /// <remarks>Creates a new instance of MRDbConnection.</remarks>
        /// <returns>MRDbConnection object.</returns>
        /// <example>Example:<code>
        /// MRDbConnection mrdb = new MRDbConnection();
        /// </code></example>
        public MRDbConnection()
		{
			this.DbConnected = false;
            this.SyncDbConnected = false;
		}

        /// <summary>
        /// OpenDatabase method.
        /// </summary>
        /// <remarks>Method to open a database connection. This connection is also the primary connection when using MRDbSync
        /// to synchronize two or more SQL databases with each other using the MRDbSync library which utilizes Microsoft's
        /// SyncFramework.</remarks>
        /// <param name="dbServerName">SQL database server name as a string.</param>
        /// <param name="dbInstanceName">SQL database instance name as a string.</param>
        /// <param name="dbUserName">SQL database user name as a string.</param>
        /// <param name="dbPassword">SQL database password as a string.</param>
        /// <returns>System.Data.SqlClient.SqlConnection object.</returns>
        /// <example>Example:<code>
        /// MRDbConnection mrdb = new MRDbConnection();
        /// SqlConnection conn = mrdb.OpenDatabase("ServerName", "InstanceName", "User Name", "Password");
        /// </code></example>
        public SqlConnection OpenDatabase(string dbServerName, string dbInstanceName, string dbUserName, string dbPassword)
		{
			//Set property values for master SQL database
			this.ServerName = dbServerName;
			this.DatabaseName = dbInstanceName;
			this.UserName = dbUserName;
			this.Password = dbPassword;
			
			//Connect to master SQL Database
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


        /// <summary>
        /// OpenSyncDatabase method.
        /// </summary>
        /// <remarks>Method to open a database connection to the backup SQL synchronization database.</remarks>
        /// <param name="dbSyncServerName">SQL database server name as a string.</param>
        /// <param name="dbSyncInstanceName">SQL database instance name as a string.</param>
        /// <param name="dbSyncUserName">SQL database user name as a string.</param>
        /// <param name="dbSyncPassword">SQL database password as a string.</param>
        /// <returns>System.Data.SqlClient.SqlConnection object.</returns>
        /// <example>Example:<code>
        /// MRDbConnection mrdbSync = new MRDbConnection();
        /// SqlConnection conn = mrdbSync.OpenDatabase("SyncServerName", "SyncInstanceName", "Sync User Name", "Sync Password");
        /// </code></example>
        public SqlConnection OpenSyncDatabase(string dbSyncServerName = null, string dbSyncInstanceName = null, string dbSyncUserName = null, string dbSyncPassword = null)
        {
            //Set property values for master SQL database
            this.SyncServerName = dbSyncServerName;
            this.SyncDatabaseName = dbSyncInstanceName;
            this.SyncUserName = dbSyncUserName;
            this.SyncPassword = dbSyncPassword;

            //Connect to sync SQL Database
            string syncConnStr = "Server=" + this.SyncServerName + "; Database=" + this.SyncDatabaseName + "; User Id=" + this.SyncUserName + "; Password=" + this.SyncPassword + ";";
            this._dbSyncConn = new SqlConnection(syncConnStr);

            try
            {
                this._dbSyncConn.Open();

                if (this._dbSyncConn.State == ConnectionState.Open)
                {
                    SyncDbConnected = true;
                    return this._dbSyncConn;
                }
                return null;
            }
            catch (InvalidOperationException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("InvalidOperationException: " + e.Message);
                return null;
            }
            catch (SqlException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("SqlException: " + e.Message);
                return null;
            }
            catch (ArgumentException e)
            {
                WinEventLog winel = new WinEventLog();
                winel.WriteEvent("ArgumentException: " + e.Message);
                return null;
            }
        }

		//Master SQL database properties
		public string ConnectionString { get; set; }
		public string ServerName { get; set; }
		public string DatabaseName { get; set; }
		public string UserName { get; set; }
		private  string Password { get; set; }
		public bool DbConnected { get; set; }

        //Sync SQL database properties
        public string SyncConnectionString { get; set; }
        public string SyncServerName { get; set; }
        public string SyncDatabaseName { get; set; }
        public string SyncUserName { get; set; }
        private string SyncPassword { get; set; }
        public bool SyncDbConnected { get; set; }
    }
}
