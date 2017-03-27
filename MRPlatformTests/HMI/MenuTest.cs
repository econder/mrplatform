using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MRPlatform.DB.Sql;
using MRPlatform.HMI;

namespace MRPlatformTests.HMI
{
    [TestClass]
    public class MenuTest
    {
        private Menu _menu;

        private MRDbConnection _mrdb;
        private string _provider = "SQLNCLI11";
        private string _dbServer = "WIN-1I5C3456H92\\SQLEXPRESS";
        private string _dbName = "mrsystems";
        private string _dbUser = "mrsystems";
        private string _dbPass = "Reggie#123";

        private int _pageNumber = 1;
        private int _pageNumberInvalid = 0;
        private int _resultsPerPage = 20;
        private int _resultsPerPageInvalid = 0;

        private DataSet _ds;

        [TestInitialize]
        public void Initialize()
        {
            _mrdb = new MRDbConnection(_provider, _dbServer, _dbName, _dbUser, _dbPass);
            _menu = new Menu(_mrdb);
        }

        // GetMessages(string area)
        [TestMethod]
        public void GetMenuItems()
        {
            _ds = new DataSet();
            _ds = _menu.GetNavigationItems(_pageNumber, _resultsPerPage);

            Assert.IsTrue(_ds.Tables.Count == 1);
            Assert.IsTrue(_ds.Tables[0].Rows.Count >= 1);
        }
    }
}
