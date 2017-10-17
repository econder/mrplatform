using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace MRPlatform.WMI
{
    [ComVisible(true)]
    [Guid("2E4F8422-6001-402F-AEB5-780971E2D2E3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILogicalDisks : IEnumerable
    {
        void Add(int index, LogicalDisk disk);
        void Remove(int index);
        int Count { get; }
        new IEnumerator GetEnumerator();
        LogicalDisk this[int index] { get; set; }
    }


    [ComVisible(true)]
    [Guid("A1AE76B8-CCF6-486E-B4A9-7BBC289F3C12")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ILogicalDisksEvents
    {

    }
}
