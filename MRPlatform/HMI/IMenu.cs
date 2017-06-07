using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("49BE52F5-1A3D-4119-8E02-7913F2BA35A3"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenu
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom);
        int DeleteNavigationItem(string screenName);
        MRDbConnection DbConnection { get; set; }
        int ResultsPageNumber { get; set; }
        int ResultsPerPage { get; set; }
    }

    [ComVisible(true),
    Guid("3620D6CE-C6EF-4AC9-ADDA-6AE85FA52B67"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
