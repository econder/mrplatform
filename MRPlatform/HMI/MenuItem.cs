using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("4B8040F6-C00E-4FED-979C-C21134AA2B66"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItem))]
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(int id, string screenName, string titleTop, string titleBottom, int menuOrder, int parentMenuId, int childCount = -1)
        {
            MenuId = id;
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
            ParentMenuId = parentMenuId;
            ChildCount = childCount;
        }

        public int MenuId { get; set; }
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
        public int ParentMenuId { get; set; }
        public int ChildCount { get; set; }
    }
}
