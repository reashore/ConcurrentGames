using System;

namespace SportsIq.Models.GamesDto.Nfl
{
    public class NflGameDto : IGameDto
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
        // additional NFL properties
        public Guid HomeQuarterBackId { get; set; }
        public Guid AwayQuarterBackId { get; set; }
    }
}
