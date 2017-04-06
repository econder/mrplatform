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

        public enum ItemMoveDirection
        {
            Up = 0,
            Down
        }
   
        public Menu(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        public DataSet GetNavigationItemsDataSet(int pageNumber, int resultsPerPage, bool sortAscending = true)
        {
            if (pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();
                
                OleDbCommand sqlCmd = new OleDbCommand(GetNavigationItemsQuery(sortAscending), (OleDbConnection)dbConnection);
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
        public Recordset GetNavigationItemsRecordset(int pageNumber, int resultsPerPage, bool sortAscending = true)
        {
            if(pageNumber < 1) { throw new ArgumentOutOfRangeException("pageNumber", (object)pageNumber, "Page number value must be greater than zero."); }
            if (resultsPerPage < 1) { throw new ArgumentOutOfRangeException("resultsPerPage", (object)resultsPerPage, "Results per page value must be greater than zero."); }

            Connection dbConnection = _dbConnection.ADODBConnection;
            dbConnection.Open();

            Command dbCmd = new Command();
            dbCmd.ActiveConnection = dbConnection;
            dbCmd.CommandText = GetNavigationItemsQuery(sortAscending);
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

        private string GetNavigationItemsQuery(bool sortAscending)
        {
            string sortOrder = null;
            if(sortAscending) { sortOrder = "ASC"; } else { sortOrder = "DESC"; }

            string sQuery = String.Format("SELECT screenName, titleTop, titleBottom, orderMenu" +
                            " FROM NavMenu ORDER BY orderMenu {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);

            return sQuery;
        }


        [ComVisible(true)]
        // Use mrspMoveItem SQL stored procedure
        public int MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        {
            if(currentOrderId <= 0) { throw new ArgumentOutOfRangeException("currentOrderId", (object)currentOrderId, "currentOrderId must be greater than or equal to zero."); }

            if(!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetMoveNavigationItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@orderId", currentOrderId);
                    sqlCmd.Parameters.AddWithValue("@direction", Convert.ToBoolean(direction));

                    try
                    {
                        return sqlCmd.ExecuteNonQuery();
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);
                        return -1;
                    }
                }
            }
            else
            {
                // Use ADODB Connection
                Connection dbConnection = _dbConnection.ADODBConnection;
                dbConnection.Open();

                Command dbCmd = new Command();
                dbCmd.ActiveConnection = dbConnection;
                dbCmd.CommandText = GetMoveNavigationItemQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;
                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("orderId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, currentOrderId);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("direction", DataTypeEnum.adBoolean, ParameterDirectionEnum.adParamInput, 1, Convert.ToBoolean(direction));
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                object recAffected = 0;
                rs = dbCmd.Execute(out recAffected);

                return Convert.ToInt32(recAffected);
            }
        }

        private string GetMoveNavigationItemQuery()
        {
            string sQuery = "EXEC [dbo].[mrspMoveItem] @currentOrderId = ?, @moveUp = ?";
            return sQuery;
        }
    }
}