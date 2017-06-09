using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("F41622A8-3901-4CA4-ADBB-13758CE9E2FE"),
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
        bool SortAscending { get; set; }
    }

    [ComVisible(true),
    Guid("EC30B1AB-B7CB-457A-B63C-DAAEF26D3E2B"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
