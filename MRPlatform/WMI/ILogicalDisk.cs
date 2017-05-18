using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.WMI
{
    [ComVisible(true),
    Guid("A78CC312-25AC-4EEB-8E04-20F8C0E50245"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ILogicalDisk
    {
        string Caption { get; set; }
        string Description { get; set; }
        string DeviceId { get; set; }
        string ErrorDescription { get; set; }
        double FreeSpace { get; set; }
        DateTime InstallDate { get; set; }
        int LastErrorCode { get; set; }
        string Name { get; set; }
        double Size { get; set; }
        string Status { get; set; }
        string SystemName { get; set; }
    }


    [ComVisible(true),
    Guid("332A5B08-0A10-46AB-90E8-660BD4E24811"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ILogicalDiskEvents
    {

    }
}
