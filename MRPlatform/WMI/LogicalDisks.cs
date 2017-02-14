using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Threading;


namespace MRPlatform.WMI
{
    [ComVisible(true)]
    [Guid("455EE884-F3F1-46C1-B4E2-35BA2E31CE83"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(ILogicalDisksEvents))]
    public class LogicalDisks : IEnumerable
    {
        public List<LogicalDisk> Disks = new List<LogicalDisk>();


        public LogicalDisks()
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            ManagementObjectCollection objCol = searcher.Get();

            foreach (ManagementObject obj in objCol)
            {
                LogicalDisk ld = new LogicalDisk();
                ld.Caption = Convert.ToString(obj["Caption"]);
                ld.Description = Convert.ToString(obj["Description"]);
                ld.DeviceId = Convert.ToString(obj["DeviceId"]);
                ld.ErrorDescription = Convert.ToString(obj["ErrorDescription"]);
                ld.FreeSpace = Convert.ToDouble(obj["FreeSpace"]) / 1024 / 1024 / 1024;
                ld.InstallDate = Convert.ToDateTime(obj["InstallDate"]);
                ld.LastErrorCode = Convert.ToInt32(obj["LastErrorCode"]);
                ld.Name = Convert.ToString(obj["Name"]);
                ld.Size = Convert.ToDouble(obj["Size"]) / 1024 / 1024 / 1024;
                ld.Status = Convert.ToString(obj["Status"]);
                ld.SystemName = Convert.ToString(obj["SystemName"]);

                // Add LogicalDisk to Disks Collection
                Disks.Add(ld);
            }

            // Cleanup resources
            objCol.Dispose();
        }


        //IEnumerable require these methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }


        public LogicalDisks GetEnumerator()
        {
            return new LogicalDisks();
        }


        public LogicalDisk this[int index]
        {
            get { return Disks[index]; }
        }


        public LogicalDisk Disk(string driveLetter)
        {
            if (driveLetter == null)
                throw new ArgumentNullException(driveLetter, "Drive letter parameter cannot be null.");

            try
            {
                return Disks.Find(ld => ld.Name == string.Format("{0}:", driveLetter));
            }
            catch(ArgumentNullException)
            {
                LogicalDisk ld = new LogicalDisk();
                ld.Description = String.Format("Drive letter {0} not found.", driveLetter);
                return null;
            }
        }
    }
}
