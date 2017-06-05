using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRPlatform.WMI
{
    [ComVisible(true),
    Guid("D85F1176-E3AD-474B-AA55-D53D6CA7B6C1"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IProcessor
    {
        int LoadPercentage { get; }
    }


    [ComVisible(true),
    Guid("D1FA9E95-7CE1-4317-8636-698E07DDC2AB"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IProcessorEvents
    {

    }
}
