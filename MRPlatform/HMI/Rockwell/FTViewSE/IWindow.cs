using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI.Rockwell.FTViewSE
{
    [Guid("18741FF8-D07A-40D5-B855-72729174D6DB")]
    public interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
    }


    [Guid("F1721ED4-049F-416A-8A43-E8A4ED698BF4")]
    public interface IWindowEvents
    {

    }
}
