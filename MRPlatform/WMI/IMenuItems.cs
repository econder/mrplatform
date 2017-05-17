using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MRPlatform.WMI
{
    [Guid("260AC2D9-1D20-4257-91BA-505B714B5E9E")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItems : IEnumerable
    {

    }

    [Guid("28798E0B-FDB0-486D-A525-0D7D1586B85D")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItemsEvent
    {

    }
}
