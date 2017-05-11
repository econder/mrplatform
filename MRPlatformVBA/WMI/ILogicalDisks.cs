using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatformVBA.WMI
{
    [Guid("E8176784-D0AF-46E0-AB08-E43105CAF517")]
    public interface ILogicalDisks : IEnumerable
    {
        //LogicalDisk this[int index] { get; }
        LogicalDisk Disk(string driveLetter);
    }


    [Guid("88979E17-FA67-4B56-B5B1-7389563875F7")]
    public interface ILogicalDisksEvents
    {

    }
}
