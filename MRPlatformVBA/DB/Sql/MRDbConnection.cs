using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatformVBA.AlarmEvent;


namespace MRPlatformVBA.DB.Sql
{
    [Guid("F6B9AA33-F828-4438-9EAE-AC1FF4A1ED3D")]
    public class MRDbConnection
    {
        private ErrorLog _errorLog;

        public string ConnectionString { get; set; }
        public string Provider { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        private string Password { get; set; }

        public MRDbConnection(string provider, string serverName, string databaseName, string userName, string password)
        {
            _errorLog = new ErrorLog();

            Provider = provider;
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = UserName;
            Password = password;

            ConnectionString = String.Format("Provider={0};Server={1};Database={2};Uid={3};Pwd={4};DataTypeCompatibility=80;", provider, serverName, databaseName, userName, password);
        }

        public ADODB.Connection Connection
        {
            get
            {
                ADODB.Connection conn = new Connection();
                conn.Open(ConnectionString);
                return conn;
            }
        }
    }
}
