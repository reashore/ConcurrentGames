using SportsIq.Games.Nba;
using SportsIq.Models.GamesDto.Nba;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Nba;
using SportsIq.SqlDataAccess.Nba;

namespace SportsIq.MainConsoleTemplate.Nba
{
    public class NbaMainConsole : MainConsole<NbaGame, NbaGameDto, IRadarNba, IDataAccessBaseNba>
    {
        public NbaMainConsole(IDataAccessBaseNba dataAccess, IRadarNba radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
