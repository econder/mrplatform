using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatformVBA.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [Guid("F2847204-AC1B-42B0-9382-A8A80C13E6BA")]
    public interface IUserEvent
    {
        void LogEvent(string userName, string nodeName, string eventMessage, int eventType, string eventSource);
        DataSet GetHistory(int nRecordCount);
        DataSet GetHistory(DateTime startDate);
        DataSet GetHistory(DateTime startDateTime, DateTime endDateTime);
    }

    [Guid("E41D01D6-96CA-400E-8035-1D82D7CCA1D9")]
    public interface IUserEventEvents
    {

    }
}
