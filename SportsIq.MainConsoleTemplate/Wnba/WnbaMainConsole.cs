using SportsIq.Games.Wnba;
using SportsIq.Models.GamesDto.Wnba;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Wnba;
using SportsIq.SqlDataAccess.Wnba;

namespace SportsIq.MainConsoleTemplate.Wnba
{
    public class WnbaMainConsole : MainConsole<WnbaGame, WnbaGameDto, IRadarWnba, IDataAccessBaseWnba>
    {
        public WnbaMainConsole(IDataAccessBaseWnba dataAccess, IRadarWnba radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
