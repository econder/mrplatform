﻿using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("CBC67601-61F1-422E-83D1-6A92C0BC01FA"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenu))]
    public class Menu : IMenu
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;
        private MenuItems _itemsCollection;

        public enum ItemMoveDirection
        {
            Up = 0,
            Down
        }

        public enum ItemSortOrder
        {
            Custom = 0,
            Ascending,
            Descending
        }


        public Menu()
        {
            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
        }
        

        public Menu(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;

            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
        }   


        #region " Properties "

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

        public int ResultsPageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public ItemSortOrder ResultsSortOrder { get; set; }
        public bool HierarchyView { get; set; }

        #endregion

        //[DispId(-4)]
        public MenuItems MenuItemsCollection
        {
            get
            {
                _itemsCollection = DoGetItems();
                return _itemsCollection;
            }
        }


        private MenuItems DoGetItems()
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetNavigationItemsQuery(ResultsSortOrder), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                MenuItems menuItems = new MenuItems();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();

                    if(ds.Tables.Count > 0)
                    {
                        int i = 0;
                        foreach(DataRow row in ds.Tables[0].Rows)
                        {
                            menuItems.Add(i, new MenuItem((int)row["id"],
                                                       row["screenName"].ToString(),
                                                       row["titleTop"].ToString(),
                                                       row["titleBottom"].ToString(),
                                                       (int)row["orderMenu"],
                                                       (int)row["parentMenuId"]));
                            i++;
                        }
                    }

                    return menuItems;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return menuItems;
                }
            }
        }


        [ComVisible(false)]
        private string GetNavigationItemsQuery(ItemSortOrder itemSortOrder, int parentMenuId = 0)
        {
            string sortOrder = null;
            string hierarchy = null;

            switch(itemSortOrder)
            {
                case ItemSortOrder.Custom:
                    sortOrder = "orderMenu ASC";
                    break;
                case ItemSortOrder.Ascending:
                    sortOrder = "titleTop + ' ' + titleBottom ASC";
                    break;
                case ItemSortOrder.Descending:
                    sortOrder = "titleTop + ' ' + titleBottom DESC";
                    break;
            }

            if(HierarchyView)
            {
                hierarchy = string.Format("parentMenuId = {0}", parentMenuId);
            }
            else
            {
                hierarchy = string.Format("parentMenuId = 0");
            }

            string sQuery = String.Format("SELECT id, screenName, titleTop, titleBottom, orderMenu, parentMenuId" +
                            " FROM NavMenu ORDER BY {0}" +
                            " WHERE {1}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder, hierarchy);

            return sQuery;
        }


        // Use mrspMoveItem SQL stored procedure
        public int MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        {
            if (currentOrderId <= 0) { throw new ArgumentOutOfRangeException("currentOrderId", (object)currentOrderId, "currentOrderId must be greater than or equal to zero."); }

            if (!_dbConnection.UseADODB)
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
                    dbConnection.Close();
                    dbConnection = null;
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetMoveNavigationItemQuery()
        {
            string sQuery = "EXEC [dbo].[mrspMoveItem] ?, ?";
            return sQuery;
        }


        public int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0)
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
                    sqlCmd.Parameters.AddWithValue("@parentMenuId", parentMenuId);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0)", ex.Message);
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();
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
                dbParam = dbCmd.CreateParameter("parentMenuId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, parentMenuId);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    dbConnection.Close();
                    dbConnection = null;
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetAddNavigationItemQuery()
        {
            string sQuery = "INSERT INTO NavMenu(screenName, titleTop, titleBottom, orderMenu, parentMenuId)" +
                            " VALUES(?, ?, ?, (SELECT CASE WHEN MAX(orderMenu) IS NULL THEN 1 ELSE MAX(orderMenu) + 1 END AS calcOrderMenu FROM NavMenu), ?)";
            return sQuery;
        }


        public int DeleteNavigationItem(int menuItemId)
        {
            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetDeleteNavigationItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@id", menuItemId);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationItem(int menuItemId)", ex.Message);
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();
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
                dbParam = dbCmd.CreateParameter("id", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, menuItemId);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset();
                rs.CursorType = CursorTypeEnum.adOpenStatic;

                try
                {
                    object recAffected;
                    rs = dbCmd.Execute(out recAffected);
                    dbConnection.Close();
                    dbConnection = null;
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationItem(int menuItemId)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetDeleteNavigationItemQuery()
        {
            string sQuery = "DELETE FROM NavMenu WHERE id = ?";
            return sQuery;
        }
    }
}