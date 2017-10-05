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
        private int _currentParentMenuId = 12;
        private string _screenName = "zFS - Test Screen";
        private string _titleTop = "Test Screen";
        private string _titleBottom = "Name #1";
        private string _screenNameInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleTopInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _titleBottomInvalid = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        private string _alarmGroup = "TotalSystem";

        private string _userName = "econder";
        private string _userNameInvalid = "";

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
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottom, 0, _alarmGroup);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidScreenName()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenNameInvalid, _titleTop, _titleBottom, 0, _alarmGroup);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleTop()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTopInvalid, _titleBottom, 0, _alarmGroup);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottomInvalid, 0, _alarmGroup);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItemBlankTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, "", 0, _alarmGroup);
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


        #region " AddNavigationHistory "

        [TestMethod]
        public void AddNavigationHistory()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = (MenuItem)items[0];

            int result = -2;
            result = _menu.AddNavigationHistory(_userName, item.MenuId);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNavigationHistoryInvalidUser()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = (MenuItem)items[0];

            int result = -2;
            result = _menu.AddNavigationHistory(_userNameInvalid, item.MenuId);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        public void AddNavigationHistoryInvalidMenuId()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = (MenuItem)items[0];

            int result = -2;
            result = _menu.AddNavigationHistory(_userName, -1);
            Assert.IsTrue(result == -1);
        }

        public int AddNavigationHistoryItem(string userName, int menuId)
        {
            return _menu.AddNavigationHistory(userName, menuId);
        }

        #endregion


        #region " GetNavigationForwardBackwardHistory "

        [TestMethod]
        public void CheckForwardAndBackwardHistory()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            int result = -2;
            int[] itemId;
            itemId = new int[2];

            //Add some menu items to the history first
            for (int i = 0; i < 2; i++)
            {
                item = (MenuItem)items[i];
                itemId[i] = item.MenuId;
                result = _menu.AddNavigationHistory(_userName, item.MenuId);
                Assert.IsTrue(result == 0);
            }

            MenuItem resItem = new MenuItem();

            // Check backward history
            resItem = _menu.GetNavigationHistoryLastItem(_userName, itemId[1]);
            Assert.IsTrue(resItem.MenuId == itemId[0]);

            // Check forward history
            resItem = _menu.GetNavigationHistoryNextItem(_userName, itemId[0]);
            Assert.IsTrue(resItem.MenuId == itemId[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNavigationHistoryLastItemInvalidUser()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = _menu.GetNavigationHistoryLastItem(_userNameInvalid, item.MenuId);
        }

        [TestMethod]
        public void GetNavigationHistoryLastItemInvalidMenuId()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = _menu.GetNavigationHistoryLastItem(_userName, -1);
            Assert.IsTrue(item.MenuId == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNavigationHistoryNextItemInvalidUser()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = _menu.GetNavigationHistoryNextItem(_userNameInvalid, item.MenuId);
        }

        [TestMethod]
        public void GetNavigationHistoryNextItemInvalidMenuId()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            item = _menu.GetNavigationHistoryNextItem(_userName, -1);
            Assert.IsTrue(item.MenuId > 0);
        }

        #endregion


        #region " DeleteNavigationHistory "

        [TestMethod]
        public void DeleteNavigationHistory()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            MenuItem item = new MenuItem();
            int result = -2;

            //Add some menu items to the history first
            for (int i = 0; i < 2; i++)
            {
                item = (MenuItem)items[i];
                result = _menu.AddNavigationHistory(_userName, item.MenuId);
                Assert.IsTrue(result == 0);
            }

            // Delete all history
            result = _menu.DeleteNavigationHistory(_userName);
            Assert.IsTrue(result == 0);


            //Add some more menu items to the history
            for (int i = 0; i < 2; i++)
            {
                item = (MenuItem)items[i];
                result = _menu.AddNavigationHistory(_userName, item.MenuId);
                Assert.IsTrue(result == 0);
            }

            item = (MenuItem)items[0];
            result = _menu.DeleteNavigationForwardHistory(_userName, item.MenuId);
            Assert.IsTrue(result == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNavigationHistoryInvalidUser()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            int result = _menu.DeleteNavigationHistory(_userNameInvalid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNavigationForwardHistoryInvalidUser()
        {
            MenuItems items = new MenuItems();
            items = _menu.MenuItemsCollection;

            int result = _menu.DeleteNavigationForwardHistory(_userNameInvalid, 0);
        }

        #endregion

    }
}
