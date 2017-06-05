using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("55159BB3-B19A-4CF4-820F-A110119AD97C"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItem))]
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(string screenName, string titleTop, string titleBottom, int menuOrder)
        {
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
        }


        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
    }
}
