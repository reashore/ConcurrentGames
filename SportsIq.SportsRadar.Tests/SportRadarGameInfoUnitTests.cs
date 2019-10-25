using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsIq.Utilities;
using System;
using SimpleInjector;
using SportsIq.DependencyInjection;
using SportsIq.Models.SportRadar.Mlb;
using SportsIq.Models.SportRadar.Wnba;

namespace SportsIq.SportsRadar.Tests
{
    [TestClass]
    public class SportRadarGameInfoUnitTests
    {
        [TestMethod]
        public void DeserializeMlbGameInfoTest()
        {
            // Arrange
            Container dependencyInjectionContainer = DependencyInjector.ConfigureMlb();
            IRadarMlb radarMlb = dependencyInjectionContainer.GetInstance<RadarMlb>();
            Guid mlbGameId = Guid.Parse("33bd70eb-03cc-4d18-9798-764d859900b7");

            // Act
            Uri mlbGameInfoUri = radarMlb.GetGameInfoUri(mlbGameId);
            MlbGameInfo mlbGameInfoXml = radarMlb.GetGameInfo(mlbGameId);

            // Assert
            Assert.IsTrue(mlbGameInfoUri.ToString().IsNotNullOrWhiteSpace());
            Assert.IsNotNull(mlbGameInfoXml);
        }

        [TestMethod]
        public void DeserializeWnbaGameInfoTest()
        {
            // Arrange
            Container dependencyInjectionContainer = DependencyInjector.ConfigureMlb();
            IRadarWnba radarWnba = dependencyInjectionContainer.GetInstance<RadarWnba>();

            Guid wnbaGameId = Guid.Parse("0050c156-9057-49b5-9555-0025c09a713d");

            // Act
            Uri wnbaGameInfoUri = radarWnba.GetGameInfoUri(wnbaGameId);
            WnbaGameInfo wnbaGameInfo = radarWnba.GetGameInfo(wnbaGameId);

            // Assert
            Assert.IsTrue(wnbaGameInfoUri.ToString().IsNotNullOrWhiteSpace());
            Assert.IsNotNull(wnbaGameInfo);
        }
    }
}
