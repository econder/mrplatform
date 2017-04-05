using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatform.AlarmEvent;


namespace MRPlatform.DB.Sql
{
    [Guid("D098F6B4-0FB6-4695-92FF-B78724BAAAE6")]
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

            ConnectionString = String.Format("Provider={0};Server={1};Database={2};Uid={3};Pwd={4};", provider, serverName, databaseName, userName, password);
        }

        public IDbConnection Connection
        {
            get
            {
                return new OleDbConnection(ConnectionString);
            }
        }
    }
}
