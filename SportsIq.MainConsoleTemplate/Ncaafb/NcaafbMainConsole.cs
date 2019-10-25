using SportsIq.Games.Ncaafb;
using SportsIq.Models.GamesDto.Ncaafb;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Ncaafb;
using SportsIq.SqlDataAccess.Ncaafb;

namespace SportsIq.MainConsoleTemplate.Ncaafb
{
    public class NcaafbMainConsole : MainConsole<NcaafbGame, NcaafbGameDto, IRadarNcaafb, IDataAccessBaseNcaafb>
    {
        public NcaafbMainConsole(IDataAccessBaseNcaafb dataAccess, IRadarNcaafb radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
