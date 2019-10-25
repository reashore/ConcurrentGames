using System;
using System.Collections.Generic;

namespace SportsIq.Models.GamesDto
{
    public interface IGameDto
    {
        Guid GameId { get; }
        DateTime StartDateTime { get; set; }
        bool Started { get; set; }
    }

    public class GameDto : IGameDto
    {
        public Guid GameId { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool Started { get; set; }
    }
    
    public class TeamDto
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public List<PlayerDto> PlayerList { get; set; }
    }

    public class PlayerDto
    {
        public Guid PlayerId { get; set; }
        public string FullName { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
    }
}