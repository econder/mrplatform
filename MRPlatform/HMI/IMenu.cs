using System;
using System.Data;
using System.Runtime.InteropServices;
using ADODB;


namespace MRPlatform.HMI
{
    [Guid("C0CB67B1-B4B6-4140-BE7B-03CD12850A5D")]
    public interface IMenu
    {
         Recordset GetNavigationItemsRecordset(int pageNumber, int resultsPerPage);
        
    }

    [Guid("FA730550-AC4D-4BA5-A442-4387CA07601C")]
    public interface IMenuEvents
    {

    }
}
