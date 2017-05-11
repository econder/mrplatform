using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatformVBA.WMI
{
    [Guid("16D1DACC-3957-4FA6-B556-E90B57D122CF")]
    public interface IProcessor
    {
        int LoadPercentage { get; }
    }


    [Guid("D46C9159-EC63-4446-834F-490C8A750378")]
    public interface IProcessorEvents
    {

    }
}
