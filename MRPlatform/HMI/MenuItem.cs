using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("2DE75518-8140-41C6-8A35-ED7AD4D5895A")]
    [ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItem))]
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {
            MenuId = 0;
            ScreenName = "";
            TitleTop = "";
            TitleBottom = "";
            MenuOrder = 0;
            ParentMenuId = 0;
            ChildCount = 0;
            AlarmGroup = "";
        }

        public MenuItem(int id, string screenName, string titleTop, string titleBottom, int menuOrder, int parentMenuId, int childCount = -1, string alarmGroup = "")
        {
            MenuId = id;
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
            ParentMenuId = parentMenuId;
            ChildCount = childCount;
            AlarmGroup = alarmGroup;
        }

        public int MenuId { get; set; }
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
        public int ParentMenuId { get; set; }
        public int ChildCount { get; set; }
        public string AlarmGroup { get; set; }
    }
}
