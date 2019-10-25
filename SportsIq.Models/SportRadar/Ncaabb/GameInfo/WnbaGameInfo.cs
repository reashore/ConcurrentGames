using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable All

namespace SportsIq.Models.SportRadar.Ncaabb.GameInfo
{
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("game", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public class NcaabbGameInfo : IBaseGameAttributes, IGameStateAttributes, IGameMetadataAttributes
    {
        [XmlElement("venue", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public VenueType Venue { get; set; }
        
        [XmlIgnore]
        private Collection<TeamType> _team;
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamType> Team
        {
            get
            {
                return _team;
            }
            private set
            {
                _team = value;
            }
        }
        
        [XmlIgnore]
        public bool TeamSpecified
        {
            get
            {
                return Team.Count != 0;
            }
        }
        
        public NcaabbGameInfo()
        {
            _team = new Collection<TeamType>();
            _officials = new Collection<GameTypeOfficialsOfficial>();
        }
        
        [XmlIgnore]
        private Collection<GameTypeOfficialsOfficial> _officials;
        
        [XmlArray("officials", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("official", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<GameTypeOfficialsOfficial> Officials
        {
            get
            {
                return _officials;
            }
            private set
            {
                _officials = value;
            }
        }
        
        [XmlIgnore]
        public bool OfficialsSpecified
        {
            get
            {
                return Officials.Count != 0;
            }
        }
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("title", Form=XmlSchemaForm.Unqualified)]
        public string Title { get; set; }
        
        [XmlAttribute("status", Form=XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesStatus Status { get; set; }
        
        [XmlIgnore]
        public bool StatusSpecified { get; set; }
        
        [XmlAttribute("coverage", Form=XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesCoverage Coverage { get; set; }
        
        [XmlIgnore]
        public bool CoverageSpecified { get; set; }
        
        [XmlAttribute("away_team", Form=XmlSchemaForm.Unqualified)]
        public string Away_Team { get; set; }
        
        [XmlAttribute("home_team", Form=XmlSchemaForm.Unqualified)]
        public string Home_Team { get; set; }
        
        [XmlAttribute("scheduled", Form=XmlSchemaForm.Unqualified, DataType="dateTime")]
        public DateTime Scheduled { get; set; }
        
        [XmlIgnore]
        public bool ScheduledSpecified { get; set; }
        
        [XmlAttribute("possession_arrow", Form=XmlSchemaForm.Unqualified)]
        public string Possession_Arrow { get; set; }
        
        [XmlAttribute("conference_game", Form=XmlSchemaForm.Unqualified)]
        public bool Conference_Game { get; set; }
        
        [XmlIgnore]
        public bool Conference_GameSpecified { get; set; }
        
        [XmlAttribute("neutral_site", Form=XmlSchemaForm.Unqualified)]
        public bool Neutral_Site { get; set; }
        
        [XmlIgnore]
        public bool Neutral_SiteSpecified { get; set; }
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        [XmlAttribute("track_on_court", Form=XmlSchemaForm.Unqualified)]
        public bool Track_On_Court { get; set; }
        
        [XmlIgnore]
        public bool Track_On_CourtSpecified { get; set; }
        
        [XmlAttribute("entry_mode", Form=XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesEntry_Mode Entry_Mode { get; set; }
        
        [XmlIgnore]
        public bool Entry_ModeSpecified { get; set; }
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        [XmlAttribute("quarter", Form=XmlSchemaForm.Unqualified)]
        public string Quarter { get; set; }
        
        [XmlAttribute("half", Form=XmlSchemaForm.Unqualified)]
        public string Half { get; set; }
        
        [XmlAttribute("attendance", Form=XmlSchemaForm.Unqualified)]
        public string Attendance { get; set; }
        
        [XmlAttribute("lead_changes", Form=XmlSchemaForm.Unqualified)]
        public string Lead_Changes { get; set; }
        
        [XmlAttribute("times_tied", Form=XmlSchemaForm.Unqualified)]
        public string Times_Tied { get; set; }
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("venueType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class VenueType
    {
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        [XmlAttribute("desc", Form=XmlSchemaForm.Unqualified)]
        public string Desc { get; set; }
        
        [XmlAttribute("address", Form=XmlSchemaForm.Unqualified)]
        public string Address { get; set; }
        
        [XmlAttribute("capacity", Form=XmlSchemaForm.Unqualified)]
        public string Capacity { get; set; }
        
        [XmlAttribute("city", Form=XmlSchemaForm.Unqualified)]
        public string City { get; set; }
        
        [XmlAttribute("country", Form=XmlSchemaForm.Unqualified)]
        public string Country { get; set; }
        
        [XmlAttribute("state", Form=XmlSchemaForm.Unqualified)]
        public string State { get; set; }
        
        [XmlAttribute("zip", Form=XmlSchemaForm.Unqualified)]
        public string Zip { get; set; }
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
    }
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("injuryType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class InjuryType
    {
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("desc", Form=XmlSchemaForm.Unqualified)]
        public string Desc { get; set; }
        
        
        
        [XmlAttribute("comment", Form=XmlSchemaForm.Unqualified)]
        public string Comment { get; set; }
        
        
        
        [XmlAttribute("start_date", Form=XmlSchemaForm.Unqualified, DataType="date")]
        public DateTime Start_Date { get; set; }
        
        
        
        [XmlAttribute("update_date", Form=XmlSchemaForm.Unqualified, DataType="date")]
        public DateTime Update_Date { get; set; }
        
        [XmlIgnore]
        public bool Update_DateSpecified { get; set; }
        
        
        
        [XmlAttribute("status", Form=XmlSchemaForm.Unqualified)]
        public InjuryTypeStatus Status { get; set; }
        
        [XmlIgnore]
        public bool StatusSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("InjuryTypeStatus", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum InjuryTypeStatus
    {
        
        
        
        Unknown,
        
        
        
        [XmlEnum("Day To Day")]
        Day_To_Day,
        
        
        
        Out,
        
        
        
        [XmlEnum("Out For Season")]
        Out_For_Season,
        
        
        
        [XmlEnum("Out Indefinitely")]
        Out_Indefinitely
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("organizationType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OrganizationType : IBaseOrganizationAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseOrganizationAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Reference
        {
            get;
            set;
        }
        
        
        
        string Name
        {
            get;
            set;
        }
        
        
        
        string Alias
        {
            get;
            set;
        }
        
        
        
        string Sr_Id
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("draftType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class DraftType
    {
        
        
        
        [XmlAttribute("pick", Form=XmlSchemaForm.Unqualified)]
        public string Pick { get; set; }
        
        
        
        [XmlAttribute("round", Form=XmlSchemaForm.Unqualified)]
        public string Round { get; set; }
        
        
        
        [XmlAttribute("team_id", Form=XmlSchemaForm.Unqualified)]
        public string Team_Id { get; set; }
        
        
        
        [XmlAttribute("year", Form=XmlSchemaForm.Unqualified)]
        public string Year { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("positionType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum PositionType
    {
        
        
        
        NA,
        
        
        
        C,
        
        
        
        [XmlEnum("C-F")]
        C_F,
        
        
        
        F,
        
        
        
        [XmlEnum("F-C")]
        F_C,
        
        
        
        [XmlEnum("F-G")]
        F_G,
        
        
        
        G,
        
        
        
        [XmlEnum("G-F")]
        G_F,
        
        
        
        PF,
        
        
        
        PG,
        
        
        
        SF,
        
        
        
        SG
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePersonnelAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Reference
        {
            get;
            set;
        }
        
        
        
        string First_Name
        {
            get;
            set;
        }
        
        
        
        string Last_Name
        {
            get;
            set;
        }
        
        
        
        string Full_Name
        {
            get;
            set;
        }
        
        
        
        string Sr_Id
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("periodScoreType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PeriodScoreType
    {
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("teamType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamType : IBaseTeamAttributes
    {
        
        
        
        [XmlElement("scoring", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public TeamTypeScoring Scoring { get; set; }
        
        
        
        [XmlElement("leaders", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public TeamTypeLeaders Leaders { get; set; }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public TeamTypeStatistics Statistics { get; set; }
        
        [XmlIgnore]
        private Collection<TeamTypeCoaches> _coaches;
        
        
        
        [XmlElement("coaches", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypeCoaches> Coaches
        {
            get
            {
                return _coaches;
            }
            private set
            {
                _coaches = value;
            }
        }
        
        [XmlIgnore]
        public bool CoachesSpecified
        {
            get
            {
                return Coaches.Count != 0;
            }
        }
        
        public TeamType()
        {
            _coaches = new Collection<TeamTypeCoaches>();
            _players = new Collection<TeamTypePlayersPlayer>();
        }
        
        [XmlIgnore]
        private Collection<TeamTypePlayersPlayer> _players;
        
        
        
        [XmlArray("players", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypePlayersPlayer> Players
        {
            get
            {
                return _players;
            }
            private set
            {
                _players = value;
            }
        }
        
        [XmlIgnore]
        public bool PlayersSpecified
        {
            get
            {
                return Players.Count != 0;
            }
        }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("market", Form=XmlSchemaForm.Unqualified)]
        public string Market { get; set; }
        
        
        
        [XmlAttribute("founded", Form=XmlSchemaForm.Unqualified)]
        public string Founded { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rank", Form=XmlSchemaForm.Unqualified)]
        public string Rank { get; set; }
        
        
        
        [XmlAttribute("bonus", Form=XmlSchemaForm.Unqualified)]
        public bool Bonus { get; set; }
        
        [XmlIgnore]
        public bool BonusSpecified { get; set; }
        
        
        
        [XmlAttribute("double_bonus", Form=XmlSchemaForm.Unqualified)]
        public bool Double_Bonus { get; set; }
        
        [XmlIgnore]
        public bool Double_BonusSpecified { get; set; }
        
        
        
        [XmlAttribute("remaining_timeouts", Form=XmlSchemaForm.Unqualified)]
        public string Remaining_Timeouts { get; set; }
        
        
        
        [XmlAttribute("home", Form=XmlSchemaForm.Unqualified)]
        public bool Home { get; set; }
        
        [XmlIgnore]
        public bool HomeSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeScoring", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeScoring
    {
        
        [XmlIgnore]
        private Collection<PeriodScoreType> _quarter;
        
        
        
        [XmlElement("quarter", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<PeriodScoreType> Quarter
        {
            get
            {
                return _quarter;
            }
            private set
            {
                _quarter = value;
            }
        }
        
        [XmlIgnore]
        public bool QuarterSpecified
        {
            get
            {
                return Quarter.Count != 0;
            }
        }
        
        public TeamTypeScoring()
        {
            _quarter = new Collection<PeriodScoreType>();
            _half = new Collection<PeriodScoreType>();
            _overtime = new Collection<PeriodScoreType>();
        }
        
        [XmlIgnore]
        private Collection<PeriodScoreType> _half;
        
        
        
        [XmlElement("half", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<PeriodScoreType> Half
        {
            get
            {
                return _half;
            }
            private set
            {
                _half = value;
            }
        }
        
        [XmlIgnore]
        public bool HalfSpecified
        {
            get
            {
                return Half.Count != 0;
            }
        }
        
        [XmlIgnore]
        private Collection<PeriodScoreType> _overtime;
        
        
        
        [XmlElement("overtime", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<PeriodScoreType> Overtime
        {
            get
            {
                return _overtime;
            }
            private set
            {
                _overtime = value;
            }
        }
        
        [XmlIgnore]
        public bool OvertimeSpecified
        {
            get
            {
                return Overtime.Count != 0;
            }
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeLeaders", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeLeaders
    {
        
        [XmlIgnore]
        private Collection<StatsLeaderTypePlayer> _points;
        
        
        
        [XmlArray("points", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<StatsLeaderTypePlayer> Points
        {
            get
            {
                return _points;
            }
            private set
            {
                _points = value;
            }
        }
        
        [XmlIgnore]
        public bool PointsSpecified
        {
            get
            {
                return Points.Count != 0;
            }
        }
        
        public TeamTypeLeaders()
        {
            _points = new Collection<StatsLeaderTypePlayer>();
            _rebounds = new Collection<StatsLeaderTypePlayer>();
            _assists = new Collection<StatsLeaderTypePlayer>();
        }
        
        [XmlIgnore]
        private Collection<StatsLeaderTypePlayer> _rebounds;
        
        
        
        [XmlArray("rebounds", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<StatsLeaderTypePlayer> Rebounds
        {
            get
            {
                return _rebounds;
            }
            private set
            {
                _rebounds = value;
            }
        }
        
        [XmlIgnore]
        public bool ReboundsSpecified
        {
            get
            {
                return Rebounds.Count != 0;
            }
        }
        
        [XmlIgnore]
        private Collection<StatsLeaderTypePlayer> _assists;
        
        
        
        [XmlArray("assists", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<StatsLeaderTypePlayer> Assists
        {
            get
            {
                return _assists;
            }
            private set
            {
                _assists = value;
            }
        }
        
        
        [XmlIgnore]
        public bool AssistsSpecified
        {
            get
            {
                return Assists.Count != 0;
            }
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("statsLeaderType", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class StatsLeaderType
    {
        
        [XmlIgnore]
        private Collection<StatsLeaderTypePlayer> _player;
        
        
        
        [XmlElement("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<StatsLeaderTypePlayer> Player
        {
            get
            {
                return _player;
            }
            private set
            {
                _player = value;
            }
        }
        
        [XmlIgnore]
        public bool PlayerSpecified
        {
            get
            {
                return Player.Count != 0;
            }
        }
        
        public StatsLeaderType()
        {
            _player = new Collection<StatsLeaderTypePlayer>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("StatsLeaderTypePlayer", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class StatsLeaderTypePlayer : IBasePlayerAttributes
    {
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public StatsLeaderTypePlayerStatistics Statistics { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("first_name", Form=XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }
        
        
        
        [XmlAttribute("last_name", Form=XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }
        
        
        
        [XmlAttribute("full_name", Form=XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("abbr_name", Form=XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }
        
        
        
        [XmlAttribute("jersey_number", Form=XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }
        
        
        
        [XmlAttribute("position", Form=XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }
        
        [XmlIgnore]
        public bool PositionSpecified { get; set; }
        
        
        
        [XmlAttribute("primary_position", Form=XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }
        
        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }
        
        
        
        [XmlAttribute("status", Form=XmlSchemaForm.Unqualified)]
        public IBasePlayerAttributesStatus Status { get; set; }
        
        [XmlIgnore]
        public bool StatusSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("StatsLeaderTypePlayerStatistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class StatsLeaderTypePlayerStatistics : IBaseStatisticsAttributes, IPlayerStatisticsAttributes
    {
        
        
        
        [XmlAttribute("assists", Form=XmlSchemaForm.Unqualified)]
        public string Assists { get; set; }
        
        
        
        [XmlAttribute("assists_turnover_ratio", Form=XmlSchemaForm.Unqualified)]
        public decimal Assists_Turnover_Ratio { get; set; }
        
        [XmlIgnore]
        public bool Assists_Turnover_RatioSpecified { get; set; }
        
        
        
        [XmlAttribute("blocked_att", Form=XmlSchemaForm.Unqualified)]
        public string Blocked_Att { get; set; }
        
        
        
        [XmlAttribute("blocks", Form=XmlSchemaForm.Unqualified)]
        public string Blocks { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("field_goals_att", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Att { get; set; }
        
        
        
        [XmlAttribute("field_goals_made", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Made { get; set; }
        
        
        
        [XmlAttribute("field_goals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Field_Goals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Field_Goals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("flagrant_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Flagrant_Fouls { get; set; }
        
        
        
        [XmlAttribute("free_throws_att", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Att { get; set; }
        
        
        
        [XmlAttribute("free_throws_made", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Made { get; set; }
        
        
        
        [XmlAttribute("free_throws_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Free_Throws_Pct { get; set; }
        
        [XmlIgnore]
        public bool Free_Throws_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fast_break_pts", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Pts { get; set; }
        
        
        
        [XmlAttribute("fast_break_att", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Att { get; set; }
        
        
        
        [XmlAttribute("fast_break_made", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Made { get; set; }
        
        
        
        [XmlAttribute("fast_break_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Fast_Break_Pct { get; set; }
        
        [XmlIgnore]
        public bool Fast_Break_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("second_chance_pts", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Pts { get; set; }
        
        
        
        [XmlAttribute("second_chance_att", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Att { get; set; }
        
        
        
        [XmlAttribute("second_chance_made", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Made { get; set; }
        
        
        
        [XmlAttribute("second_chance_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Second_Chance_Pct { get; set; }
        
        [XmlIgnore]
        public bool Second_Chance_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("points_in_paint", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_att", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Att { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_made", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Made { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Points_In_Paint_Pct { get; set; }
        
        [XmlIgnore]
        public bool Points_In_Paint_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_att", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Att { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_AttSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Pct { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("minutes", Form=XmlSchemaForm.Unqualified)]
        public string Minutes { get; set; }
        
        
        
        [XmlAttribute("effective_fg_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Effective_Fg_Pct { get; set; }
        
        [XmlIgnore]
        public bool Effective_Fg_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("personal_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Personal_Fouls { get; set; }
        
        
        
        [XmlAttribute("ejections", Form=XmlSchemaForm.Unqualified)]
        public string Ejections { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Rebounds { get; set; }
        
        
        
        [XmlAttribute("steals", Form=XmlSchemaForm.Unqualified)]
        public string Steals { get; set; }
        
        
        
        [XmlAttribute("tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("three_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Att { get; set; }
        
        
        
        [XmlAttribute("three_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Made { get; set; }
        
        
        
        [XmlAttribute("three_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Three_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Three_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Turnovers { get; set; }
        
        
        
        [XmlAttribute("two_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Att { get; set; }
        
        
        
        [XmlAttribute("two_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Made { get; set; }
        
        
        
        [XmlAttribute("two_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Two_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Two_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fouls_drawn", Form=XmlSchemaForm.Unqualified)]
        public string Fouls_Drawn { get; set; }
        
        
        
        [XmlAttribute("offensive_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Fouls { get; set; }
        
        
        
        [XmlAttribute("technical_other", Form=XmlSchemaForm.Unqualified)]
        public string Technical_Other { get; set; }
        
        
        
        [XmlAttribute("coach_ejections", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Ejections { get; set; }
        
        
        
        [XmlAttribute("offensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Offensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Defensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_assists", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Assists { get; set; }
        
        
        
        [XmlAttribute("efficiency", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency { get; set; }
        
        [XmlIgnore]
        public bool EfficiencySpecified { get; set; }
        
        
        
        [XmlAttribute("efficiency_game_score", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency_Game_Score { get; set; }
        
        [XmlIgnore]
        public bool Efficiency_Game_ScoreSpecified { get; set; }
        
        
        
        [XmlAttribute("coach_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("pls_min", Form=XmlSchemaForm.Unqualified)]
        public string Pls_Min { get; set; }
        
        
        
        [XmlAttribute("points_off_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Points_Off_Turnovers { get; set; }
        
        
        
        [XmlAttribute("plus", Form=XmlSchemaForm.Unqualified)]
        public string Plus { get; set; }
        
        
        
        [XmlAttribute("minus", Form=XmlSchemaForm.Unqualified)]
        public string Minus { get; set; }
        
        
        
        [XmlAttribute("double_double", Form=XmlSchemaForm.Unqualified)]
        public bool Double_Double { get; set; }
        
        [XmlIgnore]
        public bool Double_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("triple_double", Form=XmlSchemaForm.Unqualified)]
        public bool Triple_Double { get; set; }
        
        [XmlIgnore]
        public bool Triple_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Defensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Offensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("steals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Steals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Steals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Turnovers_Pct { get; set; }
        
        [XmlIgnore]
        public bool Turnovers_PctSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseStatisticsAttributes
    {
        
        
        
        string Assists
        {
            get;
            set;
        }
        
        
        
        decimal Assists_Turnover_Ratio
        {
            get;
            set;
        }
        
        
        
        string Blocked_Att
        {
            get;
            set;
        }
        
        
        
        string Blocks
        {
            get;
            set;
        }
        
        
        
        string Defensive_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Field_Goals_Att
        {
            get;
            set;
        }
        
        
        
        string Field_Goals_Made
        {
            get;
            set;
        }
        
        
        
        decimal Field_Goals_Pct
        {
            get;
            set;
        }
        
        
        
        string Flagrant_Fouls
        {
            get;
            set;
        }
        
        
        
        string Free_Throws_Att
        {
            get;
            set;
        }
        
        
        
        string Free_Throws_Made
        {
            get;
            set;
        }
        
        
        
        decimal Free_Throws_Pct
        {
            get;
            set;
        }
        
        
        
        string Fast_Break_Pts
        {
            get;
            set;
        }
        
        
        
        string Fast_Break_Att
        {
            get;
            set;
        }
        
        
        
        string Fast_Break_Made
        {
            get;
            set;
        }
        
        
        
        decimal Fast_Break_Pct
        {
            get;
            set;
        }
        
        
        
        string Second_Chance_Pts
        {
            get;
            set;
        }
        
        
        
        string Second_Chance_Att
        {
            get;
            set;
        }
        
        
        
        string Second_Chance_Made
        {
            get;
            set;
        }
        
        
        
        decimal Second_Chance_Pct
        {
            get;
            set;
        }
        
        
        
        string Points_In_Paint
        {
            get;
            set;
        }
        
        
        
        string Points_In_Paint_Att
        {
            get;
            set;
        }
        
        
        
        string Points_In_Paint_Made
        {
            get;
            set;
        }
        
        
        
        decimal Points_In_Paint_Pct
        {
            get;
            set;
        }
        
        
        
        decimal True_Shooting_Att
        {
            get;
            set;
        }
        
        
        
        decimal True_Shooting_Pct
        {
            get;
            set;
        }
        
        
        
        string Minutes
        {
            get;
            set;
        }
        
        
        
        decimal Effective_Fg_Pct
        {
            get;
            set;
        }
        
        
        
        string Offensive_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Personal_Fouls
        {
            get;
            set;
        }
        
        
        
        string Ejections
        {
            get;
            set;
        }
        
        
        
        string Points
        {
            get;
            set;
        }
        
        
        
        string Rebounds
        {
            get;
            set;
        }
        
        
        
        string Steals
        {
            get;
            set;
        }
        
        
        
        string Tech_Fouls
        {
            get;
            set;
        }
        
        
        
        string Three_Points_Att
        {
            get;
            set;
        }
        
        
        
        string Three_Points_Made
        {
            get;
            set;
        }
        
        
        
        decimal Three_Points_Pct
        {
            get;
            set;
        }
        
        
        
        string Turnovers
        {
            get;
            set;
        }
        
        
        
        string Two_Points_Att
        {
            get;
            set;
        }
        
        
        
        string Two_Points_Made
        {
            get;
            set;
        }
        
        
        
        decimal Two_Points_Pct
        {
            get;
            set;
        }
        
        
        
        string Fouls_Drawn
        {
            get;
            set;
        }
        
        
        
        string Offensive_Fouls
        {
            get;
            set;
        }
        
        
        
        string Technical_Other
        {
            get;
            set;
        }
        
        
        
        string Coach_Ejections
        {
            get;
            set;
        }
        
        
        
        decimal Offensive_Rating
        {
            get;
            set;
        }
        
        
        
        decimal Defensive_Rating
        {
            get;
            set;
        }
        
        
        
        string Defensive_Assists
        {
            get;
            set;
        }
        
        
        
        decimal Efficiency
        {
            get;
            set;
        }
        
        
        
        decimal Efficiency_Game_Score
        {
            get;
            set;
        }
        
        
        
        string Coach_Tech_Fouls
        {
            get;
            set;
        }
        
        
        
        string Pls_Min
        {
            get;
            set;
        }
        
        
        
        string Points_Off_Turnovers
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPlayerStatisticsAttributes
    {
        
        
        
        string Plus
        {
            get;
            set;
        }
        
        
        
        string Minus
        {
            get;
            set;
        }
        
        
        
        bool Double_Double
        {
            get;
            set;
        }
        
        
        
        bool Triple_Double
        {
            get;
            set;
        }
        
        
        
        decimal Defensive_Rebounds_Pct
        {
            get;
            set;
        }
        
        
        
        decimal Offensive_Rebounds_Pct
        {
            get;
            set;
        }
        
        
        
        decimal Rebounds_Pct
        {
            get;
            set;
        }
        
        
        
        decimal Steals_Pct
        {
            get;
            set;
        }
        
        
        
        decimal Turnovers_Pct
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePlayerAttributes : IBasePersonnelAttributes
    {
        
        
        
        string Abbr_Name
        {
            get;
            set;
        }
        
        
        
        string Jersey_Number
        {
            get;
            set;
        }
        
        
        
        PositionType Position
        {
            get;
            set;
        }
        
        
        
        PositionType Primary_Position
        {
            get;
            set;
        }
        
        
        
        IBasePlayerAttributesStatus Status
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBasePlayerAttributesStatus", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum IBasePlayerAttributesStatus
    {
        
        
        
        ACT,
        
        
        
        SUS,
        
        
        
        IR,
        
        
        
        [XmlEnum("D-LEAGUE")]
        D_LEAGUE,
        
        
        
        NWT,
        
        
        
        FA,
        
        
        
        RET,
        
        
        
        PRM
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeStatistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeStatistics : IBaseStatisticsAttributes, ITeamStatisticsAttributes
    {
        
        
        
        [XmlElement("most_unanswered", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public TeamTypeStatisticsMost_Unanswered Most_Unanswered { get; set; }
        
        [XmlIgnore]
        private Collection<TeamTypeStatisticsPeriods> _periods;
        
        
        
        [XmlElement("periods", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypeStatisticsPeriods> Periods
        {
            get
            {
                return _periods;
            }
            private set
            {
                _periods = value;
            }
        }
        
        
        
        
        
        [XmlIgnore]
        public bool PeriodsSpecified
        {
            get
            {
                return Periods.Count != 0;
            }
        }
        
        public TeamTypeStatistics()
        {
            _periods = new Collection<TeamTypeStatisticsPeriods>();
        }
        
        
        
        [XmlAttribute("assists", Form=XmlSchemaForm.Unqualified)]
        public string Assists { get; set; }
        
        
        
        [XmlAttribute("assists_turnover_ratio", Form=XmlSchemaForm.Unqualified)]
        public decimal Assists_Turnover_Ratio { get; set; }
        
        [XmlIgnore]
        public bool Assists_Turnover_RatioSpecified { get; set; }
        
        
        
        [XmlAttribute("blocked_att", Form=XmlSchemaForm.Unqualified)]
        public string Blocked_Att { get; set; }
        
        
        
        [XmlAttribute("blocks", Form=XmlSchemaForm.Unqualified)]
        public string Blocks { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("field_goals_att", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Att { get; set; }
        
        
        
        [XmlAttribute("field_goals_made", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Made { get; set; }
        
        
        
        [XmlAttribute("field_goals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Field_Goals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Field_Goals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("flagrant_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Flagrant_Fouls { get; set; }
        
        
        
        [XmlAttribute("free_throws_att", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Att { get; set; }
        
        
        
        [XmlAttribute("free_throws_made", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Made { get; set; }
        
        
        
        [XmlAttribute("free_throws_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Free_Throws_Pct { get; set; }
        
        [XmlIgnore]
        public bool Free_Throws_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fast_break_pts", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Pts { get; set; }
        
        
        
        [XmlAttribute("fast_break_att", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Att { get; set; }
        
        
        
        [XmlAttribute("fast_break_made", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Made { get; set; }
        
        
        
        [XmlAttribute("fast_break_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Fast_Break_Pct { get; set; }
        
        [XmlIgnore]
        public bool Fast_Break_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("second_chance_pts", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Pts { get; set; }
        
        
        
        [XmlAttribute("second_chance_att", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Att { get; set; }
        
        
        
        [XmlAttribute("second_chance_made", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Made { get; set; }
        
        
        
        [XmlAttribute("second_chance_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Second_Chance_Pct { get; set; }
        
        [XmlIgnore]
        public bool Second_Chance_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("points_in_paint", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_att", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Att { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_made", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Made { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Points_In_Paint_Pct { get; set; }
        
        [XmlIgnore]
        public bool Points_In_Paint_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_att", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Att { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_AttSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Pct { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("minutes", Form=XmlSchemaForm.Unqualified)]
        public string Minutes { get; set; }
        
        
        
        [XmlAttribute("effective_fg_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Effective_Fg_Pct { get; set; }
        
        [XmlIgnore]
        public bool Effective_Fg_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("personal_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Personal_Fouls { get; set; }
        
        
        
        [XmlAttribute("ejections", Form=XmlSchemaForm.Unqualified)]
        public string Ejections { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Rebounds { get; set; }
        
        
        
        [XmlAttribute("steals", Form=XmlSchemaForm.Unqualified)]
        public string Steals { get; set; }
        
        
        
        [XmlAttribute("tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("three_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Att { get; set; }
        
        
        
        [XmlAttribute("three_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Made { get; set; }
        
        
        
        [XmlAttribute("three_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Three_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Three_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Turnovers { get; set; }
        
        
        
        [XmlAttribute("two_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Att { get; set; }
        
        
        
        [XmlAttribute("two_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Made { get; set; }
        
        
        
        [XmlAttribute("two_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Two_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Two_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fouls_drawn", Form=XmlSchemaForm.Unqualified)]
        public string Fouls_Drawn { get; set; }
        
        
        
        [XmlAttribute("offensive_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Fouls { get; set; }
        
        
        
        [XmlAttribute("technical_other", Form=XmlSchemaForm.Unqualified)]
        public string Technical_Other { get; set; }
        
        
        
        [XmlAttribute("coach_ejections", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Ejections { get; set; }
        
        
        
        [XmlAttribute("offensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Offensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Defensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_assists", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Assists { get; set; }
        
        
        
        [XmlAttribute("efficiency", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency { get; set; }
        
        [XmlIgnore]
        public bool EfficiencySpecified { get; set; }
        
        
        
        [XmlAttribute("efficiency_game_score", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency_Game_Score { get; set; }
        
        [XmlIgnore]
        public bool Efficiency_Game_ScoreSpecified { get; set; }
        
        
        
        [XmlAttribute("coach_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("pls_min", Form=XmlSchemaForm.Unqualified)]
        public string Pls_Min { get; set; }
        
        
        
        [XmlAttribute("points_off_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Points_Off_Turnovers { get; set; }
        
        
        
        [XmlAttribute("team_offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("team_defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("foulouts", Form=XmlSchemaForm.Unqualified)]
        public string Foulouts { get; set; }
        
        
        
        [XmlAttribute("points_against", Form=XmlSchemaForm.Unqualified)]
        public string Points_Against { get; set; }
        
        
        
        [XmlAttribute("team_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Team_Turnovers { get; set; }
        
        
        
        [XmlAttribute("team_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Rebounds { get; set; }
        
        
        
        [XmlAttribute("player_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Player_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("team_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Team_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("bench_points", Form=XmlSchemaForm.Unqualified)]
        public string Bench_Points { get; set; }
        
        
        
        [XmlAttribute("biggest_lead", Form=XmlSchemaForm.Unqualified)]
        public string Biggest_Lead { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Points { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_run_own_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Run_Own_Points { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_run_opp_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Run_Opp_Points { get; set; }
        
        
        
        [XmlAttribute("time_leading", Form=XmlSchemaForm.Unqualified)]
        public string Time_Leading { get; set; }
        
        
        
        [XmlAttribute("possessions", Form=XmlSchemaForm.Unqualified)]
        public decimal Possessions { get; set; }
        
        [XmlIgnore]
        public bool PossessionsSpecified { get; set; }
        
        
        
        [XmlAttribute("opponent_possessions", Form=XmlSchemaForm.Unqualified)]
        public decimal Opponent_Possessions { get; set; }
        
        [XmlIgnore]
        public bool Opponent_PossessionsSpecified { get; set; }
        
        
        
        [XmlAttribute("pace", Form=XmlSchemaForm.Unqualified)]
        public decimal Pace { get; set; }
        
        [XmlIgnore]
        public bool PaceSpecified { get; set; }
        
        
        
        [XmlAttribute("transition_offense", Form=XmlSchemaForm.Unqualified)]
        public decimal Transition_Offense { get; set; }
        
        [XmlIgnore]
        public bool Transition_OffenseSpecified { get; set; }
        
        
        
        [XmlAttribute("transition_defense", Form=XmlSchemaForm.Unqualified)]
        public decimal Transition_Defense { get; set; }
        
        [XmlIgnore]
        public bool Transition_DefenseSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_points_per_possession", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Points_Per_Possession { get; set; }
        
        [XmlIgnore]
        public bool Defensive_Points_Per_PossessionSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_points_per_possession", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Points_Per_Possession { get; set; }
        
        [XmlIgnore]
        public bool Offensive_Points_Per_PossessionSpecified { get; set; }
        
        
        
        [XmlAttribute("team_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Team_Fouls { get; set; }
        
        
        
        [XmlAttribute("total_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Total_Rebounds { get; set; }
        
        
        
        [XmlAttribute("total_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Total_Fouls { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeStatisticsMost_Unanswered", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeStatisticsMost_Unanswered
    {
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("own_score", Form=XmlSchemaForm.Unqualified)]
        public string Own_Score { get; set; }
        
        
        
        [XmlAttribute("opp_score", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Score { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeStatisticsPeriods", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeStatisticsPeriods
    {
        
        [XmlIgnore]
        private Collection<TeamTypeStatisticsPeriodsPeriod> _period;
        
        
        
        [XmlElement("period", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypeStatisticsPeriodsPeriod> Period
        {
            get
            {
                return _period;
            }
            private set
            {
                _period = value;
            }
        }
        
        [XmlIgnore]
        public bool PeriodSpecified
        {
            get
            {
                return Period.Count != 0;
            }
        }
        
        public TeamTypeStatisticsPeriods()
        {
            _period = new Collection<TeamTypeStatisticsPeriodsPeriod>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeStatisticsPeriodsPeriod", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeStatisticsPeriodsPeriod : IBaseStatisticsAttributes, ITeamStatisticsAttributes
    {
        
        
        
        [XmlAttribute("assists", Form=XmlSchemaForm.Unqualified)]
        public string Assists { get; set; }
        
        
        
        [XmlAttribute("assists_turnover_ratio", Form=XmlSchemaForm.Unqualified)]
        public decimal Assists_Turnover_Ratio { get; set; }
        
        [XmlIgnore]
        public bool Assists_Turnover_RatioSpecified { get; set; }
        
        
        
        [XmlAttribute("blocked_att", Form=XmlSchemaForm.Unqualified)]
        public string Blocked_Att { get; set; }
        
        
        
        [XmlAttribute("blocks", Form=XmlSchemaForm.Unqualified)]
        public string Blocks { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("field_goals_att", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Att { get; set; }
        
        
        
        [XmlAttribute("field_goals_made", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Made { get; set; }
        
        
        
        [XmlAttribute("field_goals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Field_Goals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Field_Goals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("flagrant_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Flagrant_Fouls { get; set; }
        
        
        
        [XmlAttribute("free_throws_att", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Att { get; set; }
        
        
        
        [XmlAttribute("free_throws_made", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Made { get; set; }
        
        
        
        [XmlAttribute("free_throws_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Free_Throws_Pct { get; set; }
        
        [XmlIgnore]
        public bool Free_Throws_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fast_break_pts", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Pts { get; set; }
        
        
        
        [XmlAttribute("fast_break_att", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Att { get; set; }
        
        
        
        [XmlAttribute("fast_break_made", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Made { get; set; }
        
        
        
        [XmlAttribute("fast_break_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Fast_Break_Pct { get; set; }
        
        [XmlIgnore]
        public bool Fast_Break_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("second_chance_pts", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Pts { get; set; }
        
        
        
        [XmlAttribute("second_chance_att", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Att { get; set; }
        
        
        
        [XmlAttribute("second_chance_made", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Made { get; set; }
        
        
        
        [XmlAttribute("second_chance_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Second_Chance_Pct { get; set; }
        
        [XmlIgnore]
        public bool Second_Chance_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("points_in_paint", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_att", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Att { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_made", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Made { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Points_In_Paint_Pct { get; set; }
        
        [XmlIgnore]
        public bool Points_In_Paint_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_att", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Att { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_AttSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Pct { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("minutes", Form=XmlSchemaForm.Unqualified)]
        public string Minutes { get; set; }
        
        
        
        [XmlAttribute("effective_fg_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Effective_Fg_Pct { get; set; }
        
        [XmlIgnore]
        public bool Effective_Fg_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("personal_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Personal_Fouls { get; set; }
        
        
        
        [XmlAttribute("ejections", Form=XmlSchemaForm.Unqualified)]
        public string Ejections { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Rebounds { get; set; }
        
        
        
        [XmlAttribute("steals", Form=XmlSchemaForm.Unqualified)]
        public string Steals { get; set; }
        
        
        
        [XmlAttribute("tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("three_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Att { get; set; }
        
        
        
        [XmlAttribute("three_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Made { get; set; }
        
        
        
        [XmlAttribute("three_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Three_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Three_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Turnovers { get; set; }
        
        
        
        [XmlAttribute("two_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Att { get; set; }
        
        
        
        [XmlAttribute("two_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Made { get; set; }
        
        
        
        [XmlAttribute("two_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Two_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Two_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fouls_drawn", Form=XmlSchemaForm.Unqualified)]
        public string Fouls_Drawn { get; set; }
        
        
        
        [XmlAttribute("offensive_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Fouls { get; set; }
        
        
        
        [XmlAttribute("technical_other", Form=XmlSchemaForm.Unqualified)]
        public string Technical_Other { get; set; }
        
        
        
        [XmlAttribute("coach_ejections", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Ejections { get; set; }
        
        
        
        [XmlAttribute("offensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Offensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Defensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_assists", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Assists { get; set; }
        
        
        
        [XmlAttribute("efficiency", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency { get; set; }
        
        [XmlIgnore]
        public bool EfficiencySpecified { get; set; }
        
        
        
        [XmlAttribute("efficiency_game_score", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency_Game_Score { get; set; }
        
        [XmlIgnore]
        public bool Efficiency_Game_ScoreSpecified { get; set; }
        
        
        
        [XmlAttribute("coach_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("pls_min", Form=XmlSchemaForm.Unqualified)]
        public string Pls_Min { get; set; }
        
        
        
        [XmlAttribute("points_off_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Points_Off_Turnovers { get; set; }
        
        
        
        [XmlAttribute("team_offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("team_defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("foulouts", Form=XmlSchemaForm.Unqualified)]
        public string Foulouts { get; set; }
        
        
        
        [XmlAttribute("points_against", Form=XmlSchemaForm.Unqualified)]
        public string Points_Against { get; set; }
        
        
        
        [XmlAttribute("team_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Team_Turnovers { get; set; }
        
        
        
        [XmlAttribute("team_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Team_Rebounds { get; set; }
        
        
        
        [XmlAttribute("player_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Player_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("team_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Team_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("bench_points", Form=XmlSchemaForm.Unqualified)]
        public string Bench_Points { get; set; }
        
        
        
        [XmlAttribute("biggest_lead", Form=XmlSchemaForm.Unqualified)]
        public string Biggest_Lead { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Points { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_run_own_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Run_Own_Points { get; set; }
        
        
        
        [XmlAttribute("most_unanswered_run_opp_points", Form=XmlSchemaForm.Unqualified)]
        public string Most_Unanswered_Run_Opp_Points { get; set; }
        
        
        
        [XmlAttribute("time_leading", Form=XmlSchemaForm.Unqualified)]
        public string Time_Leading { get; set; }
        
        
        
        [XmlAttribute("possessions", Form=XmlSchemaForm.Unqualified)]
        public decimal Possessions { get; set; }
        
        [XmlIgnore]
        public bool PossessionsSpecified { get; set; }
        
        
        
        [XmlAttribute("opponent_possessions", Form=XmlSchemaForm.Unqualified)]
        public decimal Opponent_Possessions { get; set; }
        
        [XmlIgnore]
        public bool Opponent_PossessionsSpecified { get; set; }
        
        
        
        [XmlAttribute("pace", Form=XmlSchemaForm.Unqualified)]
        public decimal Pace { get; set; }
        
        [XmlIgnore]
        public bool PaceSpecified { get; set; }
        
        
        
        [XmlAttribute("transition_offense", Form=XmlSchemaForm.Unqualified)]
        public decimal Transition_Offense { get; set; }
        
        [XmlIgnore]
        public bool Transition_OffenseSpecified { get; set; }
        
        
        
        [XmlAttribute("transition_defense", Form=XmlSchemaForm.Unqualified)]
        public decimal Transition_Defense { get; set; }
        
        [XmlIgnore]
        public bool Transition_DefenseSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_points_per_possession", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Points_Per_Possession { get; set; }
        
        [XmlIgnore]
        public bool Defensive_Points_Per_PossessionSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_points_per_possession", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Points_Per_Possession { get; set; }
        
        [XmlIgnore]
        public bool Offensive_Points_Per_PossessionSpecified { get; set; }
        
        
        
        [XmlAttribute("team_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Team_Fouls { get; set; }
        
        
        
        [XmlAttribute("total_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Total_Rebounds { get; set; }
        
        
        
        [XmlAttribute("total_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Total_Fouls { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface ITeamStatisticsAttributes
    {
        
        
        
        string Team_Offensive_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Team_Defensive_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Foulouts
        {
            get;
            set;
        }
        
        
        
        string Points_Against
        {
            get;
            set;
        }
        
        
        
        string Team_Turnovers
        {
            get;
            set;
        }
        
        
        
        string Team_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Player_Tech_Fouls
        {
            get;
            set;
        }
        
        
        
        string Team_Tech_Fouls
        {
            get;
            set;
        }
        
        
        
        string Bench_Points
        {
            get;
            set;
        }
        
        
        
        string Biggest_Lead
        {
            get;
            set;
        }
        
        
        
        string Most_Unanswered_Points
        {
            get;
            set;
        }
        
        
        
        string Most_Unanswered_Run_Own_Points
        {
            get;
            set;
        }
        
        
        
        string Most_Unanswered_Run_Opp_Points
        {
            get;
            set;
        }
        
        
        
        string Time_Leading
        {
            get;
            set;
        }
        
        
        
        decimal Possessions
        {
            get;
            set;
        }
        
        
        
        decimal Opponent_Possessions
        {
            get;
            set;
        }
        
        
        
        decimal Pace
        {
            get;
            set;
        }
        
        
        
        decimal Transition_Offense
        {
            get;
            set;
        }
        
        
        
        decimal Transition_Defense
        {
            get;
            set;
        }
        
        
        
        decimal Defensive_Points_Per_Possession
        {
            get;
            set;
        }
        
        
        
        decimal Offensive_Points_Per_Possession
        {
            get;
            set;
        }
        
        
        
        string Team_Fouls
        {
            get;
            set;
        }
        
        
        
        string Total_Rebounds
        {
            get;
            set;
        }
        
        
        
        string Total_Fouls
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeCoaches", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeCoaches
    {
        
        [XmlIgnore]
        private Collection<TeamTypeCoachesCoach> _coach;
        
        
        
        [XmlElement("coach", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypeCoachesCoach> Coach
        {
            get
            {
                return _coach;
            }
            private set
            {
                _coach = value;
            }
        }
        
        [XmlIgnore]
        public bool CoachSpecified
        {
            get
            {
                return Coach.Count != 0;
            }
        }
        
        public TeamTypeCoaches()
        {
            _coach = new Collection<TeamTypeCoachesCoach>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeCoachesCoach", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeCoachesCoach : IBaseCoachAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("first_name", Form=XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }
        
        
        
        [XmlAttribute("last_name", Form=XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }
        
        
        
        [XmlAttribute("full_name", Form=XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("position", Form=XmlSchemaForm.Unqualified)]
        public string Position { get; set; }
        
        
        
        [XmlAttribute("experience", Form=XmlSchemaForm.Unqualified)]
        public string Experience { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseCoachAttributes : IBasePersonnelAttributes
    {
        
        
        
        string Position
        {
            get;
            set;
        }
        
        
        
        string Experience
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayers", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayers
    {
        
        [XmlIgnore]
        private Collection<TeamTypePlayersPlayer> _player;
        
        
        
        [XmlElement("player", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypePlayersPlayer> Player
        {
            get
            {
                return _player;
            }
            private set
            {
                _player = value;
            }
        }
        
        [XmlIgnore]
        public bool PlayerSpecified
        {
            get
            {
                return Player.Count != 0;
            }
        }
        
        public TeamTypePlayers()
        {
            _player = new Collection<TeamTypePlayersPlayer>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayer", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayer : IBasePlayerAttributes
    {
        
        [XmlIgnore]
        private Collection<InjuryType> _injuries;
        
        
        
        [XmlArray("injuries", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        [XmlArrayItem("injury", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<InjuryType> Injuries
        {
            get
            {
                return _injuries;
            }
            private set
            {
                _injuries = value;
            }
        }
        
        [XmlIgnore]
        public bool InjuriesSpecified
        {
            get
            {
                return Injuries.Count != 0;
            }
        }
        
        public TeamTypePlayersPlayer()
        {
            _injuries = new Collection<InjuryType>();
        }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public TeamTypePlayersPlayerStatistics Statistics { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("first_name", Form=XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }
        
        
        
        [XmlAttribute("last_name", Form=XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }
        
        
        
        [XmlAttribute("full_name", Form=XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("abbr_name", Form=XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }
        
        
        
        [XmlAttribute("jersey_number", Form=XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }
        
        
        
        [XmlAttribute("position", Form=XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }
        
        [XmlIgnore]
        public bool PositionSpecified { get; set; }
        
        
        
        [XmlAttribute("primary_position", Form=XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }
        
        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }
        
        
        
        [XmlAttribute("status", Form=XmlSchemaForm.Unqualified)]
        public IBasePlayerAttributesStatus Status { get; set; }
        
        [XmlIgnore]
        public bool StatusSpecified { get; set; }
        
        
        
        [XmlAttribute("active", Form=XmlSchemaForm.Unqualified)]
        public bool Active { get; set; }
        
        [XmlIgnore]
        public bool ActiveSpecified { get; set; }
        
        
        
        [XmlAttribute("played", Form=XmlSchemaForm.Unqualified)]
        public bool Played { get; set; }
        
        [XmlIgnore]
        public bool PlayedSpecified { get; set; }
        
        
        
        [XmlAttribute("starter", Form=XmlSchemaForm.Unqualified)]
        public bool Starter { get; set; }
        
        [XmlIgnore]
        public bool StarterSpecified { get; set; }
        
        
        
        [XmlAttribute("fouled_out", Form=XmlSchemaForm.Unqualified)]
        public bool Fouled_Out { get; set; }
        
        [XmlIgnore]
        public bool Fouled_OutSpecified { get; set; }
        
        
        
        [XmlAttribute("ejected", Form=XmlSchemaForm.Unqualified)]
        public bool Ejected { get; set; }
        
        [XmlIgnore]
        public bool EjectedSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayerInjuries", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayerInjuries
    {
        
        [XmlIgnore]
        private Collection<InjuryType> _injury;
        
        
        
        [XmlElement("injury", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<InjuryType> Injury
        {
            get
            {
                return _injury;
            }
            private set
            {
                _injury = value;
            }
        }
        
        [XmlIgnore]
        public bool InjurySpecified
        {
            get
            {
                return Injury.Count != 0;
            }
        }
        
        public TeamTypePlayersPlayerInjuries()
        {
            _injury = new Collection<InjuryType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayerStatistics", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayerStatistics : IBaseStatisticsAttributes, IPlayerStatisticsAttributes
    {
        
        [XmlIgnore]
        private Collection<TeamTypePlayersPlayerStatisticsPeriods> _periods;
        
        
        
        [XmlElement("periods", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypePlayersPlayerStatisticsPeriods> Periods
        {
            get
            {
                return _periods;
            }
            private set
            {
                _periods = value;
            }
        }
        
        
        
        
        
        [XmlIgnore]
        public bool PeriodsSpecified
        {
            get
            {
                return Periods.Count != 0;
            }
        }
        
        public TeamTypePlayersPlayerStatistics()
        {
            _periods = new Collection<TeamTypePlayersPlayerStatisticsPeriods>();
        }
        
        
        
        [XmlAttribute("assists", Form=XmlSchemaForm.Unqualified)]
        public string Assists { get; set; }
        
        
        
        [XmlAttribute("assists_turnover_ratio", Form=XmlSchemaForm.Unqualified)]
        public decimal Assists_Turnover_Ratio { get; set; }
        
        [XmlIgnore]
        public bool Assists_Turnover_RatioSpecified { get; set; }
        
        
        
        [XmlAttribute("blocked_att", Form=XmlSchemaForm.Unqualified)]
        public string Blocked_Att { get; set; }
        
        
        
        [XmlAttribute("blocks", Form=XmlSchemaForm.Unqualified)]
        public string Blocks { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("field_goals_att", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Att { get; set; }
        
        
        
        [XmlAttribute("field_goals_made", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Made { get; set; }
        
        
        
        [XmlAttribute("field_goals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Field_Goals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Field_Goals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("flagrant_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Flagrant_Fouls { get; set; }
        
        
        
        [XmlAttribute("free_throws_att", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Att { get; set; }
        
        
        
        [XmlAttribute("free_throws_made", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Made { get; set; }
        
        
        
        [XmlAttribute("free_throws_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Free_Throws_Pct { get; set; }
        
        [XmlIgnore]
        public bool Free_Throws_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fast_break_pts", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Pts { get; set; }
        
        
        
        [XmlAttribute("fast_break_att", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Att { get; set; }
        
        
        
        [XmlAttribute("fast_break_made", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Made { get; set; }
        
        
        
        [XmlAttribute("fast_break_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Fast_Break_Pct { get; set; }
        
        [XmlIgnore]
        public bool Fast_Break_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("second_chance_pts", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Pts { get; set; }
        
        
        
        [XmlAttribute("second_chance_att", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Att { get; set; }
        
        
        
        [XmlAttribute("second_chance_made", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Made { get; set; }
        
        
        
        [XmlAttribute("second_chance_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Second_Chance_Pct { get; set; }
        
        [XmlIgnore]
        public bool Second_Chance_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("points_in_paint", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_att", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Att { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_made", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Made { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Points_In_Paint_Pct { get; set; }
        
        [XmlIgnore]
        public bool Points_In_Paint_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_att", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Att { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_AttSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Pct { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("minutes", Form=XmlSchemaForm.Unqualified)]
        public string Minutes { get; set; }
        
        
        
        [XmlAttribute("effective_fg_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Effective_Fg_Pct { get; set; }
        
        [XmlIgnore]
        public bool Effective_Fg_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("personal_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Personal_Fouls { get; set; }
        
        
        
        [XmlAttribute("ejections", Form=XmlSchemaForm.Unqualified)]
        public string Ejections { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Rebounds { get; set; }
        
        
        
        [XmlAttribute("steals", Form=XmlSchemaForm.Unqualified)]
        public string Steals { get; set; }
        
        
        
        [XmlAttribute("tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("three_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Att { get; set; }
        
        
        
        [XmlAttribute("three_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Made { get; set; }
        
        
        
        [XmlAttribute("three_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Three_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Three_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Turnovers { get; set; }
        
        
        
        [XmlAttribute("two_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Att { get; set; }
        
        
        
        [XmlAttribute("two_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Made { get; set; }
        
        
        
        [XmlAttribute("two_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Two_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Two_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fouls_drawn", Form=XmlSchemaForm.Unqualified)]
        public string Fouls_Drawn { get; set; }
        
        
        
        [XmlAttribute("offensive_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Fouls { get; set; }
        
        
        
        [XmlAttribute("technical_other", Form=XmlSchemaForm.Unqualified)]
        public string Technical_Other { get; set; }
        
        
        
        [XmlAttribute("coach_ejections", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Ejections { get; set; }
        
        
        
        [XmlAttribute("offensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Offensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Defensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_assists", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Assists { get; set; }
        
        
        
        [XmlAttribute("efficiency", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency { get; set; }
        
        [XmlIgnore]
        public bool EfficiencySpecified { get; set; }
        
        
        
        [XmlAttribute("efficiency_game_score", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency_Game_Score { get; set; }
        
        [XmlIgnore]
        public bool Efficiency_Game_ScoreSpecified { get; set; }
        
        
        
        [XmlAttribute("coach_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("pls_min", Form=XmlSchemaForm.Unqualified)]
        public string Pls_Min { get; set; }
        
        
        
        [XmlAttribute("points_off_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Points_Off_Turnovers { get; set; }
        
        
        
        [XmlAttribute("plus", Form=XmlSchemaForm.Unqualified)]
        public string Plus { get; set; }
        
        
        
        [XmlAttribute("minus", Form=XmlSchemaForm.Unqualified)]
        public string Minus { get; set; }
        
        
        
        [XmlAttribute("double_double", Form=XmlSchemaForm.Unqualified)]
        public bool Double_Double { get; set; }
        
        [XmlIgnore]
        public bool Double_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("triple_double", Form=XmlSchemaForm.Unqualified)]
        public bool Triple_Double { get; set; }
        
        [XmlIgnore]
        public bool Triple_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Defensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Offensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("steals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Steals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Steals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Turnovers_Pct { get; set; }
        
        [XmlIgnore]
        public bool Turnovers_PctSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayerStatisticsPeriods", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayerStatisticsPeriods
    {
        
        [XmlIgnore]
        private Collection<TeamTypePlayersPlayerStatisticsPeriodsPeriod> _period;
        
        
        
        [XmlElement("period", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<TeamTypePlayersPlayerStatisticsPeriodsPeriod> Period
        {
            get
            {
                return _period;
            }
            private set
            {
                _period = value;
            }
        }
        
        [XmlIgnore]
        public bool PeriodSpecified
        {
            get
            {
                return Period.Count != 0;
            }
        }
        
        public TeamTypePlayersPlayerStatisticsPeriods()
        {
            _period = new Collection<TeamTypePlayersPlayerStatisticsPeriodsPeriod>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayerStatisticsPeriodsPeriod", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayerStatisticsPeriodsPeriod : IBaseStatisticsAttributes, IPlayerStatisticsAttributes
    {
        
        
        
        [XmlAttribute("assists", Form=XmlSchemaForm.Unqualified)]
        public string Assists { get; set; }
        
        
        
        [XmlAttribute("assists_turnover_ratio", Form=XmlSchemaForm.Unqualified)]
        public decimal Assists_Turnover_Ratio { get; set; }
        
        [XmlIgnore]
        public bool Assists_Turnover_RatioSpecified { get; set; }
        
        
        
        [XmlAttribute("blocked_att", Form=XmlSchemaForm.Unqualified)]
        public string Blocked_Att { get; set; }
        
        
        
        [XmlAttribute("blocks", Form=XmlSchemaForm.Unqualified)]
        public string Blocks { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("field_goals_att", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Att { get; set; }
        
        
        
        [XmlAttribute("field_goals_made", Form=XmlSchemaForm.Unqualified)]
        public string Field_Goals_Made { get; set; }
        
        
        
        [XmlAttribute("field_goals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Field_Goals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Field_Goals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("flagrant_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Flagrant_Fouls { get; set; }
        
        
        
        [XmlAttribute("free_throws_att", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Att { get; set; }
        
        
        
        [XmlAttribute("free_throws_made", Form=XmlSchemaForm.Unqualified)]
        public string Free_Throws_Made { get; set; }
        
        
        
        [XmlAttribute("free_throws_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Free_Throws_Pct { get; set; }
        
        [XmlIgnore]
        public bool Free_Throws_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fast_break_pts", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Pts { get; set; }
        
        
        
        [XmlAttribute("fast_break_att", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Att { get; set; }
        
        
        
        [XmlAttribute("fast_break_made", Form=XmlSchemaForm.Unqualified)]
        public string Fast_Break_Made { get; set; }
        
        
        
        [XmlAttribute("fast_break_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Fast_Break_Pct { get; set; }
        
        [XmlIgnore]
        public bool Fast_Break_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("second_chance_pts", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Pts { get; set; }
        
        
        
        [XmlAttribute("second_chance_att", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Att { get; set; }
        
        
        
        [XmlAttribute("second_chance_made", Form=XmlSchemaForm.Unqualified)]
        public string Second_Chance_Made { get; set; }
        
        
        
        [XmlAttribute("second_chance_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Second_Chance_Pct { get; set; }
        
        [XmlIgnore]
        public bool Second_Chance_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("points_in_paint", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_att", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Att { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_made", Form=XmlSchemaForm.Unqualified)]
        public string Points_In_Paint_Made { get; set; }
        
        
        
        [XmlAttribute("points_in_paint_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Points_In_Paint_Pct { get; set; }
        
        [XmlIgnore]
        public bool Points_In_Paint_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_att", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Att { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_AttSpecified { get; set; }
        
        
        
        [XmlAttribute("true_shooting_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal True_Shooting_Pct { get; set; }
        
        [XmlIgnore]
        public bool True_Shooting_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("minutes", Form=XmlSchemaForm.Unqualified)]
        public string Minutes { get; set; }
        
        
        
        [XmlAttribute("effective_fg_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Effective_Fg_Pct { get; set; }
        
        [XmlIgnore]
        public bool Effective_Fg_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Rebounds { get; set; }
        
        
        
        [XmlAttribute("personal_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Personal_Fouls { get; set; }
        
        
        
        [XmlAttribute("ejections", Form=XmlSchemaForm.Unqualified)]
        public string Ejections { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("rebounds", Form=XmlSchemaForm.Unqualified)]
        public string Rebounds { get; set; }
        
        
        
        [XmlAttribute("steals", Form=XmlSchemaForm.Unqualified)]
        public string Steals { get; set; }
        
        
        
        [XmlAttribute("tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("three_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Att { get; set; }
        
        
        
        [XmlAttribute("three_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Three_Points_Made { get; set; }
        
        
        
        [XmlAttribute("three_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Three_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Three_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Turnovers { get; set; }
        
        
        
        [XmlAttribute("two_points_att", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Att { get; set; }
        
        
        
        [XmlAttribute("two_points_made", Form=XmlSchemaForm.Unqualified)]
        public string Two_Points_Made { get; set; }
        
        
        
        [XmlAttribute("two_points_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Two_Points_Pct { get; set; }
        
        [XmlIgnore]
        public bool Two_Points_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("fouls_drawn", Form=XmlSchemaForm.Unqualified)]
        public string Fouls_Drawn { get; set; }
        
        
        
        [XmlAttribute("offensive_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Offensive_Fouls { get; set; }
        
        
        
        [XmlAttribute("technical_other", Form=XmlSchemaForm.Unqualified)]
        public string Technical_Other { get; set; }
        
        
        
        [XmlAttribute("coach_ejections", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Ejections { get; set; }
        
        
        
        [XmlAttribute("offensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Offensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rating", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rating { get; set; }
        
        [XmlIgnore]
        public bool Defensive_RatingSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_assists", Form=XmlSchemaForm.Unqualified)]
        public string Defensive_Assists { get; set; }
        
        
        
        [XmlAttribute("efficiency", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency { get; set; }
        
        [XmlIgnore]
        public bool EfficiencySpecified { get; set; }
        
        
        
        [XmlAttribute("efficiency_game_score", Form=XmlSchemaForm.Unqualified)]
        public decimal Efficiency_Game_Score { get; set; }
        
        [XmlIgnore]
        public bool Efficiency_Game_ScoreSpecified { get; set; }
        
        
        
        [XmlAttribute("coach_tech_fouls", Form=XmlSchemaForm.Unqualified)]
        public string Coach_Tech_Fouls { get; set; }
        
        
        
        [XmlAttribute("pls_min", Form=XmlSchemaForm.Unqualified)]
        public string Pls_Min { get; set; }
        
        
        
        [XmlAttribute("points_off_turnovers", Form=XmlSchemaForm.Unqualified)]
        public string Points_Off_Turnovers { get; set; }
        
        
        
        [XmlAttribute("plus", Form=XmlSchemaForm.Unqualified)]
        public string Plus { get; set; }
        
        
        
        [XmlAttribute("minus", Form=XmlSchemaForm.Unqualified)]
        public string Minus { get; set; }
        
        
        
        [XmlAttribute("double_double", Form=XmlSchemaForm.Unqualified)]
        public bool Double_Double { get; set; }
        
        [XmlIgnore]
        public bool Double_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("triple_double", Form=XmlSchemaForm.Unqualified)]
        public bool Triple_Double { get; set; }
        
        [XmlIgnore]
        public bool Triple_DoubleSpecified { get; set; }
        
        
        
        [XmlAttribute("defensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Defensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Defensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("offensive_rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Offensive_Rebounds_Pct { get; set; }
        
        [XmlIgnore]
        public bool Offensive_Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("rebounds_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Rebounds_Pct { get; set; }
       
        [XmlIgnore]
        public bool Rebounds_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("steals_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Steals_Pct { get; set; }
        
        [XmlIgnore]
        public bool Steals_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("turnovers_pct", Form=XmlSchemaForm.Unqualified)]
        public decimal Turnovers_Pct { get; set; }
        
        [XmlIgnore]
        public bool Turnovers_PctSpecified { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseTeamAttributes : IBaseOrganizationAttributes
    {
        
        
        
        string Market
        {
            get;
            set;
        }
        
        
        
        string Founded
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeOfficials", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeOfficials
    {
        
        [XmlIgnore]
        private Collection<GameTypeOfficialsOfficial> _official;
        
        
        
        [XmlElement("official", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
        public Collection<GameTypeOfficialsOfficial> Official
        {
            get
            {
                return _official;
            }
            private set
            {
                _official = value;
            }
        }
        
        [XmlIgnore]
        public bool OfficialSpecified
        {
            get
            {
                return Official.Count != 0;
            }
        }
        
        public GameTypeOfficials()
        {
            _official = new Collection<GameTypeOfficialsOfficial>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeOfficialsOfficial", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeOfficialsOfficial : IBasePersonnelAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("first_name", Form=XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }
        
        
        
        [XmlAttribute("last_name", Form=XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }
        
        
        
        [XmlAttribute("full_name", Form=XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("assignment", Form=XmlSchemaForm.Unqualified)]
        public string Assignment { get; set; }
        
        
        
        [XmlAttribute("experience", Form=XmlSchemaForm.Unqualified)]
        public string Experience { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseGameAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Title
        {
            get;
            set;
        }
        
        
        
        IBaseGameAttributesStatus Status
        {
            get;
            set;
        }
        
        
        
        IBaseGameAttributesCoverage Coverage
        {
            get;
            set;
        }
        
        
        
        string Away_Team
        {
            get;
            set;
        }
        
        
        
        string Home_Team
        {
            get;
            set;
        }
        
        
        
        DateTime Scheduled
        {
            get;
            set;
        }
        
        
        
        string Possession_Arrow
        {
            get;
            set;
        }
        
        
        
        bool Conference_Game
        {
            get;
            set;
        }
        
        
        
        bool Neutral_Site
        {
            get;
            set;
        }
        
        
        
        string Reference
        {
            get;
            set;
        }
        
        
        
        bool Track_On_Court
        {
            get;
            set;
        }
        
        
        
        IBaseGameAttributesEntry_Mode Entry_Mode
        {
            get;
            set;
        }
        
        
        
        string Sr_Id
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesStatus", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum IBaseGameAttributesStatus
    {
        
        
        
        [XmlEnum("scheduled")]
        Scheduled,
        
        
        
        [XmlEnum("created")]
        Created,
        
        
        
        [XmlEnum("inprogress")]
        Inprogress,
        
        
        
        [XmlEnum("halftime")]
        Halftime,
        
        
        
        [XmlEnum("complete")]
        Complete,
        
        
        
        [XmlEnum("closed")]
        Closed,
        
        
        
        [XmlEnum("cancelled")]
        Cancelled,
        
        
        
        [XmlEnum("postponed")]
        Postponed,
        
        
        
        [XmlEnum("delayed")]
        Delayed,
        
        
        
        [XmlEnum("unnecessary")]
        Unnecessary,
        
        
        
        [XmlEnum("time-tbd")]
        Time_Tbd,
        
        
        
        [XmlEnum("if-necessary")]
        If_Necessary
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesCoverage", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum IBaseGameAttributesCoverage
    {
        
        
        
        [XmlEnum("full")]
        Full,
        
        
        
        [XmlEnum("extended_boxscore")]
        Extended_Boxscore,
        
        
        
        [XmlEnum("boxscore")]
        Boxscore
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesEntry_Mode", Namespace="http://feed.elasticstats.com/schema/basketball/game-v3.0.xsd")]
    public enum IBaseGameAttributesEntry_Mode
    {
        
        
        
        WEBSOCKET,
        
        
        
        HTTP,
        
        
        
        LDE
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IGameStateAttributes
    {
        
        
        
        string Clock
        {
            get;
            set;
        }
        
        
        
        string Quarter
        {
            get;
            set;
        }
        
        
        
        string Half
        {
            get;
            set;
        }
    }
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IGameMetadataAttributes
    {
        string Attendance
        {
            get;
            set;
        }
        
        string Lead_Changes
        {
            get;
            set;
        }
        
        string Times_Tied
        {
            get;
            set;
        }
        
        string Duration
        {
            get;
            set;
        }
    }
}
