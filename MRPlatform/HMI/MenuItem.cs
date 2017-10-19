using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("E3763F98-4761-4BDE-8B35-121418279C8B")]
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
            ScreenTitle = "";
        }

        public MenuItem(int id, string screenName, string titleTop, string titleBottom, int menuOrder, int parentMenuId, int childCount = -1, string alarmGroup = "", string screenTitle = "")
        {
            MenuId = id;
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
            ParentMenuId = parentMenuId;
            ChildCount = childCount;
            AlarmGroup = alarmGroup;
            ScreenTitle = screenTitle;
        }

        public int MenuId { get; set; }
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
        public int ParentMenuId { get; set; }
        public int ChildCount { get; set; }
        public string AlarmGroup { get; set; }
        public string ScreenTitle { get; set; }
    }
}
