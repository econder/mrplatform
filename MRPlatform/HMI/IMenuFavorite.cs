using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("E9C3F17E-B005-4127-957E-5356DC74C5D8")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuFavorite
    {
        MenuItems MenuItemsCollection { get; }
        MenuItems GetItems(string userName);
        int MoveFavoriteItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddFavoriteItem(string userName, int parentMenuId = 0);
        int DeleteFavoriteItem(int menuItemId);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        int ParentMenuId { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
        //MenuItem GetPreviousParentMenuItem(int currentParentMenuId); // Not used at this time
    }

    [ComVisible(true)]
    [Guid("E272993D-BBED-4BC4-B3AD-4B92EF067DD2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuFavoriteEvents
    {

    }
}
