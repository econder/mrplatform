using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Management.Instrumentation;


namespace MRPlatform.WMI
{
    [ComVisible(true)]
    [Guid("62E2A76E-0471-4FB3-8293-A2F50D4A83DA"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IOperatingSystem))]
    public class OperatingSystem : IOperatingSystem
    {
        public string Computer { get; set; }
        public string LastBootUpTime { get; set; }
        public double FreePhysicalMemory { get; set; }
        public double TotalVisibleMemorySize { get; set; }


        public OperatingSystem()
        {
            Computer = ".";

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
