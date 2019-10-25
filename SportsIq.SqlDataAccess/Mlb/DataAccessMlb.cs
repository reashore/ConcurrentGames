using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Mlb;

namespace SportsIq.SqlDataAccess.Mlb
{
    public class MlbGamePitchers
    {
        public PlayerDto HomePitcherDto { get; set; }
        public PlayerDto AwayPitcherDto { get; set; }
    }

    public interface IDataAccessBaseMlb : IDataAccessBase<MlbGameDto>
    {
    }

    public interface IDataAccessMlb : IDataAccessBaseMlb
    {
        MlbGamePitchers GetGamePitchers(Guid gameId);
        Dictionary<string, double> GetScoreAverage(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor);
        Dictionary<string, double> GetScoreAveragePvL(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor);
        Dictionary<string, double> GetScoreAverageTvT(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor);
        Dictionary<string, double> GetScoreAverageTvL(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor);
        Dictionary<int, string> GetMarkets();
    }

    public class DataAccessMlb : DataAccessBase, IDataAccessMlb
    {
        private readonly object _lockObject = new object();

        #region Public Methods

        public List<MlbGameDto> GetGames(int numberGameDays, bool loadPlayers)
        {
            lock (_lockObject)
            {
                List<MlbShortGame> mlbShortGameList = new List<MlbShortGame>();
                List<MlbGameDto> mlbGameList = new List<MlbGameDto>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_games(@number_games_days)";

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

                                return mlbGameList;
                            }

                            while (npgsqlDataReader.Read())
                            {
                                Guid gameId = npgsqlDataReader.GetGuid("game_id");
                                //                                DateTime startDateTime = npgsqlDataReader.GetDateTime("schedule");
                                DateTime startDateTime = npgsqlDataReader.GetDateTime("start_time");
                                Guid homeTeamId = npgsqlDataReader.GetGuid("home_id");
                                Guid awayTeamId = npgsqlDataReader.GetGuid("away_id");
                                Guid? homePitcherId = npgsqlDataReader.GetNullableGuid("home_pitcher_id");
                                Guid? awayPitcherId = npgsqlDataReader.GetNullableGuid("away_pitcher_id");

                                MlbShortGame mlbShortGame = new MlbShortGame
                                {
                                    GameId = gameId,
                                    StartDateTime = startDateTime,
                                    HomeTeamId = homeTeamId,
                                    AwayTeamId = awayTeamId,
                                    HomePitcherId = homePitcherId,
                                    AwayPitcherId = awayPitcherId
                                };

                                mlbShortGameList.Add(mlbShortGame);
                            }
                        }
                    }
                }

                foreach (MlbShortGame mlbShortGame in mlbShortGameList)
                {
                    MlbGameDto mlbGame = CreateGameDto(mlbShortGame, loadPlayers);
                    mlbGameList.Add(mlbGame);
                }

                return mlbGameList;
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
                    const string sqlCommandText = "select * from mlb.get_markets()";

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

        public Dictionary<string, double> GetScoreAverage(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_score_average(@team1_id_in, @team2_id_in, @team2_id_in, @home_or_visitor_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team1_id_in", team1Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("team2_id_in", team2Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("team2_id_in", team2PitcherId);
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
                                    // note that the inningList is 0-based
                                    dictionary[key] = inningList[inning - 1];
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        /*pitcher vs all*/
        public Dictionary<string, double> GetScoreAveragePvL(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_score_average_pvL(@team1_id_in, @team2_id_in, @team2_pitcher_id_in, @home_or_visitor_in)";

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
                                const string message = "GetScoreAveragePvL() has no rows";
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
                                    // note that the inningList is 0-based
                                    dictionary[key] = inningList[inning - 1];
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        /*team vs team*/
        public Dictionary<string, double> GetScoreAverageTvT(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_score_average_tvt(@team1_id_in, @team2_id_in, @team2_pitcher_id_in, @home_or_visitor_in)";

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
                                const string message = "GetScoreAverageTvT() has no rows";
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
                                    // note that the inningList is 0-based
                                    dictionary[key] = inningList[inning - 1];
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        /*team vs team*/
        public Dictionary<string, double> GetScoreAverageTvL(Guid team1Id, Guid team2Id, Guid team2PitcherId, string homeOrVisitor)
        {
            lock (_lockObject)
            {
                Dictionary<string, double> defaultDictionary = new Dictionary<string, double>();
                Dictionary<string, double> dictionary = new Dictionary<string, double>();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_score_average_tvl(@team1_id_in, @team2_id_in, @team2_pitcher_id, @home_or_visitor_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("team1_id_in", team1Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        NpgsqlParameter npgsqlParameter2 = new NpgsqlParameter("team2_id_in", team2Id);
                        npgsqlCommand.Parameters.Add(npgsqlParameter2);

                        NpgsqlParameter npgsqlParameter3 = new NpgsqlParameter("team2_pitcher_id", team2PitcherId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter3);

                        NpgsqlParameter npgsqlParameter4 = new NpgsqlParameter("home_or_visitor_in", homeOrVisitor);
                        npgsqlCommand.Parameters.Add(npgsqlParameter4);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetScoreAverageTvL() has no rows";
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
                                    // note that the inningList is 0-based
                                    dictionary[key] = inningList[inning - 1];
                                }
                            }

                            return dictionary;
                        }
                    }
                }
            }
        }

        public MlbGamePitchers GetGamePitchers(Guid gameId)
        {
            lock (_lockObject)
            {
                Guid homePitcherId = new Guid();
                Guid awayPitcherId = new Guid();

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
                {
                    npgsqlConnection.Open();
                    const string sqlCommandText = "select * from mlb.get_game_pitchers(@game_id_in)";

                    using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                    {
                        npgsqlCommand.CommandType = CommandType.Text;

                        NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("game_id_in", gameId);
                        npgsqlCommand.Parameters.Add(npgsqlParameter1);

                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            if (!npgsqlDataReader.HasRows)
                            {
                                const string message = "GetGamePitchers() has no rows";
                                Logger.Info(message);
                                throw new Exception(message);
                            }

                            while (npgsqlDataReader.Read())
                            {
                                homePitcherId = npgsqlDataReader.GetGuid("home_pitcher_id");
                                awayPitcherId = npgsqlDataReader.GetGuid("away_pitcher_id");
                            }
                        }
                    }
                }

                PlayerDto homePitcherDto = GetPlayer(homePitcherId);
                PlayerDto awayPitcherDto = GetPlayer(awayPitcherId);

                MlbGamePitchers mlbGamePitchers = new MlbGamePitchers
                {
                    HomePitcherDto = homePitcherDto,
                    AwayPitcherDto = awayPitcherDto
                };

                return mlbGamePitchers;
            }
        }

        #endregion

        #region Private Methods

        private class MlbShortGame
        {
            public Guid GameId { get; set; }
            public DateTime StartDateTime { get; set; }
            public Guid HomeTeamId { get; set; }
            public Guid AwayTeamId { get; set; }
            public Guid? HomePitcherId { get; set; }
            public Guid? AwayPitcherId { get; set; }
        }

        private MlbGameDto CreateGameDto(MlbShortGame mlbShortGame, bool loadPlayers)
        {
            if (loadPlayers)
            {
                return CreateGameDtoWithPlayers(mlbShortGame);
            }

            return CreateGameDtoWithoutPlayers(mlbShortGame);
        }

        private MlbGameDto CreateGameDtoWithPlayers(MlbShortGame mlbShortGame)
        {
            Guid gameId = mlbShortGame.GameId;
            DateTime startDateTime = mlbShortGame.StartDateTime;
            Guid homeTeamId = mlbShortGame.HomeTeamId;
            Guid awayTeamId = mlbShortGame.AwayTeamId;
            Guid? homePitcherId = mlbShortGame.HomePitcherId;
            Guid? awayPitcherId = mlbShortGame.AwayPitcherId;

            TeamDto homeTeamDto = GetTeam(homeTeamId);
            TeamDto awayTeamDto = GetTeam(awayTeamId);

            List<PlayerDto> homePlayerDtoList = GetPlayers(homeTeamId);
            List<PlayerDto> awayPlayerDtoList = GetPlayers(awayTeamId);

            if (homePlayerDtoList == null || homePlayerDtoList.Count == 0)
            {
                Logger.Info("CreateGameDtoWithPlayers(): homePlayerDtoList is null or empty");
            }

            if (awayPlayerDtoList == null || awayPlayerDtoList.Count == 0)
            {
                Logger.Info("CreateGameDtoWithPlayers(): awayPlayerDtoList is null or empty");
            }

            homeTeamDto.PlayerList = homePlayerDtoList;
            awayTeamDto.PlayerList = awayPlayerDtoList;

            PlayerDto homePitcher;
            PlayerDto awayPitcher;

            if (homePitcherId != null)
            {
                homePitcher = homeTeamDto.PlayerList?.Find(p => p.PlayerId == homePitcherId);
            }
            else
            {
                homePitcher = null;
                Logger.Info("CreateGameDtoWithPlayers(): homePitcher is null");
            }

            if (awayPitcherId != null)
            {
                awayPitcher = homeTeamDto.PlayerList?.Find(p => p.PlayerId == awayPitcherId);
            }
            else
            {
                awayPitcher = null;
                Logger.Info("CreateGameDtoWithPlayers(): awayPitcher is null");
            }

            MlbGameDto mlbGameDto = new MlbGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeamDto,
                AwayTeam = awayTeamDto,
                HomePitcher = homePitcher,
                AwayPitcher = awayPitcher
            };

            return mlbGameDto;
        }

        private MlbGameDto CreateGameDtoWithoutPlayers(MlbShortGame mlbShortGame)
        {
            Guid gameId = mlbShortGame.GameId;
            DateTime startDateTime = mlbShortGame.StartDateTime;
            Guid homeTeamId = mlbShortGame.HomeTeamId;
            Guid awayTeamId = mlbShortGame.AwayTeamId;
            Guid? homePitcherId = mlbShortGame.HomePitcherId;
            Guid? awayPitcherId = mlbShortGame.AwayPitcherId;

            TeamDto homeTeamDto = GetTeam(homeTeamId);
            TeamDto awayTeamDto = GetTeam(awayTeamId);

            PlayerDto homePitcherDto;
            PlayerDto awayPitcherDto;

            if (homePitcherId != null)
            {
                homePitcherDto = GetPlayer(homePitcherId.Value);
            }
            else
            {
                homePitcherDto = null;
                Logger.Info("CreateGameDtoWithoutPlayers(): homePitcher is null");
            }

            if (awayPitcherId != null)
            {
                awayPitcherDto = GetPlayer(awayPitcherId.Value);
            }
            else
            {
                awayPitcherDto = null;
                Logger.Info("CreateGameDtoWithoutPlayers(): awayPitcher is null");
            }

            MlbGameDto mlbGame = new MlbGameDto
            {
                GameId = gameId,
                StartDateTime = startDateTime,
                HomeTeam = homeTeamDto,
                AwayTeam = awayTeamDto,
                HomePitcher = homePitcherDto,
                AwayPitcher = awayPitcherDto
            };

            return mlbGame;
        }

        private TeamDto GetTeam(Guid teamId)
        {
            TeamDto teamDto = new TeamDto();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from mlb.get_team(@team_id_in)";

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

        private PlayerDto GetPlayer(Guid playerId)
        {
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from mlb.get_player(@player_id_in)";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sqlCommandText, npgsqlConnection))
                {
                    npgsqlCommand.CommandType = CommandType.Text;

                    NpgsqlParameter npgsqlParameter1 = new NpgsqlParameter("player_id_in", playerId);
                    npgsqlCommand.Parameters.Add(npgsqlParameter1);

                    using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        if (!npgsqlDataReader.HasRows)
                        {
                            const string message = "GetPlayer() has no rows";
                            Logger.Info(message);
                            return null;
                        }

                        PlayerDto playerDto = null;

                        while (npgsqlDataReader.Read())
                        {
                            string fullName = npgsqlDataReader.GetString("full_name");
                            int number = npgsqlDataReader.GetInt("jersey");

                            playerDto = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName,
                                Number = number
                            };
                        }

                        return playerDto;
                    }
                }
            }
        }

        private List<PlayerDto> GetPlayers(Guid teamId)
        {
            List<PlayerDto> playerDtoList = new List<PlayerDto>();

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                npgsqlConnection.Open();
                const string sqlCommandText = "select * from mlb.get_players(@team_id_in)";

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
                            int number = npgsqlDataReader.GetInt("jersey");

                            PlayerDto mlbPlayer = new PlayerDto
                            {
                                PlayerId = playerId,
                                FullName = fullName,
                                Number = number
                            };

                            playerDtoList.Add(mlbPlayer);
                        }

                        return playerDtoList;
                    }
                }
            }
        }

        #endregion
    }
}
