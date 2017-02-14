using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;


namespace MRPlatform.InTouch
{
    [ComVisible(true)]
    [Guid("7DE85542-10D9-4337-A2E7-EA5E72BA301A"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IWindowEvents))]
    class Window : IWindow
    {
        private const string INDEX_FILENAME = "ww_wdws.ndx";
        private ErrorLog _errorLog;

        public int WindowCount { get; }


        public Window()
        {
            _errorLog = new ErrorLog();
        }


        public DataSet Windows(string indexFileName)
        {
            DataSet ds = new DataSet();
            return ds;
        }


        private void ParseWindowsIndexFile(string applicationPath)
        {
            NameValueCollection arWindows = new NameValueCollection();
            string fileLine, windowIndex, windowName;

            try
            {
                StreamReader file = new StreamReader(string.Format("{0}\\{1}", applicationPath, INDEX_FILENAME));

                while((fileLine = file.ReadLine()) != null)
                {
                    windowIndex = fileLine.Substring(0, 6);
                    windowName = fileLine.Substring(6, fileLine.Length - 6);

                    arWindows.Add(windowIndex, windowName);
                }
            }
            catch(IOException ex)
            {
                _errorLog.LogMessage(this.GetType().Name, "ParseWindowsIndexFile(string applicationPath)", ex.Message);
            }
        }
    }
}
