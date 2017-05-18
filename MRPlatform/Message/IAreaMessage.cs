using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
    [ComVisible(true),
    Guid("A2FCAB54-9614-431A-B003-CEE659E0E35F"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAreaMessage
    {
        int Send(string sender, string area, string message, int priority = 2);
        MRDbConnection DbConnection { get; set; }
    }


    [ComVisible(true),
    Guid("39F74C06-CDA0-4677-9CD2-25D39BF540D5"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAreaMessageEvents
    {

    }
}
