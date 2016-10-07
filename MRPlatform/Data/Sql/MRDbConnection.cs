﻿/***************************************************************************************************
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
        public enum RedundantNode : int
        {
            Master,
            Backup
        }

        public enum SyncDirection : int
        {
            Download = 0,
            DownloadAndUpload,
            Upload,
            UploadAndDownload
        }

        /// <summary>
        /// MRDbConnection class constructor.
        /// </summary>
        /// <remarks>Creates a new instance of MRDbConnection.</remarks>
        /// <param name="dbConnection">SqlConnection object</param>
        /// <returns>MRDbConnection object.</returns>
        /// <example>Example:<code>
        /// MRDbConnection mrdb = new MRDbConnection();
        /// </code></example>
        public MRDbConnection(SqlConnection dbConnection, RedundantNode redundantNodeAssignment = RedundantNode.Master)
        {
            ThisNode = redundantNodeAssignment;

            DbConnection = dbConnection;
            OpenDatabase(DbConnection);
        }


        public MRDbConnection(SqlConnection dbConnection, SqlConnection dbSyncConnection, RedundantNode redundantNodeAssignment = RedundantNode.Master)
        {
            ThisNode = redundantNodeAssignment;

            DbConnection = dbConnection;
            OpenDatabase(DbConnection);

            SyncDbConnection = dbSyncConnection;
            OpenDatabase(SyncDbConnection);
        }


        public MRDbConnection(string dbServerName, string dbInstanceName, string dbUserName, string dbPassword, RedundantNode redundantNodeAssignment = RedundantNode.Master)
        {
            ThisNode = redundantNodeAssignment;

            //Set property values for master SQL database
            ServerName = dbServerName;
            DatabaseName = dbInstanceName;
            UserName = dbUserName;
            Password = dbPassword;

            //Connect to master SQL Database
            string connStr = "Server=" + ServerName + "; Database=" + DatabaseName + "; User Id=" + UserName + "; Password=" + Password + ";";
            DbConnection = new SqlConnection(connStr);
            OpenDatabase(DbConnection);
        }


        public MRDbConnection(string dbServerName, string dbInstanceName, string dbUserName, string dbPassword,
                              string dbSyncServerName, string dbSyncInstanceName, string dbSyncUserName, string dbSyncPassword,
                              RedundantNode redundantNodeAssignment = RedundantNode.Master)
        {
            ThisNode = redundantNodeAssignment;

            //Set property values for master SQL database
            ServerName = dbServerName;
            DatabaseName = dbInstanceName;
            UserName = dbUserName;
            Password = dbPassword;

            //Connect to master SQL Database
            string connStr = "Server=" + ServerName + "; Database=" + DatabaseName + "; User Id=" + UserName + "; Password=" + Password + ";";
            DbConnection = new SqlConnection(connStr);
            OpenDatabase(DbConnection);

            //Set property values for sync SQL database
            SyncServerName = dbSyncServerName;
            SyncDatabaseName = dbSyncInstanceName;
            SyncUserName = dbSyncUserName;
            SyncPassword = dbSyncPassword;

            //Connect to sync SQL Database
            string syncConnStr = "Server=" + SyncServerName + "; Database=" + SyncDatabaseName + "; User Id=" + SyncUserName + "; Password=" + SyncPassword + ";";
            SyncDbConnection = new SqlConnection(syncConnStr);
            OpenDatabase(SyncDbConnection);
        }


        /// <summary>
        /// OpenDatabase method.
        /// </summary>
        /// <remarks>Method to open a database connection.</remarks>
        /// <param name="dbConnection">SQL database connection object reference.</param>
        private void OpenDatabase(SqlConnection dbConnection)
		{
			try
			{
				dbConnection.Open();
				
				if(dbConnection.State == ConnectionState.Open)
				{
                    DbConnected = true;
				}
			}
			catch(InvalidOperationException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("InvalidOperationException: " + e.Message);
			}
			catch(SqlException e)
			{
				WinEventLog winel = new WinEventLog();
				winel.WriteEvent("SqlException: " + e.Message);
			}
			catch(ArgumentException e)
			{
				WinEventLog winel	 = new WinEventLog();
				winel.WriteEvent("ArgumentException: " + e.Message);
			}
        }


        public void Sync(SyncDirection syncDirection)
        {
            MRDbSync.CProvisionSync ps = new MRDbSync.CProvisionSync(DbConnection, SyncDbConnection);

            switch(syncDirection)
            {
                case SyncDirection.Download:
                    ps.Sync(MRDbSync.CProvisionSync.MRDbSyncDirection.Download);
                    return;

                case SyncDirection.DownloadAndUpload:
                    ps.Sync(MRDbSync.CProvisionSync.MRDbSyncDirection.DownloadAndUpload);
                    return;

                case SyncDirection.Upload:
                    ps.Sync(MRDbSync.CProvisionSync.MRDbSyncDirection.Upload);
                    return;

                case SyncDirection.UploadAndDownload:
                    ps.Sync(MRDbSync.CProvisionSync.MRDbSyncDirection.UploadAndDownload);
                    return;

                default:
                    return;
            }
        }


		//Master SQL database properties
		public string ConnectionString { get; set; }
        public SqlConnection DbConnection { get; set; }
		public string ServerName { get; set; }
		public string DatabaseName { get; set; }
		public string UserName { get; set; }
		private  string Password { get; set; }
		public bool DbConnected { get; set; }

        //Sync SQL database properties
        public string SyncConnectionString { get; set; }
        public SqlConnection SyncDbConnection { get; set; }
        public string SyncServerName { get; set; }
        public string SyncDatabaseName { get; set; }
        public string SyncUserName { get; set; }
        private string SyncPassword { get; set; }
        public bool SyncDbConnected { get; set; }

        private RedundantNode ThisNode { get; set; }
    }
}
