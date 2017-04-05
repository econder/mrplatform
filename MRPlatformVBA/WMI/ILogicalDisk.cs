using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatformVBA.WMI
{
    [Guid("D42C1077-752E-4823-AC59-AE92862562B2")]
    public interface ILogicalDisk
    {
        string Caption { get; set; }
        string Description { get; set; }
        string DeviceId { get; set; }
        string ErrorDescription { get; set; }
        double FreeSpace { get; set; }
        DateTime InstallDate { get; set; }
        int LastErrorCode { get; set; }
        string Name { get; set; }
        double Size { get; set; }
        string Status { get; set; }
        string SystemName { get; set; }
    }


    [Guid("ED0559E2-70E9-4A70-B1E7-7CFF4505CC7B")]
    public interface ILogicalDiskEvents
    {

    }
}
