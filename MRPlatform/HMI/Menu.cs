using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB;
using MRPlatform.DB.Sql;

namespace MRPlatform.HMI
{
    [Guid("A5BBE422-69A6-40BF-996F-9B79A07A6831")]
    public class Menu : IMenu
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;


        public Menu(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        public DataSet GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();
                
                OleDbCommand sqlCmd = new OleDbCommand(GetNavigationItemsQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@offset", (pageNumber - 1) * resultsPerPage);
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
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);
                    return ds;
                }
            }
        }

        [ComVisible(true)]
        public Recordset GetNavigationItemsRecordset(int pageNumber, int resultsPerPage)
        {
            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetNavigationItemsQuery();
            dbCmd.CommandType = CommandTypeEnum.adCmdText;
            Parameter dbParam = new Parameter();
            dbParam = dbCmd.CreateParameter("offset", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, (pageNumber - 1) * resultsPerPage);
            dbCmd.Parameters.Append(dbParam);
            dbParam = dbCmd.CreateParameter("rowCount", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 20, resultsPerPage);
            dbCmd.Parameters.Append(dbParam);

            Recordset rs = new Recordset();
            rs.CursorType = CursorTypeEnum.adOpenStatic;

            object recAffected = 0;
            rs = dbCmd.Execute(out recAffected);
            return rs;
        }

        private string GetNavigationItemsQuery()
        {
            string sQuery = "SELECT screenName, titleTop, titleBottom, orderMenu" +
                            " FROM NavMenu ORDER BY orderMenu" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY";

            return sQuery;
        }
    }
}