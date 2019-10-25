using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsIq.Models.GamesDto.Mlb;
using SportsIq.Utilities;

namespace SportsIq.SqlDataAccess.Tests
{
    [TestClass]
    public class MlbSqlDataAccessUnitTests
    {
        private readonly IDataAccessMlb _dataAccessMlb;

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

        public MlbSqlDataAccessUnitTests()
        {
            _dataAccessMlb = new DataAccessMlb();
        }

        [TestMethod]
        public void GetGamesReturnsNonEmptyRecordsetTest()
        {
            // Arrange

            // Act
            List<MlbGameDto> mlbGameList = _dataAccessMlb.GetGames();

            // Assert
            Assert.IsTrue(mlbGameList.Count > 0);
        }

        [TestMethod]
        public void GetScoreAverageReturnsNonEmptyRecordsetTest()
        {
            // Arrange
            Guid team1Id = Guid.Parse("03556285-bdbb-4576-a06d-42f71f46ddc5");
            Guid team2Id = Guid.Parse("f246a5e5-afdb-479c-9aaa-c68beeda7af6");
            Guid team2PitcherId = Guid.Parse("03fda22a-f8a3-45e8-80dc-bf8190a22020");
            const string side = "H";

            // Act
            Dictionary<string, double> scoreAverageDictionary = _dataAccessMlb.GetScoreAverage(team1Id, team2Id, team2PitcherId, side);

            // Assert
            Utils.PrintDictionary(scoreAverageDictionary);
            Assert.IsTrue(scoreAverageDictionary.Count == 9);
        }

        [TestMethod]
        public void GetMarketsReturnsNonEmptyRecordsetTest()
        {
            // Arrange

            // Act
            Dictionary<int, string> marketsDictionary = _dataAccessMlb.GetMarkets();

            // Assert
            Assert.IsTrue(marketsDictionary.Count > 0);
        }
    }
}