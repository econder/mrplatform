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
        DataSet GetHistory(string tagName, int recordCount);
        DataSet GetHistory(DateTime startDate);
        DataSet GetHistory(DateTime startDate, DateTime endDate);
    }
}
