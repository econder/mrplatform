using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("FF1EE640-A29C-41B0-B8DF-B8B2E4A81BB4"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItem))]
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(int id, string screenName, string titleTop, string titleBottom, int menuOrder)
        {
            MenuId = id;
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
        }

        public int MenuId { get; set; }
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
    }
}
