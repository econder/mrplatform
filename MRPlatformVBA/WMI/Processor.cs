using System;
using System.Management;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Threading;


namespace MRPlatformVBA.WMI
{
    [ComVisible(true)]
    [Guid("D9C4BDC9-5792-4008-BE71-6822A5D116D5"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IProcessorEvents))]
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
