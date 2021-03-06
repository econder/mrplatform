using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

using MRPlatformVBA.DB;
using MRPlatformVBA.DB.Sql;

namespace MRPlatformVBA.HMI
{
    [ComVisible(true)]
    [Guid("FD4A0E0F-5766-44A0-9641-44FE480BA507"),
        ClassInterface(ClassInterfaceType.None),
        ComSourceInterfaces(typeof(IMenuEvents))]
    public class Menu : IMenu
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;


        public Menu(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        public DataSet GetNavigationItems(int pageNumber, int resultsPerPage)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = "SELECT screenName, titleTop, titleBottom, orderMenu" +
                                " FROM NavMenu ORDER BY orderMenu" +
                                " OFFSET ? ROWS" + 
                                " FETCH NEXT ? ROWS ONLY";

                OleDbCommand sqlCmd = new OleDbCommand(sQuery, (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@offset", (pageNumber * resultsPerPage) - 1);
                sqlCmd.Parameters.AddWithValue("@rowCount", resultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                try
                {
                    dbAdapt.Fill(ds);
                    return ds;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationItems(int pageNumber, int resultsPerPage)", ex.Message);
                    return ds;
                }
            }
        }
    }
}