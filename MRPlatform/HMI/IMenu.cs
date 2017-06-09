using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("4BF35626-DD4F-404A-A0AD-A2C47C0C5EC6"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenu
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom);
        int DeleteNavigationItem(int menuItemId);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
    }

    [ComVisible(true),
    Guid("1543CDFE-250B-43DE-9BA4-7C90B2A8F909"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
