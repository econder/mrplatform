using System;
using System.Data;
using System.Runtime.InteropServices;


namespace MRPlatformVBA.HMI.Wonderware.InTouch
{
    [Guid("64060B04-B976-480F-9FC6-97151432F629")]
    public interface IWindow
    {
        DataSet Windows(string windowIndexFileName);
    }


    [Guid("1A2EA57A-EBD3-4912-8E63-FDECD7146433")]
    public interface IWindowEvents
    {

    }
}
