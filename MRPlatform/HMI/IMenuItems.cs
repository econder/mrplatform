using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("7CAA2663-CDDF-4412-AFC5-AFA5F0E76862")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItems : IEnumerable
    {
        void Add(int key, MenuItem item);
        void Remove(int key);
        int Count { get; }
        new IEnumerator GetEnumerator();
        object this[int key] { get; set; }
    }

    [ComVisible(true)]
    [Guid("5F74252F-DAE7-4001-B199-1111D41D1601")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMenuItemsEvents
    {

    }
}