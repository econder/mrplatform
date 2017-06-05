using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.WMI
{
    [ComVisible(true),
    Guid("AF44AE77-9547-4135-92E8-0C090E83D2AD"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IOperatingSystem
    {
        string LastBootUpTime { get; }
        double FreePhysicalMemory { get; }
        double TotalVisibleMemorySize { get; }
    }

    [ComVisible(true),
    Guid("2BCA9644-9CC5-405F-8167-C94022FFBF2E"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IOperatingSystemEvents
    {

    }
}
