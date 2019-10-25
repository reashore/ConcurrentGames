//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SportsIq.Utilities;
//using System;
//using SportsIq.DependencyInjection;
//using SportsIq.Models.SportRadar.Mlb;
//using Conatiner = SimpleInjector.Container;

//namespace SportsIq.SportsRadar.Tests
//{
//    [TestClass]
//    public class SportRadarGameInfoUnitTests
//    {
//        [TestMethod]
//        public void DeserializeMlbGameInfoTest()
//        {
//            // Arrange
//            Conatiner dependencyInjectionContainer = DependencyInjector.ConfigureMlb();
//            IRadarMlb radarMlb = dependencyInjectionContainer.GetInstance<RadarMlb>();
//            Guid mlbGameId = Guid.Parse("33bd70eb-03cc-4d18-9798-764d859900b7");

//            // Act
//            Uri mbaGameInfoUri = radarMlb.GetGameInfoUri(mlbGameId);
//            MlbGameInfo mlbGameInfoJson = radarMlb.GetGameInfo(mlbGameId);

//            // Assert
//            Assert.IsTrue(mbaGameInfoUri.ToString().IsNotNullOrWhiteSpace());
//            Assert.IsNotNull(mlbGameInfoJson);
//        }

//        //[TestMethod]
//        //public void DeserializeNbaGameInfoTest()
//        //{
//        //    // Arrange
//        //    IRadarNba radarNba = new RadarNba();
//        //    Guid nbaGameId = Guid.Parse("013dd2a7-fec4-4cc5-b819-f3cf16a1f820");

//        //    // Act
//        //    Uri nbaGameInfoUri = radarNba.GetGameInfoUri(nbaGameId);
//        //    NbaGameInfo nbaGameInfo = radarNba.GetGameInfo(nbaGameId);

//        //    // Assert
//        //    Assert.IsTrue(nbaGameInfoUri.ToString().IsNotNullOrWhiteSpace());
//        //    Assert.IsNotNull(nbaGameInfo);
//        //}

//        //[TestMethod]
//        //public void DeserializeWmbaGameInfoTest()
//        //{
//        //    // Arrange
//        //    IRadarWnba radarWnba = new RadarWnba();
//        //    Guid wnbaGameId = Guid.Parse("24f58f8f-7743-4229-8cdc-0ff13689e792");

//        //    // Act
//        //    Uri wnbaGameInfoUri = radarWnba.GetGameInfoUri(wnbaGameId);
//        //    WnbaGameInfoJson wnbaGameInfoJson = radarWnba.GetGameInfo(wnbaGameId);

//        //    // Assert
//        //    Assert.IsTrue(wnbaGameInfoUri.ToString().IsNotNullOrWhiteSpace());
//        //    Assert.IsNotNull(wnbaGameInfoJson);
//        //}

//    }

//    //[TestClass]
//    //public class SportRadarGameEventUnitTests
//    //{
//    //    [TestMethod]
//    //    public async Task DeserializeMlbGameEventTest()
//    //    {
//    //        // Arrange
//    //        IRadarMlb radarMlb = new RadarMlb();

//    //        // Act
//    //        Uri mlbGameEventUri = radarMlb.GetGameEventUri();
//    //        MlbGameEvent mlbGameEvent = await radarMlb.GetGameEvent();

//    //        // Assert
//    //        Assert.IsTrue(mlbGameEventUri.ToString().IsNotNullOrWhiteSpace());
//    //        Assert.IsNotNull(mlbGameEvent);
//    //    }

//    //    [TestMethod]
//    //    public async Task DeserializeNbaGameEventTest()
//    //    {
//    //        // Arrange
//    //        IRadarNba radarNba = new RadarNba();

//    //        // Act
//    //        Uri nbaGameEventUri = radarNba.GetGameEventUri();
//    //        NbaGameEvent nbaGameEvent = await radarNba.GetGameEvent();

//    //        // Assert
//    //        Assert.IsTrue(nbaGameEventUri.ToString().IsNotNullOrWhiteSpace());
//    //        Assert.IsNotNull(nbaGameEvent);
//    //    }

//    //    [TestMethod]
//    //    public async Task DeserializeWnbaGameEventTest()
//    //    {
//    //        // Arrange
//    //        IRadarWnba radarWnba = new RadarWnba();

//    //        // Act
//    //        Uri wnbaGameEventUri = radarWnba.GetGameEventUri();
//    //        WnbaGameEvent wnbaGameEvent = await radarWnba.GetGameEvent();

//    //        // Assert
//    //        Assert.IsTrue(wnbaGameEventUri.ToString().IsNotNullOrWhiteSpace());
//    //        Assert.IsNotNull(wnbaGameEvent);
//    //    }

//    //    [TestMethod]
//    //    public async Task DeserializeNhlGameEventTest()
//    //    {
//    //        // Arrange
//    //        IRadarNhl radarNhl = new RadarNhl();

//    //        // Act
//    //        Uri nhlGameEventUri = radarNhl.GetGameEventUri();
//    //        NhlGameEvent nhlGameEvent = await radarNhl.GetGameEvent();

//    //        // Assert
//    //        Assert.IsTrue(nhlGameEventUri.ToString().IsNotNullOrWhiteSpace());
//    //        Assert.IsNotNull(nhlGameEvent);
//    //    }
//    //}
//}
