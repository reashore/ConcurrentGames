using System;

namespace SportsIq.Models.GamesDto.Nhl
{
    public class NhlGameDto : IGameDto
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
    }
}
