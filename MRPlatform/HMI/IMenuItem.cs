using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("228B643A-5806-452E-9739-C475164A6587")]
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
        string ScreenTitle { get; set; }
    }

    [ComVisible(true)]
    [Guid("FF61BF59-1EB4-4C23-A10C-2782EC333919")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemEvents
    {

    }
}