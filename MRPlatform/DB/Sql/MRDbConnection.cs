using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;


namespace MRPlatform.DB.Sql
{
    [ComVisible(true)]
    [Guid("F59194D9-0FF0-4244-994A-E0887AF2B8A1"),
    ClassInterface(ClassInterfaceType.None),
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
        public bool UseADODB { get; set; }

        public enum State
        {
            Error = -1,
            Broken = 0,
            Closed = 1,
            Open = 2,
            Connecting = 4,
            Executing = 8,
            Fetching = 16
        }


        public MRDbConnection()
        {
            
        }

        public MRDbConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false)
        {
            OpenConnection(provider, serverName, databaseName, userName, password, useADODB);
        }


        public State ConnectionState
        {
            get
            {
                if(!UseADODB)
                {
                    switch (Connection.State)
                    {
                        case System.Data.ConnectionState.Broken:
                            return State.Broken;

                        case System.Data.ConnectionState.Closed:
                            return State.Closed;

                        case System.Data.ConnectionState.Connecting:
                            return State.Connecting;

                        case System.Data.ConnectionState.Executing:
                            return State.Executing;

                        case System.Data.ConnectionState.Fetching:
                            return State.Fetching;

                        case System.Data.ConnectionState.Open:
                            return State.Open;

                        default:
                            return State.Error;
                    }
                }
                else
                {
                    switch(ADODBConnection.State)
                    {
                        case 0: // adStateClosed
                            return State.Closed;

                        case 1: // adStateOpen
                            return State.Open;

                        case 2: // adStateConnecting
                            return State.Connecting;

                        case 4: // adStateExecuting
                            return State.Executing;

                        case 8: // adStateFetching
                            return State.Fetching;

                        default:
                            return State.Error;
                    }
                }
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
            UseADODB = useADODB;

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
            UseADODB = useADODB;

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
