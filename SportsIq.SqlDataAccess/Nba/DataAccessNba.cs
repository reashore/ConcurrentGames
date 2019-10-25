using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Nba;
using SportsIq.Models.Markets;
using SportsIq.Utilities;

namespace SportsIq.SqlDataAccess.Nba
{
    public interface IDataAccessBaseNba : IDataAccessBase<NbaGameDto>
    {
    }

    public interface IDataAccessNba : IDataAccessBaseNba
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

    public class DataAccessNba : DataAccessBase, IDataAccessNba
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<NbaGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                List<NbaShortGame> nbaShortGameList = new List<NbaShortGame>();
                List<NbaGameDto> nbaGameList = new List<NbaGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nba.get_games(@number_games_days)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("number_games_days", numberGameDays);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetGames() has no rows";
                                Logger.Info(message);

                                return nbaGameList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("start_time");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");

                                NbaShortGame nbaShortGame = new NbaShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId
                                };

                                nbaShortGameList.Add(nbaShortGame);
                            }
                        }
                    }
                }

                foreach (NbaShortGame nbaShortGame in nbaShortGameList)
                {
                    NbaGameDto nbaGame = CreateGameDto(nbaShortGame, loadPlayers);
                    nbaGameList.Add(nbaGame);
                }

                return nbaGameList;
            }
        }

        // todo the  next two functions can be combined into one
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

        public List<MarketDescription> GetMarketsDescriptions()
        {
            lock (_lockObject)
            {
                List<MarketDescription> marketsDictionary = new List<MarketDescription>();

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
                                MarketDescription md = new MarketDescription
                                {
                                    MarketId = npgsqlDataReader.GetInt("market_id"),
                                    Name = npgsqlDataReader.GetString("name"),
                                    ShortName = npgsqlDataReader.GetString("abbr"),
                                    InPlay = npgsqlDataReader.GetInt("inplay"),
                                    Overtime = npgsqlDataReader.GetInt("ot"),
                                    Period = npgsqlDataReader.GetInt("period")
                                };

                                marketsDictionary.Add(md);
                            }

                            return marketsDictionary;
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
                    const string sqlCommandText = "select * from nba.get_pop(@player_id_in, @side_in)";

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

                                for (int minute = 0; minute <= 11; minute++)
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

                    const string sqlCommandText = "select * from nba.get_posc(@player_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetPosc() has no rows";
                                Logger.Info(message);
                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 11; minute++)
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
                    const string sqlCommandText = "select * from nba.get_potm(@player_id_in, @side_in, @opponent_team_id)";

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
                                const string message = "GetPotm() has no rows";
                                Logger.Info(message);
                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 11; minute++)
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
                    const string sqlCommandText = "select * from nba.get_psco(@player_id_in, @side_in)";

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
                                const string message = "GetPsco() has no rows";
                                Logger.Info(message);
                                return dictionary;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                string iMtr = npgsqlDataReader.GetString("iMTR");
                                string iQt = npgsqlDataReader.GetString("iQT");
                                int scope = npgsqlDataReader.GetInt("iTS");
                                scope = Utils.AdjustScope(scope);

                                for (int minute = 0; minute <= 11; minute++)
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
                    const string sqlCommandText = "select * from nba.get_sdom(@player_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetSdom() has no rows";
                                Logger.Info(message);
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
                    const string sqlCommandText = "select * from nba.get_sdvtm(@player_id_in, @opponent_team_id)";

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
                    const string sqlCommandText = "select * from nba.get_ttm(@team_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetTtm() has no rows";
                                Logger.Info(message);

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
                    const string sqlCommandText = "select * from nba.get_team_insc(@team_id_in, @side_in)";

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

                                for (int minute = 0; minute <= 11; minute++)
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
                    const string sqlCommandText = "select * from nba.get_intss(@team_id_in, @side_in)";

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
                    const string sqlCommandText = "select * from nba.get_team_intss_fge()";

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
                    const string sqlCommandText = "select * from nba.get_team_intsf(@team_id_in, @side_in)";

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

                                for (int minute = 0; minute <= 11; minute++)
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

        public Dictionary<string, double> GetTeamInLsf(string quarter)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from nba.get_team_inlsf(@quarter_in)";

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

                                for (int minute = 0; minute <= 11; minute++)
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

        private class NbaShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
        }

        private NbaGameDto CreateGameDto(NbaShortGame nbaShortGame, bool loadPlayers)
        {
            if (loadPlayers)
            {
                return CreateGameDtoWithPlayers(nbaShortGame);
            }

            return CreateGameDtoWithoutPlayers(nbaShortGame);
        }

        private NbaGameDto CreateGameDtoWithPlayers(NbaShortGame nbaShortGame)
        {
            Guid gameId = nbaShortGame.GameId;
            DateTime startDateTime = nbaShortGame.StartDateTime;
            Guid homeTeamId = nbaShortGame.HomeTeamId;
            Guid awayTeamId = nbaShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            List<PlayerDto> homePlayerList = GetPlayers(homeTeamId);
            List<PlayerDto> awayPlayerList = GetPlayers(awayTeamId);

            homeTeam.PlayerList = homePlayerList;
            awayTeam.PlayerList = awayPlayerList;

            NbaGameDto nbaGameDto = new NbaGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nbaGameDto;
        }

        private NbaGameDto CreateGameDtoWithoutPlayers(NbaShortGame nbaShortGame)
        {
            Guid gameId = nbaShortGame.GameId;
            DateTime startDateTime = nbaShortGame.StartDateTime;
            Guid homeTeamId = nbaShortGame.HomeTeamId;
            Guid awayTeamId = nbaShortGame.AwayTeamId;

            TeamDto homeTeam = GetTeam(homeTeamId);
            TeamDto awayTeam = GetTeam(awayTeamId);

            List<PlayerDto> homePlayerList = GetPlayers(homeTeamId);
            List<PlayerDto> awayPlayerList = GetPlayers(awayTeamId);

            homeTeam.PlayerList = homePlayerList;
            awayTeam.PlayerList = awayPlayerList;

            NbaGameDto nbaGameDto = new NbaGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            };

            return nbaGameDto;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from nba.get_team(@team_id_in)";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                {
                    npgsqlCommand.CommandType = CommandType.Text;

                    NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team_id_in", teamId);
                    npgsqlCommand.Parameters.Add(npgsqlParameter1);

                    using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        if (!npgsqlDataReader.HasRows)
                        {
                            string message = $"GetTeam() has no rows {teamId}";
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
                const string sqlCommandText = "select * from nba.get_players(@team_id_in)";

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
}
