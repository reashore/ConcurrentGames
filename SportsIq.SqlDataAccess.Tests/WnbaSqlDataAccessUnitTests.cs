using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsIq.Models.GamesDto.Wnba;

namespace SportsIq.SqlDataAccess.Tests
{
    [TestClass]
    public class WnbaSqlDataAccessUnitTests
    {
        private readonly IDataAccessWnba _dataAccessWnba;

        public WnbaSqlDataAccessUnitTests()
        {
            _dataAccessWnba = new DataAccessWnba();
        }

        [TestMethod]
        public void GetGamesReturnsNonEmptyRecordsetTest()
        {
            // Arrange

            // Act
            List<WnbaGameDto> wnbaGameList = _dataAccessWnba.GetGames();

            // Assert
            Assert.IsTrue(wnbaGameList.Count > 0);
        }

        [TestMethod]
        public void GetMarketsReturnsNonEmptyRecordsetTest()
        {
            // Arrange

            // Act
            Dictionary<int, string> marketsDictionary = _dataAccessWnba.GetMarkets();

            // Assert
            Assert.IsTrue(marketsDictionary.Count > 0);
        }

        [TestMethod]
        public void GetInTsfReturnsNonEmptyRecordsetTest()
        {
            // Arrange
            Guid teamId = Guid.Parse("0699edf3-5993-4182-b9b4-ec935cbd4fcc");
            const string sideValue = "H";
            const string statsAspectScope = "OS";
            const string statisticType = "points";
            const string periodValue = "Q1";
            const int numberGame = 10;

            // Act
            Dictionary<string, double> inTsfDictionary = _dataAccessWnba.GetInTsf(teamId, sideValue, statsAspectScope, statisticType, periodValue, numberGame);

            // Assert
            Assert.IsTrue(inTsfDictionary.Count >= 0);
        }
    }
}