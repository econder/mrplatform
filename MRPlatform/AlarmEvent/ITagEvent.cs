using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [ComVisible(true)]
    [Guid("7BB5187C-0964-4920-8CC5-612E136B36BA")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITagEvent
    {
        void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew);
        ADODB.Recordset GetHistoryRecordset(string tagName, int nRecordCount);
        ADODB.Recordset GetHistoryRecordset(DateTime startDate);
        ADODB.Recordset GetHistoryRecordset(DateTime startDateTime, DateTime endDateTime);
        MRDbConnection DbConnection { get; set; }
    }


    [Guid("ED2D534D-58BC-487F-893B-9AE3AE83AAE7")]
    public interface ITagEventEvents
    {

    }
}
