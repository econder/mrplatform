using System;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("FD0FD4F7-98F9-4802-A29C-D0931A603EB6"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IMenuNavItemEvents))]
    public class MenuNavigationItem
    {
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int Order { get; set; }
    }
}
