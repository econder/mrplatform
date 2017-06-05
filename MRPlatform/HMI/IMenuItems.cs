using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true),
    Guid("76134D72-9EE6-43D4-B03D-2E5D0D9E1294"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItems : IEnumerable
    {
        void Add(int key, MenuItem item);
        void Remove(int key);
        int Count { get; }
        new IEnumerator GetEnumerator();
        object this[int key] { get; set; }
    }

    [ComVisible(true),
    Guid("DEC9F308-E586-4D40-97CA-84114760321E"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemsEvents
    {

    }
}