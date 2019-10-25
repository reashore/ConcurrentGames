using SportsIq.Games.Nhl;
using SportsIq.Models.GamesDto.Nhl;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Nhl;
using SportsIq.SqlDataAccess.Nhl;

namespace SportsIq.MainConsoleTemplate.Nhl
{
    public class NhlMainConsole : MainConsole<NhlGame, NhlGameDto, IRadarNhl, IDataAccessBaseNhl>
    {
        public NhlMainConsole(IDataAccessBaseNhl dataAccess, IRadarNhl radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
