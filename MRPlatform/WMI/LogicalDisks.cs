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

        public object this[int index]
        {
            get
            {
                return _disks[index];
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
