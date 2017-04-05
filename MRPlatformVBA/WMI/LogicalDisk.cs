using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatformVBA.WMI
{
    [ComVisible(true)]
    [Guid("0B32AC98-9468-4B57-AD94-B0E148B4852C"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(ILogicalDiskEvents))]
    public class LogicalDisk : ILogicalDisk
    {
        public LogicalDisk()
        {
            
        }

        public string Caption { get; set; }

        public string Description { get; set; }

        public string DeviceId { get; set; }

        public string ErrorDescription { get; set; }

        public double FreeSpace { get; set; }

        public DateTime InstallDate { get; set; }

        public int LastErrorCode { get; set; }

        public string Name { get; set; }

        public double Size { get; set; }

        public string Status { get; set; }

        public string SystemName { get; set; }
    }
}
