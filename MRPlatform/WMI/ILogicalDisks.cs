using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace MRPlatform.WMI
{
    [ComVisible(true),
    Guid("E8176784-D0AF-46E0-AB08-E43105CAF517"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILogicalDisks : IEnumerable
    {
        //LogicalDisk this[int index] { get; }
        LogicalDisk Disk(string driveLetter);
    }


    [ComVisible(true),
    Guid("3A3AB048-9847-4545-A042-C35B92AF8E9E"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ILogicalDisksEvents
    {

    }
}
