using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("50659C53-7D02-4A99-B09F-26B01E260E07")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuNavigation
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(MenuNavigation.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0, string alarmGroup = "");
        int DeleteNavigationItem(int menuItemId, Menu.ItemOrphanAction itemOrphanAction = Menu.ItemOrphanAction.SetToRoot);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
        MenuItem GetPreviousParentMenuItem(int currentParentMenuId);
        MenuItem GetNavigationHistoryLastItem(string userName, int currentNavMenuId);
        MenuItem GetNavigationHistoryNextItem(string userName, int currentNavMenuId);
        int AddNavigationHistory(string userName, int currentNavMenuId);
        int DeleteNavigationForwardHistory(string userName, int currentNavMenuId);
        int DeleteNavigationHistory(string userName);
    }

    [ComVisible(true)]
    [Guid("56FA663E-812A-4FD2-A3A8-F203A5126500")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuNavigationEvents
    {

    }
}
