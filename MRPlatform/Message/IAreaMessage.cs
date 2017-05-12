using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.Message
{
    [Guid("A2FCAB54-9614-431A-B003-CEE659E0E35F")]
    public interface IAreaMessage
    {
        int Send(string sender, string area, string message, int priority = 2);
        MRDbConnection DbConnection { get; set; }
    }


    [Guid("39F74C06-CDA0-4677-9CD2-25D39BF540D5")]
    public interface IAreaMessageEvent
    {

    }
}
