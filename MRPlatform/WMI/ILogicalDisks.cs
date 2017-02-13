using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.WMI
{
    [Guid("E8176784-D0AF-46E0-AB08-E43105CAF517")]
    public interface ILogicalDisks : IEnumerable<ILogicalDisk>
    {
        LogicalDisk this[int index] { get; }
        LogicalDisk Disk(string driveLetter);
        new IEnumerator<LogicalDisk> GetEnumerator();
    }
}
