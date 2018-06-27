using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("9BE99A62-435A-49AA-AB3D-D96C0817FA0A")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuNavigation
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(MenuNavigation.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0, string alarmGroup = "", string screenTitle = "");
        int DeleteNavigationItem(int menuItemId, Menu.ItemOrphanAction itemOrphanAction = Menu.ItemOrphanAction.SetToRoot);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
        MenuItem GetPreviousParentMenuItem(int currentParentMenuId);
        MenuItem GetNavigationHistoryLastItem(string userName);
        MenuItem GetNavigationHistoryNextItem(string userName);
        int AddNavigationHistory(string userName, int currentNavMenuId);
        int DeleteNavigationForwardHistory(string userName);
        int DeleteNavigationBackHistory(string userName);
    }

    [ComVisible(true)]
    [Guid("AAAA15C8-E3AD-425C-9624-92E75433356C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuNavigationEvents
    {

    }
}
