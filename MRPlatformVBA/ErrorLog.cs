using System;
using System.IO;

namespace MRPlatformVBA
{
    public class ErrorLog
    {
        public ErrorLog()
        {

        }

        public ErrorLog(string className, string methodName, string message)
        {
            LogMessage(className, methodName, message);
        }

        public void LogMessage(string className, string methodName, string message)
        {

            using (StreamWriter sw = File.AppendText("MRPlatform_ErrorLog.txt"))
            {
                sw.WriteLine(string.Format("{0:yyyy-MM-dd HH:mm:ss}   {1}::{2}->{3}", DateTime.Now, className, methodName, message));
            }
        }
    }
}
