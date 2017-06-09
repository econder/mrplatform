using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("ACA77FAE-1E8F-427A-91B4-69775F138D77"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuItem))]
    public class MenuItem : IMenuItem
    {
        public MenuItem()
        {

        }

        public MenuItem(int id, string screenName, string titleTop, string titleBottom, int menuOrder)
        {
            Id = id;
            ScreenName = screenName;
            TitleTop = titleTop;
            TitleBottom = titleBottom;
            MenuOrder = menuOrder;
        }


        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string TitleTop { get; set; }
        public string TitleBottom { get; set; }
        public int MenuOrder { get; set; }
    }
}
