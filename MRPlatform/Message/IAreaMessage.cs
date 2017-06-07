using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
    [ComVisible(true),
    Guid("CEB87A17-BC06-4C18-A8C0-5B53F94B4AFB"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAreaMessage
    {
        MRDbConnection DbConnection { get; set; }
        int Send(string sender, string area, string message, int priority = 2);
        Recordset GetMessagesRecordset(string area);
        Recordset GetMessagesRecordset(string area, int priority);
        Recordset GetMessagesRecordset(string area, int priority, double messageDate);
        Recordset GetMessagesRecordset(string area, int priority, double messageStartDate, double messageEndDate);
        Recordset GetUnreadMessagesRecordset(string userName, string area);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double messageDate);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double messageStartDate, double messageEndDate);
        int Count(string area);
        int Count(string area, int priority);
        int Count(string area, int priority, DateTime dtDate);
        int Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
        int UnreadCount(string userName, string area);
        int UnreadCount(string userName, string area, int priority);
        int UnreadCount(string userName, string area, int priority, DateTime dtDate);
        int UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
    }


    [ComVisible(true),
    Guid("0E0B1122-04BF-4240-862C-969D265654D5"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAreaMessageEvents
    {

    }
}
