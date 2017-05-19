using System;
using System.Runtime.InteropServices;


namespace MRPlatform.DB.Sql
{
    [ComVisible(true),
    Guid("56F7A25D-6F89-46F6-8353-134CA3B22D0C"),
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
    Guid("E0CED698-AAFC-49F1-BE08-5BAFD1FE3F3A"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMRDbConnectionEvents
    {

    }
}