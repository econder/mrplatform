using System;
using System.Data;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [Guid("DC3554E8-18C3-447F-AE21-0C4C8C480488")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IUserEvent
    {
        int LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource);
        Recordset GetHistoryRecordset(int pageNumber, int resultsPerPage, bool sortAscending = true);
        Recordset GetHistoryRecordset(double eventDate, int pageNumber, int resultsPerPage, bool sortAscending = true);
        Recordset GetHistoryRecordset(double eventStartDate, double eventEndDate, int pageNumber, int resultsPerPage, bool sortAscending = true);
        MRDbConnection DbConnection { get; set; }
    }

    [Guid("35835EC3-87A3-4A0D-A563-A0514D2BBE1F")]
    public interface IUserEventEvents
    {

    }
}
