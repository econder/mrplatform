using System;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;


namespace MRPlatform.HMI
{
    [Guid("A3313A06-4401-4A28-B0DE-421ED3B482CC")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenu
    {
        Recordset GetNavigationItemsRecordset(int pageNumber, int resultsPerPage, bool sortAscending = true);
        int MoveNavigationItem(Menu.ItemMoveDirection direction, int currentOrderId);
        int AddNavigationItem(string screenName, string titleTop, string titleBottom);
        int DeleteNavigationItem(string screenName);
        MRDbConnection DbConnection { get; set; }
    }

    [Guid("41DFA2B1-0812-4717-AA62-D6146042312E")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuEvents
    {

    }
}
