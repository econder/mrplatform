using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [Guid("C653721F-AFD0-434A-9F77-CBCB97D49D77")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItem
    {
        string ScreenName { get; set; }
        string TitleTop { get; set; }
        string TitleBottom { get; set; }
        int MenuOrder { get; set; }
    }

    [Guid("090943E8-7A3D-40E3-BE80-6CED0873D361")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItemEvents
    {

    }
}