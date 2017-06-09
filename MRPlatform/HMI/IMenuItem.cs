using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("2E0169C5-46DC-4016-8BAE-A3B326D7F3C1"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItem
    {
        int Id { get; set; }
        string ScreenName { get; set; }
        string TitleTop { get; set; }
        string TitleBottom { get; set; }
        int MenuOrder { get; set; }
    }

    [ComVisible(true),
    Guid("8A818445-F64D-4B67-BD03-B674E1C6345A"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemEvents
    {

    }
}