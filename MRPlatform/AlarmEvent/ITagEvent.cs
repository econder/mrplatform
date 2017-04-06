using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [Guid("7BB5187C-0964-4920-8CC5-612E136B36BA")]
    public interface ITagEvent
    {
        [ComVisible(true)]
        void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew);
        [ComVisible(true)]
        ADODB.Recordset GetHistoryRecordset(string tagName, int nRecordCount);
        [ComVisible(true)]
        ADODB.Recordset GetHistoryRecordset(DateTime startDate);
        [ComVisible(true)]
        ADODB.Recordset GetHistoryRecordset(DateTime startDateTime, DateTime endDateTime);
    }


    [Guid("ED2D534D-58BC-487F-893B-9AE3AE83AAE7")]
    public interface ITagEventEvents
    {

    }
}
