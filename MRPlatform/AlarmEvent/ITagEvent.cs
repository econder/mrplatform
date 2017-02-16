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
        void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew);
        DataSet GetHistory(string tagName, int nRecordCount);
        DataSet GetHistory(DateTime startDate);
        DataSet GetHistory(DateTime startDateTime, DateTime endDateTime);
    }


    [Guid("ED2D534D-58BC-487F-893B-9AE3AE83AAE7")]
    public interface ITagEventEvents
    {

    }
}
