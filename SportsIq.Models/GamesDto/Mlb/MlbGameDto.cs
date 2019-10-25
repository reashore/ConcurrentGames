using System;

namespace SportsIq.Models.GamesDto.Mlb
{
    public class MlbGameDto : IGameDto
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }
        public PlayerDto HomePitcher { get; set; }
        public PlayerDto AwayPitcher { get; set; }
    }
}
