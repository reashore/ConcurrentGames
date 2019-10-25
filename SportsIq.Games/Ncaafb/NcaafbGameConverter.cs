using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Ncaafb;

namespace SportsIq.Games.Ncaafb
{
    public class NcaafbGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static NcaafbGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NcaafbGameConverter));
        }

        public static NcaafbGame CreateNcaafbGameFromNcaafbGameDto(NcaafbGameDto ncaafbGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            NcaafbGame ncaafbGame = dependencyInjectionContainer.GetInstance<NcaafbGame>();

            ncaafbGame.GameId = ncaafbGameDto.GameId;
            ncaafbGame.StartDateTime = ncaafbGameDto.StartDateTime;
            ncaafbGame.Started = ncaafbGameDto.Started;
            ncaafbGame.HomeTeam = CreateTeamFromTeamDto(ncaafbGameDto.HomeTeam, loadPlayers);
            ncaafbGame.AwayTeam = CreateTeamFromTeamDto(ncaafbGameDto.AwayTeam, loadPlayers);

            if (loadPlayers)
            {
                AssignPlayerNumbers(ncaafbGame.HomeTeam.PlayerList, ncaafbGame.AwayTeam.PlayerList);
            }

            return ncaafbGame;
        }
    }
}