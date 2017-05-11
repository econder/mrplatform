using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;


namespace MRPlatformVBA.WMI
{
    [ComVisible(true)]
    [Guid("EEF6907D-F499-4D36-81AD-E5F652BC1304"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IOperatingSystemEvents))]
    public class OperatingSystem : IOperatingSystem
    {
        public string LastBootUpTime { get; set; }
        public double FreePhysicalMemory { get; set; }
        public double TotalVisibleMemorySize { get; set; }


        public OperatingSystem()
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            ManagementObjectCollection objCol = searcher.Get();

            foreach (ManagementObject obj in objCol)
            {
                LastBootUpTime = Convert.ToString(obj["LastBootUpTime"]);

                // Memory
                FreePhysicalMemory = Convert.ToDouble(obj["FreePhysicalMemory"]) / (1024 ^ 3);
                TotalVisibleMemorySize = Convert.ToDouble(obj["TotalVisibleMemorySize"]) / (1024 ^ 3);
            }

            // Cleanup resources
            objCol.Dispose();
        }
    }
}
