using System;
using System.Runtime.InteropServices;


namespace MRPlatform.DB.Sql
{
    [ComVisible(true),
    Guid("32272D87-D33E-4A44-B1C7-B13CAFF772A2"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMRDbConnection
    {
        string Provider { get; set; }
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool UseADODB { get; set; }
        void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false);
    }

    [ComVisible(true),
    Guid("4281EA48-B5F6-456F-8535-C0B775A482CF"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMRDbConnectionEvents
    {

    }
}