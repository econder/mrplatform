using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("A56C5B12-E1B3-4FA2-8E51-8E130D445EDD"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItem
    {
        int MenuId { get; set; }
        string ScreenName { get; set; }
        string TitleTop { get; set; }
        string TitleBottom { get; set; }
        int MenuOrder { get; set; }
        int ParentMenuId { get; set; }
    }

    [ComVisible(true),
    Guid("89BDEF66-FD91-434B-9AAD-7B628F2CA0B7"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemEvents
    {

    }
}