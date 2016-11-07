using System;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;


namespace MRPlatform.WMI
{   
    class Processor
    {
        public string Computer { get; set; }
        public int LoadPercentage { get; set; }

        public Processor()
        {
            Computer = ".";

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
