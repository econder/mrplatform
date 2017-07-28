using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI.Rockwell.FTViewSE
{
    [ComVisible(true)]
    [Guid("18741FF8-D07A-40D5-B855-72729174D6DB")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
    }


    [ComVisible(true)]
    [Guid("F1721ED4-049F-416A-8A43-E8A4ED698BF4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IWindowEvents
    {

    }
}
