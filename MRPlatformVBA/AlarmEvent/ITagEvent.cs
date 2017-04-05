using System;
using System.Data;
using System.Runtime.InteropServices;
using ADODB;


namespace MRPlatformVBA.AlarmEvent
{
    /// <summary>
    /// IMRTagEvent interface.
    /// </summary>
    [Guid("96327C83-E426-4613-90C7-2AF70BFDADB8")]
    public interface ITagEvent
    {
        void LogEvent(string userName, string nodeName, string tagName, float tagValueOrig, float tagValueNew);
        ADODB.Recordset GetHistory(string tagName, int nRecordCount);
        ADODB.Recordset GetHistory(DateTime startDate);
        ADODB.Recordset GetHistory(DateTime startDateTime, DateTime endDateTime);
    }


    [Guid("3AD97519-4B17-4BA5-A25E-30B7DCF2E42A")]
    public interface ITagEventEvents
    {

    }
}
