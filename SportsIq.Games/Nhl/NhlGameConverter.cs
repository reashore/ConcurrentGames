using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Nhl;

namespace SportsIq.Games.Nhl
{
    public class NhlGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static NhlGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(NhlGameConverter));
        }

        public static NhlGame CreateNhlGameFromNhlGameDto(NhlGameDto nhlGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            NhlGame nhlGame = dependencyInjectionContainer.GetInstance<NhlGame>();

            nhlGame.GameId = nhlGameDto.GameId;
            nhlGame.StartDateTime = nhlGameDto.StartDateTime;
            nhlGame.Started = nhlGameDto.Started;
            nhlGame.HomeTeam = CreateTeamFromTeamDto(nhlGameDto.HomeTeam, loadPlayers);
            nhlGame.AwayTeam = CreateTeamFromTeamDto(nhlGameDto.AwayTeam, loadPlayers);

            if (loadPlayers)
            {
                AssignPlayerNumbers(nhlGame.HomeTeam.PlayerList, nhlGame.AwayTeam.PlayerList);
            }

            return nhlGame;
        }
    }
}