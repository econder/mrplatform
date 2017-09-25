using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("6C5CE3D8-7076-4345-8F31-CDBEB64FBE40")]
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
    }

    [ComVisible(true)]
    [Guid("867AAB40-453A-4148-9708-BFF410BF7756")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuNavigationEvents
    {

    }
}
