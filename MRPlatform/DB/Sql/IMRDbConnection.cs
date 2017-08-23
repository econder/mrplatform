using System;
using System.Runtime.InteropServices;

namespace MRPlatform.DB.Sql
{
    [ComVisible(true)]
    [Guid("9F038439-7685-4BD4-9279-911E8F1F98A9")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMRDbConnection
    {
        string Provider { get; set; }
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool UseADODB { get; }
        void OpenConnection();
        void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false);
    }

    [ComVisible(true)]
    [Guid("6192233C-34BF-448C-B621-1FFFB1AB32EE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMRDbConnectionEvents
    {

    }
}