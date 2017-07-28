using System;
using System.Runtime.InteropServices;

namespace MRPlatform.DB.Sql
{
    [ComVisible(true),
    Guid("46677521-57B5-45D8-89ED-7BB58D4222C1"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMRDbConnection
    {
        string Provider { get; set; }
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool UseADODB { get; set; }
        void OpenConnection();
        void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false);
    }

    [ComVisible(true),
    Guid("007AFB6A-2137-413D-B4CE-292B792541D1"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMRDbConnectionEvents
    {

    }
}