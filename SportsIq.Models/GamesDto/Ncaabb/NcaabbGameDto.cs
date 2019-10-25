using System;

namespace SportsIq.Models.GamesDto.Ncaabb
{
    public class NcaabbGameDto : IGameDto
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
    }
}
