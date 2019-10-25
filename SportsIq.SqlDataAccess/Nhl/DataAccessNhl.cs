using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Nhl;

namespace SportsIq.SqlDataAccess.Nhl
{
    // todo move to common market area
    public class NhlMarket
    {
        public int MarketId { get; set; }
        public string Name { get; set; }
        public int OverTime { get; set; }
        public int Inplay { get; set; }
        public int Prematch { get; set; }
        public string ShortName { get; set; }
        public int Period { get; set; }
    }

    public interface IDataAccessBaseNhl : IDataAccessBase<NhlGameDto>
    {
    }

    public interface IDataAccessNhl : IDataAccessBaseNhl
    {
        Dictionary<string, double> GetScoreAverage1(Guid teamId, Guid opponentTeamId, string statistic, string numberGames);
        Dictionary<string, double> GetScoreAverage2(Guid teamId, string statistic, string numberGames);
        Dictionary<int, NhlMarket> GetMarkets();
        Dictionary<string, double> GetInggp(Guid goalieId, int primary, string side);
        Dictionary<string, double> GetIngsl(Guid goalieId, int numberGames, int primary, string side, string stat, string querystat);
        Dictionary<string, double> GetIngst(Guid goalieId, Guid opponentId, int numberGames, int primary, string side, string stat, string querystat);



        // void GetIngst(Guid goalieId, Guid opponentId, int numberGames);
        Dictionary<string, double> GetInsgp(Guid playerId, string side, int playerNumber);

        Dictionary<string, double> GetInssg(Guid playerId, Guid goalieId, int numberGames, int primary, string side, int playerNumber, string stat, string querystat);
        Dictionary<string, double> GetInssl(Guid playerId, int numberGames, string side, int playerNumber, string stat, string querystat);
        Dictionary<string, double> GetInsst(Guid playerId, Guid opponentId, int numberGames, string side, int playerNumber, string stat, string querystat);
    }

    public class DataAccessNhl : DataAccessBase, IDataAccessNhl
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<NhlGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                //
               // loadPlayers = true;
                List<NhlShortGame> nhlShortGameList = new List<NhlShortGame>();
                List<NhlGameDto> nhlGameDtoList = new List<NhlGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                  //  const string sqlCommandText = "select * from nhl.get_games(@number_game_days)";
                    const string sqlCommandText = @"select * from nhl.game where
                    date(start_time - interval '8 hour') >= date(timezone('utc', now() - interval '10 hour'))  and
                    date(start_time - interval '8 hour')  <= date(start_time + interval '3 Day' - interval '8 hour')
                    order by start_time asc limit 10;";
                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("number_game_days", numberGameDays);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetGames() has no rows";
                                Logger.Info(message);

                                return nhlGameDtoList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("start_time");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");

                                NhlShortGame nhlShortGame = new NhlShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId
                                };

                                nhlShortGameList.Add(nhlShortGame);
                            }
                        }
                    }
                }

                foreach (NhlShortGame nhlShortGame in nhlShortGameList)
                {
                    NhlGameDto nhlGameDto = CreateGameDto(nhlShortGame, loadPlayers);
                    nhlGameDtoList.Add(nhlGameDto);
                }

                return nhlGameDtoList;
            }
        }

        public Dictionary<int, NhlMarket> GetMarkets()
        {
            lock (_lockObject)
            {
                Dictionary<int, NhlMarket> marketsDictionary = new Dictionary<int, NhlMarket>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_markets()";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetMarkets() has no rows";
                                Logger.Info(message);

                                return marketsDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                int marketId = npgsqlDataReader.GetInt("market_id");
                                string name = npgsqlDataReader.GetString("name");
                              //  int overtime = npgsqlDataReader.GetInt("overtime");
                                //int inplay = npgsqlDataReader.GetInt("inplay");
                                //int prematch = npgsqlDataReader.GetInt("prematch");
                                string shortName = npgsqlDataReader.GetString("short_name");
                                int period = npgsqlDataReader.GetInt("period");

                                NhlMarket nhlMarket = new NhlMarket
                                {
                                    MarketId = marketId,
                                    Name = name,
                                  //  OverTime = overtime,
                                //    Inplay = inplay,
                                //    Prematch = prematch,
                                    ShortName = shortName,
                                    Period = period

                                };

                                marketsDictionary[marketId] = nhlMarket;
                            }

                            return marketsDictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetScoreAverage1(Guid teamId, Guid opponentTeamId, string statistic, string numberGames)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_score_average1(@team_id_in, @opponent_id_in, @statistic, @number_games)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("opponent_id_in", opponentTeamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("statistic", statistic);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("number_games", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetScoreAverage1() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                //List<double> inningList = new List<double>();
                                //string side = npgsqlDataReader.GetString("side");

                                //for (int inning = 1; inning <= 9; inning++)
                                //{
                                //    double inningValue = npgsqlDataReader.GetDouble($"I{inning}");
                                //    inningList.Add(inningValue);
                                //}

                                //for (int inning = 1; inning <= 9; inning++)
                                //{
                                //    string key = $"I{inning},{side}";
                                //    // note that the inningList is 0-based
                                //    dictionary[key] = inningList[inning - 1];
                                //}
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetScoreAverage2(Guid teamId, string statistic, string numberGames)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_score_average1(@team_id_in, @statistic, @number_games)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("statistic", statistic);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("number_games", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetScoreAverage2() has no rows";
                                Logger.Info(message);

                                throw new Exception(message);
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string ithv = npgsqlDataReader.GetString("iTVH");
                                string iqt = npgsqlDataReader.GetString("iQt");
                                string isas = npgsqlDataReader.GetString("iSAS");

                                for (int minute = 0; minute <= 14; minute++)
                                {
                                    string key = $"{ithv},{iqt},{isas},M{minute}";

                                    double value = npgsqlDataReader.GetDouble($"M{minute}");

                                    if (isas == "DS")
                                    {
                                        value = npgsqlDataReader.GetDouble($"F_M{minute}");
                                    }

                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetInggp(Guid goalieId, int primary, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_inggp(@goalie_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("goalie_id_in", goalieId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInggp() has no rows";
                                Logger.Info(message);
                                //return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {

                                string key = $"{side},G{primary},L";
                                double gamesPlayed = npgsqlDataReader.GetDouble("games_played");
                                dictionary.Add(key,gamesPlayed);
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetIngsl(Guid goalieId, int numberGames, int primary, string side, string stat, string querystat)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_ingsl(@goalie_id_in, @statistic_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("goalie_id_in", goalieId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("statistic_in", querystat);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("number_games_in", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetIngsl() has no rows";
                                Logger.Info(message);
                                //return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                try
                                {
                                    double average = npgsqlDataReader.GetDouble("average");
                                    string key = $"{side},P0,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p1");
                                    key = $"{side},P1,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p2");
                                    key = $"{side},P2,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p3");
                                    key = $"{side},P3,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                   // average = npgsqlDataReader.GetDouble("average_p4");
                                   // key = $"{side},P4,{stat},T{numberGames},G{primary}";
                                   // dictionary.Add(key, average);
                                }
                                catch (Exception e)
                                {
                                    //
                                }


                            }

                            return dictionary;
                        }
                    }
                }
            }

        }

        public Dictionary<string, double> GetIngst(Guid goalieId, Guid opponentId, int numberGames, int primary, string side, string stat, string querystat)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_ingst(@goalie_id_in, @opponent_id_in, @statistic_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("goalie_id_in", goalieId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("opponent_id_in", opponentId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("statistic_in", querystat);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("number_games_in", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetIngst() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                try
                                {
                                    double average = npgsqlDataReader.GetDouble("average");
                                    string key = $"{side},P0,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p1");
                                    key = $"{side},P1,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p2");
                                    key = $"{side},P2,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);

                                    average = npgsqlDataReader.GetDouble("average_p3");
                                    key = $"{side},P3,{stat},T{numberGames},G{primary}";
                                    dictionary.Add(key, average);


                                //    average = npgsqlDataReader.GetDouble("average_p4");
                                //    key = $"{side},P3,{stat},T{numberGames},G{primary},{side}";
                                //    dictionary.Add(key, average);


                                }
                                catch (Exception e)
                                {
                                    //
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetInsgp(Guid playerId, string side, int playerNumber)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_insgp(@player_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInsgp() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                int gamesPlayed = npgsqlDataReader.GetInt("games_played");
                                string key = $"{side},P{playerNumber},L";
                                dictionary.Add(key,gamesPlayed);
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetInssg(Guid playerId, Guid goalieId, int numberGames,  int primary,  string side, int playerNumber, string stat, string querystat)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_inssg(@player_id_in, @goalie_id_in, @statistic_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("goalie_id_in", goalieId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);


                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("statistic_in", querystat);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("number_games_in", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInssg() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                try
                                {
                                    double average = npgsqlDataReader.GetDouble("average");
                                    string key = $"{side},P0,G{primary},{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, average);


                                     average = npgsqlDataReader.GetDouble("average_p1");
                                     key = $"{side},P1,G{primary},{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p2");
                                    key = $"{side},P2,G{primary},{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, average);


                                    average = npgsqlDataReader.GetDouble("average_p3");
                                    key = $"{side},P3,G{primary},{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, average);

                                    



                                }
                                catch (Exception e)
                                {
                          //           Logger.Info(e);
                                }

                           

                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetInssl(Guid playerId, int numberGames , string side, int playerNumber, string stat, string querystat)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_inssl(@player_id_in, @statistic_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("statistic_in", querystat);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("number_games_in", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInssl() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                try
                                {
                                    //{ "iTVH", "iGP", "iSMT", "iTS", "iPLY" }},
                                    string key = $"{side},P0,{stat},T{numberGames},P{playerNumber}";
                                    double average = npgsqlDataReader.GetDouble("average");
                                    dictionary.Add(key, average);

                                    key = $"{side},P1,{stat},T{numberGames},P{playerNumber}";
                                    average = npgsqlDataReader.GetDouble("average_p1");
                                    dictionary.Add(key, average);

                                    key = $"{side},P2,{stat},T{numberGames},P{playerNumber}";
                                    average = npgsqlDataReader.GetDouble("average_p2");
                                    dictionary.Add(key, average);

                                    key = $"{side},P3,{stat},T{numberGames},P{playerNumber}";
                                    average = npgsqlDataReader.GetDouble("average_p3");
                                    dictionary.Add(key, average);
                                    
                                }
                                catch (Exception e)
                                {
                                    //
                                }
                            }

                            return dictionary;
                        }

                    }
                }
            }
        }

        public Dictionary<string, double> GetInsst(Guid playerId, Guid opponentId, int numberGames, string side, int playerNumber, string stat, string querystat)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nhl.get_insst(@player_id_in, @opponent_id_in, @statistic_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("opponent_id_in", opponentId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("statistic_in", querystat);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("number_games_in", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInsst() has no rows";
                                Logger.Info(message);
                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {

                                try
                                {
                                    double averagePoints = npgsqlDataReader.GetDouble("average");
                                    string key = $"{side},P0,{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, averagePoints);


                                    averagePoints = npgsqlDataReader.GetDouble("average_p1");
                                    key = $"{side},P1,{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, averagePoints);


                                    averagePoints = npgsqlDataReader.GetDouble("average_p2");
                                    key = $"{side},P2,{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, averagePoints);


                                    averagePoints = npgsqlDataReader.GetDouble("average_p3");
                                    key = $"{side},P3,{stat},T{numberGames},P{playerNumber}";
                                    dictionary.Add(key, averagePoints);

                                    
                                }
                                catch (Exception e)
                                {
                                              Logger.Info(e);
                                }

                                
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private class NhlShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
        }

        private NhlGameDto CreateGameDto(NhlShortGame nhlShortGame, bool loadPlayers)
        {
            if (loadPlayers)
            {
                return CreateGameDtoWithPlayers(nhlShortGame);
            }

            return CreateGameDtoWithoutPlayers(nhlShortGame);
        }

        private NhlGameDto CreateGameDtoWithPlayers(NhlShortGame nhlShortGame)
        {
            Guid gameId = nhlShortGame.GameId;
            DateTime startDateTime = nhlShortGame.StartDateTime;
            Guid homeTeamId = nhlShortGame.HomeTeamId;
            Guid awayTeamId = nhlShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            List<PlayerDto> homePlayerList = GetPlayers(homeTeamId);
            List<PlayerDto> awayPlayerList = GetPlayers(awayTeamId);

            if (homePlayerList == null || homePlayerList.Count == 0)
            {
                Logger.Info("CreateGameDtoWithPlayers() error: homePlayerList is null or empty");
            }

            if (awayPlayerList == null || awayPlayerList.Count == 0)
            {
                Logger.Info("CreateGameDtoWithPlayers() error: awayPlayerList is null or empty");
            }

            homeTeam.PlayerList = homePlayerList;
            awayTeam.PlayerList = awayPlayerList;

            NhlGameDto nhlGameDto = new NhlGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nhlGameDto;
        }

        private NhlGameDto CreateGameDtoWithoutPlayers(NhlShortGame nhlShortGame)
        {
            Guid gameId = nhlShortGame.GameId;
            DateTime startDateTime = nhlShortGame.StartDateTime;
            Guid homeTeamId = nhlShortGame.HomeTeamId;
            Guid awayTeamId = nhlShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            NhlGameDto nhlGameDto = new NhlGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nhlGameDto;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from nhl.get_team(@team_id_in)";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                {
                    npgsqlCommand.CommandType = CommandType.Text;

                    NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                    npgsqlCommand.Parameters.Add(npgsqlParameter1);

                    using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        if (!npgsqlDataReader.HasRows)
                        {
                            const string message = "GetTeam() has no rows";
                            Logger.Info(message);
                            throw new Exception(message);
                        }

                        while (npgsqlDataReader.Read())
                        {
                            string name = npgsqlDataReader.GetString("name");
                            string shortName = npgsqlDataReader.GetString("short_name");

                            teamDto = new TeamDto
                            {
                                TeamId = teamId,
                                Name = name,
                                ShortName = shortName
                            };
                        }

                        return teamDto;
                    }
                }
            }
        }

        private List<PlayerDto> GetPlayers(Guid teamId)
        {
            List<PlayerDto> playerList = new List<PlayerDto>();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from nhl.get_players(@team_id_in)";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                {
                    npgsqlCommand.CommandType = CommandType.Text;

                    NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                    npgsqlCommand.Parameters.Add(npgsqlParameter1);

                    using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        if (!npgsqlDataReader.HasRows)
                        {
                            const string message = "GetPlayers() has no rows";
                            Logger.Info(message);
                            throw new Exception(message);
                        }

                        while (npgsqlDataReader.Read())
                        {
                            Guid playerId = npgsqlDataReader.GetGuid("player_id");
                            string fullName = npgsqlDataReader.GetString("full_name");
                            int jerseyNumber = npgsqlDataReader.GetInt("jersey");
                            string position = npgsqlDataReader.GetString("pos");

                            PlayerDto nhlPlayerDto = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName,
                                Number = jerseyNumber,
                                Position = position
                            };

                            playerList.Add(nhlPlayerDto);
                        }

                        return playerList;
                    }
                }
            }
        }

        #endregion
    }
}
