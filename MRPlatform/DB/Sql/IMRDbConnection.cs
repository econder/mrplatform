using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;


namespace MRPlatform.DB.Sql
{
    [Guid("32272D87-D33E-4A44-B1C7-B13CAFF772A2")]
    public interface IMRDbConnection
    {
        string Provider { get; set; }
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        bool UseADODB { get; set; }
        void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false);
    }


    [Guid("4281EA48-B5F6-456F-8535-C0B775A482CF")]
    public interface IMRDbConnectionEvents
    {

    }
}