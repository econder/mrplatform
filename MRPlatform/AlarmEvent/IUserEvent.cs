using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [Guid("DC3554E8-18C3-447F-AE21-0C4C8C480488")]
    public interface IUserEvent
    {
        void LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource);
        DataSet GetHistory(int nRecordCount);
        DataSet GetHistory(DateTime startDate);
        DataSet GetHistory(DateTime startDateTime, DateTime endDateTime);
    }

    [Guid("35835EC3-87A3-4A0D-A563-A0514D2BBE1F")]
    public interface IUserEventEvents
    {

    }
}
