using System;
using System.Runtime.InteropServices;


namespace MRPlatform.HMI
{
    [ComVisible(false)]
    public class Menu
    {
        [Guid("DC716A2E-68F9-40F2-A861-AD61BB88E0B3")]
        public enum ItemMoveDirection
        {
            Up = 0,
            Down
        }

        [Guid("E79A1125-B245-424F-A45C-96E816C1E988")]
        public enum ItemSortOrder
        {
            Custom = 0,
            Ascending,
            Descending
        }

        [Guid("95601294-ADD8-44BC-BDB9-2B9F59156583")]
        public enum ItemOrphanAction
        {
            SetToRoot = 0,
            Delete
        }


        #region " Properties "

        public int ResultsPageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public ItemSortOrder ResultsSortOrder { get; set; }
        public int ParentMenuId { get; set; }

        #endregion
    }
}