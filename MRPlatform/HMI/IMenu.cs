using System;
using System.Runtime.InteropServices;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("DF942625-5F43-4356-83B7-52A841301E55"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenu
    {
        MenuItems MenuItemsCollection { get; }
        int MoveNavigationItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom);
        int DeleteNavigationItem(string screenName);
        MRDbConnection DbConnection { get; set; }
    }

    [ComVisible(true),
    Guid("F34D3F08-2781-4AEC-9237-AB5CF8355EBE"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuEvents
    {

    }
}
