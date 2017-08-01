using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("621B47AD-3D1A-4B30-9776-4A36111B76E1")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuFavorite
    {
        MenuItems MenuItemsCollection { get; }
        int MoveFavoriteItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddFavoriteItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0);
        int DeleteFavoriteItem(int menuItemId, Menu.ItemOrphanAction itemOrphanAction = Menu.ItemOrphanAction.SetToRoot);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
        MenuItem GetPreviousParentMenuItem(int currentParentMenuId);
    }

    [ComVisible(true)]
    [Guid("E2CBDE66-C914-4B06-A559-C40231BA0425")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuFavoriteEvents
    {

    }
}
