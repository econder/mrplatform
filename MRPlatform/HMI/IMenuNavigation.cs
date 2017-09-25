using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("BF66D216-254E-41C2-8515-4A9986A88FA9")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuNavigation
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(MenuNavigation.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0);
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
    [Guid("B3C3D236-0641-4AAA-9002-8CE2E4D69B2E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuNavigationEvents
    {

    }
}
