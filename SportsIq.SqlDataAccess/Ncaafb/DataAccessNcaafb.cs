using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Ncaafb;
using SportsIq.Utilities;

namespace SportsIq.SqlDataAccess.Ncaafb
{
    public interface IDataAccessBaseNcaafb : IDataAccessBase<NcaafbGameDto>
    {
    }

    public interface IDataAccessNcaafb : IDataAccessBaseNcaafb
    {
        Dictionary<string, double> GetScoreAverage(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor);
        Dictionary<int, string> GetMarkets();
        Dictionary<string, double> GetInTsf(Guid teamId, string sideValue, string statsAspectScope, string statisticType, string periodValue, int numberGames);
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
    }

    public class DataAccessNcaafb : DataAccessBase, IDataAccessNcaafb
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<NcaafbGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                List<NcaafbShortGame> ncaafbShortGameList = new List<NcaafbShortGame>();
                List<NcaafbGameDto> ncaafbGameDtoList = new List<NcaafbGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from ncaafb.get_games(@number_game_days)";

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

                                return ncaafbGameDtoList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("schedule");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");

                                NcaafbShortGame ncaafbShortGame = new NcaafbShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId
                                };

                                ncaafbShortGameList.Add(ncaafbShortGame);
                            }
                        }
                    }
                }

                foreach (NcaafbShortGame ncaafbShortGame in ncaafbShortGameList)
                {
                    NcaafbGameDto ncaafbGame = CreateGameDto(ncaafbShortGame, loadPlayers);
                    ncaafbGameDtoList.Add(ncaafbGame);
                }

                return ncaafbGameDtoList;
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
                    const string sqlCommandText = "select * from ncaafb.get_markets()";

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
                                int id = npgsqlDataReader.GetInt("id");
                                string name = npgsqlDataReader.GetString("name");

                                marketsDictionary[id] = name;
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
                    const string sqlCommandText = "select * from ncaafb.get_score_average(@team1_id_in, @team2_id_in, @team2_pitcher_id_in, @home_or_visitor_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_instf(@team_id_in, @side_in, @stats_aspect_scope_in, @statistic_type_in, @period_in, @number_games_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        // todo rename sideValue?
                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", sideValue);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("stats_aspect_scope_in", statsAspectScope);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("statistic_type_in", statisticType);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        // todo rename periodValue?
                        NpgsqlParameter npgsqlParameter5 = new NpgsqlParameter("period_in", periodValue);
                        npgsqlCommand.Parameters.Add(npgsqlParameter5);

                        NpgsqlParameter npgsqlParameter6 = new NpgsqlParameter("number_games_in", numberGames);
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

        public Dictionary<string, double> GetPop(Guid playerId, int playerNumber, string side)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from ncaafb.get_pop()";

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
                    const string sqlCommandText = "select * from ncaafb.get_posc()";

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
                    const string sqlCommandText = "select * from ncaafb.get_potm";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("side_in", side);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("opponent_team_id_in", opponentTeamId);
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
                    const string sqlCommandText = "select * from ncaafb.get_psco(@player_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_sdom(@player_id_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_svdtm(@player_id_in, @opponent_team_id)";

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
                    const string sqlCommandText = "select * from ncaafb.get_ttm(@team_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_team_insc(@team_id_in, @side_in)";

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
                                const string message = "GetInSc() has no rows";
                                Logger.Info(message);

                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string quarter = npgsqlDataReader.GetString("quarter");

                                for (int minute = 0; minute <= 9; minute++)
                                {
                                    string key = $"{side},Q{quarter},AvgM{minute}";
                                    double value = npgsqlDataReader.GetDouble(minute);

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
                List<string> stats = new List<string> { "F3Acc", "F2Acc", "FTAcc" };
                List<string> ratios = new List<string> { "F3R", "F2R", "FTR" };

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from ncaafb.get_intss(@team_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_team_intssfge(@team_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from ncaafb.get_team_intsf(@team_id_in, @side_in)";

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
                                    double value = npgsqlDataReader.GetDouble($"F_M{minute}");

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

        private class NcaafbShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
        }

        private NcaafbGameDto CreateGameDto(NcaafbShortGame mlbShortGame, bool loadPlayers)
        {
            if (loadPlayers)
            {
                return CreateGameDtoWithPlayers(mlbShortGame);
            }

            return CreateGameDtoWithoutPlayers(mlbShortGame);
        }

        private NcaafbGameDto CreateGameDtoWithPlayers(NcaafbShortGame ncaafbShortGame)
        {
            Guid gameId = ncaafbShortGame.GameId;
            DateTime startDateTime = ncaafbShortGame.StartDateTime;
            Guid homeTeamId = ncaafbShortGame.HomeTeamId;
            Guid awayTeamId = ncaafbShortGame.AwayTeamId;

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

            NcaafbGameDto ncaafbGameDto = new NcaafbGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return ncaafbGameDto;
        }

        private NcaafbGameDto CreateGameDtoWithoutPlayers(NcaafbShortGame ncaafbShortGame)
        {
            Guid gameId = ncaafbShortGame.GameId;
            DateTime startDateTime = ncaafbShortGame.StartDateTime;
            Guid homeTeamId = ncaafbShortGame.HomeTeamId;
            Guid awayTeamId = ncaafbShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            // todo get the quarterbacks

            NcaafbGameDto ncaafbGameDto = new NcaafbGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return ncaafbGameDto;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from ncaafb.get_team(@team_id_in)";

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
                const string sqlCommandText = "select * from ncaafb.get_players1(@team_id_in)";

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

                            PlayerDto ncaafbPlayer = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName
                            };

                            playerList.Add(ncaafbPlayer);
                        }

                        return playerList;
                    }
                }
            }
        }

        #endregion
    }
}
