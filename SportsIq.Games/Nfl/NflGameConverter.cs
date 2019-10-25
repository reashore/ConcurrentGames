using System.Collections.Generic;
using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto;
using SportsIq.Models.GamesDto.Nfl;

namespace SportsIq.Games.Nfl
{
    public class NflGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static NflGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NflGameConverter));
        }

        private new static NflTeam CreateTeamFromTeamDto(TeamDto teamDto, bool loadPlayers)
        {
            List<Player> playerList;

            if (loadPlayers)
            {
                playerList = CreatePlayerListFromPlayerDtoList(teamDto.PlayerList);
            }
            else
            {
                playerList = new List<Player>();
            }

            NflTeam nflTeam = new NflTeam
            {
                TeamId = teamDto.TeamId,
                Name = teamDto.Name,
                ShortName = teamDto.ShortName,
                PlayerList = playerList
            };

            return nflTeam;
        }

        private static List<Player> CreatePlayerListFromPlayerDtoList(List<PlayerDto> playerDtoList)
        {
            List<Player> playerList = new List<Player>();

            if (playerDtoList == null || playerDtoList.Count == 0)
            {
                return new List<Player>();
            }

            foreach (PlayerDto playerDto in playerDtoList)
            {
                Player player = CreatePlayerFromPlayerDto(playerDto);
                playerList.Add(player);
            }

            return playerList;
        }

        public static NflGame CreateNflGameFromNflGameDto(NflGameDto nflGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            NflGame nflGame = dependencyInjectionContainer.GetInstance<NflGame>();

            nflGame.GameId = nflGameDto.GameId;
            nflGame.StartDateTime = nflGameDto.StartDateTime;
            nflGame.Started = nflGameDto.Started;
            nflGame.HomeTeam = CreateTeamFromTeamDto(nflGameDto.HomeTeam, loadPlayers);
            nflGame.AwayTeam = CreateTeamFromTeamDto(nflGameDto.AwayTeam, loadPlayers);
            nflGame.HomeQuarterBackId = nflGameDto.HomeQuarterBackId;
            nflGame.AwayQuarterBackId = nflGameDto.AwayQuarterBackId;

            if (loadPlayers)
            {
                AssignPlayerNumbers(nflGame.HomeTeam.PlayerList, nflGame.AwayTeam.PlayerList);
            }

            return nflGame;
        }
    }
}