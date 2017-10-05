using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("3E7AE062-DCE0-4BFA-A780-D034695E7BFF")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItem
    {
        int MenuId { get; set; }
        string ScreenName { get; set; }
        string TitleTop { get; set; }
        string TitleBottom { get; set; }
        int MenuOrder { get; set; }
        int ParentMenuId { get; set; }
        int ChildCount { get; set; }
        string AlarmGroup { get; set; }
    }

    [ComVisible(true)]
    [Guid("CF521F8D-2CEE-45A0-8BA6-3DCE2B90DCD2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemEvents
    {

    }
}