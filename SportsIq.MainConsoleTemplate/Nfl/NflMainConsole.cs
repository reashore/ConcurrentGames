using SportsIq.Games.Nfl;
using SportsIq.Models.GamesDto.Nfl;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Nfl;
using SportsIq.SqlDataAccess.Nfl;

namespace SportsIq.MainConsoleTemplate.Nfl
{
    public class NflMainConsole : MainConsole<NflGame, NflGameDto, IRadarNfl, IDataAccessBaseNfl>
    {
        public NflMainConsole(IDataAccessBaseNfl dataAccess, IRadarNfl radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
