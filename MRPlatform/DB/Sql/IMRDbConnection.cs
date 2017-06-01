using System;
using System.Runtime.InteropServices;


namespace MRPlatform.DB.Sql
{
    [ComVisible(true),
    Guid("78480383-748D-4D56-A0C6-036BD66D0F68"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMRDbConnection
    {
        string Provider { get; set; }
        string ServerName { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool UseADODB { get; set; }
        MRDbConnection.State ConnectionState { get; }
        void OpenConnection();
        void OpenConnection(string provider, string serverName, string databaseName, string userName, string password, bool useADODB = false);
    }

    [ComVisible(true),
    Guid("0E154126-60F9-47F3-BE12-AEAA71E4634D"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMRDbConnectionEvents
    {

    }
}