using log4net;
using SimpleInjector;
using SportsIq.Models.GamesDto.Mlb;

namespace SportsIq.Games.Mlb
{
    public class MlbGameConverter : GameConverterBase
    {
        private static readonly ILog Logger;

        static MlbGameConverter()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(MlbGameConverter));
        }

        public static MlbGame CreateMlbGameFromMlbGameDto(MlbGameDto mlbGameDto, bool loadPlayers, Container dependencyInjectionContainer)
        {
            MlbGame mlbGame = dependencyInjectionContainer.GetInstance<MlbGame>();

            mlbGame.GameId = mlbGameDto.GameId;
            mlbGame.StartDateTime = mlbGameDto.StartDateTime;
            mlbGame.Started = mlbGameDto.Started;
            mlbGame.HomeTeam = CreateTeamFromTeamDto(mlbGameDto.HomeTeam, loadPlayers);
            mlbGame.AwayTeam = CreateTeamFromTeamDto(mlbGameDto.AwayTeam, loadPlayers);

            if (mlbGameDto.HomePitcher != null)
            {
                mlbGame.HomePitcher = CreatePlayerFromPlayerDto(mlbGameDto.HomePitcher);
            }
            else
            {
                Logger.Error("CreateMlbGameFromMlbGameDto(): HomePitcher is null");
            }

            if (mlbGameDto.AwayPitcher != null)
            {
                mlbGame.AwayPitcher = CreatePlayerFromPlayerDto(mlbGameDto.AwayPitcher);
            }
            else
            {
                Logger.Error("CreateMlbGameFromMlbGameDto(): AwayPitcher is null");
            }

            if (loadPlayers)
            {
                AssignPlayerNumbers(mlbGame.HomeTeam.PlayerList, mlbGame.AwayTeam.PlayerList);
            }

            return mlbGame;
        }
    }
}