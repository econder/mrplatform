using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.WMI
{
    [ComVisible(true)]
    [Guid("877B18D5-8DA7-4933-8D8C-E4FDEECFE947"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(ILogicalDisk))]
    public class LogicalDisk
    {
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
