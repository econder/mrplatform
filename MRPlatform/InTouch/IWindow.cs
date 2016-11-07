using System;
using System.Collections.Specialized;
using System.Data;


namespace MRPlatform.InTouch
{
    interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
        //int UpdateWindows(DataSet windows);
    }
}
