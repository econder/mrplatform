using System;
using System.Management;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Threading;


namespace MRPlatform.WMI
{
    [ComVisible(true)]
    [Guid("D7413FD7-F779-487D-865A-F8C32EACFE4B"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IProcessor))]
    public class Processor : IProcessor
    {
        public int LoadPercentage { get; set; }

        public Processor()
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * FROM Win32_Processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            ManagementObjectCollection objCol = searcher.Get();

            int cpuCount = 0;
            int cpuCalcLoad = 0;
            int cpuLoadPer = 0;

            foreach(ManagementObject obj in objCol)
            {
                cpuCount++;
                cpuCalcLoad = Convert.ToInt32(obj["LoadPercentage"]);
                cpuLoadPer += cpuCalcLoad;
            }

            LoadPercentage = cpuLoadPer / cpuCount;

            // Cleanup resources
            objCol.Dispose();
        }
    }
}
