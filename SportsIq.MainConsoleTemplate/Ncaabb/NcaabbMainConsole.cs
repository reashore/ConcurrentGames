using SportsIq.Games.Ncaabb;
using SportsIq.Models.GamesDto.Ncaabb;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Ncaabb;
using SportsIq.SqlDataAccess.Ncaabb;

namespace SportsIq.MainConsoleTemplate.Ncaabb
{
    public class NcaabbMainConsole : MainConsole<NcaabbGame, NcaabbGameDto, IRadarNcaabb, IDataAccessBaseNcaabb>
    {
        public NcaabbMainConsole(IDataAccessBaseNcaabb dataAccess, IRadarNcaabb radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
