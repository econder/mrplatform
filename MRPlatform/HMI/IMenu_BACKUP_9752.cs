using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true),
<<<<<<< HEAD
    Guid("6E12A0E9-CA82-4D90-A9AE-B1286BF6AE78"),
=======
    Guid("F41622A8-3901-4CA4-ADBB-13758CE9E2FE"),
>>>>>>> hotfix/Issue-2
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
<<<<<<< HEAD
    Guid("90810E09-9970-4990-94F4-7CB9513C1D4D"),
=======
    Guid("EC30B1AB-B7CB-457A-B63C-DAAEF26D3E2B"),
>>>>>>> hotfix/Issue-2
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
