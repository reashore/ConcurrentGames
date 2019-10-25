using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Nba;

namespace SportsIq.Games.Nba
{
    public class NbaGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static NbaGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NbaGameConverter));
        }

        public static NbaGame CreateNbaGameFromNbaGameDto(NbaGameDto nbaGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            NbaGame nbaGame = dependencyInjectionContainer.GetInstance<NbaGame>();

            nbaGame.GameId = nbaGameDto.GameId;
            nbaGame.StartDateTime = nbaGameDto.StartDateTime;
            nbaGame.Started = nbaGameDto.Started;
            nbaGame.HomeTeam = CreateTeamFromTeamDto(nbaGameDto.HomeTeam, loadPlayers);
            nbaGame.AwayTeam = CreateTeamFromTeamDto(nbaGameDto.AwayTeam, loadPlayers);

            AssignPlayerNumbers(nbaGame.HomeTeam.PlayerList, nbaGame.AwayTeam.PlayerList);

            return nbaGame;
        }
    }
}