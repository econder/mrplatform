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
    public class MenuNavigationTest
    {
        private MenuNavigation _menu, _menuADO;

        private MRDbConnection _mrdb, _mrDbADO;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "MRPlatform";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private string _providerADO = "SQLOLEDB";

        private int _id = 0;
        private int _parentMenuId = 10;
        private int _currentParentMenuId = 12;
        private string _screenName = "zFS - Test Screen";
        private string _titleTop = "Test Screen";
        private string _titleBottom = "Name #1";
        private string _screenNameInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleTopInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleBottomInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        private DataSet _ds;
        private Recordset _rs;

        [TestInitialize]
        public void Initialize()
        {
            // OleDbConnection
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _menu = new MenuNavigation(_mrdb);
            _menu.ParentMenuId = _parentMenuId;

            // ADODB Connection
            _mrDbADO = new MRDbConnection(_providerADO, _dbServer, _dbName, _dbUser, _dbPass, true);
            _menuADO = new MenuNavigation(_mrDbADO);
            _menuADO.ParentMenuId = _parentMenuId;
        }

        
        #region " GetNavigationItems "

        [TestMethod]
        public void MenuItems()
        {
            MenuItems items = _menu.MenuItemsCollection;

            Assert.IsTrue(items.Count > 0);

            MenuItem item = new MenuItem();
            item = (MenuItem)items[0];
            Assert.IsTrue(item.ScreenName.Length > 0);
            Assert.IsTrue(item.ChildCount == -1 || item.ChildCount >= 1);
        }


        [TestMethod]
        public void GetPreviousParentMenuItem()
        {
            MenuItem item = new MenuItem();
            item = _menu.GetPreviousParentMenuItem(_currentParentMenuId);
            
            Assert.IsTrue(item.ParentMenuId > 0);
            Assert.IsTrue(item.ScreenName.Length > 0);
            Assert.IsTrue(item.ChildCount == -1 || item.ChildCount >= 1);
        }

        #endregion


        #region " MoveNavigationItem "

        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(MenuNavigation.ItemMoveDirection.Up, 4);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveNavigationItemEnd()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(MenuNavigation.ItemMoveDirection.Down, 15);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveNavigationItemInvalidParams()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(MenuNavigation.ItemMoveDirection.Up, 0);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveNavigationItemInvalidParamsADO()
        {
            int recCount = -1;
            recCount = _menuADO.MoveNavigationItem(MenuNavigation.ItemMoveDirection.Up, 0);
            Assert.IsTrue(recCount >= 0);
        }

        #endregion


        #region " AddNavigationItem "

        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottom, 0);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItemADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTop, _titleBottom, 0);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidScreenName()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenNameInvalid, _titleTop, _titleBottom, 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleTop()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTopInvalid, _titleBottom, 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottomInvalid, 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItemBlankTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, "", 0);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidScreenNameADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenNameInvalid, _titleTop, _titleBottom, 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleTopADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTopInvalid, _titleBottom, 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleBottomADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTop, _titleBottomInvalid, 0);
        }

        #endregion

        #region " DeleteNavigationItem "

        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void DeleteNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.DeleteNavigationItem(_id);
            Assert.IsTrue(recCount >= 0);

            MenuItems items = _menu.MenuItemsCollection;
            Assert.IsTrue(items.Count > 0);
        }


        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void DeleteNavigationItemADO()
        {
            int recCount = -1;
            recCount = _menuADO.DeleteNavigationItem(_id);
            Assert.IsTrue(recCount >= 0);
        }

        #endregion
    }
}
