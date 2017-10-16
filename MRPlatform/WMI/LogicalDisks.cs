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
    ComSourceInterfaces(typeof(ILogicalDisks))]
    public class LogicalDisks : ILogicalDisks
    {
        private SortedList _disks;


        public LogicalDisks()
        {
            _disks = new SortedList();

            SelectQuery selectQuery = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            ManagementObjectCollection objCol = searcher.Get();
            int i = 0;

            foreach (ManagementObject obj in objCol)
            {
                LogicalDisk ld = new LogicalDisk()
                {
                    Caption = Convert.ToString(obj["Caption"]),
                    Description = Convert.ToString(obj["Description"]),
                    DeviceId = Convert.ToString(obj["DeviceId"]),
                    ErrorDescription = Convert.ToString(obj["ErrorDescription"]),
                    FreeSpace = Convert.ToDouble(obj["FreeSpace"]) / 1024 / 1024 / 1024,
                    InstallDate = Convert.ToDateTime(obj["InstallDate"]),
                    LastErrorCode = Convert.ToInt32(obj["LastErrorCode"]),
                    Name = Convert.ToString(obj["Name"]),
                    Size = Convert.ToDouble(obj["Size"]) / 1024 / 1024 / 1024,
                    Status = Convert.ToString(obj["Status"]),
                    SystemName = Convert.ToString(obj["SystemName"])
                };

                // Increment counter
                i++;

                // Add LogicalDisk to Disks Collection
                _disks.Add(i, ld);
            }

            // Cleanup resources
            objCol.Dispose();
        }

        public void Add(int index, LogicalDisk disk)
        {
            _disks.Add(index, disk);
        }

        public void Remove(int index)
        {
            _disks.Remove(index);
        }

        public int Count
        {
            get
            {
                return _disks.Count;
            }
        }

        public LogicalDisk this[int index]
        {
            get
            {
                return (LogicalDisk)_disks[index];
            }
            set
            {
                _disks[index] = value;
            }
        }


        [DispId(-4)]
        public IEnumerator GetEnumerator()
        {
            ICollection keys = _disks.Keys;
            return (IEnumerator)keys.GetEnumerator();
        }
    }
}
