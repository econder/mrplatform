using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("5B6452DC-75CB-4BF1-A993-DC47AA251DFC"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuEvents))]
    public class Menu : IMenu
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;

        public enum ItemMoveDirection
        {
            Up = 0,
            Down
        }



        public Menu()
        {

        }
        

        public Menu(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;
        }


        /// <summary>
        /// DbConnection Property
        /// </summary>
        public MRDbConnection DbConnection
        {
            get {
                return _dbConnection;
            }
            set {
                _dbConnection = value;
            }
        }


        [ComVisible(false)]
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


        [ComVisible(false)]
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
                        
                        sqlCmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)", ex.Message);
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

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    return 0;
                }
                catch(COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)", ex.Message);
                    return -1;
                }
            }
        }

        private string GetMoveNavigationItemQuery()
        {
            string sQuery = "EXEC [dbo].[mrspMoveItem] ?, ?";
            return sQuery;
        }


        public int AddNavigationItem(string screenName, string titleTop, string titleBottom)
        {
            if (screenName == null) { throw new ArgumentNullException("screenName", "screenName must not be null or empty."); }
            if (screenName == "") { throw new ArgumentNullException("screenName", "screenName must not be null or empty."); }
            if (screenName.Length > 50 ) { throw new ArgumentOutOfRangeException("screenName", "screenName must be 50 characters or less."); }
            if (titleTop.Length > 50) { throw new ArgumentOutOfRangeException("titleTop", "titleTop must be 50 characters or less."); }
            if (titleBottom.Length > 50) { throw new ArgumentOutOfRangeException("titleBottom", "titleBottom must be 50 characters or less."); }

            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetAddNavigationItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@screenName", screenName);
                    sqlCmd.Parameters.AddWithValue("@titleTop", titleTop);
                    sqlCmd.Parameters.AddWithValue("@titleBottom", titleBottom);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom)", ex.Message);
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
                dbCmd.CommandText = GetAddNavigationItemQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;
                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("screenName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, screenName);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("titleTop", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, titleTop);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("titleBottom", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, titleBottom);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom)", ex.Message);
                    return -1;
                }
            }
        }

        private string GetAddNavigationItemQuery()
        {
            string sQuery = "INSERT INTO NavMenu(screenName, titleTop, titleBottom, orderMenu)" +
                            " VALUES(?, ?, ?, (SELECT MAX(orderMenu) + 1 FROM NavMenu))";
            return sQuery;
        }


        public int DeleteNavigationItem(string screenName)
        {
            if (screenName == null) { throw new ArgumentNullException("screenName", "screenName must not be null or empty."); }
            if (screenName == "") { throw new ArgumentNullException("screenName", "screenName must not be null or empty."); }
            if (screenName.Length > 50) { throw new ArgumentOutOfRangeException("screenName", "screenName must be 50 characters or less."); }

            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetDeleteNavigationItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@screenName", screenName);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationItem(string screenName)", ex.Message);
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
                dbCmd.CommandText = GetDeleteNavigationItemQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;
                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("screenName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, screenName);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationItem(string screenName)", ex.Message);
                    return -1;
                }
            }
        }

        private string GetDeleteNavigationItemQuery()
        {
            string sQuery = "DELETE FROM NavMenu WHERE screenName = ?";
            return sQuery;
        }
    }
}