using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Nfl;
using SportsIq.Models.Markets;
using SportsIq.Utilities;

namespace SportsIq.SqlDataAccess.Nfl
{
    public interface IDataAccessBaseNfl : IDataAccessBase<NflGameDto>
    {
    }

    public interface IDataAccessNfl : IDataAccessBaseNfl
    {
        Dictionary<int, string> GetMarkets();
        List<MarketDescription> GetMarketsDescriptions();
        Dictionary<string, double> GetTeamInTss(Guid teamId, string side);
        Dictionary<string, double> GetTeamInTssFge(Guid teamId, string side);
        Dictionary<string, double> GetTeamInSc(Guid teamId, string side);
        Dictionary<string, double> GetTeamIntsf(Guid teamId, string side);
        Dictionary<string, double> GetTeamXs(Guid teamId, string side);
    }

    public class DataAccessNfl : DataAccessBase, IDataAccessNfl
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<NflGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                List<NflShortGame> nflShortGameList = new List<NflShortGame>();
                List<NflGameDto> nflGameDtoList = new List<NflGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_games(@number_game_days)";

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

                                return nflGameDtoList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("start_time");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");
                                Guid? homeQuarterbackId = npgsqlDataReader.GetNullableGuid("home_qb_id");
                                Guid? awayQuarterbackId = npgsqlDataReader.GetNullableGuid("away_qb_id");

                                NflShortGame nflShortGame = new NflShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId,
                                    HomeQuarterBackId = homeQuarterbackId,
                                    AwayQuarterBackId = awayQuarterbackId
                                };

                                nflShortGameList.Add(nflShortGame);
                            }
                        }
                    }
                }

                foreach (NflShortGame nflShortGame in nflShortGameList)
                {
                    NflGameDto nflGame = CreateGameDto(nflShortGame, loadPlayers);
                    nflGameDtoList.Add(nflGame);
                }

                return nflGameDtoList;
            }
        }

        #region Copied Code

        public Dictionary<int, string> GetMarkets()
        {
            lock (_lockObject)
            {
                Dictionary<int, string> marketsDictionary = new Dictionary<int, string>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_markets()";

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
                                int id = npgsqlDataReader.GetInt("market_id");
                                string name = npgsqlDataReader.GetString("name");

                                marketsDictionary[id] = name;
                            }

                            return marketsDictionary;
                        }
                    }
                }
            }
        }

        // todo combine with above function
        public List<MarketDescription> GetMarketsDescriptions()
        {
            lock (_lockObject)
            {
                List<MarketDescription> marketsDictionary = new List<MarketDescription>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_markets()";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetMarketsDescriptions() has no rows";
                                Logger.Info(message);

                                return marketsDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                MarketDescription md = new MarketDescription
                                {
                                    MarketId = npgsqlDataReader.GetInt("market_id"),
                                    Name = npgsqlDataReader.GetString("name")
                                };


                                //md.abbr = npgsqlDataReader.GetString("abbr");
                                //md.inplay = npgsqlDataReader.GetInt32("inplay");
                                //md.ot = npgsqlDataReader.GetInt32("ot");
                                //md.period = npgsqlDataReader.GetInt32("period");

                                marketsDictionary.Add(md);
                            }

                            return marketsDictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetScoreAverage(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_score_average(@team1_id_in, @team2_id_in, @team2_pitcher_id_in, @home_or_visitor_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team1_id_in", team1Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("team2_id_in", team2Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("team2_pitcher_id_in", team2PitcherId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("home_or_visitor_in", homeOrVisitor);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetScoreAverage() has no rows";
                                Logger.Info(message);

                                return defaultDictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                List<double> inningList = new List<double>();
                                string side = npgsqlDataReader.GetString("side");

                                for (int inning = 1; inning <= 9; inning++)
                                {
                                    double inningValue = npgsqlDataReader.GetDouble($"I{inning}");
                                    inningList.Add(inningValue);
                                }

                                for (int inning = 1; inning <= 9; inning++)
                                {
                                    string key = $"I{inning},{side}";
                                    // the inningList is 0-based
                                    dictionary[key] = inningList[inning - 1];
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetInTsf(Guid teamId, string sideValue, string statsAspectScope, string statisticType, string periodValue, int numberGames)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_intsf(@team_id_in, @side_in, @stats_aspect_ratio_in, @statistic_type_in, @period_in, @number_games)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", sideValue);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("stats_aspect_ratio_in", statsAspectScope);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("statistic_type_in", statisticType);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        NpgsqlParameter npgsqlParameter5 = new NpgsqlParameter("period_in", periodValue);
                        npgsqlCommand.Parameters.Add(npgsqlParameter5);

                        NpgsqlParameter npgsqlParameter6 = new NpgsqlParameter("number_games", numberGames);
                        npgsqlCommand.Parameters.Add(npgsqlParameter6);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInTsf() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                for (int n = 0; n <= 9; n++)
                                {
                                    string key = $"M{n}";
                                    double value = npgsqlDataReader.GetDouble(key);
                                    dictionary[key] = value;

                                    key = $"SD_M{n}";
                                    value = npgsqlDataReader.GetDouble(key);
                                    dictionary[key] = value;

                                    key = $"LG_M{n}";
                                    value = npgsqlDataReader.GetDouble(key);
                                    dictionary[key] = value;

                                    key = $"LG_M{n}SD";
                                    value = npgsqlDataReader.GetDouble(key);
                                    dictionary[key] = value;

                                    key = $"F_M{n}";
                                    value = npgsqlDataReader.GetDouble(key);
                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        // todo rename 
        public Dictionary<string, double> GetTeamXs(Guid teamId, string sideValue)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    // todo changed from get_intsf() to get_xs(), otherwise the arguments do not match
                    const string sqlCommandText = "select * from nfl.get_xs(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", sideValue);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetX() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                for (int n = 0; n <= 9; n++)
                                {
                                    string key = $"{sideValue},FGA";
                                    double value = npgsqlDataReader.GetDouble("fga");
                                    dictionary[key] = value;

                                    key = $"{sideValue},PA";
                                    value = npgsqlDataReader.GetDouble("PA");
                                    dictionary[key] = value;

                                    key = $"{sideValue},ScA";
                                    value = npgsqlDataReader.GetDouble("ScA");
                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetPop(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_pop(@player_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetPop() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T{scope}";
                                    double value = npgsqlDataReader.GetDouble($"M{minute}");

                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetPosc(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_posc(@player_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetPosc() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T{scope}";
                                    double value = npgsqlDataReader.GetDouble($"M{minute}");

                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetPotm(Guid playerId, int playerNumber, string side, Guid opponentTeamId)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_potm(@player_id_in, @side_in, @opponent_team_id)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("opponent_team_id", opponentTeamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetPotm() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    double value = npgsqlDataReader.GetDouble($"M{minute}");

                                    string key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T{scope}";
                                    dictionary[key] = value;

                                    key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T10";
                                    dictionary[key] = value;

                                    key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T1000";
                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetPsco(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_psco(@player_id_in, @)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetPsco() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    double value = npgsqlDataReader.GetDouble($"M{minute}");

                                    string key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T{scope}";
                                    dictionary[key] = value;

                                    key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T10";
                                    dictionary[key] = value;

                                    key = $"{side},{iMtr},{iQt},M{minute},P{playerNumber},T1000";
                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetSdom(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select *  from nfl.get_sdom(@player_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetSdom() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string key = $"{side},{iMtr},P{playerNumber}";
                                double value = npgsqlDataReader.GetDouble("CG");

                                dictionary[key] = value;
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetSdvtm(Guid playerId, int playerNumber, string side, Guid opponentTeamId)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_sdvtm(@player_id_in, @)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("opponent_team_id_in", opponentTeamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetSdvtm() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string key = $"{side},{iMtr},P{playerNumber}";
                                double value = npgsqlDataReader.GetDouble("CG");

                                dictionary[key] = value;
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetTtm(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_ttm(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                //const string message = "GetTtm() has no rows";
                                //Logger.Info(message);

                                return dictionary;
                            }

                            List<string> periodList = new List<string> { "CG", "H1", "H2", "Q1", "Q2", "Q3", "Q4" };

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");

                                foreach (string period in periodList)
                                {
                                    string key = $"{side},{period},{iMtr}";
                                    double value = npgsqlDataReader.GetDouble(period);

                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetTeamInSc(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_team_insc(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        // todo why is this parameter hard-coded to "A" ************************
                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", "A");
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInSc() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iqt = npgsqlDataReader.GetString("iQT");
                                string ithv = side;
                                // todo why is this parameter not used
                                //string isas = npgsqlDataReader.GetString("iSAS");

                                double total = 0;
                                for (int minute = 0; minute <= 14; minute++)
                                {
                                    total += npgsqlDataReader.GetDouble($"avg_m{minute}");
                                }

                                for (int minute = 0; minute <= 14; minute++)
                                {
                                    string key = $"{ithv},{iqt},M{minute}";
                                    double value = npgsqlDataReader.GetDouble($"avg_m{minute}");

                                    dictionary[key] = value / total;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetTeamInTss(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();
                List<string> stats = new List<string> { "F3Acc", "F2Acc", "FTAcc" };
                List<string> ratios = new List<string> { "F3R", "F2R", "FTR" };

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_intss(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInTss() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                foreach (string stat in stats)
                                {
                                    string key = $"{side},A,{stat}";
                                    double value = npgsqlDataReader.GetDouble(stat);

                                    dictionary[key] = value;
                                }

                                foreach (string ratio in ratios)
                                {
                                    string key = $"{side},R,{ratio}";
                                    double value = npgsqlDataReader.GetDouble(ratio);

                                    dictionary[key] = value;
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetTeamInTssFge(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_team_intss_fge(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetInTssFge() has no rows";
                                Logger.Info(message);

                                throw new Exception(message);
                            }

                            while (npgsqlDataReader.Read())
                            {
                                double value = npgsqlDataReader.GetDouble("FGE");

                                string key = $"{side},A,FGE";
                                dictionary[key] = value;

                                key = $"{side},R,FGE";
                                dictionary[key] = 1;
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<string, double> GetTeamIntsf(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nfl.get_team_intsf(@team_id_in, @side_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetTeamIntsf() has no rows";
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

        #endregion 

        #endregion

        #region Private Methods

        private class NflShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
            public Guid? HomeQuarterBackId { get; set; }
            public Guid? AwayQuarterBackId { get; set; }
        }

        private NflGameDto CreateGameDto(NflShortGame mlbShortGame, bool loadPlayers)
        {
            if (loadPlayers)
            {
                return CreateGameDtoWithPlayers(mlbShortGame);
            }

            return CreateGameDtoWithoutPlayers(mlbShortGame);
        }

        private NflGameDto CreateGameDtoWithPlayers(NflShortGame nflShortGame)
        {
            Guid gameId = nflShortGame.GameId;
            DateTime startDateTime = nflShortGame.StartDateTime;
            Guid homeTeamId = nflShortGame.HomeTeamId;
            Guid awayTeamId = nflShortGame.AwayTeamId;

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

            // todo get the quarterbacks

            NflGameDto nflGameDto = new NflGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nflGameDto;
        }

        private NflGameDto CreateGameDtoWithoutPlayers(NflShortGame nflShortGame)
        {
            Guid gameId = nflShortGame.GameId;
            DateTime startDateTime = nflShortGame.StartDateTime;
            Guid homeTeamId = nflShortGame.HomeTeamId;
            Guid awayTeamId = nflShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            // todo get the quarterbacks

            NflGameDto nflGameDto = new NflGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nflGameDto;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from nfl.get_team(@team_id_in)";

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
                            Logger.Error(message);
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
                // todo why not just get_players()
                const string sqlCommandText = "select * from nfl.get_players1(@team_id_in)";

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
                            Logger.Error(message);

                            return playerList;
                        }

                        while (npgsqlDataReader.Read())
                        {
                            Guid playerId = npgsqlDataReader.GetGuid("player_id");
                            string fullName = npgsqlDataReader.GetString("full_name");

                            PlayerDto nflPlayer = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName
                            };

                            playerList.Add(nflPlayer);
                        }

                        return playerList;
                    }
                }
            }
        }

        #endregion


    }
}
