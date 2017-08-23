using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("CF3DCA39-C410-4A1F-8CD7-F3565F42BC2D")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuFavorite
    {
        MenuItems MenuItemsCollection { get; }
        int MoveFavoriteItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddFavoriteItem(string userName, int parentMenuId = 0);
        int DeleteFavoriteItem(int menuItemId);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
        MenuItem GetPreviousParentMenuItem(int currentParentMenuId);
    }

    [ComVisible(true)]
    [Guid("416FAA99-48E8-435A-AE8B-C9A28ACAA05D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuFavoriteEvents
    {

    }
}
