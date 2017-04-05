using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatformVBA.WMI
{
    [Guid("9863ED89-EC9C-4327-BF46-7C62FB023234")]
    public interface IOperatingSystem
    {
        string LastBootUpTime { get; }
        double FreePhysicalMemory { get; }
        double TotalVisibleMemorySize { get; }
    }

    [Guid("53A9181C-77A6-44C1-84FF-BE30F7DC2C74")]
    public interface IOperatingSystemEvents
    {

    }
}
