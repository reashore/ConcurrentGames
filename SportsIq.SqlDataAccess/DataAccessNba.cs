using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using SportsIq.Models.GamesOld.Nba;
using static System.Console;

namespace SportsIq.SqlDataAccess
{
    public interface IDataAccessNba
    {
        List<NbaGame> GetGames();
        List<NbaPlayer> GetPlayers(Guid gameId, string side);
        Dictionary<string, double> GetPlayersOldScoringCurve(string statisticType, Guid playerId, Guid opponentId, string quarterString, int numberGames);
        Dictionary<string, double> GetTeamAverageStats(string statisticType);
        Dictionary<string, double> GetPlayersOldTeamPercentage(string statisticType, Guid playerId, Guid opponentId, string quarterString, int numberGames);
        Dictionary<string, double> GetPlayersOverallPercentage(string statisticType, Guid playerId, string quarterString, int numberGames);
        Dictionary<string, double> GetPlayersScoringCurveOverall(string statisticType, Guid playerId, string quarterString, int numberGames);
        double? GetPlayerVersusTeamStandardDeviation(Guid playerId, Guid opponentId, string statisticType);
        void Close();
        void Dispose();
    }

    public class DataAccessNba : DataAccessBase, IDataAccessNba
    {
        #region Allowable values of the statisticType parameter

        // "assists"
        // "points"
        // "blocks"
        // "rebounds"
        // "steals"
        // "threepointattempted"
        // "threepointmade"
        // "turnovers"
        // "twopointattempted"
        // "twopointmade"
        // "freethrowattempted"
        // "freethrowmade"

        #endregion

        public List<NbaGame> GetGames()
        {
            List<NbaGame> gameEventList = new List<NbaGame>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "pbp.GetGames";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetGameEvents() has no rows";
                            string message = $"{error,40}, {storedProcedure,40}\n";
                            Write(message);

                            return gameEventList;
                        }

                        while (mySqlDataReader.Read())
                        {
                            NbaGame nbaGame = new NbaGame
                            {
                                GameId = mySqlDataReader.GetGuid("comp_id"),
                                GameTime = mySqlDataReader.GetDateTime("game_time"),
                                AwayTeamId = mySqlDataReader.GetGuid("away_team_id"),
                                HomeTeamId = mySqlDataReader.GetGuid("home_team_id"),
                                Away = mySqlDataReader.GetString("away"),
                                Home = mySqlDataReader.GetString("home")
                            };

                            gameEventList.Add(nbaGame);
                        }

                        return gameEventList;
                    }
                }
            }
        }

        public List<NbaPlayer> GetPlayers(Guid gameId, string side)
        {
            // Allowable values of parameter side are "home" or "away"

            List<NbaPlayer> playerList = new List<NbaPlayer>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "pbp.GetPlayers";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(gameId), gameId);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(side), side);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayers() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {gameId,40}, {side}\n";
                            Write(message);

                            return playerList;
                        }

                        int playerNumber = 1;

                        while (mySqlDataReader.Read())
                        {
                            NbaPlayer nbaPlayer = new NbaPlayer
                            {
                                GameId = mySqlDataReader.GetGuid("game_id"),
                                PlayerId = mySqlDataReader.GetGuid("player_id"),
                                FullName = mySqlDataReader.GetString("full_name"),
                                JerseyNumber = mySqlDataReader.GetInt32("jersey"),
                                TeamId = mySqlDataReader.GetGuid("team_id"),
                                Side = side,
                                Number = playerNumber
                            };

                            playerNumber++;
                            playerList.Add(nbaPlayer);
                        }

                        return playerList;
                    }
                }
            }
        }

        public Dictionary<string, double> GetPlayersOldScoringCurve(string statisticType, Guid playerId, Guid opponentId, string quarterString, int numberGames)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetPlayersOldScoringCurve";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(playerId), playerId);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    MySqlParameter mySqlParameter3 = new MySqlParameter(nameof(opponentId), opponentId);
                    mySqlCommand.Parameters.Add(mySqlParameter3);

                    MySqlParameter mySqlParameter4 = new MySqlParameter(nameof(quarterString), quarterString);
                    mySqlCommand.Parameters.Add(mySqlParameter4);

                    MySqlParameter mySqlParameter5 = new MySqlParameter(nameof(numberGames), numberGames);
                    mySqlCommand.Parameters.Add(mySqlParameter5);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayersOldScoringCurve() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {statisticType,20}, {playerId,40}, {opponentId,40}, {quarterString}, {numberGames}\n";
                            Write(message);

                            return dictionary;
                        }

                        while (mySqlDataReader.Read())
                        {
                            for (int n = 0; n < 12; n++)
                            {
                                string key = $"M{n}";
                                dictionary[key] = mySqlDataReader.GetDouble(key);
                            }
                        }

                        return dictionary;
                    }
                }
            }
        }

        public Dictionary<string, double> GetTeamAverageStats(string statisticType)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetTeamAverageStats";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter typeParameter = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(typeParameter);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetTeamAverageStats() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {statisticType,20}\n";
                            Write(message);

                            return dictionary;
                        }

                        while (mySqlDataReader.Read())
                        {
                            dictionary["CG"] = mySqlDataReader.GetDouble("LG_CG");
                            dictionary["H1"] = mySqlDataReader.GetDouble("LG_H1");
                            dictionary["H2"] = mySqlDataReader.GetDouble("LG_H2");
                            dictionary["Q1"] = mySqlDataReader.GetDouble("LG_Q1");
                            dictionary["Q2"] = mySqlDataReader.GetDouble("LG_Q2");
                            dictionary["Q3"] = mySqlDataReader.GetDouble("LG_Q3");
                            dictionary["Q4"] = mySqlDataReader.GetDouble("LG_Q4");
                        }

                        return dictionary;
                    }
                }
            }
        }

        public Dictionary<string, double> GetPlayersOldTeamPercentage(string statisticType, Guid playerId, Guid opponentId, string quarterString, int numberGames)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetPlayersOldTeamPercentage";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(playerId), playerId);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    MySqlParameter mySqlParameter3 = new MySqlParameter(nameof(opponentId), opponentId);
                    mySqlCommand.Parameters.Add(mySqlParameter3);

                    MySqlParameter mySqlParameter4 = new MySqlParameter(nameof(quarterString), quarterString);
                    mySqlCommand.Parameters.Add(mySqlParameter4);

                    MySqlParameter mySqlParameter5 = new MySqlParameter(nameof(numberGames), numberGames);
                    mySqlCommand.Parameters.Add(mySqlParameter5);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayersOldTeamPercentage() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {statisticType,20}, {playerId,40}, {opponentId,40}, {quarterString}, {numberGames}\n";
                            Write(message);

                            return dictionary;
                        }

                        while (mySqlDataReader.Read())
                        {
                            for (int n = 0; n < 12; n++)
                            {
                                string key = $"M{n}";
                                dictionary[key] = mySqlDataReader.GetDouble(key);
                            }
                        }

                        return dictionary;
                    }
                }
            }
        }

        public Dictionary<string, double> GetPlayersOverallPercentage(string statisticType, Guid playerId, string quarterString, int numberGames)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetPlayersOverallPercentage";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(playerId), playerId);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    MySqlParameter mySqlParameter3 = new MySqlParameter(nameof(quarterString), quarterString);
                    mySqlCommand.Parameters.Add(mySqlParameter3);

                    MySqlParameter mySqlParameter4 = new MySqlParameter(nameof(numberGames), numberGames);
                    mySqlCommand.Parameters.Add(mySqlParameter4);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayersOverallPercentage() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {statisticType,20}, {playerId,40}, {quarterString}, {numberGames}\n";
                            Write(message);

                            return dictionary;
                        }

                        while (mySqlDataReader.Read())
                        {
                            for (int n = 0; n < 12; n++)
                            {
                                string key = $"M{n}";
                                dictionary[key] = mySqlDataReader.GetDouble(key);
                            }
                        }

                        return dictionary;
                    }
                }
            }
        }

        public Dictionary<string, double> GetPlayersScoringCurveOverall(string statisticType, Guid playerId, string quarterString, int numberGames)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetPlayersScoringCurveOverall";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(playerId), playerId);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    MySqlParameter mySqlParameter3 = new MySqlParameter(nameof(quarterString), quarterString);
                    mySqlCommand.Parameters.Add(mySqlParameter3);

                    MySqlParameter mySqlParameter4 = new MySqlParameter(nameof(numberGames), numberGames);
                    mySqlCommand.Parameters.Add(mySqlParameter4);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayersScoringCurveOverall() has no rows";
                            string message = $"{error,50}: {storedProcedure,40}, {statisticType,20}, {playerId,40}, {quarterString}, {numberGames}\n";
                            Write(message);

                            return dictionary;
                        }

                        while (mySqlDataReader.Read())
                        {
                            for (int n = 0; n < 12; n++)
                            {
                                string key1 = $"M{n}";
                                dictionary[key1] = mySqlDataReader.GetDouble(key1);

                                string key2 = $"SD_M{n}";
                                dictionary[key2] = mySqlDataReader.GetDouble(key2);
                            }
                        }

                        return dictionary;
                    }
                }
            }
        }

        public double? GetPlayerVersusTeamStandardDeviation(Guid playerId, Guid opponentId, string statisticType)
        {
            using (MySqlConnection mySqlConnection = MySqlConnection)
            {
                mySqlConnection.Open();
                const string storedProcedure = "stats.GetPlayerVersusTeamStandardDeviation";

                using (MySqlCommand mySqlCommand = new MySqlCommand(storedProcedure, mySqlConnection))
                {
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    MySqlParameter mySqlParameter1 = new MySqlParameter(nameof(statisticType), statisticType);
                    mySqlCommand.Parameters.Add(mySqlParameter1);

                    MySqlParameter mySqlParameter2 = new MySqlParameter(nameof(playerId), playerId);
                    mySqlCommand.Parameters.Add(mySqlParameter2);

                    MySqlParameter mySqlParameter3 = new MySqlParameter(nameof(opponentId), opponentId);
                    mySqlCommand.Parameters.Add(mySqlParameter3);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        double? standardDeviation = -1.0;

                        if (!mySqlDataReader.HasRows)
                        {
                            const string error = "GetPlayerVersusTeamStandardDeviation() had no rows";
                            string message = $"{error,50}: {storedProcedure,40}\n";
                            Write(message);

                            return standardDeviation;
                        }

                        while (mySqlDataReader.Read())
                        {
                            // todo converted code to read nullable doubles
                            standardDeviation = mySqlDataReader.GetNullableDouble("standardDeviation");
                        }

                        return standardDeviation;
                    }
                }
            }
        }
    }
}