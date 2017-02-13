﻿/***************************************************************************************************
 * Class: 			MRConnection.cs
 * Created By:		Eric Conder
 * Created On:		2014-01-06
 * 
 * Changes:
 * 
 * 2014-03-06	Recreated the MRConnection under new namespace MRPlatform2014.Data.Sql.
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


namespace MRPlatform.DB.Sql
{
    public class MRDbConnection
	{
        private ErrorLog _errorLog;

        public MRDbConnection(string serverName, string databaseName, string userName, string password)
        {
            _errorLog = new ErrorLog();

            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = UserName;
            Password = password;

            ConnectionString = "Server=" + serverName + "; Database=" + databaseName + "; User Id=" + userName + "; Password=" + password + ";";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }


        //Master SQL database properties
        public string ConnectionString { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        private string Password { get; set; }
    }
}