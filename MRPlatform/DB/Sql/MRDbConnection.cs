using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

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
        public bool UseADODB { get; set; }

        public MRDbConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false)
        {
            _errorLog = new ErrorLog();

            Provider = provider;
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
            UseADODB = useADODB;

            if(useADODB)
            {
                // ADODB database connection string
                ConnectionString = String.Format("Provider={0};Server={1};Database={2};Uid={3};Pwd={4};DataTypeCompatibility=80;", Provider, ServerName, DatabaseName, UserName, Password);
            }
            else
            {
                // SQL Native Client OLE database connection string
                ConnectionString = String.Format("Provider={0};Data Source={1};Initial Catalog={2};User ID={3};Password={4};", Provider, ServerName, DatabaseName, UserName, Password);
            }
        }

        // Used for SQL Native Client OLEDB connectivity from .NET-based clients
        // such as Wonderware InTouch
        public IDbConnection Connection
        {
            get
            {
                return new OleDbConnection(ConnectionString);
            }
        }

        // Used for ADODB connectivity from COM-based clients
        // such as FactoryTalk View SE and iFix Workspace
        public ADODB.Connection ADODBConnection
        {
            get
            {
                ADODB.Connection conn = new ADODB.Connection();
                conn.ConnectionString = ConnectionString;
                conn.CursorLocation = CursorLocationEnum.adUseClient;
                return conn;
            }
        }
    }
}
