using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.InTouch
{
    class Window : IWindow
    {
        // Class Properties
        public int WindowCount { get; }


        public DataSet Windows(string indexFileName)
        {


            DataSet ds = new DataSet();
            return ds;
        }


        private void ParseWindowsIndexFile(string indexFileName)
        {
            NameValueCollection arWindows = new NameValueCollection();
            string fileLine, windowIndex, windowName;

            try
            {
                StreamReader file = new StreamReader(indexFileName);

                while((fileLine = file.ReadLine()) != null)
                {
                    windowIndex = fileLine.Substring(0, 6);
                    windowName = fileLine.Substring(6, fileLine.Length - 6);

                    arWindows.Add(windowIndex, windowName);
                }
            }
            catch(OutOfMemoryException ex)
            {
                ///TODO: Handle error
            }
            catch(IOException ex)
            {
                ///TODO: Handle error
            }
        }
    }
}
