using System;
using System.Collections;
using System.Data;


namespace MRPlatform.InTouch
{
    interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
        Array Windows(string windowIndexFileName);
        int UpdateWindows(DataSet windows);
    }
}
