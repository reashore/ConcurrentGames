using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Wnba;

namespace SportsIq.Games.Wnba
{
    public class WnbaGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static WnbaGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(WnbaGameConverter));
        }

        public static WnbaGame CreateWnbaGameFromWnbaGameDto(WnbaGameDto wnbaGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            WnbaGame wnbaGame = dependencyInjectionContainer.GetInstance<WnbaGame>();

            wnbaGame.GameId = wnbaGameDto.GameId;
            wnbaGame.StartDateTime = wnbaGameDto.StartDateTime;
            wnbaGame.Started = wnbaGameDto.Started;
            wnbaGame.HomeTeam = CreateTeamFromTeamDto(wnbaGameDto.HomeTeam, loadPlayers);
            wnbaGame.AwayTeam = CreateTeamFromTeamDto(wnbaGameDto.AwayTeam, loadPlayers);

            AssignPlayerNumbers(wnbaGame.HomeTeam.PlayerList, wnbaGame.AwayTeam.PlayerList);

            return wnbaGame;
        }
    }
}