using System;
using System.Data;
using System.Runtime.InteropServices;

namespace MRPlatformVBA.HMI
{
    [Guid("CE3F79C5-95EF-4D5B-BCA5-845CAF74E5A5")]
    public interface IMenu
    {
        DataSet GetNavigationItems(int pageNumber, int resultsPerPage);
        
    }

    [Guid("9A34793F-7A5A-4151-A0C6-0179A715E434")]
    public interface IMenuEvents
    {

    }
}
