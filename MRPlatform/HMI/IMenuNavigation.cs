using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("00AD8E73-166C-4513-9046-3564F439516F")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuNavigation
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(MenuNavigation.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0);
        int DeleteNavigationItem(int menuItemId, MenuNavigation.ItemOrphanAction itemOrphanAction = MenuNavigation.ItemOrphanAction.SetToRoot);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        MenuNavigation.ItemSortOrder ResultsSortOrder { get; set; }
        MenuItem GetPreviousParentMenuItem(int currentParentMenuId);
    }

    [ComVisible(true)]
    [Guid("CF6949A3-7418-4370-847F-7057E16A3CCE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuNavigationEvents
    {

    }
}
