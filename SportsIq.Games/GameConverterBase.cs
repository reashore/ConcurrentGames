using System.Collections.Generic;
using SportsIq.Models.GamesDto;

namespace SportsIq.Games
{
    public class GameConverterBase
    {
        protected static Team CreateTeamFromTeamDto(TeamDto teamDto, bool loadPlayers)
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

            Team team = new Team
            {
                TeamId = teamDto.TeamId,
                Name = teamDto.Name,
                ShortName = teamDto.ShortName,
                PlayerList = playerList
            };

            return team;
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

        public static Player CreatePlayerFromPlayerDto(PlayerDto playerDto)
        {
            if (playerDto == null)
            {
                return null;
            }

            Player player = new Player
            {
                PlayerId = playerDto.PlayerId,
                FullName = playerDto.FullName,
                Position = playerDto.Position
            };

            return player;
        }

        protected static void AssignPlayerNumbers(IEnumerable<Player> homePlayerList, IEnumerable<Player> awayPlayerList)
        {
            int playerNumber = 1;

            foreach (Player player in homePlayerList)
            {
                player.Number = playerNumber++;
            }

            playerNumber = 1;
            foreach (Player player in awayPlayerList)
            {
                player.Number = playerNumber++;
            }
        }
    }
}
