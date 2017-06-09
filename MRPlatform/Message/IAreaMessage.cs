using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
    [Guid("A2FCAB54-9614-431A-B003-CEE659E0E35F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAreaMessage
    {
        int Send(string sender, string area, string message, int priority = 2);
        MRDbConnection DbConnection { get; set; }
        Recordset GetMessagesRecordset(string area);
        Recordset GetMessagesRecordset(string area, int priority);
        Recordset GetMessagesRecordset(string area, int priority, double dtDate);
        Recordset GetMessagesRecordset(string area, int priority, double dtStartDate, double dtEndDate);
        Recordset GetUnreadMessagesRecordset(string userName, string area);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double dtDate);
        Recordset GetUnreadMessagesRecordset(string userName, string area, int priority, double dtStartDate, double dtEndDate);
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
    Guid("39F74C06-CDA0-4677-9CD2-25D39BF540D5"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAreaMessageEvents
    {

    }
}
