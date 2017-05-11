using System;
using System.Runtime.InteropServices;

namespace MRPlatformVBA.HMI
{
    [ComVisible(true)]
    [Guid("3256CDEA-24D2-4B24-9161-58021495CE6C"),
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
