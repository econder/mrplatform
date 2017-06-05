using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
<<<<<<< HEAD
    [Guid("A2FCAB54-9614-431A-B003-CEE659E0E35F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAreaMessage
    {
        void Send(string sender, string area, string message, int priority = 2);
        Recordset GetMessages(string area);
        Recordset GetMessages(string area, int priority);
        Recordset GetMessages(string area, int priority, DateTime dtDate);
        Recordset GetMessages(string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
        Recordset GetUnreadMessages(string userName, string area);
        Recordset GetUnreadMessages(string userName, string area, int priority);
        Recordset GetUnreadMessages(string userName, string area, int priority, DateTime dtDate);
        Recordset GetUnreadMessages(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
        int Count(string area);
        int Count(string area, int priority);
        int Count(string area, int priority, DateTime dtDate);
        int Count(string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
        int UnreadCount(string userName, string area);
        int UnreadCount(string userName, string area, int priority);
        int UnreadCount(string userName, string area, int priority, DateTime dtDate);
        int UnreadCount(string userName, string area, int priority, DateTime dtStartDate, DateTime dtEndDate);
=======
    [ComVisible(true),
    Guid("A2FCAB54-9614-431A-B003-CEE659E0E35F"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAreaMessage
    {
        int Send(string sender, string area, string message, int priority = 2);
>>>>>>> feature/ConvertToADO
        MRDbConnection DbConnection { get; set; }
    }


<<<<<<< HEAD
    [Guid("39F74C06-CDA0-4677-9CD2-25D39BF540D5")]
    public interface IAreaMessageEvent
=======
    [ComVisible(true),
    Guid("39F74C06-CDA0-4677-9CD2-25D39BF540D5"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAreaMessageEvents
>>>>>>> feature/ConvertToADO
    {

    }
}
