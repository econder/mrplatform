using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("58EE114B-C517-4E3C-BA5D-4393EC07FEA8"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenu
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0);
        int DeleteNavigationItem(int menuItemId);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
        Menu.ItemSortOrder ResultsSortOrder { get; set; }
    }

    [ComVisible(true)]
    [Guid("898B0056-085E-4088-A0AA-8C38C6BB3A4E"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
