using SportsIq.Games.Mlb;
using SportsIq.Models.GamesDto.Mlb;
using SportsIq.PubSub;
using SportsIq.SportsRadar.Mlb;
using SportsIq.SqlDataAccess.Mlb;

namespace SportsIq.MainConsoleTemplate.Mlb
{
    public class MlbMainConsole : MainConsole<MlbGame, MlbGameDto, IRadarMlb, IDataAccessBaseMlb>
    {
        public MlbMainConsole(IDataAccessBaseMlb dataAccess, IRadarMlb radar, IPubSubUtil pubSubUtil) :
            base(dataAccess, radar, pubSubUtil)
        {
        }
    }
}
