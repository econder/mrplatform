using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("5EBBAFFB-CA9C-4D6F-AF27-CB06A37232F6")]
    [ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuFavorite))]
    public class MenuFavorite : Menu, IMenuFavorite
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;
        private MenuItems _itemsCollection;


        public MenuFavorite()
        {
            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
            ParentMenuId = 0;
        }


        public MenuFavorite(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;

            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
            ParentMenuId = 0;
        }


        /// <summary>
        /// DbConnection Property
        /// </summary>
        public MRDbConnection DbConnection
        {
            get
            {
                return _dbConnection;
            }
            set
            {
                _dbConnection = value;
            }
        }


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

                OleDbCommand sqlCmd = new OleDbCommand(GetFavoriteItemsQuery(ResultsSortOrder), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@parentMenuId", ParentMenuId);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                MenuItems menuItems = new MenuItems();

                try
                {
                    dbAdapt.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            menuItems.Add(i, new MenuItem((int)row["id"],
                                                          row["screenName"].ToString(),
                                                          row["titleTop"].ToString(),
                                                          row["titleBottom"].ToString(),
                                                          (int)row["orderFavorite"],
                                                          (int)row["parentMenuId"],
                                                          (int)row["childCount"]));
                            i++;
                        }
                    }

                    return menuItems;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetFavoriteItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();

                    // Return MenuItems collection with one blank MenuItem rather than null object.
                    menuItems.Add(0, new MenuItem(0, "", "", "", 0, 0));
                    return menuItems;
                }
            }
        }


        [ComVisible(false)]
        private string GetFavoriteItemsQuery(ItemSortOrder itemSortOrder)
        {
            string sortOrder = null;

            switch (itemSortOrder)
            {
                case ItemSortOrder.Custom:
                    sortOrder = "orderFavorite ASC";
                    break;
                case ItemSortOrder.Ascending:
                    sortOrder = "titleTop + ' ' + titleBottom ASC";
                    break;
                case ItemSortOrder.Descending:
                    sortOrder = "titleTop + ' ' + titleBottom DESC";
                    break;
            }

            string sQuery = String.Format("SELECT id, screenName, titleTop, titleBottom, orderFavorite, parentMenuId, childCount" +
                            " FROM vNavFavorite" +
                            " WHERE parentMenuId = ?" +
                            " ORDER BY {0}" +
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);

            return sQuery;
        }

        /*
        // This feature isn't needed right for Favorites.
        public MenuItem GetPreviousParentMenuItem(int currentParentMenuId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetPreviousParentMenuIdQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@ID", currentParentMenuId);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                MenuItem mi = new MenuItem();

                try
                {
                    dbAdapt.Fill(ds);
                    dbConnection.Close();

                    if (ds.Tables.Count > 0)
                    {
                        // Should only be 1 row
                        DataRow row = ds.Tables[0].Rows[0];

                        if (row != null)
                        {
                            mi.MenuId = (int)row["ID"];
                            mi.ScreenName = row["screenName"].ToString();
                            mi.TitleTop = row["titleTop"].ToString();
                            mi.TitleBottom = row["titleBottom"].ToString();
                            mi.MenuOrder = (int)row["orderFavorite"];
                            mi.ParentMenuId = (int)row["parentMenuId"];
                            mi.ChildCount = (int)row["childCount"];
                        }
                        else
                        {
                            mi.MenuId = 0;
                            mi.ScreenName = "";
                            mi.TitleTop = "";
                            mi.TitleBottom = "";
                            mi.MenuOrder = 0;
                            mi.ParentMenuId = 0;
                            mi.ChildCount = -1;
                        }

                        return mi;
                    }
                    else
                    {
                        return mi;
                    }
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "GetFavoriteItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();

                    // Return root parentMenuId on error
                    return mi;
                }
            }
        }


        [ComVisible(false)]
        private string GetPreviousParentMenuIdQuery()
        {
            string sQuery = "SELECT id, screenName, titleTop, titleBottom, orderFavorite, parentMenuId, childCount" +
                            " FROM vNavFavorite" +
                            " WHERE id = ?";
            return sQuery;
        }
        */

        // Use mrspMoveItem SQL stored procedure
        public int MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)
        {
            if (currentOrderId <= 0) { throw new ArgumentOutOfRangeException("currentOrderId", (object)currentOrderId, "currentOrderId must be greater than or equal to zero."); }

            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetMoveFavoriteItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@orderId", currentOrderId);
                    sqlCmd.Parameters.AddWithValue("@direction", Convert.ToBoolean(direction));

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)", ex.Message);
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
                dbCmd.CommandText = GetMoveFavoriteItemQuery();
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
                    _errorLog.LogMessage(this.GetType().Name, "MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetMoveFavoriteItemQuery()
        {
            string sQuery = "EXEC [dbo].[mrspMoveNavFavoriteItem] ?, ?";
            return sQuery;
        }


        public int AddFavoriteItem(string userName, int navMenuId = 0)
        {
            if (userName.Length > 50) { throw new ArgumentOutOfRangeException("userName", "userName must be 50 characters or less."); }

            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetAddFavoriteItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@navMenuId", navMenuId);
                    sqlCmd.Parameters.AddWithValue("@userName", userName);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "AddFavoriteItem(string userName, int navMenuId = 0)", ex.Message);
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
                dbCmd.CommandText = GetAddFavoriteItemQuery();
                dbCmd.CommandType = CommandTypeEnum.adCmdText;
                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("navMenuId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, navMenuId);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("userName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, userName);
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
                    _errorLog.LogMessage(this.GetType().Name, "AddFavoriteItem(string userName, int navMenuId = 0)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetAddFavoriteItemQuery()
        {
            string sQuery = "INSERT INTO NavFavorite(navMenuId, userName, orderFavorite)" +
                            " VALUES(?, ?, (SELECT CASE WHEN MAX(orderFavorite) IS NULL THEN 1 ELSE MAX(orderFavorite) + 1 END AS calcOrderMenu FROM NavFavorite))";
            return sQuery;
        }


        public int DeleteFavoriteItem(int menuItemId)
        {
            if (!_dbConnection.UseADODB)
            {
                // Use OleDb Connection
                using (IDbConnection dbConnection = _dbConnection.Connection)
                {
                    dbConnection.Open();

                    OleDbCommand sqlCmd = new OleDbCommand(GetDeleteFavoriteItemQuery(), (OleDbConnection)dbConnection);
                    sqlCmd.Parameters.AddWithValue("@id", menuItemId);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();

                        //Take care of any orphaned child menu items
                        DoOrphanChildMenuAction(menuItemId);
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "DeleteFavoriteItem(int menuItemId)", ex.Message);
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
                dbCmd.CommandText = GetDeleteFavoriteItemQuery();
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
                    _errorLog.LogMessage(this.GetType().Name, "DeleteFavoriteItem(int menuItemId)", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetDeleteFavoriteItemQuery()
        {
            string sQuery = "DELETE FROM NavFavorite WHERE id = ?";
            return sQuery;
        }

        [ComVisible(false)]
        private void DoOrphanChildMenuAction(int menuItemId)
        {
            // Use OleDb Connection
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                string sQuery = null;
                sQuery = "DELETE FROM NavFavorite WHERE parentMenuId = ?";

                OleDbCommand sqlCmd = new OleDbCommand();
                sqlCmd.Connection = (OleDbConnection)dbConnection;
                sqlCmd.CommandText = sQuery;
                sqlCmd.Parameters.AddWithValue("@id", menuItemId);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                    dbConnection.Close();
                    return;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DoOrphanChildMenuAction(int menuItemId)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return;
                }
            }
        }
    }
}
