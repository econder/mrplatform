using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;


namespace MRPlatform.DB.Sql
{
    [ComVisible(true)]
    [Guid("4C6DE81C-2E45-40D8-9BDD-3A1F872FEFE8")]
    [ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMRDbConnection))]
    public class MRDbConnection : IMRDbConnection
	{
        private ErrorLog _errorLog;
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
        public string ServerName { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        // No longer an option. Leave as false.
        public bool UseADODB { get { return false; } }
        

        public MRDbConnection()
        {
            
        }

        public MRDbConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false)
        {
            OpenConnection(provider, serverName, databaseName, userName, password, useADODB);
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

        public void OpenConnection()
        {
            DoOpenConnection(Provider, ServerName, DatabaseName, UserName, Password, UseADODB);
        }

        public void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false)
        {
            Provider = provider;
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;

            DoOpenConnection(Provider, ServerName, DatabaseName, UserName, Password, UseADODB);
        }

        private void DoOpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false)
        {
            _errorLog = new ErrorLog();

            Provider = provider;
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;

            if (useADODB)
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
    }
}
