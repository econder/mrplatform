using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("F267407B-66A1-446F-BEA6-C469B1781E6B"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItem
    {
        int MenuId { get; set; }
        string ScreenName { get; set; }
        string TitleTop { get; set; }
        string TitleBottom { get; set; }
        int MenuOrder { get; set; }
    }

    [ComVisible(true),
    Guid("F19DD8E0-A6C2-4E90-B5E1-B68F12D8B062"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemEvents
    {

    }
}