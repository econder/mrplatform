using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.AlarmEvent
{
    /// <summary>
    /// MRAlarmEventLog interface.
    /// </summary>
    [Guid("C3CCC31F-E291-4422-BAD0-5A640672E85C")]
    public interface IMRAlarmEventLog
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
}
