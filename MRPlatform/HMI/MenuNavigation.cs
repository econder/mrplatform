using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using ADODB;

using MRPlatform.DB.Sql;

namespace MRPlatform.HMI
{
    [ComVisible(true)]
    [Guid("070F5B9A-8BC8-40CF-87C9-49131125A8BA")]
    [ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(IMenuNavigation))]
    public class MenuNavigation : Menu, IMenuNavigation
    {
        private ErrorLog _errorLog = new ErrorLog();
        private MRDbConnection _dbConnection;
        private MenuItems _itemsCollection;


        public MenuNavigation()
        {
            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
            ParentMenuId = 0;
        }
        

        public MenuNavigation(MRDbConnection mrDbConnection)
        {
            _dbConnection = mrDbConnection;

            // Set property defaults
            ResultsPageNumber = 1;
            ResultsPerPage = 100;
            ResultsSortOrder = ItemSortOrder.Custom;
            ParentMenuId = 0;

            _itemsCollection = DoGetItems();
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
                sqlCmd.Parameters.AddWithValue("@parentMenuId", ParentMenuId);
                sqlCmd.Parameters.AddWithValue("@offset", (ResultsPageNumber - 1) * ResultsPerPage);
                sqlCmd.Parameters.AddWithValue("@rowCount", ResultsPerPage);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                MenuItems menuItems = new MenuItems();

                try
                {
                    dbAdapt.Fill(ds);

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
                                                       (int)row["parentMenuId"],
                                                       (int)row["childCount"],
                                                       row["alarmGroup"].ToString(),
                                                       row["screenTitle"].ToString()));
                            i++;
                        }
                    }

                    return menuItems;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DoGetItems()", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();

                    // Return MenuItems collection with one blank MenuItem rather than null object.
                    menuItems.Add(0, new MenuItem(0, "", "", "", 0, 0, -1,""));
                    return menuItems;
                }
            }
        }


        [ComVisible(false)]
        private string GetNavigationItemsQuery(ItemSortOrder itemSortOrder)
        {
            string sortOrder = null;

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

            string sQuery = String.Format("SELECT id, screenName, titleTop, titleBottom, orderMenu, parentMenuId, childCount, alarmGroup, screenTitle" +
                            " FROM vNavMenu" +
                            " WHERE parentMenuId = ?" +
                            " ORDER BY {0}" + 
                            " OFFSET ? ROWS" +
                            " FETCH NEXT ? ROWS ONLY", sortOrder);

            return sQuery;
        }


        public MenuItem GetPreviousParentMenuItem(int currentParentMenuId)
        {
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetPreviousParentMenuIdQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@id", currentParentMenuId);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                // Create menuItem and set default values
                MenuItem mi = new MenuItem()
                {
                    MenuId = 0,
                    ScreenName = "",
                    TitleTop = "",
                    TitleBottom = "",
                    MenuOrder = 0,
                    ParentMenuId = 0,
                    ChildCount = -1,
                    AlarmGroup = "",
                    ScreenTitle = ""
                };

                try
                {
                    dbAdapt.Fill(ds);
                    
                    if (ds.Tables.Count > 0)
                    {
                        // Should only be 1 row
                        DataRow row = ds.Tables[0].Rows[0];

                        if (row != null)
                        {
                            mi.MenuId = (int)row["id"];
                            mi.ScreenName = row["screenName"].ToString();
                            mi.TitleTop = row["titleTop"].ToString();
                            mi.TitleBottom = row["titleBottom"].ToString();
                            mi.MenuOrder = (int)row["orderMenu"];
                            mi.ParentMenuId = (int)row["parentMenuId"];
                            mi.ChildCount = (int)row["childCount"];
                            mi.AlarmGroup = row["alarmGroup"].ToString();
                            mi.ScreenTitle = row["screenTitle"].ToString();
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
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)", ex.Message);

                    // Return root parentMenuId on error
                    return mi;
                }
            }
        }


        [ComVisible(false)]
        private string GetPreviousParentMenuIdQuery()
        {
            string sQuery = "SELECT id, screenName, titleTop, titleBottom, orderMenu, parentMenuId, childCount, alarmGroup, screenTitle" +
                            " FROM vNavMenu" +
                            " WHERE id = ?";
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
            string sQuery = "EXEC [dbo].[mrspMoveNavMenuItem] ?, ?";
            return sQuery;
        }


        public int AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0, string alarmGroup = "", string screenTitle = "")
        {
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
                    sqlCmd.Parameters.AddWithValue("@alarmGroup", alarmGroup);
                    sqlCmd.Parameters.AddWithValue("@screenTitle", screenTitle);

                    try
                    {
                        sqlCmd.ExecuteNonQuery();
                        dbConnection.Close();
                        return 0;
                    }
                    catch (OleDbException ex)
                    {
                        _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0, string alarmGroup = '', string screenTitle = 0)", ex.Message);
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

                Command dbCmd = new Command()
                {
                    ActiveConnection = dbConnection,
                    CommandText = GetAddNavigationItemQuery(),
                    CommandType = CommandTypeEnum.adCmdText
                };
                Parameter dbParam = new Parameter();
                dbParam = dbCmd.CreateParameter("screenName", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 255, screenName);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("titleTop", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, titleTop);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("titleBottom", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 50, titleBottom);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("parentMenuId", DataTypeEnum.adInteger, ParameterDirectionEnum.adParamInput, 999999999, parentMenuId);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("alarmGroup", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 255, alarmGroup);
                dbCmd.Parameters.Append(dbParam);
                dbParam = dbCmd.CreateParameter("screenTitle", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput, 255, screenTitle);
                dbCmd.Parameters.Append(dbParam);

                Recordset rs = new Recordset()
                {
                    CursorType = CursorTypeEnum.adOpenStatic
                };

                try
                {
                    rs = dbCmd.Execute(out object recAffected);
                    dbConnection.Close();
                    dbConnection = null;
                    return 0;
                }
                catch (COMException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "AddNavigationItem(string screenName, string titleTop, string titleBottom, int parentMenuId = 0, string alarmGroup = '', string screenTitle = '')", ex.Message);
                    if (dbConnection.State == (int)ObjectStateEnum.adStateOpen)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetAddNavigationItemQuery()
        {
            string sQuery = "INSERT INTO NavMenu(screenName, titleTop, titleBottom, orderMenu, parentMenuId, alarmGroup, screenTitle)" +
                            " VALUES(?, ?, ?, (SELECT CASE WHEN MAX(orderMenu) IS NULL THEN 1 ELSE MAX(orderMenu) + 1 END AS calcOrderMenu FROM NavMenu), ?, ?, ?)";
            return sQuery;
        }


        public int DeleteNavigationItem(int menuItemId, ItemOrphanAction itemOrphanAction = ItemOrphanAction.SetToRoot)
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

                        //Take care of any orphaned child menu items
                        DoOrphanChildMenuAction(menuItemId, itemOrphanAction);
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

        [ComVisible(false)]
        private void DoOrphanChildMenuAction(int menuItemId, ItemOrphanAction itemOrphanAction)
        {
            // Use OleDb Connection
            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand();
                string sQuery = null;

                switch (itemOrphanAction)
                {
                    case ItemOrphanAction.Delete:
                        sQuery = "DELETE FROM NavMenu WHERE parentMenuId = ?";
                        break;

                    case ItemOrphanAction.SetToRoot:
                        sQuery = "UPDATE NavMenu SET orderMenu = orderMenu + parentMenuId, parentMenuId = 0 WHERE parentMenuId = ?";
                        break;
                }

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
                    _errorLog.LogMessage(this.GetType().Name, "DoOrphanChildMenuAction(int menuItemId, ItemOrphanAction itemOrphanAction)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return;
                }
            }
        }


        public MenuItem GetNavigationHistoryLastItem(string userName)
        {
            if (userName.Length == 0 || userName == null) { throw new ArgumentNullException(userName, "userName cannot be null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetNavigationHistoryLastItemQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@userName", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                // Create menuItem and set default values
                MenuItem mi = new MenuItem()
                {
                    MenuId = 0,
                    ScreenName = "",
                    TitleTop = "",
                    TitleBottom = "",
                    MenuOrder = 0,
                    ParentMenuId = 0,
                    ChildCount = -1,
                    AlarmGroup = "",
                    ScreenTitle = ""
                };

                try
                {
                    dbAdapt.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if(ds.Tables[0].Rows.Count > 0)
                        {
                            // Should only be 1 row
                            DataRow row = ds.Tables[0].Rows[0];

                            mi.MenuId = (int)row["id"];
                            mi.ScreenName = row["screenName"].ToString();
                            mi.TitleTop = row["titleTop"].ToString();
                            mi.TitleBottom = row["titleBottom"].ToString();
                            mi.MenuOrder = (int)row["orderMenu"];
                            mi.ParentMenuId = (int)row["parentMenuId"];
                            mi.AlarmGroup = row["alarmGroup"].ToString();
                            mi.ScreenTitle = row["screenTitle"].ToString();
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
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationHistoryLastItem(string userName)", ex.Message);

                    // Return root parentMenuId on error
                    return mi;
                }
            }
        }

        private string GetNavigationHistoryLastItemQuery()
        {
            // Copy last record in NavBack to NavForward,
            // Delete last record in NavBack, & 
            // Return the new last record in vNavHistoryBack.
            // Returning from joined tables from vNavHistory to
            // build MenuItem object to return
            string sQuery = "BEGIN TRANSACTION;" +
                            "INSERT INTO NavForward(navMenuId, navDateTime, userName)" +
                            " SELECT navMenuId, navDateTime, userName" +
                            " FROM NavBack" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM NavBack WHERE userName = ?);" +
                            " DELETE FROM NavBack" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM NavBack WHERE userName = ?); " +
                            "COMMIT" +
                            "SELECT id, navDateTime, userName, screenName, titleTop, titleBottom, orderMenu, parentMenuId, alarmGroup" +
                            " FROM vNavHistoryBack" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM vNavHistoryBack WHERE userName = ?)";

            return sQuery;
        }


        public MenuItem GetNavigationHistoryNextItem(string userName)
        {
            if (userName.Length == 0 || userName == null) { throw new ArgumentNullException(userName, "userName cannot be null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetNavigateHistoryNextItemQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@userName", userName);

                OleDbDataAdapter dbAdapt = new OleDbDataAdapter(sqlCmd);
                DataSet ds = new DataSet();

                // Create menuItem and set default values
                MenuItem mi = new MenuItem()
                {
                    MenuId = 0,
                    ScreenName = "",
                    TitleTop = "",
                    TitleBottom = "",
                    MenuOrder = 0,
                    ParentMenuId = 0,
                    ChildCount = -1,
                    AlarmGroup = "",
                    ScreenTitle = ""
                };

                try
                {
                    dbAdapt.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // Should only be 1 row
                            DataRow row = ds.Tables[0].Rows[0];

                            mi.MenuId = (int)row["id"];
                            mi.ScreenName = row["screenName"].ToString();
                            mi.TitleTop = row["titleTop"].ToString();
                            mi.TitleBottom = row["titleBottom"].ToString();
                            mi.MenuOrder = (int)row["orderMenu"];
                            mi.ParentMenuId = (int)row["parentMenuId"];
                            mi.AlarmGroup = row["alarmGroup"].ToString();
                            mi.ScreenTitle = row["screenTitle"].ToString();
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
                    _errorLog.LogMessage(this.GetType().Name, "GetNavigationHistoryNextItem(string userName)", ex.Message);

                    // Return root parentMenuId on error
                    return mi;
                }
            }
        }

        private string GetNavigateHistoryNextItemQuery()
        {
            // Copy last record in NavBack to NavForward,
            // Delete last record in NavBack, & 
            // Return the new last record in vNavHistoryBack.
            // Returning from joined tables from vNavHistory to
            // build MenuItem object to return
            string sQuery = "BEGIN TRANSACTION;" +
                            "INSERT INTO NavBack(navMenuId, navDateTime, userName)" +
                            " SELECT navMenuId, navDateTime, userName" +
                            " FROM NavForward" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM NavForward WHERE userName = ?);" +
                            " DELETE FROM NavForward" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM NavForward WHERE userName = ?); " +
                            "COMMIT" +
                            "SELECT id, navDateTime, userName, screenName, titleTop, titleBottom, orderMenu, parentMenuId, alarmGroup" +
                            " FROM vNavHistoryForward" +
                            " WHERE navDateTime = (SELECT MAX(navDateTime) FROM vNavHistoryForward WHERE userName = ?)";

            return sQuery;
        }


        public int AddNavigationHistory(string userName, int currentNavMenuId)
        {
            if (userName.Length == 0 || userName == null) { throw new ArgumentNullException(userName, "userName cannot be null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetAddNavigationHistoryQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@userName", userName);
                sqlCmd.Parameters.AddWithValue("@navMenuId", currentNavMenuId);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                    return 0;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "AddNavigationHistory(string userName, int currentNavMenuId)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetAddNavigationHistoryQuery()
        {
            string sQuery = "INSERT INTO NavBack(userName, navMenuId) VALUES(?, ?)";

            return sQuery;
        }


        public int DeleteNavigationForwardHistory(string userName)
        {
            if (userName.Length == 0 || userName == null) { throw new ArgumentNullException(userName, "userName cannot be null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetDeleteNavigationForwardHistoryQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@userName", userName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                    return 0;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationForwardHistory(string userName)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetDeleteNavigationForwardHistoryQuery()
        {
            string sQuery = "DELETE FROM NavForward" +
                            " WHERE userName = ?";

            return sQuery;
        }


        public int DeleteNavigationBackHistory(string userName)
        {
            if (userName.Length == 0 || userName == null) { throw new ArgumentNullException(userName, "userName cannot be null."); }

            using (IDbConnection dbConnection = _dbConnection.Connection)
            {
                dbConnection.Open();

                OleDbCommand sqlCmd = new OleDbCommand(GetDeleteNavigationHistoryQuery(), (OleDbConnection)dbConnection);
                sqlCmd.Parameters.AddWithValue("@userName", userName);

                try
                {
                    sqlCmd.ExecuteNonQuery();
                    return 0;
                }
                catch (OleDbException ex)
                {
                    _errorLog.LogMessage(this.GetType().Name, "DeleteNavigationHistory(string userName)", ex.Message);
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                    return -1;
                }
            }
        }

        private string GetDeleteNavigationHistoryQuery()
        {
            string sQuery = "DELETE FROM NavBack" +
                            " WHERE userName = ?";

            return sQuery;
        }
    }
}
