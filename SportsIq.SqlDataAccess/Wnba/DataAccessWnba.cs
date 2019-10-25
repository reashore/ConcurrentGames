using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Wnba;
using SportsIq.Utilities;

namespace SportsIq.SqlDataAccess.Wnba
{
    public interface IDataAccessBaseWnba : IDataAccessBase<WnbaGameDto>
    {
    }

    public interface IDataAccessWnba : IDataAccessBaseWnba
    {
        Dictionary<int, string> GetMarkets();
        List<MarketDescription> GetMarketsDescriptions();
        Dictionary<string, double> GetPop(Guid playerId, int playerNumber, string side);
        Dictionary<string, double> GetPosc(Guid playerId, int playerNumber, string side);
        Dictionary<string, double> GetPotm(Guid playerId, int playerNumber, string side, Guid opponentTeamId);
        Dictionary<string, double> GetPsco(Guid playerId, int playerNumber, string side);
        Dictionary<string, double> GetSdom(Guid playerId, int playerNumber, string side);
        Dictionary<string, double> GetSdvtm(Guid playerId, int playerNumber, string side, Guid opponentTeamId);
        Dictionary<string, double> GetTtm(Guid teamId, string side);
        Dictionary<string, double> GetTeamInTss(Guid teamId, string side);
        Dictionary<string, double> GetTeamInTssFge(Guid teamId, string side);
        Dictionary<string, double> GetTeamInSc(Guid teamId, string side);
        Dictionary<string, double> GetTeamIntsf(Guid teamId, string side);
        Dictionary<string, double> GetTeamInLsf(string quarter);
    }

    public class DataAccessWnba : DataAccessBase, IDataAccessWnba
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<WnbaGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                List<WnbaShortGame> wnbaShortGameList = new List<WnbaShortGame>();
                List<WnbaGameDto> wnbaGameList = new List<WnbaGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from wnba.get_games(@number_game_days)";

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

                                return wnbaGameList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("schedule");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");

                                WnbaShortGame wnbaShortGame = new WnbaShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId
                                };

                                wnbaShortGameList.Add(wnbaShortGame);
                            }
                        }
                    }
                }

                foreach (WnbaShortGame wnbaShortGame in wnbaShortGameList)
                {
                    WnbaGameDto wnbaGame = CreateGame(wnbaShortGame);
                    wnbaGameList.Add(wnbaGame);
                }

                return wnbaGameList;
            }
        }

        public List<MarketDescription> GetMarketsDescriptions()
        {
            lock (_lockObject)
            {
                List<MarketDescription> marketsDictionary = new List<MarketDescription>();

                using (NpgsqlConnection mySqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    mySqlConnection.Open();
                    const string storedProcedure = "sports.GetNBAMarkets";

                    using (NpgsqlCommand mySqlCommand = new NpgsqlCommand(storedProcedure, mySqlConnection))
                    {
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        using (NpgsqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                        {
                            if (!mySqlDataReader.HasRows)
                            {
                                const string message = "GetNBAMarkets() has no rows";
                                Logger.Info(message);

                                return marketsDictionary;
                            }

                            while (mySqlDataReader.Read())
                            {
                                MarketDescription md = new MarketDescription
                                {
                                    MarketId = mySqlDataReader.GetInt("market_id"),
                                    Name = mySqlDataReader.GetString("name"),
                                    ShortName = mySqlDataReader.GetString("short_name"),
                                    InPlay = mySqlDataReader.GetInt("inplay"),
                                    Ot = mySqlDataReader.GetInt("ot"),
                                    Period = mySqlDataReader.GetInt("period")
                                };

                                marketsDictionary.Add(md);
                            }

                            return marketsDictionary;
                        }
                    }
                }
            }
        }

        public Dictionary<int, string> GetMarkets()
        {
            lock (_lockObject)
            {
                Dictionary<int, string> marketsDictionary = new Dictionary<int, string>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nba.get_markets()";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetNBAMarkets() has no rows";
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

        //public Dictionary<string, double> GetInTsf(Guid teamId, string sideValue, string statsAspectScope, string statisticType, string periodValue, int numberGames)
        //{
        //    lock (_lockObject)
        //    {
        //        Dictionary<string, double> dictionary = new Dictionary<string, double>();

        //        using (NpgsqlConnection npgsqlConnection = NpgsqlConnection)
        //        {
        //            npgsqlConnection.Open();
        //            const string sqlCommandText = "wnba_stats.GetInTSF";

        //            using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
        //            {
        //                npgsqlCommand.CommandType = CommandType.Text;

        //                NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter(nameof(teamId), teamId);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter1);

        //                NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter(nameof(sideValue), sideValue);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter2);

        //                NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter(nameof(statsAspectScope), statsAspectScope);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter3);

        //                NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter(nameof(statisticType), statisticType);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter4);

        //                NpgsqlParameter npgsqlParameter5 = new NpgsqlParameter(nameof(periodValue), periodValue);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter5);

        //                NpgsqlParameter npgsqlParameter6 = new NpgsqlParameter(nameof(numberGames), numberGames);
        //                npgsqlCommand.Parameters.Add(npgsqlParameter6);

        //                using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
        //                {
        //                    if (!npgsqlDataReader.HasRows)
        //                    {
        //                        const string message = "GetInTsf() has no rows";
        //                        Logger.Info(message);

        //                        return dictionary;
        //                    }

        //                    while (npgsqlDataReader.Read())
        //                    {
        //                        for (int n = 0; n <= 9; n++)
        //                        {
        //                            string key = $"M{n}";
        //                            double value = npgsqlDataReader.GetDouble(key);
        //                            dictionary[key] = value;

        //                            key = $"SD_M{n}";
        //                            value = npgsqlDataReader.GetDouble(key);
        //                            dictionary[key] = value;

        //                            key = $"LG_M{n}";
        //                            value = npgsqlDataReader.GetDouble(key);
        //                            dictionary[key] = value;

        //                            key = $"LG_M{n}SD";
        //                            value = npgsqlDataReader.GetDouble(key);
        //                            dictionary[key] = value;

        //                            key = $"F_M{n}";
        //                            value = npgsqlDataReader.GetDouble(key);
        //                            dictionary[key] = value;
        //                        }
        //                    }

        //                    return dictionary;
        //                }
        //            }
        //        }
        //    }
        //}

        public Dictionary<string, double> GetPop(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from wnba.get_pop(@player_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from wnba.get_posc(@player_id_in)";

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
                    const string sqlCommandText = "select * from wnba.get_potm(@player_id_in, @side_in, @opponent_team_id)";

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
                    const string sqlCommandText = "select * from wnba.get_psco(@player_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from wnba.get_sdom(@player_id_in)";

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
                    const string sqlCommandText = "select * from wnba.get_sdvtm(@player_id_in, @opponent_team_id)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("opponent_team_id", opponentTeamId);
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
                    const string sqlCommandText = "select * from wnba.get_ttm(@team_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from wnba.get_Team_insc(@team_id_in, @side_in)";

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
                                const string message = "GetTeamInSc() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string quarter = npgsqlDataReader.GetString("quarter");

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{side},Q{quarter},M{minute}";
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

        public Dictionary<string, double> GetTeamInTss(Guid teamId, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();
                List<string> statsKey = new List<string> { "F3", "F2", "FT" };
                List<string> stats = new List<string> { "F3Acc", "F2Acc", "FTAcc" };
                List<string> ratios = new List<string> { "F3R", "F2R", "FTR" };

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from wnba.get_intss(@team_id_in, @side_in)";

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
                                int statKeyIndex = 0;

                                foreach (string stat in stats)
                                {
                                    string key = $"{side},A,{statsKey[statKeyIndex]}";
                                    double value = npgsqlDataReader.GetDouble(stat);

                                    dictionary[key] = value;
                                    statKeyIndex++;
                                }

                                statKeyIndex = 0;

                                foreach (string ratio in ratios)
                                {
                                    string key = $"{side},R,{statsKey[statKeyIndex]}";
                                    double value = npgsqlDataReader.GetDouble(ratio);

                                    dictionary[key] = value;
                                    statKeyIndex++;
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
                    const string sqlCommandText = "select * from wnba.get_team_intss_fge(@team_id_in, @side_in)";

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
                                double value = npgsqlDataReader.GetDouble("fge");

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
                    const string sqlCommandText = "select * from wnba.get_team_intsf(@team_id_in, @side_in)";

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

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{ithv},{iqt},{isas},M{minute}";
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

        public Dictionary<string, double> GetTeamInLsf(string quarter)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from wnba.get_team_inlsf(@quarter_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;
                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("quarter_in", quarter);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetTeamInLsf() has no rows";
                                Logger.Info(message);

                                throw new Exception(message);
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iqt = npgsqlDataReader.GetString("iQT");

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{iqt},M{minute}";
                                    double value = npgsqlDataReader.GetDouble($"LG_M{minute}");

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

        #region Private Methods

        private class WnbaShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
        }

        private WnbaGameDto CreateGame(WnbaShortGame wnbaShortGame)
        {
            Guid gameId = wnbaShortGame.GameId;
            DateTime startDateTime = wnbaShortGame.StartDateTime;
            Guid homeTeamId = wnbaShortGame.HomeTeamId;
            Guid awayTeamId = wnbaShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            List<PlayerDto> homePlayerList = GetPlayers(homeTeamId);
            List<PlayerDto> awayPlayerList = GetPlayers(awayTeamId);

            homeTeam.PlayerList = homePlayerList;
            awayTeam.PlayerList = awayPlayerList;

            WnbaGameDto wnbaGameDto = new WnbaGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return wnbaGameDto;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from wnba.get_team(@team_id_in)";

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
                            string shortName = npgsqlDataReader.GetString("abr");

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
                const string sqlCommandText = "select * from wnba.get_players(@team_id_in)";

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

                            PlayerDto playerDto = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName
                            };

                            playerList.Add(playerDto);
                        }

                        return playerList;
                    }
                }
            }
        }

        #endregion
    }

    public class MarketDescription
    {
        public int MarketId;
        public string Name;
        public int Ot;
        public int InPlay;
        public int Prematch;
        public string ShortName;
        public int Period;
    }
}
