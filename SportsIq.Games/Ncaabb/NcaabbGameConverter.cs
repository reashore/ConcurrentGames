using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Ncaabb;

namespace SportsIq.Games.Ncaabb
{
    public class NcaabbGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static NcaabbGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NcaabbGameConverter));
        }

        public static NcaabbGame CreateNcaabbGameFromNcaabbGameDto(NcaabbGameDto ncaabbGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            NcaabbGame ncaabbGame = dependencyInjectionContainer.GetInstance<NcaabbGame>();

            ncaabbGame.GameId = ncaabbGameDto.GameId;
            ncaabbGame.StartDateTime = ncaabbGameDto.StartDateTime;
            ncaabbGame.Started = ncaabbGameDto.Started;
            ncaabbGame.HomeTeam = CreateTeamFromTeamDto(ncaabbGameDto.HomeTeam, loadPlayers);
            ncaabbGame.AwayTeam = CreateTeamFromTeamDto(ncaabbGameDto.AwayTeam, loadPlayers);

            AssignPlayerNumbers(ncaabbGame.HomeTeam.PlayerList, ncaabbGame.AwayTeam.PlayerList);

            return ncaabbGame;
        }
    }
}