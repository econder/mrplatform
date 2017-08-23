using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.DB.Sql;
using MRPlatform.HMI;


namespace MRPlatformTests.HMI
{
    [TestClass]
    public class MenuNavigationTest
    {
        private MenuNavigation _menu;

        private MRDbConnection _mrdb;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "MRPlatform";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private int _id = 0;
        private int _parentMenuId = 0;
        private int _currentParentMenuId = 12;
        private string _screenName = "zFS - Test Screen";
        private string _titleTop = "Test Screen";
        private string _titleBottom = "Name #1";
        private string _screenNameInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleTopInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleBottomInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        [TestInitialize]
        public void Initialize()
        {
            // OleDbConnection
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _menu = new MenuNavigation();
            _menu.DbConnection = _mrdb;
            _menu.ResultsPageNumber = 1;
            _menu.ResultsPerPage = 20;
            _menu.ResultsSortOrder = Menu.ItemSortOrder.Custom;
            _menu.ParentMenuId = 0;
        }

        
        #region " GetNavigationItems "

        [TestMethod]
        public void MenuItems()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;
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

        #endregion
    }
}
