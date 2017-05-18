using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MRPlatform.HMI
{
    [Guid("802E6E43-850F-402F-968F-E042F2D5BFB0")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItems : IEnumerator
    {
        void Add(MenuItem item);
        void Remove(int index);
        int Count { get; }
        MenuItem this[int index] { get; }
        IEnumerator GetEnumerator();
    }

    [Guid("DEC9F308-E586-4D40-97CA-84114760321E")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IMenuItemsEvent
    {

    }
}