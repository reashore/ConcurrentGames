//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.IO;

//namespace SportsIq.Analytica.Tests
//{
//    [TestClass]
//    public class AnalyticaUnitTests
//    {
//        #region Test Initialization/Cleanup

//        [ClassInitialize]
//        public static void TestFixtureSetup(TestContext context)
//        {
//            // Executes once for the test class. (Optional)

//            //_testDirectory = context.TestDir;
//            //_testRunDirectory = context.TestRunDirectory;
//        }

//        [ClassCleanup]
//        public static void TestFixtureTearDown()
//        {
//            // Runs once after all tests in this class are executed. (Optional)
//            // Not guaranteed that it executes instantly after all tests from the class.
//        }

//        [TestInitialize]
//        public void Setup()
//        {
//            // Runs before each test. (Optional)
//        }

//        [TestCleanup]
//        public void TearDown()
//        {
//            // Runs after each test. (Optional)
//        }

//        #endregion

//        [TestMethod]
//        [DeploymentItem(@"AnalyticaModels\SIQPROPS1.ana")]
//        [TestCategory("Analytica")]
//        public void ReadAnalyticaModelFileFromTestTest()
//        {
//            // Arrange
//            const string fileName = "SIQPROPS1.ana";

//            // Act
//            string testData = File.ReadAllText(fileName);

//            // Assert
//            Assert.IsTrue(testData.Length > 0);
//        }

//        [TestMethod]
//        [DeploymentItem(@"AnalyticaModels\SIQPROPS1.ana")]
//        [TestCategory("Analytica")]
//        public void LoadPLayerStatisticsTablesTableTest()
//        {
//            // Arrange
//            const string analyticaModelFileName = "SIQPROPS1.ana";
//            string analyticaModelFullFileName = Utils.GetFullFileName(analyticaModelFileName);
//            AnalyticaNba analyticaNba = new AnalyticaNba(analyticaModelFullFileName);
//            string[] indexArray = { "iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS" };
//            IEnumerable<NbaPlayer> playerList = GetPlayers();
//            Dictionary<string, double> dictionary = GetLoadedDictionary(playerList);
//            List<string> tableNameList = new List<string> { "POSC", "POTM", "POP", "PSCO" };

//            foreach (string tableName in tableNameList)
//            {
//                CATable caTable = analyticaNba.GetDefTable(tableName);

//                // Act
//                analyticaNba.LoadTable(caTable, indexArray, dictionary);

//                // Assert
//                Array array = analyticaNba.LogTableValues(caTable, indexArray);

//                Assert.IsNotNull(array);
//            }
//        }

//        [TestMethod]
//        [DeploymentItem(@"AnalyticaModels\SIQPROPS1.ana")]
//        [TestCategory("Analytica")]
//        public void InitializeTableTest()
//        {
//            // Arrange
//            const string analyticaModelFileName = "SIQPROPS1.ana";
//            string analyticaModelFullFileName = Utils.GetFullFileName(analyticaModelFileName);
//            AnalyticaNba analyticaNba = new AnalyticaNba(analyticaModelFullFileName);
//            List<string> tableNameList = new List<string> { "POSC", "POTM", "POP", "PSCO" };

//            foreach (string tableName in tableNameList)
//            {
//                CATable caTable = analyticaNba.GetDefTable(tableName);

//                // Act
//                const double initialValue = 0.0;
//                analyticaNba.InitializeTable(caTable, initialValue);

//                // Assert
//                //Array array = analyticaNba.LogTableValues(caTable, indexArray);

//                Assert.IsTrue(true);
//            }
//        }

//        #region Helper Methods

//        //private static IEnumerable<NbaPlayer> GetPlayers()
//        //{
//        //    List<NbaPlayer> playerList = new List<NbaPlayer>
//        //    {
//        //        new NbaPlayer {Number = 1, Side = "home"},
//        //        new NbaPlayer {Number = 2, Side = "home"},
//        //        new NbaPlayer {Number = 3, Side = "home"},
//        //        new NbaPlayer {Number = 4, Side = "home"},
//        //        new NbaPlayer {Number = 5, Side = "home"},
//        //        new NbaPlayer {Number = 6, Side = "home"},
//        //        new NbaPlayer {Number = 7, Side = "home"},
//        //        new NbaPlayer {Number = 8, Side = "home"},
//        //        new NbaPlayer {Number = 9, Side = "home"},
//        //        new NbaPlayer {Number = 10, Side = "home"},
//        //        new NbaPlayer {Number = 11, Side = "home"},
//        //        new NbaPlayer {Number = 12, Side = "home"},
//        //        new NbaPlayer {Number = 13, Side = "home"},
//        //        new NbaPlayer {Number = 14, Side = "home"},
//        //        new NbaPlayer {Number = 15, Side = "home"},

//        //        new NbaPlayer {Number = 1, Side = "away"},
//        //        new NbaPlayer {Number = 2, Side = "away"},
//        //        new NbaPlayer {Number = 3, Side = "away"},
//        //        new NbaPlayer {Number = 4, Side = "away"},
//        //        new NbaPlayer {Number = 5, Side = "away"},
//        //        new NbaPlayer {Number = 6, Side = "away"},
//        //        new NbaPlayer {Number = 7, Side = "away"},
//        //        new NbaPlayer {Number = 8, Side = "away"},
//        //        new NbaPlayer {Number = 9, Side = "away"},
//        //        new NbaPlayer {Number = 10, Side = "away"},
//        //        new NbaPlayer {Number = 11, Side = "away"},
//        //        new NbaPlayer {Number = 12, Side = "away"},
//        //        new NbaPlayer {Number = 13, Side = "away"},
//        //        new NbaPlayer {Number = 14, Side = "away"},
//        //        new NbaPlayer {Number = 15, Side = "away"}
//        //    };

//        //    return playerList;
//        //}

//        //private static Dictionary<string, double> GetLoadedDictionary(IEnumerable<NbaPlayer> playerList)
//        //{
//        //    Dictionary<string, double> dictionary = new Dictionary<string, double>();
//        //    List<int> numberGamesList = new List<int> { 1, 3, 5, 10, 1000 };
//        //    Dictionary<string, string> statisticsTypeDictionary = new Dictionary<string, string>
//        //    {
//        //        ["assists"] = "A",
//        //        ["points"] = "P",
//        //        ["blocks"] = "B",
//        //        ["rebounds"] = "R",
//        //        ["steals"] = "S",
//        //        ["threepointmade"] = "F3M",
//        //        ["turnovers"] = "T"
//        //    };

//        //    foreach (NbaPlayer player in playerList)
//        //    {
//        //        int playerNumber = player.Number;
//        //        string playerSide = player.Side;
//        //        string side = playerSide == "home" ? "H" : "V";

//        //        foreach (KeyValuePair<string, string> statisticKeyValuePair in statisticsTypeDictionary)
//        //        {
//        //            string statisticShortName = statisticKeyValuePair.Value;

//        //            for (int quarter = 1; quarter <= 4; quarter++)
//        //            {
//        //                foreach (int numberGames in numberGamesList)
//        //                {
//        //                    for (int minute = 0; minute < 12; minute++)
//        //                    {
//        //                        string key = $"{side},{statisticShortName},Q{quarter},M{minute},P{playerNumber},T{numberGames}";
//        //                        const double value = 1.0;
//        //                        dictionary[key] = value;
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }

//        //    return dictionary;
//        //}

//        #endregion
//    }
//}
