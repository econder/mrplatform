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
    public class MenuTest
    {
        private Menu _menu, _menuADO;

        private MRDbConnection _mrdb, _mrDbADO;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private string _providerADO = "SQLOLEDB";

        private int _pageNumber = 1;
        private int _pageNumberInvalid = 0;
        private int _resultsPerPage = 20;
        private int _resultsPerPageInvalid = 0;

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
            _menu = new Menu(_mrdb);

            // ADODB Connection
            _mrDbADO = new MRDbConnection(_providerADO, _dbServer, _dbName, _dbUser, _dbPass, true);
            _menuADO = new Menu(_mrDbADO);
        }


        #region " GetNavigationItems "

        [TestMethod]
        public void Items()
        {
            MenuItems items = _menu.MenuItemsCollection;
            Assert.IsTrue(items.Count > 0);

            MenuItem item = new MenuItem();
            item = (MenuItem)items[1];
            Assert.IsTrue(item.ScreenName.Length > 0);
        }

        /*
        // GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)
        [TestMethod]
        public void GetNavigationItemsDS()
        {
            _ds = new DataSet();
            _ds = _menu.GetNavigationItemsDataSet(_pageNumber, _resultsPerPage);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count == 20);
        }

        // GetNavigationItemsDataSet(int pageNumber, int resultsPerPage)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNavigationItemsDSInvalidParams()
        {
            _ds = new DataSet();
            _ds = _menu.GetNavigationItemsDataSet(_pageNumberInvalid, _resultsPerPageInvalid);
        }


        // GetNavigationItemsRecordset(int pageNumber, int resultsPerPage)
        [TestMethod]
        public void GetNavigationItemsRS()
        {
            _rs = new Recordset();
            _rs = _menuADO.GetNavigationItemsRecordset(_pageNumber, _resultsPerPage);

            Assert.IsTrue(_rs.RecordCount == 20);
        }

        // GetNavigationItemsRecordset(int pageNumber, int resultsPerPage)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNavigationItemsRSInvalidParams()
        {
            _rs = new Recordset();
            _rs = _menuADO.GetNavigationItemsRecordset(_pageNumberInvalid, _resultsPerPageInvalid);
        }
        */
        #endregion


        #region " MoveNavigationItem "

        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(Menu.ItemMoveDirection.Up, 4);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveNavigationItemEnd()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(Menu.ItemMoveDirection.Down, 3);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        public void MoveNavigationItemADO()
        {
            int recCount = -1;
            recCount = _menuADO.MoveNavigationItem(Menu.ItemMoveDirection.Up, 4);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveNavigationItemInvalidParams()
        {
            int recCount = -1;
            recCount = _menu.MoveNavigationItem(Menu.ItemMoveDirection.Up, 0);
            Assert.IsTrue(recCount >= 0);
        }


        // MoveNavigationItem(ItemMoveDirection direction, int currentOrderId)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveNavigationItemInvalidParamsADO()
        {
            int recCount = -1;
            recCount = _menuADO.MoveNavigationItem(Menu.ItemMoveDirection.Up, 0);
            Assert.IsTrue(recCount >= 0);
        }

        #endregion


        #region " AddNavigationItem "

        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottom);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItemADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTop, _titleBottom);
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNavigationItemNullScreenName()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(null, _titleTop, _titleBottom);
        }

        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNavigationItemEmptyScreenName()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem("", _titleTop, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidScreenName()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenNameInvalid, _titleTop, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleTop()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTopInvalid, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, _titleBottomInvalid);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void AddNavigationItemBlankTitleBottom()
        {
            int recCount = -1;
            recCount = _menu.AddNavigationItem(_screenName, _titleTop, "");
            Assert.IsTrue(recCount == 0);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNavigationItemNullScreenNameADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(null, _titleTop, _titleBottom);
        }

        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNavigationItemEmptyScreenNameADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem("", _titleTop, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidScreenNameADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenNameInvalid, _titleTop, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleTopADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTopInvalid, _titleBottom);
        }


        // AddNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddNavigationItemInvalidTitleBottomADO()
        {
            int recCount = -1;
            recCount = _menuADO.AddNavigationItem(_screenName, _titleTop, _titleBottomInvalid);
        }

        #endregion

        #region " DeleteNavigationItem "

        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void DeleteNavigationItem()
        {
            int recCount = -1;
            recCount = _menu.DeleteNavigationItem(_screenName);
            Assert.IsTrue(recCount >= 0);
        }


        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        public void DeleteNavigationItemADO()
        {
            int recCount = -1;
            recCount = _menuADO.DeleteNavigationItem(_screenName);
            Assert.IsTrue(recCount >= 0);
        }


        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNavigationItemNullScreenName()
        {
            int recCount = -1;
            recCount = _menu.DeleteNavigationItem(null);
        }

        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteNavigationItemEmptyScreenName()
        {
            int recCount = -1;
            recCount = _menu.DeleteNavigationItem("");
        }


        // DeleteNavigationItem(string screenName, string titleTop, string titleBottom)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeleteNavigationItemInvalidScreenName()
        {
            int recCount = -1;
            recCount = _menu.DeleteNavigationItem(_screenNameInvalid);
        }

        #endregion
    }
}
