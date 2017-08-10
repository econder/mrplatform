using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADODB;

using MRPlatform.DB.Sql;
using MRPlatform.HMI;


namespace MRPlatformTests.HMI
{
    [TestClass]
    public class MenuFavoriteTest
    {
        private MenuFavorite _menu, _menuADO;

        private MRDbConnection _mrdb, _mrDbADO;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "MRPlatform";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private string _providerADO = "SQLOLEDB";

        private int _id = 0;
        private int _parentMenuId = 0;
        private int _currentParentMenuId = 20158;
        private string _userName = "econder";

        private DataSet _ds;
        private Recordset _rs;

        [TestInitialize]
        public void Initialize()
        {
            // OleDbConnection
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _menu = new MenuFavorite(_mrdb);
            _menu.ParentMenuId = _parentMenuId;

            // ADODB Connection
            _mrDbADO = new MRDbConnection(_providerADO, _dbServer, _dbName, _dbUser, _dbPass, true);
            _menuADO = new MenuFavorite(_mrDbADO);
            _menuADO.ParentMenuId = _parentMenuId;
        }


        #region " GetNavigationItems "

        [TestMethod]
        public void MenuFavoriteItems()
        {
            MenuItems items = _menu.MenuItemsCollection;

            Assert.IsTrue(items.Count > 0);

            MenuItem item = new MenuItem();
            item = (MenuItem)items[0];
            Assert.IsTrue(item.ScreenName.Length > 0);
            Assert.IsTrue(item.ChildCount == -1 || item.ChildCount >= 1);
        }


        [TestMethod]
        public void GetPreviousFavoriteParentMenuItem()
        {
            MenuItem item = new MenuItem();
            item = _menu.GetPreviousParentMenuItem(_currentParentMenuId);

            Assert.IsTrue(item.ParentMenuId > 0);
            Assert.IsTrue(item.ScreenName.Length > 0);
            Assert.IsTrue(item.ChildCount == -1 || item.ChildCount >= 1);
        }

        #endregion


        #region " MoveFavoriteItem "

        // MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveFavoriteItem()
        {
            int recCount = -1;
            recCount = _menu.MoveFavoriteItem(MenuFavorite.ItemMoveDirection.Up, 4);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveFavoriteItemEnd()
        {
            int recCount = -1;
            recCount = _menu.MoveFavoriteItem(MenuFavorite.ItemMoveDirection.Down, 3);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveFavoriteItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveFavoriteItemInvalidParams()
        {
            int recCount = -1;
            recCount = _menu.MoveFavoriteItem(MenuFavorite.ItemMoveDirection.Up, 0);
            Assert.IsTrue(recCount >= 0);
        }

        #endregion


        #region " AddFavoriteItem "

        // AddFavoriteItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddFavoriteItem()
        {
            int recCount = -1;
            recCount = _menu.AddFavoriteItem(_userName, 4);
            Assert.IsTrue(recCount == 0);
        }

        #endregion

        #region " DeleteNavigationItem "

        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void DeleteFavoriteItem()
        {
            int recCount = -1;
            recCount = _menu.DeleteFavoriteItem(_id);
            Assert.IsTrue(recCount >= 0);

            MenuItems items = _menu.MenuItemsCollection;
            Assert.IsTrue(items.Count > 0);
        }

        #endregion
    }
}
