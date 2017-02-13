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
        public LogicalDisk()
        {
                
        }


        [ComVisible(true)]
        public string Caption { get; set; }

        [ComVisible(true)]
        public string Description { get; set; }

        [ComVisible(true)]
        public string DeviceId { get; set; }

        [ComVisible(true)]
        public string ErrorDescription { get; set; }

        [ComVisible(true)]
        public double FreeSpace { get; set; }

        [ComVisible(true)]
        public DateTime InstallDate { get; set; }

        [ComVisible(true)]
        public int LastErrorCode { get; set; }

        [ComVisible(true)]
        public string Name { get; set; }

        [ComVisible(true)]
        public double Size { get; set; }

        [ComVisible(true)]
        public string Status { get; set; }

        [ComVisible(true)]
        public string SystemName { get; set; }
    }
}
