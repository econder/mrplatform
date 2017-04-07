using System;
using System.Data;
using System.Data.OleDb;
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
    }
}
