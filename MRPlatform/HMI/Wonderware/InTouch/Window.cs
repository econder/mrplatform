using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI.Rockwell.FTViewSE
{
    [ComVisible(true)]
    [Guid("20927EEC-4F15-491C-AF96-8F8766D27BBC")]
    class Window : IWindow
    {
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
            
        }
    }
}
