using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatformVBA.Wonderware.AlarmEvent
{
    /// <summary>
    /// MRAlarmEventLog interface.
    /// </summary>
    [Guid("46FEC693-32C7-4DAA-8AED-27FAD07B8D35")]
    public interface IAlarmEventLog
    {
        DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate);
        DataSet GetTopAlarmOccurrences(int topCount, string startDate);
        DataSet GetTopAlarmOccurrences(int topCount, DateTime startDate, DateTime endDate);
        DataSet GetTopAlarmOccurrences(int topCount, string startDate, string endDate);
        DataSet GetTopAlarmOccurrences(int topCount, DateTime endDate, int numDays);
        DataSet GetTopAlarmOccurrences(int topCount, string endDate, int numDays);

        DataSet GetTopEventOccurrences(int topCount, DateTime startDate);
        DataSet GetTopEventOccurrences(int topCount, string startDate);
        DataSet GetTopEventOccurrences(int topCount, DateTime startDate, DateTime endDate);
        DataSet GetTopEventOccurrences(int topCount, string startDate, string endDate);
        DataSet GetTopEventOccurrences(int topCount, DateTime endDate, int numDays);
        DataSet GetTopEventOccurrences(int topCount, string endDate, int numDays);

        DataSet GetUserHistory(string userName);
        DataSet GetUserHistory(string userName, int topCount);
        DataSet GetUserHistory(string userName, int topCount, DateTime startDate);
        DataSet GetUserHistory(string userName, int topCount, string startDate);
        DataSet GetUserHistory(string userName, int topCount, DateTime startDate, DateTime endDate);
        DataSet GetUserHistory(string userName, int topCount, string startDate, string endDate);
        DataSet GetUserHistory(string userName, DateTime startDate);
        DataSet GetUserHistory(string userName, string startDate);
        DataSet GetUserHistory(string userName, DateTime startDate, DateTime endDate);
        DataSet GetUserHistory(string userName, string startDate, string endDate);
    }


    [Guid("826D61B7-BF8C-4615-9306-92C453EBEE48")]
    public interface IAlarmEventLogEvents
    {

    }
}
