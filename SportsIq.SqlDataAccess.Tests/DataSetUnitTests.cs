using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SportsIq.SqlDataAccess.Tests
{
    [TestClass]
    public class DataSetUnitTests
    {
        private readonly IDatasetOperations _dataAccessDataSetOperations;

        #region Test Initialization/Cleanup

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Executes once for the test class. (Optional)

            //_testDirectory = context.TestDir;
            //_testRunDirectory = context.TestRunDirectory;
        }

        [ClassCleanup]
        public static void TestFixtureTearDown()
        {
            // Runs once after all tests in this class are executed. (Optional)
            // Not guaranteed that it executes instantly after all tests from the class.
        }

        [TestInitialize]
        public void Setup()
        {
            // Runs before each test. (Optional)
        }

        [TestCleanup]
        public void TearDown()
        {
            // Runs after each test. (Optional)
        }

        #endregion

        public DataSetUnitTests()
        {
            _dataAccessDataSetOperations = new DataAccessDatasetOperations();
        }

        [TestMethod]
        public void GetGameEventsReturnsNonEmptyDataSetTest()
        {
            // Arrange
            Guid gameId = Guid.Parse("0036a1fa-ff9a-44a5-8061-68b828ee2b1e");
            const string side = "Home";

            // Act
            DataSet playersDataSet = _dataAccessDataSetOperations.GetPlayersDataSet(gameId, side);

            // Assert
            //DataTable playersDataTable = playersDataSet.Tables["Players"];

            //foreach (DataRow dataRow in playersDataTable.Rows)
            //{
            //    string value = dataRow.ToString();
            //}

            Assert.IsNotNull(playersDataSet);
            //Assert.IsNotNull(playersDataTable);
            // Assert that the table has rows
        }
    }
}
