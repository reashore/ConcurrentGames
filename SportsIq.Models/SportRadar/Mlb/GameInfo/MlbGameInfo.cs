using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable All

namespace SportsIq.Models.SportRadar.Mlb.GameInfo
{
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("game", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public class MlbGameInfo : IBaseGameAttributes
    {
        [XmlElement("venue", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public VenueType Venue { get; set; }

        [XmlElement("broadcast", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public BroadcastType Broadcast { get; set; }

        [XmlElement("weather", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public WeatherType Weather { get; set; }

        [XmlIgnore]
        private Collection<RescheduledType> _rescheduled_From;

        [XmlElement("rescheduled_from", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<RescheduledType> Rescheduled_From
        {
            get => _rescheduled_From;
            private set => _rescheduled_From = value;
        }

        [XmlIgnore]
        public bool Rescheduled_FromSpecified => Rescheduled_From.Count != 0;

        public MlbGameInfo()
        {
            _rescheduled_From = new Collection<RescheduledType>();
            _home = new Collection<TeamType>();
            _away = new Collection<TeamType>();
            _pitching = new Collection<PitchingType>();
            _officials = new Collection<GameTypeOfficialsOfficial>();
        }

        [XmlElement("final", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public GameTypeFinal Final { get; set; }

        [XmlElement("outcome", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public OutcomeType Outcome { get; set; }

        [XmlIgnore]
        private Collection<TeamType> _home;

        [XmlElement("home", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamType> Home
        {
            get => _home;
            private set => _home = value;
        }

        [XmlIgnore]
        public bool HomeSpecified => Home.Count != 0;

        [XmlIgnore]
        private Collection<TeamType> _away;

        [XmlElement("away", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamType> Away
        {
            get
            {
                return _away;
            }
            private set
            {
                _away = value;
            }
        }

        [XmlIgnore]
        public bool AwaySpecified
        {
            get
            {
                return Away.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingType> _pitching;

        [XmlElement("pitching", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingType> Pitching
        {
            get
            {
                return _pitching;
            }
            private set
            {
                _pitching = value;
            }
        }

        [XmlIgnore]
        public bool PitchingSpecified
        {
            get
            {
                return Pitching.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<GameTypeOfficialsOfficial> _officials;

        [XmlArray("officials", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("official", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
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

        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("title", Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }

        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesStatus Status { get; set; }

        [XmlIgnore]
        public bool StatusSpecified { get; set; }

        [XmlAttribute("coverage", Form = XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesCoverage Coverage { get; set; }

        [XmlIgnore]
        public bool CoverageSpecified { get; set; }

        [XmlAttribute("attendance", Form = XmlSchemaForm.Unqualified)]
        public string Attendance { get; set; }

        [XmlAttribute("duration", Form = XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }

        [XmlAttribute("game_number", Form = XmlSchemaForm.Unqualified)]
        public string Game_Number { get; set; }

        [XmlAttribute("day_night", Form = XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesDay_Night Day_Night { get; set; }

        [XmlIgnore]
        public bool Day_NightSpecified { get; set; }

        [XmlAttribute("double_header", Form = XmlSchemaForm.Unqualified)]
        public bool Double_Header { get; set; }

        [XmlIgnore]
        public bool Double_HeaderSpecified { get; set; }

        [XmlAttribute("split_squad", Form = XmlSchemaForm.Unqualified)]
        public bool Split_Squad { get; set; }

        [XmlIgnore]
        public bool Split_SquadSpecified { get; set; }

        [XmlAttribute("away_team", Form = XmlSchemaForm.Unqualified)]
        public string Away_Team { get; set; }

        [XmlAttribute("home_team", Form = XmlSchemaForm.Unqualified)]
        public string Home_Team { get; set; }

        [XmlAttribute("scheduled", Form = XmlSchemaForm.Unqualified, DataType = "dateTime")]
        public DateTime Scheduled { get; set; }

        [XmlIgnore]
        public bool ScheduledSpecified { get; set; }

        [XmlAttribute("tbd", Form = XmlSchemaForm.Unqualified)]
        public bool Tbd { get; set; }

        [XmlIgnore]
        public bool TbdSpecified { get; set; }

        [XmlAttribute("ps_round", Form = XmlSchemaForm.Unqualified)]
        public string Ps_Round { get; set; }

        [XmlAttribute("ps_game", Form = XmlSchemaForm.Unqualified)]
        public string Ps_Game { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("conditionsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ConditionsType
    {
        [XmlElement("wind", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public ConditionsTypeWind Wind { get; set; }

        [XmlAttribute("temp_f", Form = XmlSchemaForm.Unqualified)]
        public string TempF { get; set; }

        [XmlAttribute("condition", Form = XmlSchemaForm.Unqualified)]
        public string Condition { get; set; }

        [XmlAttribute("humidity", Form = XmlSchemaForm.Unqualified)]
        public string Humidity { get; set; }

        [XmlAttribute("dew_point_f", Form = XmlSchemaForm.Unqualified)]
        public string DewPointF { get; set; }

        [XmlAttribute("cloud_cover", Form = XmlSchemaForm.Unqualified)]
        public string CloudCover { get; set; }

        [XmlAttribute("obs_time", Form = XmlSchemaForm.Unqualified, DataType = "dateTime")]
        public DateTime ObsTime { get; set; }

        [XmlIgnore]
        public bool ObsTimeSpecified { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ConditionsTypeWind", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ConditionsTypeWind
    {
        [XmlAttribute("speed_mph", Form = XmlSchemaForm.Unqualified)]
        public string SpeedMph { get; set; }

        [XmlAttribute("direction", Form = XmlSchemaForm.Unqualified)]
        public string Direction { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("weatherType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class WeatherType
    {
        [XmlElement("forecast", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public ConditionsType Forecast { get; set; }

        [XmlElement("current_conditions", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public ConditionsType CurrentConditions { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("venueType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class VenueType
    {
        [XmlElement("distances", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Distances Distances { get; set; }

        [XmlElement("location", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Location Location { get; set; }

        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute("market", Form = XmlSchemaForm.Unqualified)]
        public string Market { get; set; }

        [XmlAttribute("desc", Form = XmlSchemaForm.Unqualified)]
        public string Desc { get; set; }

        [XmlAttribute("address", Form = XmlSchemaForm.Unqualified)]
        public string Address { get; set; }

        [XmlAttribute("capacity", Form = XmlSchemaForm.Unqualified)]
        public string Capacity { get; set; }

        [XmlAttribute("city", Form = XmlSchemaForm.Unqualified)]
        public string City { get; set; }

        [XmlAttribute("country", Form = XmlSchemaForm.Unqualified)]
        public string Country { get; set; }

        [XmlAttribute("state", Form = XmlSchemaForm.Unqualified)]
        public string State { get; set; }

        [XmlAttribute("zip", Form = XmlSchemaForm.Unqualified)]
        public string Zip { get; set; }

        [XmlAttribute("surface", Form = XmlSchemaForm.Unqualified)]
        public string Surface { get; set; }

        [XmlAttribute("field_orientation", Form = XmlSchemaForm.Unqualified)]
        public string FieldOrientation { get; set; }

        [XmlAttribute("stadium_type", Form = XmlSchemaForm.Unqualified)]
        public string StadiumType { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("distances", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Distances
    {
        [XmlAttribute("cf", Form = XmlSchemaForm.Unqualified)]
        public string Cf { get; set; }

        [XmlAttribute("lcf", Form = XmlSchemaForm.Unqualified)]
        public string Lcf { get; set; }

        [XmlAttribute("lf", Form = XmlSchemaForm.Unqualified)]
        public string Lf { get; set; }

        [XmlAttribute("mlcf", Form = XmlSchemaForm.Unqualified)]
        public string Mlcf { get; set; }

        [XmlAttribute("mlf", Form = XmlSchemaForm.Unqualified)]
        public string Mlf { get; set; }

        [XmlAttribute("mrcf", Form = XmlSchemaForm.Unqualified)]
        public string Mrcf { get; set; }

        [XmlAttribute("mrf", Form = XmlSchemaForm.Unqualified)]
        public string Mrf { get; set; }

        [XmlAttribute("rcf", Form = XmlSchemaForm.Unqualified)]
        public string Rcf { get; set; }

        [XmlAttribute("rf", Form = XmlSchemaForm.Unqualified)]
        public string Rf { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("location", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Location
    {
        [XmlAttribute("lat", Form = XmlSchemaForm.Unqualified)]
        public string Lat { get; set; }

        [XmlAttribute("lng", Form = XmlSchemaForm.Unqualified)]
        public string Lng { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("injuryType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class InjuryType
    {
        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("desc", Form = XmlSchemaForm.Unqualified)]
        public string Desc { get; set; }

        [XmlAttribute("comment", Form = XmlSchemaForm.Unqualified)]
        public string Comment { get; set; }

        [XmlAttribute("start_date", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime StartDate { get; set; }

        [XmlAttribute("update_date", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime UpdateDate { get; set; }

        [XmlIgnore]
        public bool UpdateDateSpecified { get; set; }

        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public InjuryTypeStatus Status { get; set; }

        [XmlIgnore]
        public bool StatusSpecified { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("InjuryTypeStatus", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum InjuryTypeStatus
    {
        Unknown,
        D7,
        D10,
        D60,
        [XmlEnum("Day-to-Day")]
        DayToDay
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("organizationType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OrganizationType : IBaseOrganizationAttributes
    {
        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute("alias", Form = XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseOrganizationAttributes
    {
        string Id { get; set; }
        string Name { get; set; }
        string Alias { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("draftType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class DraftType
    {
        [XmlAttribute("pick", Form = XmlSchemaForm.Unqualified)]
        public string Pick { get; set; }

        [XmlAttribute("round", Form = XmlSchemaForm.Unqualified)]
        public string Round { get; set; }

        [XmlAttribute("team_id", Form = XmlSchemaForm.Unqualified)]
        public string TeamId { get; set; }

        [XmlAttribute("year", Form = XmlSchemaForm.Unqualified)]
        public string Year { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("positionType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum PositionType
    {
        [XmlEnum("")]
        Item,
        P,
        IF,
        OF,
        C,
        [XmlEnum("1B")]
        Item1B,
        [XmlEnum("2B")]
        Item2B,
        [XmlEnum("3B")]
        Item3B,
        SS,
        RF,
        CF,
        LF,
        SP,
        RP,
        DH,
        PH,
        PR
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("handType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum HandType
    {
        R,
        L,
        B
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("depthChartPositionType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class DepthChartPositionType
    {
        [XmlIgnore]
        private Collection<DepthChartPlayerType> _player;

        [XmlElement("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<DepthChartPlayerType> Player
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

        public DepthChartPositionType()
        {
            _player = new Collection<DepthChartPlayerType>();
        }

        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlAttribute("desc", Form = XmlSchemaForm.Unqualified)]
        public string Desc { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("depthChartPlayerType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class DepthChartPlayerType : IExtendedPlayerAttributes
    {
        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }

        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }

        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }

        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }

        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }

        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }

        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }

        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool PrimaryPositionSpecified { get; set; }

        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }

        [XmlAttribute("mlbam_id", Form = XmlSchemaForm.Unqualified)]
        public string Mlbam_Id { get; set; }

        [XmlAttribute("college", Form = XmlSchemaForm.Unqualified)]
        public string College { get; set; }

        [XmlAttribute("high_school", Form = XmlSchemaForm.Unqualified)]
        public string High_School { get; set; }

        [XmlAttribute("birthcity", Form = XmlSchemaForm.Unqualified)]
        public string Birthcity { get; set; }

        [XmlAttribute("birthcountry", Form = XmlSchemaForm.Unqualified)]
        public string Birthcountry { get; set; }

        [XmlAttribute("birthstate", Form = XmlSchemaForm.Unqualified)]
        public string Birthstate { get; set; }

        [XmlAttribute("birthdate", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime Birthdate { get; set; }

        [XmlIgnore]
        public bool BirthdateSpecified { get; set; }

        [XmlAttribute("experience", Form = XmlSchemaForm.Unqualified)]
        public string Experience { get; set; }

        [XmlAttribute("height", Form = XmlSchemaForm.Unqualified)]
        public string Height { get; set; }

        [XmlAttribute("weight", Form = XmlSchemaForm.Unqualified)]
        public string Weight { get; set; }

        [XmlAttribute("bat_hand", Form = XmlSchemaForm.Unqualified)]
        public HandType Bat_Hand { get; set; }

        [XmlIgnore]
        public bool BatHandSpecified { get; set; }

        [XmlAttribute("throw_hand", Form = XmlSchemaForm.Unqualified)]
        public HandType Throw_Hand { get; set; }

        [XmlIgnore]
        public bool ThrowHandSpecified { get; set; }

        [XmlAttribute("pro_debut", Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime Pro_Debut { get; set; }

        [XmlIgnore]
        public bool ProDebutSpecified { get; set; }

        [XmlAttribute("updated", Form = XmlSchemaForm.Unqualified, DataType = "dateTime")]
        public DateTime Updated { get; set; }

        [XmlIgnore]
        public bool UpdatedSpecified { get; set; }

        [XmlAttribute("depth", Form = XmlSchemaForm.Unqualified)]
        public string Depth { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IExtendedPlayerAttributes : IBasePlayerAttributes
    {
        string Mlbam_Id { get; set; }
        string College { get; set; }
        string High_School { get; set; }
        string Birthcity { get; set; }
        string Birthcountry { get; set; }
        string Birthstate { get; set; }
        DateTime Birthdate { get; set; }
        string Experience { get; set; }
        string Height { get; set; }
        string Weight { get; set; }
        HandType Bat_Hand { get; set; }
        HandType Throw_Hand { get; set; }
        DateTime Pro_Debut { get; set; }
        DateTime Updated { get; set; }
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



        string Status
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePersonnelAttributes
    {



        string Id
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



        string Preferred_Name
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("positionTypeExt", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum PositionTypeExt
    {



        BP,



        CL
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("broadcastType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class BroadcastType
    {



        [XmlAttribute("network", Form = XmlSchemaForm.Unqualified)]
        public string Network { get; set; }



        [XmlAttribute("satellite", Form = XmlSchemaForm.Unqualified)]
        public string Satellite { get; set; }



        [XmlAttribute("cable", Form = XmlSchemaForm.Unqualified)]
        public string Cable { get; set; }



        [XmlAttribute("radio", Form = XmlSchemaForm.Unqualified)]
        public string Radio { get; set; }



        [XmlAttribute("internet", Form = XmlSchemaForm.Unqualified)]
        public string Internet { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("playerLineupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayerLineupType : ILineupAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("inning", Form = XmlSchemaForm.Unqualified)]
        public string Inning { get; set; }



        [XmlAttribute("inning_half", Form = XmlSchemaForm.Unqualified)]
        public string Inning_Half { get; set; }



        [XmlAttribute("sequence", Form = XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }



        [XmlAttribute("order", Form = XmlSchemaForm.Unqualified)]
        public string Order { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public string Position { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface ILineupAttributes
    {



        string Order
        {
            get;
            set;
        }



        string Position
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("lineupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class LineupType : ILineupAttributes
    {



        [XmlAttribute("order", Form = XmlSchemaForm.Unqualified)]
        public string Order { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public string Position { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("player_id", Form = XmlSchemaForm.Unqualified)]
        public string Player_Id { get; set; }



        [XmlAttribute("team_id", Form = XmlSchemaForm.Unqualified)]
        public string Team_Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("description", Form = XmlSchemaForm.Unqualified)]
        public string Description { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("rescheduledType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class RescheduledType
    {
        [XmlText(DataType = "dateTime")]
        public DateTime Value { get; set; }

        [XmlAttribute("reason", Form = XmlSchemaForm.Unqualified)]
        public string Reason { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("statisticsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class StatisticsGroupType
    {
        [XmlIgnore]
        private Collection<HittingStatsGroupType> _hitting;

        [XmlElement("hitting", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingStatsGroupType> Hitting
        {
            get
            {
                return _hitting;
            }
            private set
            {
                _hitting = value;
            }
        }

        [XmlIgnore]
        public bool HittingSpecified
        {
            get
            {
                return Hitting.Count != 0;
            }
        }

        public StatisticsGroupType()
        {
            _hitting = new Collection<HittingStatsGroupType>();
            _fielding = new Collection<FieldingStatsGroupType>();
            _pitching = new Collection<PitchingStatsGroupType>();
            _pitch_Metrics = new Collection<PitchMetricsStatsGroupType>();
        }

        [XmlIgnore]
        private Collection<FieldingStatsGroupType> _fielding;



        [XmlElement("fielding", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldingStatsGroupType> Fielding
        {
            get
            {
                return _fielding;
            }
            private set
            {
                _fielding = value;
            }
        }

        [XmlIgnore]
        public bool FieldingSpecified
        {
            get
            {
                return Fielding.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingStatsGroupType> _pitching;



        [XmlElement("pitching", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsGroupType> Pitching
        {
            get
            {
                return _pitching;
            }
            private set
            {
                _pitching = value;
            }
        }

        [XmlIgnore]
        public bool PitchingSpecified
        {
            get
            {
                return Pitching.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchMetricsStatsGroupType> _pitch_Metrics;



        [XmlElement("pitch_metrics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchMetricsStatsGroupType> Pitch_Metrics
        {
            get
            {
                return _pitch_Metrics;
            }
            private set
            {
                _pitch_Metrics = value;
            }
        }

        [XmlIgnore]
        public bool Pitch_MetricsSpecified
        {
            get
            {
                return Pitch_Metrics.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingStatsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingStatsGroupType
    {



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public HittingStatsType Overall { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingStatsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingStatsType : IHittingStatsAttributes
    {



        [XmlElement("onbase", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Onbase Onbase { get; set; }



        [XmlElement("runs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Runs Runs { get; set; }



        [XmlElement("outcome", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outcome Outcome { get; set; }



        [XmlElement("outs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outs Outs { get; set; }



        [XmlElement("steal", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Steal Steal { get; set; }



        [XmlElement("games", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Games Games { get; set; }



        [XmlElement("pitches", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Pitches Pitches { get; set; }



        [XmlAttribute("ab", Form = XmlSchemaForm.Unqualified)]
        public string Ab { get; set; }



        [XmlAttribute("abk", Form = XmlSchemaForm.Unqualified)]
        public decimal Abk { get; set; }



        [XmlIgnore]
        public bool AbkSpecified { get; set; }



        [XmlAttribute("abhr", Form = XmlSchemaForm.Unqualified)]
        public decimal Abhr { get; set; }



        [XmlIgnore]
        public bool AbhrSpecified { get; set; }



        [XmlAttribute("ap", Form = XmlSchemaForm.Unqualified)]
        public string Ap { get; set; }



        [XmlAttribute("avg", Form = XmlSchemaForm.Unqualified)]
        public decimal Avg { get; set; }

        [XmlIgnore]
        public bool AvgSpecified { get; set; }



        [XmlAttribute("babip", Form = XmlSchemaForm.Unqualified)]
        public decimal Babip { get; set; }

        [XmlIgnore]
        public bool BabipSpecified { get; set; }



        [XmlAttribute("bbk", Form = XmlSchemaForm.Unqualified)]
        public decimal Bbk { get; set; }

        [XmlIgnore]
        public bool BbkSpecified { get; set; }



        [XmlAttribute("bbpa", Form = XmlSchemaForm.Unqualified)]
        public decimal Bbpa { get; set; }

        [XmlIgnore]
        public bool BbpaSpecified { get; set; }

        [XmlAttribute("bip", Form = XmlSchemaForm.Unqualified)]
        public string Bip { get; set; }

        [XmlAttribute("gofo", Form = XmlSchemaForm.Unqualified)]
        public decimal Gofo { get; set; }

        [XmlIgnore]
        public bool GofoSpecified { get; set; }

        [XmlAttribute("iso", Form = XmlSchemaForm.Unqualified)]
        public decimal Iso { get; set; }

        [XmlIgnore]
        public bool IsoSpecified { get; set; }



        [XmlAttribute("lob", Form = XmlSchemaForm.Unqualified)]
        public string Lob { get; set; }



        [XmlAttribute("ab_risp", Form = XmlSchemaForm.Unqualified)]
        public string Ab_Risp { get; set; }



        [XmlAttribute("hit_risp", Form = XmlSchemaForm.Unqualified)]
        public string Hit_Risp { get; set; }



        [XmlAttribute("lob_risp_2out", Form = XmlSchemaForm.Unqualified)]
        public string Lob_Risp_2Out { get; set; }



        [XmlAttribute("obp", Form = XmlSchemaForm.Unqualified)]
        public decimal Obp { get; set; }

        [XmlIgnore]
        public bool ObpSpecified { get; set; }



        [XmlAttribute("ops", Form = XmlSchemaForm.Unqualified)]
        public decimal Ops { get; set; }


        [XmlIgnore]
        public bool OpsSpecified { get; set; }



        [XmlAttribute("pitch_count", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Count { get; set; }



        [XmlAttribute("rbi", Form = XmlSchemaForm.Unqualified)]
        public string Rbi { get; set; }



        [XmlAttribute("rbi_2out", Form = XmlSchemaForm.Unqualified)]
        public string Rbi_2Out { get; set; }



        [XmlAttribute("seca", Form = XmlSchemaForm.Unqualified)]
        public decimal Seca { get; set; }

        [XmlIgnore]
        public bool SecaSpecified { get; set; }



        [XmlAttribute("slg", Form = XmlSchemaForm.Unqualified)]
        public decimal Slg { get; set; }

        [XmlIgnore]
        public bool SlgSpecified { get; set; }



        [XmlAttribute("tb", Form = XmlSchemaForm.Unqualified)]
        public string Tb { get; set; }



        [XmlAttribute("xbh", Form = XmlSchemaForm.Unqualified)]
        public string Xbh { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("team_lob", Form = XmlSchemaForm.Unqualified)]
        public string Team_Lob { get; set; }



        [XmlAttribute("linedrive", Form = XmlSchemaForm.Unqualified)]
        public string Linedrive { get; set; }



        [XmlAttribute("flyball", Form = XmlSchemaForm.Unqualified)]
        public string Flyball { get; set; }



        [XmlAttribute("popup", Form = XmlSchemaForm.Unqualified)]
        public string Popup { get; set; }



        [XmlAttribute("groundball", Form = XmlSchemaForm.Unqualified)]
        public string Groundball { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_onbase", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Onbase
    {



        [XmlAttribute("bb", Form = XmlSchemaForm.Unqualified)]
        public string Bb { get; set; }



        [XmlAttribute("d", Form = XmlSchemaForm.Unqualified)]
        public string D { get; set; }



        [XmlAttribute("h", Form = XmlSchemaForm.Unqualified)]
        public string H { get; set; }



        [XmlAttribute("hbp", Form = XmlSchemaForm.Unqualified)]
        public string Hbp { get; set; }



        [XmlAttribute("hr", Form = XmlSchemaForm.Unqualified)]
        public string Hr { get; set; }



        [XmlAttribute("ibb", Form = XmlSchemaForm.Unqualified)]
        public string Ibb { get; set; }



        [XmlAttribute("tb", Form = XmlSchemaForm.Unqualified)]
        public string Tb { get; set; }



        [XmlAttribute("s", Form = XmlSchemaForm.Unqualified)]
        public string S { get; set; }



        [XmlAttribute("t", Form = XmlSchemaForm.Unqualified)]
        public string T { get; set; }



        [XmlAttribute("fc", Form = XmlSchemaForm.Unqualified)]
        public string Fc { get; set; }



        [XmlAttribute("roe", Form = XmlSchemaForm.Unqualified)]
        public string Roe { get; set; }



        [XmlAttribute("cycle", Form = XmlSchemaForm.Unqualified)]
        public string Cycle { get; set; }



        [XmlAttribute("h9", Form = XmlSchemaForm.Unqualified)]
        public decimal H9 { get; set; }

        [XmlIgnore]
        public bool H9Specified { get; set; }



        [XmlAttribute("hr9", Form = XmlSchemaForm.Unqualified)]
        public decimal Hr9 { get; set; }

        [XmlIgnore]
        public bool Hr9Specified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_runs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Runs
    {



        [XmlAttribute("total", Form = XmlSchemaForm.Unqualified)]
        public string Total { get; set; }



        [XmlAttribute("earned", Form = XmlSchemaForm.Unqualified)]
        public string Earned { get; set; }



        [XmlAttribute("unearned", Form = XmlSchemaForm.Unqualified)]
        public string Unearned { get; set; }



        [XmlAttribute("ir", Form = XmlSchemaForm.Unqualified)]
        public string Ir { get; set; }



        [XmlAttribute("ira", Form = XmlSchemaForm.Unqualified)]
        public string Ira { get; set; }



        [XmlAttribute("bqr", Form = XmlSchemaForm.Unqualified)]
        public string Bqr { get; set; }



        [XmlAttribute("bqra", Form = XmlSchemaForm.Unqualified)]
        public string Bqra { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_outcome", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Outcome
    {



        [XmlAttribute("klook", Form = XmlSchemaForm.Unqualified)]
        public string Klook { get; set; }



        [XmlAttribute("kswing", Form = XmlSchemaForm.Unqualified)]
        public string Kswing { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("ball", Form = XmlSchemaForm.Unqualified)]
        public string Ball { get; set; }



        [XmlAttribute("iball", Form = XmlSchemaForm.Unqualified)]
        public string Iball { get; set; }



        [XmlAttribute("dirtball", Form = XmlSchemaForm.Unqualified)]
        public string Dirtball { get; set; }



        [XmlAttribute("foul", Form = XmlSchemaForm.Unqualified)]
        public string Foul { get; set; }



        [XmlAttribute("btotal", Form = XmlSchemaForm.Unqualified)]
        public string Btotal { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_outs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Outs
    {



        [XmlAttribute("fidp", Form = XmlSchemaForm.Unqualified)]
        public string Fidp { get; set; }



        [XmlAttribute("fo", Form = XmlSchemaForm.Unqualified)]
        public string Fo { get; set; }



        [XmlAttribute("gidp", Form = XmlSchemaForm.Unqualified)]
        public string Gidp { get; set; }



        [XmlAttribute("go", Form = XmlSchemaForm.Unqualified)]
        public string Go { get; set; }



        [XmlAttribute("klook", Form = XmlSchemaForm.Unqualified)]
        public string Klook { get; set; }



        [XmlAttribute("kswing", Form = XmlSchemaForm.Unqualified)]
        public string Kswing { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("lidp", Form = XmlSchemaForm.Unqualified)]
        public string Lidp { get; set; }



        [XmlAttribute("lo", Form = XmlSchemaForm.Unqualified)]
        public string Lo { get; set; }



        [XmlAttribute("po", Form = XmlSchemaForm.Unqualified)]
        public string Po { get; set; }



        [XmlAttribute("sachit", Form = XmlSchemaForm.Unqualified)]
        public string Sachit { get; set; }



        [XmlAttribute("sacbnt", Form = XmlSchemaForm.Unqualified)]
        public string Sacbnt { get; set; }



        [XmlAttribute("sacfly", Form = XmlSchemaForm.Unqualified)]
        public string Sacfly { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_steal", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Steal
    {



        [XmlAttribute("caught", Form = XmlSchemaForm.Unqualified)]
        public string Caught { get; set; }



        [XmlAttribute("stolen", Form = XmlSchemaForm.Unqualified)]
        public string Stolen { get; set; }



        [XmlAttribute("pct", Form = XmlSchemaForm.Unqualified)]
        public decimal Pct { get; set; }

        [XmlIgnore]
        public bool PctSpecified { get; set; }



        [XmlAttribute("pickoff", Form = XmlSchemaForm.Unqualified)]
        public decimal Pickoff { get; set; }

        [XmlIgnore]
        public bool PickoffSpecified { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_games", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Games
    {



        [XmlAttribute("play", Form = XmlSchemaForm.Unqualified)]
        public string Play { get; set; }



        [XmlAttribute("complete", Form = XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }



        [XmlAttribute("finish", Form = XmlSchemaForm.Unqualified)]
        public string Finish { get; set; }



        [XmlAttribute("start", Form = XmlSchemaForm.Unqualified)]
        public string Start { get; set; }



        [XmlAttribute("save", Form = XmlSchemaForm.Unqualified)]
        public string Save { get; set; }



        [XmlAttribute("win", Form = XmlSchemaForm.Unqualified)]
        public string Win { get; set; }



        [XmlAttribute("loss", Form = XmlSchemaForm.Unqualified)]
        public string Loss { get; set; }



        [XmlAttribute("svo", Form = XmlSchemaForm.Unqualified)]
        public string Svo { get; set; }



        [XmlAttribute("hold", Form = XmlSchemaForm.Unqualified)]
        public string Hold { get; set; }



        [XmlAttribute("blown_save", Form = XmlSchemaForm.Unqualified)]
        public string Blown_Save { get; set; }



        [XmlAttribute("qstart", Form = XmlSchemaForm.Unqualified)]
        public string Qstart { get; set; }



        [XmlAttribute("shutout", Form = XmlSchemaForm.Unqualified)]
        public string Shutout { get; set; }



        [XmlAttribute("team_win", Form = XmlSchemaForm.Unqualified)]
        public string Team_Win { get; set; }



        [XmlAttribute("team_loss", Form = XmlSchemaForm.Unqualified)]
        public string Team_Loss { get; set; }



        [XmlAttribute("team_shutout", Form = XmlSchemaForm.Unqualified)]
        public string Team_Shutout { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_pitches", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Pitches
    {



        [XmlAttribute("count", Form = XmlSchemaForm.Unqualified)]
        public string Count { get; set; }



        [XmlAttribute("btotal", Form = XmlSchemaForm.Unqualified)]
        public string Btotal { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("per_ip", Form = XmlSchemaForm.Unqualified)]
        public decimal Per_Ip { get; set; }

        [XmlIgnore]
        public bool Per_IpSpecified { get; set; }



        [XmlAttribute("per_bf", Form = XmlSchemaForm.Unqualified)]
        public decimal Per_Bf { get; set; }

        [XmlIgnore]
        public bool Per_BfSpecified { get; set; }



        [XmlAttribute("per_start", Form = XmlSchemaForm.Unqualified)]
        public decimal Per_Start { get; set; }

        [XmlIgnore]
        public bool Per_StartSpecified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IHittingStatsAttributes
    {



        string Ab
        {
            get;
            set;
        }



        decimal Abk
        {
            get;
            set;
        }



        decimal Abhr
        {
            get;
            set;
        }



        string Ap
        {
            get;
            set;
        }



        decimal Avg
        {
            get;
            set;
        }



        decimal Babip
        {
            get;
            set;
        }



        decimal Bbk
        {
            get;
            set;
        }



        decimal Bbpa
        {
            get;
            set;
        }



        string Bip
        {
            get;
            set;
        }



        decimal Gofo
        {
            get;
            set;
        }



        decimal Iso
        {
            get;
            set;
        }



        string Lob
        {
            get;
            set;
        }



        string Ab_Risp
        {
            get;
            set;
        }



        string Hit_Risp
        {
            get;
            set;
        }



        string Lob_Risp_2Out
        {
            get;
            set;
        }



        decimal Obp
        {
            get;
            set;
        }



        decimal Ops
        {
            get;
            set;
        }



        string Pitch_Count
        {
            get;
            set;
        }



        string Rbi
        {
            get;
            set;
        }



        string Rbi_2Out
        {
            get;
            set;
        }



        decimal Seca
        {
            get;
            set;
        }



        decimal Slg
        {
            get;
            set;
        }



        string Tb
        {
            get;
            set;
        }



        string Xbh
        {
            get;
            set;
        }



        string Id
        {
            get;
            set;
        }



        string Team_Lob
        {
            get;
            set;
        }



        string Linedrive
        {
            get;
            set;
        }



        string Flyball
        {
            get;
            set;
        }



        string Popup
        {
            get;
            set;
        }



        string Groundball
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("fieldingStatsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldingStatsGroupType
    {



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public FieldingStatsType Overall { get; set; }

        [XmlIgnore]
        private Collection<FieldingStatsType> _positions;



        [XmlArray("positions", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("position", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldingStatsType> Positions
        {
            get
            {
                return _positions;
            }
            private set
            {
                _positions = value;
            }
        }

        [XmlIgnore]
        public bool PositionsSpecified
        {
            get
            {
                return Positions.Count != 0;
            }
        }

        public FieldingStatsGroupType()
        {
            _positions = new Collection<FieldingStatsType>();
        }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("fieldingStatsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldingStatsType : IFieldingStatsAttributes
    {



        [XmlElement("assists", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Assists Assists { get; set; }



        [XmlElement("steal", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Steal Steal { get; set; }



        [XmlElement("errors", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Errors Errors { get; set; }



        [XmlElement("games", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Games Games { get; set; }



        [XmlAttribute("po", Form = XmlSchemaForm.Unqualified)]
        public string Po { get; set; }



        [XmlAttribute("a", Form = XmlSchemaForm.Unqualified)]
        public string A { get; set; }



        [XmlAttribute("dp", Form = XmlSchemaForm.Unqualified)]
        public string Dp { get; set; }



        [XmlAttribute("tp", Form = XmlSchemaForm.Unqualified)]
        public string Tp { get; set; }



        [XmlAttribute("error", Form = XmlSchemaForm.Unqualified)]
        public string Error { get; set; }



        [XmlAttribute("tc", Form = XmlSchemaForm.Unqualified)]
        public string Tc { get; set; }



        [XmlAttribute("fpct", Form = XmlSchemaForm.Unqualified)]
        public decimal Fpct { get; set; }

        [XmlIgnore]
        public bool FpctSpecified { get; set; }



        [XmlAttribute("rf", Form = XmlSchemaForm.Unqualified)]
        public decimal Rf { get; set; }

        [XmlIgnore]
        public bool RfSpecified { get; set; }



        [XmlAttribute("c_wp", Form = XmlSchemaForm.Unqualified)]
        public string C_Wp { get; set; }



        [XmlAttribute("pb", Form = XmlSchemaForm.Unqualified)]
        public string Pb { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("inn_1", Form = XmlSchemaForm.Unqualified)]
        public string Inn_1 { get; set; }



        [XmlAttribute("inn_2", Form = XmlSchemaForm.Unqualified)]
        public decimal Inn_2 { get; set; }

        [XmlIgnore]
        public bool Inn_2Specified { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public string Position { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_assists", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Assists
    {



        [XmlAttribute("outfield", Form = XmlSchemaForm.Unqualified)]
        public string Outfield { get; set; }



        [XmlAttribute("total", Form = XmlSchemaForm.Unqualified)]
        public string Total { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_errors", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Errors
    {



        [XmlAttribute("throwing", Form = XmlSchemaForm.Unqualified)]
        public string Throwing { get; set; }



        [XmlAttribute("interference", Form = XmlSchemaForm.Unqualified)]
        public string Interference { get; set; }



        [XmlAttribute("fielding", Form = XmlSchemaForm.Unqualified)]
        public string Fielding { get; set; }



        [XmlAttribute("total", Form = XmlSchemaForm.Unqualified)]
        public string Total { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IFieldingStatsAttributes
    {



        string Po
        {
            get;
            set;
        }



        string A
        {
            get;
            set;
        }



        string Dp
        {
            get;
            set;
        }



        string Tp
        {
            get;
            set;
        }



        string Error
        {
            get;
            set;
        }



        string Tc
        {
            get;
            set;
        }



        decimal Fpct
        {
            get;
            set;
        }



        decimal Rf
        {
            get;
            set;
        }



        string C_Wp
        {
            get;
            set;
        }



        string Pb
        {
            get;
            set;
        }



        string Id
        {
            get;
            set;
        }



        string Inn_1
        {
            get;
            set;
        }



        decimal Inn_2
        {
            get;
            set;
        }



        string Position
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("FieldingStatsGroupTypePositions", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldingStatsGroupTypePositions
    {

        [XmlIgnore]
        private Collection<FieldingStatsType> _position;



        [XmlElement("position", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldingStatsType> Position
        {
            get
            {
                return _position;
            }
            private set
            {
                _position = value;
            }
        }

        [XmlIgnore]
        public bool PositionSpecified
        {
            get
            {
                return Position.Count != 0;
            }
        }

        public FieldingStatsGroupTypePositions()
        {
            _position = new Collection<FieldingStatsType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingStatsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingStatsGroupType
    {

        [XmlIgnore]
        private Collection<PitchingStatsType> _overall;



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsType> Overall
        {
            get
            {
                return _overall;
            }
            private set
            {
                _overall = value;
            }
        }

        [XmlIgnore]
        public bool OverallSpecified
        {
            get
            {
                return Overall.Count != 0;
            }
        }

        public PitchingStatsGroupType()
        {
            _overall = new Collection<PitchingStatsType>();
            _bullpen = new Collection<PitchingStatsType>();
            _starters = new Collection<PitchingStatsType>();
        }

        [XmlIgnore]
        private Collection<PitchingStatsType> _bullpen;



        [XmlElement("bullpen", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsType> Bullpen
        {
            get
            {
                return _bullpen;
            }
            private set
            {
                _bullpen = value;
            }
        }

        [XmlIgnore]
        public bool BullpenSpecified
        {
            get
            {
                return Bullpen.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingStatsType> _starters;



        [XmlElement("starters", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsType> Starters
        {
            get
            {
                return _starters;
            }
            private set
            {
                _starters = value;
            }
        }

        [XmlIgnore]
        public bool StartersSpecified
        {
            get
            {
                return Starters.Count != 0;
            }
        }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingStatsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingStatsType : IPitchingStatsAttributes
    {



        [XmlElement("onbase", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Onbase Onbase { get; set; }



        [XmlElement("runs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Runs Runs { get; set; }



        [XmlElement("outcome", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outcome Outcome { get; set; }



        [XmlElement("outs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outs Outs { get; set; }



        [XmlElement("steal", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Steal Steal { get; set; }



        [XmlElement("games", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Games Games { get; set; }



        [XmlElement("pitches", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Pitches Pitches { get; set; }



        [XmlElement("in_play", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_In_Play In_Play { get; set; }



        [XmlAttribute("bf", Form = XmlSchemaForm.Unqualified)]
        public string Bf { get; set; }



        [XmlAttribute("oba", Form = XmlSchemaForm.Unqualified)]
        public decimal Oba { get; set; }

        [XmlIgnore]
        public bool ObaSpecified { get; set; }



        [XmlAttribute("kbb", Form = XmlSchemaForm.Unqualified)]
        public decimal Kbb { get; set; }

        [XmlIgnore]
        public bool KbbSpecified { get; set; }



        [XmlAttribute("era", Form = XmlSchemaForm.Unqualified)]
        public string Era { get; set; }



        [XmlAttribute("gofo", Form = XmlSchemaForm.Unqualified)]
        public decimal Gofo { get; set; }

        [XmlIgnore]
        public bool GofoSpecified { get; set; }



        [XmlAttribute("wp", Form = XmlSchemaForm.Unqualified)]
        public string Wp { get; set; }



        [XmlAttribute("bk", Form = XmlSchemaForm.Unqualified)]
        public string Bk { get; set; }



        [XmlAttribute("ip_1", Form = XmlSchemaForm.Unqualified)]
        public string Ip_1 { get; set; }



        [XmlAttribute("ip_2", Form = XmlSchemaForm.Unqualified)]
        public decimal Ip_2 { get; set; }

        [XmlIgnore]
        public bool Ip_2Specified { get; set; }



        [XmlAttribute("error", Form = XmlSchemaForm.Unqualified)]
        public string Error { get; set; }



        [XmlAttribute("whip", Form = XmlSchemaForm.Unqualified)]
        public decimal Whip { get; set; }

        [XmlIgnore]
        public bool WhipSpecified { get; set; }



        [XmlAttribute("lob", Form = XmlSchemaForm.Unqualified)]
        public decimal Lob { get; set; }

        [XmlIgnore]
        public bool LobSpecified { get; set; }



        [XmlAttribute("k9", Form = XmlSchemaForm.Unqualified)]
        public decimal K9 { get; set; }

        [XmlIgnore]
        public bool K9Specified { get; set; }



        [XmlAttribute("qs", Form = XmlSchemaForm.Unqualified)]
        public decimal Qs { get; set; }

        [XmlIgnore]
        public bool QsSpecified { get; set; }



        [XmlAttribute("oab", Form = XmlSchemaForm.Unqualified)]
        public decimal Oab { get; set; }

        [XmlIgnore]
        public bool OabSpecified { get; set; }



        [XmlAttribute("slg", Form = XmlSchemaForm.Unqualified)]
        public decimal Slg { get; set; }

        [XmlIgnore]
        public bool SlgSpecified { get; set; }



        [XmlAttribute("obp", Form = XmlSchemaForm.Unqualified)]
        public decimal Obp { get; set; }

        [XmlIgnore]
        public bool ObpSpecified { get; set; }



        [XmlAttribute("babip", Form = XmlSchemaForm.Unqualified)]
        public decimal Babip { get; set; }

        [XmlIgnore]
        public bool BabipSpecified { get; set; }



        [XmlAttribute("gbfb", Form = XmlSchemaForm.Unqualified)]
        public decimal Gbfb { get; set; }

        [XmlIgnore]
        public bool GbfbSpecified { get; set; }



        [XmlAttribute("bf_ip", Form = XmlSchemaForm.Unqualified)]
        public decimal Bf_Ip { get; set; }

        [XmlIgnore]
        public bool Bf_IpSpecified { get; set; }



        [XmlAttribute("fip", Form = XmlSchemaForm.Unqualified)]
        public decimal Fip { get; set; }

        [XmlIgnore]
        public bool FipSpecified { get; set; }



        [XmlAttribute("pitch_count", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Count { get; set; }



        [XmlAttribute("bf_start", Form = XmlSchemaForm.Unqualified)]
        public decimal Bf_Start { get; set; }

        [XmlIgnore]
        public bool Bf_StartSpecified { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_in_play", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_In_Play
    {



        [XmlAttribute("linedrive", Form = XmlSchemaForm.Unqualified)]
        public string Linedrive { get; set; }



        [XmlAttribute("groundball", Form = XmlSchemaForm.Unqualified)]
        public string Groundball { get; set; }



        [XmlAttribute("popup", Form = XmlSchemaForm.Unqualified)]
        public string Popup { get; set; }



        [XmlAttribute("flyball", Form = XmlSchemaForm.Unqualified)]
        public string Flyball { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPitchingStatsAttributes
    {



        string Bf
        {
            get;
            set;
        }



        decimal Oba
        {
            get;
            set;
        }



        decimal Kbb
        {
            get;
            set;
        }



        string Era
        {
            get;
            set;
        }



        decimal Gofo
        {
            get;
            set;
        }



        string Wp
        {
            get;
            set;
        }



        string Bk
        {
            get;
            set;
        }



        string Ip_1
        {
            get;
            set;
        }



        decimal Ip_2
        {
            get;
            set;
        }



        string Error
        {
            get;
            set;
        }



        decimal Whip
        {
            get;
            set;
        }



        decimal Lob
        {
            get;
            set;
        }



        decimal K9
        {
            get;
            set;
        }



        decimal Qs
        {
            get;
            set;
        }



        decimal Oab
        {
            get;
            set;
        }



        decimal Slg
        {
            get;
            set;
        }



        decimal Obp
        {
            get;
            set;
        }



        decimal Babip
        {
            get;
            set;
        }



        decimal Gbfb
        {
            get;
            set;
        }



        decimal Bf_Ip
        {
            get;
            set;
        }



        decimal Fip
        {
            get;
            set;
        }



        string Pitch_Count
        {
            get;
            set;
        }



        decimal Bf_Start
        {
            get;
            set;
        }



        string Id
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchMetricsStatsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchMetricsStatsGroupType
    {



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitchStatsType Overall { get; set; }

        [XmlIgnore]
        private Collection<PitchStatsType> _pitch_Types;



        [XmlArray("pitch_types", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("pitch_type", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchStatsType> Pitch_Types
        {
            get
            {
                return _pitch_Types;
            }
            private set
            {
                _pitch_Types = value;
            }
        }

        [XmlIgnore]
        public bool Pitch_TypesSpecified
        {
            get
            {
                return Pitch_Types.Count != 0;
            }
        }

        public PitchMetricsStatsGroupType()
        {
            _pitch_Types = new Collection<PitchStatsType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchStatsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchStatsType : IPitchStatsAttributes
    {



        [XmlElement("onbase", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Onbase Onbase { get; set; }



        [XmlElement("outcome", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outcome Outcome { get; set; }



        [XmlElement("outs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Outs Outs { get; set; }



        [XmlElement("games", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Games Games { get; set; }



        [XmlElement("in_play", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_In_Play In_Play { get; set; }



        [XmlElement("swings", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Common_Swings Swings { get; set; }



        [XmlAttribute("type", Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }



        [XmlAttribute("count", Form = XmlSchemaForm.Unqualified)]
        public string Count { get; set; }



        [XmlAttribute("avg_speed", Form = XmlSchemaForm.Unqualified)]
        public decimal Avg_Speed { get; set; }



        [XmlAttribute("min_speed", Form = XmlSchemaForm.Unqualified)]
        public decimal Min_Speed { get; set; }



        [XmlAttribute("max_speed", Form = XmlSchemaForm.Unqualified)]
        public decimal Max_Speed { get; set; }



        [XmlAttribute("gbfb", Form = XmlSchemaForm.Unqualified)]
        public decimal Gbfb { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("common_swings", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class Common_Swings
    {



        [XmlAttribute("total", Form = XmlSchemaForm.Unqualified)]
        public string Total { get; set; }



        [XmlAttribute("swing_rate", Form = XmlSchemaForm.Unqualified)]
        public decimal Swing_Rate { get; set; }



        [XmlAttribute("strike_pct", Form = XmlSchemaForm.Unqualified)]
        public decimal Strike_Pct { get; set; }



        [XmlAttribute("whiff_rate", Form = XmlSchemaForm.Unqualified)]
        public decimal Whiff_Rate { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPitchStatsAttributes
    {



        string Type
        {
            get;
            set;
        }



        string Count
        {
            get;
            set;
        }



        decimal Avg_Speed
        {
            get;
            set;
        }



        decimal Min_Speed
        {
            get;
            set;
        }



        decimal Max_Speed
        {
            get;
            set;
        }



        decimal Gbfb
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PitchMetricsStatsGroupTypePitch_Types", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchMetricsStatsGroupTypePitch_Types
    {

        [XmlIgnore]
        private Collection<PitchStatsType> _pitch_Type;



        [XmlElement("pitch_type", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchStatsType> Pitch_Type
        {
            get
            {
                return _pitch_Type;
            }
            private set
            {
                _pitch_Type = value;
            }
        }

        [XmlIgnore]
        public bool Pitch_TypeSpecified
        {
            get
            {
                return Pitch_Type.Count != 0;
            }
        }

        public PitchMetricsStatsGroupTypePitch_Types()
        {
            _pitch_Type = new Collection<PitchStatsType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("splitsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class SplitsGroupType
    {

        [XmlIgnore]
        private Collection<HittingSplitsGroupType> _hitting;



        [XmlElement("hitting", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingSplitsGroupType> Hitting
        {
            get
            {
                return _hitting;
            }
            private set
            {
                _hitting = value;
            }
        }

        [XmlIgnore]
        public bool HittingSpecified
        {
            get
            {
                return Hitting.Count != 0;
            }
        }

        public SplitsGroupType()
        {
            _hitting = new Collection<HittingSplitsGroupType>();
            _pitching = new Collection<PitchingSplitsGroupType>();
        }

        [XmlIgnore]
        private Collection<PitchingSplitsGroupType> _pitching;



        [XmlElement("pitching", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingSplitsGroupType> Pitching
        {
            get
            {
                return _pitching;
            }
            private set
            {
                _pitching = value;
            }
        }

        [XmlIgnore]
        public bool PitchingSpecified
        {
            get
            {
                return Pitching.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingSplitsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingSplitsGroupType
    {



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public HittingSplitsType Overall { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingSplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingSplitsType
    {

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _month;



        [XmlElement("month", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Month
        {
            get
            {
                return _month;
            }
            private set
            {
                _month = value;
            }
        }

        [XmlIgnore]
        public bool MonthSpecified
        {
            get
            {
                return Month.Count != 0;
            }
        }

        public HittingSplitsType()
        {
            _month = new Collection<HittingCategorySplitsType>();
            _opponent = new Collection<HittingCategorySplitsType>();
            _day_Night = new Collection<HittingCategorySplitsType>();
            _surface = new Collection<HittingCategorySplitsType>();
            _venue = new Collection<HittingCategorySplitsType>();
            _home_Away = new Collection<HittingCategorySplitsType>();
            _pitcher_Hand = new Collection<HittingCategorySplitsType>();
            _total = new Collection<HittingCategorySplitsType>();
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _opponent;



        [XmlElement("opponent", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Opponent
        {
            get
            {
                return _opponent;
            }
            private set
            {
                _opponent = value;
            }
        }

        [XmlIgnore]
        public bool OpponentSpecified
        {
            get
            {
                return Opponent.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _day_Night;



        [XmlElement("day_night", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Day_Night
        {
            get
            {
                return _day_Night;
            }
            private set
            {
                _day_Night = value;
            }
        }

        [XmlIgnore]
        public bool Day_NightSpecified
        {
            get
            {
                return Day_Night.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _surface;



        [XmlElement("surface", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Surface
        {
            get
            {
                return _surface;
            }
            private set
            {
                _surface = value;
            }
        }

        [XmlIgnore]
        public bool SurfaceSpecified
        {
            get
            {
                return Surface.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _venue;



        [XmlElement("venue", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Venue
        {
            get
            {
                return _venue;
            }
            private set
            {
                _venue = value;
            }
        }

        [XmlIgnore]
        public bool VenueSpecified
        {
            get
            {
                return Venue.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _home_Away;



        [XmlElement("home_away", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Home_Away
        {
            get
            {
                return _home_Away;
            }
            private set
            {
                _home_Away = value;
            }
        }

        [XmlIgnore]
        public bool Home_AwaySpecified
        {
            get
            {
                return Home_Away.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _pitcher_Hand;



        [XmlElement("pitcher_hand", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Pitcher_Hand
        {
            get
            {
                return _pitcher_Hand;
            }
            private set
            {
                _pitcher_Hand = value;
            }
        }

        [XmlIgnore]
        public bool Pitcher_HandSpecified
        {
            get
            {
                return Pitcher_Hand.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<HittingCategorySplitsType> _total;



        [XmlElement("total", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingCategorySplitsType> Total
        {
            get
            {
                return _total;
            }
            private set
            {
                _total = value;
            }
        }

        [XmlIgnore]
        public bool TotalSpecified
        {
            get
            {
                return Total.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingCategorySplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingCategorySplitsType
    {



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public HittingSplitStatType Statistics { get; set; }



        [XmlAttribute("value", Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }



        [XmlAttribute("abbr", Form = XmlSchemaForm.Unqualified)]
        public string Abbr { get; set; }



        [XmlAttribute("market", Form = XmlSchemaForm.Unqualified)]
        public string Market { get; set; }



        [XmlAttribute("surface", Form = XmlSchemaForm.Unqualified)]
        public string Surface { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("hittingSplitStatType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class HittingSplitStatType
    {



        [XmlAttribute("ab", Form = XmlSchemaForm.Unqualified)]
        public string Ab { get; set; }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs { get; set; }



        [XmlAttribute("s", Form = XmlSchemaForm.Unqualified)]
        public string S { get; set; }



        [XmlAttribute("d", Form = XmlSchemaForm.Unqualified)]
        public string D { get; set; }



        [XmlAttribute("t", Form = XmlSchemaForm.Unqualified)]
        public string T { get; set; }



        [XmlAttribute("hr", Form = XmlSchemaForm.Unqualified)]
        public string Hr { get; set; }



        [XmlAttribute("rbi", Form = XmlSchemaForm.Unqualified)]
        public string Rbi { get; set; }



        [XmlAttribute("bb", Form = XmlSchemaForm.Unqualified)]
        public string Bb { get; set; }



        [XmlAttribute("ibb", Form = XmlSchemaForm.Unqualified)]
        public string Ibb { get; set; }



        [XmlAttribute("hbp", Form = XmlSchemaForm.Unqualified)]
        public string Hbp { get; set; }



        [XmlAttribute("sb", Form = XmlSchemaForm.Unqualified)]
        public string Sb { get; set; }



        [XmlAttribute("cs", Form = XmlSchemaForm.Unqualified)]
        public string Cs { get; set; }



        [XmlAttribute("h", Form = XmlSchemaForm.Unqualified)]
        public string H { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("obp", Form = XmlSchemaForm.Unqualified)]
        public decimal Obp { get; set; }

        [XmlIgnore]
        public bool ObpSpecified { get; set; }



        [XmlAttribute("slg", Form = XmlSchemaForm.Unqualified)]
        public decimal Slg { get; set; }

        [XmlIgnore]
        public bool SlgSpecified { get; set; }



        [XmlAttribute("ops", Form = XmlSchemaForm.Unqualified)]
        public decimal Ops { get; set; }

        [XmlIgnore]
        public bool OpsSpecified { get; set; }



        [XmlAttribute("avg", Form = XmlSchemaForm.Unqualified)]
        public decimal Avg { get; set; }

        [XmlIgnore]
        public bool AvgSpecified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingSplitsGroupType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingSplitsGroupType
    {

        [XmlIgnore]
        private Collection<PitchingSplitsType> _overall;



        [XmlElement("overall", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingSplitsType> Overall
        {
            get
            {
                return _overall;
            }
            private set
            {
                _overall = value;
            }
        }

        [XmlIgnore]
        public bool OverallSpecified
        {
            get
            {
                return Overall.Count != 0;
            }
        }

        public PitchingSplitsGroupType()
        {
            _overall = new Collection<PitchingSplitsType>();
            _bullpen = new Collection<PitchingSplitsType>();
            _starters = new Collection<PitchingSplitsType>();
        }

        [XmlIgnore]
        private Collection<PitchingSplitsType> _bullpen;



        [XmlElement("bullpen", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingSplitsType> Bullpen
        {
            get
            {
                return _bullpen;
            }
            private set
            {
                _bullpen = value;
            }
        }

        [XmlIgnore]
        public bool BullpenSpecified
        {
            get
            {
                return Bullpen.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingSplitsType> _starters;



        [XmlElement("starters", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingSplitsType> Starters
        {
            get
            {
                return _starters;
            }
            private set
            {
                _starters = value;
            }
        }

        [XmlIgnore]
        public bool StartersSpecified
        {
            get
            {
                return Starters.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingSplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingSplitsType
    {

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _month;



        [XmlElement("month", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Month
        {
            get
            {
                return _month;
            }
            private set
            {
                _month = value;
            }
        }

        [XmlIgnore]
        public bool MonthSpecified
        {
            get
            {
                return Month.Count != 0;
            }
        }

        public PitchingSplitsType()
        {
            _month = new Collection<PitchingCategorySplitsType>();
            _opponent = new Collection<PitchingCategorySplitsType>();
            _day_Night = new Collection<PitchingCategorySplitsType>();
            _surface = new Collection<PitchingCategorySplitsType>();
            _venue = new Collection<PitchingCategorySplitsType>();
            _home_Away = new Collection<PitchingCategorySplitsType>();
            _hitter_Hand = new Collection<PitchingHitterCategorySplitsType>();
            _last_Start = new Collection<PitchingCategorySplitsType>();
            _last_Starts = new Collection<PitchingCategorySplitsType>();
            _total = new Collection<PitchingTotalsCategorySplitsType>();
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _opponent;



        [XmlElement("opponent", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Opponent
        {
            get
            {
                return _opponent;
            }
            private set
            {
                _opponent = value;
            }
        }

        [XmlIgnore]
        public bool OpponentSpecified
        {
            get
            {
                return Opponent.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _day_Night;



        [XmlElement("day_night", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Day_Night
        {
            get
            {
                return _day_Night;
            }
            private set
            {
                _day_Night = value;
            }
        }

        [XmlIgnore]
        public bool Day_NightSpecified
        {
            get
            {
                return Day_Night.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _surface;



        [XmlElement("surface", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Surface
        {
            get
            {
                return _surface;
            }
            private set
            {
                _surface = value;
            }
        }

        [XmlIgnore]
        public bool SurfaceSpecified
        {
            get
            {
                return Surface.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _venue;



        [XmlElement("venue", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Venue
        {
            get
            {
                return _venue;
            }
            private set
            {
                _venue = value;
            }
        }

        [XmlIgnore]
        public bool VenueSpecified
        {
            get
            {
                return Venue.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _home_Away;



        [XmlElement("home_away", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Home_Away
        {
            get
            {
                return _home_Away;
            }
            private set
            {
                _home_Away = value;
            }
        }

        [XmlIgnore]
        public bool Home_AwaySpecified
        {
            get
            {
                return Home_Away.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingHitterCategorySplitsType> _hitter_Hand;



        [XmlElement("hitter_hand", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingHitterCategorySplitsType> Hitter_Hand
        {
            get
            {
                return _hitter_Hand;
            }
            private set
            {
                _hitter_Hand = value;
            }
        }

        [XmlIgnore]
        public bool Hitter_HandSpecified
        {
            get
            {
                return Hitter_Hand.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _last_Start;



        [XmlElement("last_start", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Last_Start
        {
            get
            {
                return _last_Start;
            }
            private set
            {
                _last_Start = value;
            }
        }

        [XmlIgnore]
        public bool Last_StartSpecified
        {
            get
            {
                return Last_Start.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingCategorySplitsType> _last_Starts;



        [XmlElement("last_starts", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingCategorySplitsType> Last_Starts
        {
            get
            {
                return _last_Starts;
            }
            private set
            {
                _last_Starts = value;
            }
        }

        [XmlIgnore]
        public bool Last_StartsSpecified
        {
            get
            {
                return Last_Starts.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchingTotalsCategorySplitsType> _total;



        [XmlElement("total", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingTotalsCategorySplitsType> Total
        {
            get
            {
                return _total;
            }
            private set
            {
                _total = value;
            }
        }

        [XmlIgnore]
        public bool TotalSpecified
        {
            get
            {
                return Total.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingCategorySplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingCategorySplitsType
    {



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitchingSplitStatType Statistics { get; set; }



        [XmlAttribute("value", Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }



        [XmlAttribute("abbr", Form = XmlSchemaForm.Unqualified)]
        public string Abbr { get; set; }



        [XmlAttribute("market", Form = XmlSchemaForm.Unqualified)]
        public string Market { get; set; }



        [XmlAttribute("surface", Form = XmlSchemaForm.Unqualified)]
        public string Surface { get; set; }



        [XmlAttribute("game_id", Form = XmlSchemaForm.Unqualified)]
        public string Game_Id { get; set; }



        [XmlAttribute("game_scheduled", Form = XmlSchemaForm.Unqualified)]
        public string Game_Scheduled { get; set; }



        [XmlAttribute("opponent_id", Form = XmlSchemaForm.Unqualified)]
        public string Opponent_Id { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingSplitStatType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingSplitStatType
    {



        [XmlAttribute("win", Form = XmlSchemaForm.Unqualified)]
        public string Win { get; set; }



        [XmlAttribute("loss", Form = XmlSchemaForm.Unqualified)]
        public string Loss { get; set; }



        [XmlAttribute("save", Form = XmlSchemaForm.Unqualified)]
        public string Save { get; set; }



        [XmlAttribute("svo", Form = XmlSchemaForm.Unqualified)]
        public string Svo { get; set; }



        [XmlAttribute("start", Form = XmlSchemaForm.Unqualified)]
        public string Start { get; set; }



        [XmlAttribute("play", Form = XmlSchemaForm.Unqualified)]
        public string Play { get; set; }



        [XmlAttribute("complete", Form = XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }



        [XmlAttribute("ip_1", Form = XmlSchemaForm.Unqualified)]
        public string Ip_1 { get; set; }



        [XmlAttribute("ip_2", Form = XmlSchemaForm.Unqualified)]
        public decimal Ip_2 { get; set; }

        [XmlIgnore]
        public bool Ip_2Specified { get; set; }



        [XmlAttribute("h", Form = XmlSchemaForm.Unqualified)]
        public string H { get; set; }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs { get; set; }



        [XmlAttribute("er", Form = XmlSchemaForm.Unqualified)]
        public string Er { get; set; }



        [XmlAttribute("hr", Form = XmlSchemaForm.Unqualified)]
        public string Hr { get; set; }



        [XmlAttribute("bb", Form = XmlSchemaForm.Unqualified)]
        public string Bb { get; set; }



        [XmlAttribute("ibb", Form = XmlSchemaForm.Unqualified)]
        public decimal Ibb { get; set; }

        [XmlIgnore]
        public bool IbbSpecified { get; set; }



        [XmlAttribute("oba", Form = XmlSchemaForm.Unqualified)]
        public decimal Oba { get; set; }

        [XmlIgnore]
        public bool ObaSpecified { get; set; }



        [XmlAttribute("era", Form = XmlSchemaForm.Unqualified)]
        public string Era { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("team_win", Form = XmlSchemaForm.Unqualified)]
        public string Team_Win { get; set; }



        [XmlAttribute("team_loss", Form = XmlSchemaForm.Unqualified)]
        public string Team_Loss { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingHitterCategorySplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingHitterCategorySplitsType
    {



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitchingHitterSplitStatType Statistics { get; set; }



        [XmlAttribute("value", Form = XmlSchemaForm.Unqualified)]
        public string Value { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingHitterSplitStatType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingHitterSplitStatType
    {



        [XmlAttribute("bf", Form = XmlSchemaForm.Unqualified)]
        public string Bf { get; set; }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs { get; set; }



        [XmlAttribute("s", Form = XmlSchemaForm.Unqualified)]
        public string S { get; set; }



        [XmlAttribute("d", Form = XmlSchemaForm.Unqualified)]
        public string D { get; set; }



        [XmlAttribute("t", Form = XmlSchemaForm.Unqualified)]
        public string T { get; set; }



        [XmlAttribute("hr", Form = XmlSchemaForm.Unqualified)]
        public string Hr { get; set; }



        [XmlAttribute("rbi", Form = XmlSchemaForm.Unqualified)]
        public string Rbi { get; set; }



        [XmlAttribute("bb", Form = XmlSchemaForm.Unqualified)]
        public string Bb { get; set; }



        [XmlAttribute("ibb", Form = XmlSchemaForm.Unqualified)]
        public string Ibb { get; set; }



        [XmlAttribute("hbp", Form = XmlSchemaForm.Unqualified)]
        public string Hbp { get; set; }



        [XmlAttribute("sb", Form = XmlSchemaForm.Unqualified)]
        public string Sb { get; set; }



        [XmlAttribute("cs", Form = XmlSchemaForm.Unqualified)]
        public string Cs { get; set; }



        [XmlAttribute("h", Form = XmlSchemaForm.Unqualified)]
        public string H { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("oba", Form = XmlSchemaForm.Unqualified)]
        public decimal Oba { get; set; }

        [XmlIgnore]
        public bool ObaSpecified { get; set; }



        [XmlAttribute("obp", Form = XmlSchemaForm.Unqualified)]
        public decimal Obp { get; set; }

        [XmlIgnore]
        public bool ObpSpecified { get; set; }



        [XmlAttribute("slg", Form = XmlSchemaForm.Unqualified)]
        public decimal Slg { get; set; }

        [XmlIgnore]
        public bool SlgSpecified { get; set; }



        [XmlAttribute("ops", Form = XmlSchemaForm.Unqualified)]
        public decimal Ops { get; set; }

        [XmlIgnore]
        public bool OpsSpecified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingTotalsCategorySplitsType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingTotalsCategorySplitsType
    {



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitchingTotalsCategorySplitsTypeStatistics Statistics { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PitchingTotalsCategorySplitsTypeStatistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingTotalsCategorySplitsTypeStatistics : IPitchingTotalsCategorySplitsAttributes
    {



        [XmlAttribute("win", Form = XmlSchemaForm.Unqualified)]
        public string Win { get; set; }



        [XmlAttribute("loss", Form = XmlSchemaForm.Unqualified)]
        public string Loss { get; set; }



        [XmlAttribute("save", Form = XmlSchemaForm.Unqualified)]
        public string Save { get; set; }



        [XmlAttribute("svo", Form = XmlSchemaForm.Unqualified)]
        public string Svo { get; set; }



        [XmlAttribute("start", Form = XmlSchemaForm.Unqualified)]
        public string Start { get; set; }



        [XmlAttribute("play", Form = XmlSchemaForm.Unqualified)]
        public string Play { get; set; }



        [XmlAttribute("complete", Form = XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }



        [XmlAttribute("ip_1", Form = XmlSchemaForm.Unqualified)]
        public string Ip_1 { get; set; }



        [XmlAttribute("ip_2", Form = XmlSchemaForm.Unqualified)]
        public decimal Ip_2 { get; set; }

        [XmlIgnore]
        public bool Ip_2Specified { get; set; }



        [XmlAttribute("h", Form = XmlSchemaForm.Unqualified)]
        public string H { get; set; }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs { get; set; }



        [XmlAttribute("er", Form = XmlSchemaForm.Unqualified)]
        public string Er { get; set; }



        [XmlAttribute("hr", Form = XmlSchemaForm.Unqualified)]
        public string Hr { get; set; }



        [XmlAttribute("bb", Form = XmlSchemaForm.Unqualified)]
        public string Bb { get; set; }



        [XmlAttribute("ibb", Form = XmlSchemaForm.Unqualified)]
        public decimal Ibb { get; set; }

        [XmlIgnore]
        public bool IbbSpecified { get; set; }



        [XmlAttribute("oba", Form = XmlSchemaForm.Unqualified)]
        public decimal Oba { get; set; }

        [XmlIgnore]
        public bool ObaSpecified { get; set; }



        [XmlAttribute("era", Form = XmlSchemaForm.Unqualified)]
        public string Era { get; set; }



        [XmlAttribute("ktotal", Form = XmlSchemaForm.Unqualified)]
        public string Ktotal { get; set; }



        [XmlAttribute("team_win", Form = XmlSchemaForm.Unqualified)]
        public string Team_Win { get; set; }



        [XmlAttribute("team_loss", Form = XmlSchemaForm.Unqualified)]
        public string Team_Loss { get; set; }



        [XmlAttribute("bf", Form = XmlSchemaForm.Unqualified)]
        public string Bf { get; set; }



        [XmlAttribute("s", Form = XmlSchemaForm.Unqualified)]
        public string S { get; set; }



        [XmlAttribute("d", Form = XmlSchemaForm.Unqualified)]
        public string D { get; set; }



        [XmlAttribute("t", Form = XmlSchemaForm.Unqualified)]
        public string T { get; set; }



        [XmlAttribute("rbi", Form = XmlSchemaForm.Unqualified)]
        public string Rbi { get; set; }



        [XmlAttribute("hbp", Form = XmlSchemaForm.Unqualified)]
        public string Hbp { get; set; }



        [XmlAttribute("sb", Form = XmlSchemaForm.Unqualified)]
        public string Sb { get; set; }



        [XmlAttribute("cs", Form = XmlSchemaForm.Unqualified)]
        public string Cs { get; set; }



        [XmlAttribute("obp", Form = XmlSchemaForm.Unqualified)]
        public decimal Obp { get; set; }

        [XmlIgnore]
        public bool ObpSpecified { get; set; }



        [XmlAttribute("slg", Form = XmlSchemaForm.Unqualified)]
        public decimal Slg { get; set; }

        [XmlIgnore]
        public bool SlgSpecified { get; set; }



        [XmlAttribute("ops", Form = XmlSchemaForm.Unqualified)]
        public decimal Ops { get; set; }

        [XmlIgnore]
        public bool OpsSpecified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPitchingTotalsCategorySplitsAttributes
    {



        string Win
        {
            get;
            set;
        }



        string Loss
        {
            get;
            set;
        }



        string Save
        {
            get;
            set;
        }



        string Svo
        {
            get;
            set;
        }



        string Start
        {
            get;
            set;
        }



        string Play
        {
            get;
            set;
        }



        string Complete
        {
            get;
            set;
        }



        string Ip_1
        {
            get;
            set;
        }



        decimal Ip_2
        {
            get;
            set;
        }



        string H
        {
            get;
            set;
        }



        string Runs
        {
            get;
            set;
        }



        string Er
        {
            get;
            set;
        }



        string Hr
        {
            get;
            set;
        }



        string Bb
        {
            get;
            set;
        }



        decimal Ibb
        {
            get;
            set;
        }



        decimal Oba
        {
            get;
            set;
        }



        string Era
        {
            get;
            set;
        }



        string Ktotal
        {
            get;
            set;
        }



        string Team_Win
        {
            get;
            set;
        }



        string Team_Loss
        {
            get;
            set;
        }



        string Bf
        {
            get;
            set;
        }



        string S
        {
            get;
            set;
        }



        string D
        {
            get;
            set;
        }



        string T
        {
            get;
            set;
        }



        string Rbi
        {
            get;
            set;
        }



        string Hbp
        {
            get;
            set;
        }



        string Sb
        {
            get;
            set;
        }



        string Cs
        {
            get;
            set;
        }



        decimal Obp
        {
            get;
            set;
        }



        decimal Slg
        {
            get;
            set;
        }



        decimal Ops
        {
            get;
            set;
        }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeFinal", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeFinal
    {



        [XmlAttribute("inning", Form = XmlSchemaForm.Unqualified)]
        public string Inning { get; set; }



        [XmlAttribute("inning_half", Form = XmlSchemaForm.Unqualified)]
        public string Inning_Half { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("outcomeType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OutcomeType
    {



        [XmlElement("count", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public OutcomeTypeCount Count { get; set; }



        [XmlElement("hitter", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public OutcomeTypeHitter Hitter { get; set; }



        [XmlElement("pitcher", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public OutcomeTypePitcher Pitcher { get; set; }

        [XmlIgnore]
        private Collection<RunnersTypeRunner> _runners;



        [XmlArray("runners", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("runner", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<RunnersTypeRunner> Runners
        {
            get
            {
                return _runners;
            }
            private set
            {
                _runners = value;
            }
        }

        [XmlIgnore]
        public bool RunnersSpecified
        {
            get
            {
                return Runners.Count != 0;
            }
        }

        public OutcomeType()
        {
            _runners = new Collection<RunnersTypeRunner>();
        }



        [XmlAttribute("type", Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }



        [XmlAttribute("current_inning", Form = XmlSchemaForm.Unqualified)]
        public string Current_Inning { get; set; }



        [XmlAttribute("current_inning_half", Form = XmlSchemaForm.Unqualified)]
        public string Current_Inning_Half { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("OutcomeTypeCount", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OutcomeTypeCount
    {



        [XmlAttribute("strikes", Form = XmlSchemaForm.Unqualified)]
        public string Strikes { get; set; }



        [XmlAttribute("balls", Form = XmlSchemaForm.Unqualified)]
        public string Balls { get; set; }



        [XmlAttribute("outs", Form = XmlSchemaForm.Unqualified)]
        public string Outs { get; set; }



        [XmlAttribute("inning", Form = XmlSchemaForm.Unqualified)]
        public string Inning { get; set; }



        [XmlAttribute("inning_half", Form = XmlSchemaForm.Unqualified)]
        public string Inning_Half { get; set; }



        [XmlAttribute("half_over", Form = XmlSchemaForm.Unqualified)]
        public bool Half_Over { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("OutcomeTypeHitter", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OutcomeTypeHitter : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("outcome_id", Form = XmlSchemaForm.Unqualified)]
        public string Outcome_Id { get; set; }



        [XmlAttribute("ab_over", Form = XmlSchemaForm.Unqualified)]
        public bool Ab_Over { get; set; }

        [XmlIgnore]
        public bool Ab_OverSpecified { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("OutcomeTypePitcher", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class OutcomeTypePitcher : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("pitch_type", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Type { get; set; }



        [XmlAttribute("pitch_speed", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Speed { get; set; }



        [XmlAttribute("pitch_zone", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Zone { get; set; }



        [XmlAttribute("pitch_x", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_X { get; set; }



        [XmlAttribute("pitch_y", Form = XmlSchemaForm.Unqualified)]
        public string Pitch_Y { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("runnersType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class RunnersType
    {

        [XmlIgnore]
        private Collection<RunnersTypeRunner> _runner;



        [XmlElement("runner", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<RunnersTypeRunner> Runner
        {
            get
            {
                return _runner;
            }
            private set
            {
                _runner = value;
            }
        }

        [XmlIgnore]
        public bool RunnerSpecified
        {
            get
            {
                return Runner.Count != 0;
            }
        }

        public RunnersType()
        {
            _runner = new Collection<RunnersTypeRunner>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("RunnersTypeRunner", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class RunnersTypeRunner : IBasePlayerAttributes
    {

        [XmlIgnore]
        private Collection<FieldersType> _fielders;



        [XmlElement("fielders", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldersType> Fielders
        {
            get
            {
                return _fielders;
            }
            private set
            {
                _fielders = value;
            }
        }

        [XmlIgnore]
        public bool FieldersSpecified
        {
            get
            {
                return Fielders.Count != 0;
            }
        }

        public RunnersTypeRunner()
        {
            _fielders = new Collection<FieldersType>();
            _description = new Collection<string>();
        }

        [XmlIgnore]
        private Collection<string> _description;



        [XmlElement("description", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<string> Description
        {
            get
            {
                return _description;
            }
            private set
            {
                _description = value;
            }
        }

        [XmlIgnore]
        public bool DescriptionSpecified
        {
            get
            {
                return Description.Count != 0;
            }
        }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("ending_base", Form = XmlSchemaForm.Unqualified)]
        public string Ending_Base { get; set; }



        [XmlAttribute("starting_base", Form = XmlSchemaForm.Unqualified)]
        public string Starting_Base { get; set; }



        [XmlAttribute("outcome_id", Form = XmlSchemaForm.Unqualified)]
        public string Outcome_Id { get; set; }



        [XmlAttribute("out", Form = XmlSchemaForm.Unqualified)]
        public bool Out { get; set; }

        [XmlIgnore]
        public bool OutSpecified { get; set; }
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("fieldersType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldersType
    {

        [XmlIgnore]
        private Collection<FieldersTypePutout> _putout;



        [XmlElement("putout", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldersTypePutout> Putout
        {
            get
            {
                return _putout;
            }
            private set
            {
                _putout = value;
            }
        }

        [XmlIgnore]
        public bool PutoutSpecified
        {
            get
            {
                return Putout.Count != 0;
            }
        }

        public FieldersType()
        {
            _putout = new Collection<FieldersTypePutout>();
            _assist = new Collection<FieldersTypeAssist>();
        }

        [XmlIgnore]
        private Collection<FieldersTypeAssist> _assist;



        [XmlElement("assist", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldersTypeAssist> Assist
        {
            get
            {
                return _assist;
            }
            private set
            {
                _assist = value;
            }
        }

        [XmlIgnore]
        public bool AssistSpecified
        {
            get
            {
                return Assist.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("FieldersTypePutout", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldersTypePutout : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("sequence", Form = XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("FieldersTypeAssist", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldersTypeAssist : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("sequence", Form = XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("teamType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamType : IBaseTeamAttributes
    {



        [XmlElement("probable_pitcher", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitcherType Probable_Pitcher { get; set; }



        [XmlElement("starting_pitcher", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitcherType Starting_Pitcher { get; set; }



        [XmlElement("current_pitcher", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public PitcherType Current_Pitcher { get; set; }

        [XmlIgnore]
        private Collection<PlayerType> _roster;



        [XmlArray("roster", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PlayerType> Roster
        {
            get
            {
                return _roster;
            }
            private set
            {
                _roster = value;
            }
        }

        [XmlIgnore]
        public bool RosterSpecified
        {
            get
            {
                return Roster.Count != 0;
            }
        }

        public TeamType()
        {
            _roster = new Collection<PlayerType>();
            _lineup = new Collection<PlayerLineupType>();
            _scoring = new Collection<InningScoreType>();
            _players = new Collection<TeamTypePlayersPlayer>();
            _runs = new Collection<TeamTypeRunsRun>();
        }

        [XmlIgnore]
        private Collection<PlayerLineupType> _lineup;



        [XmlArray("lineup", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PlayerLineupType> Lineup
        {
            get
            {
                return _lineup;
            }
            private set
            {
                _lineup = value;
            }
        }

        [XmlIgnore]
        public bool LineupSpecified
        {
            get
            {
                return Lineup.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<InningScoreType> _scoring;



        [XmlArray("scoring", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("inning", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<InningScoreType> Scoring
        {
            get
            {
                return _scoring;
            }
            private set
            {
                _scoring = value;
            }
        }

        [XmlIgnore]
        public bool ScoringSpecified
        {
            get
            {
                return Scoring.Count != 0;
            }
        }



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public TeamTypeStatistics Statistics { get; set; }

        [XmlIgnore]
        private Collection<TeamTypePlayersPlayer> _players;



        [XmlArray("players", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
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

        [XmlIgnore]
        private Collection<TeamTypeRunsRun> _runs;



        [XmlArray("runs", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("run", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamTypeRunsRun> Runs
        {
            get
            {
                return _runs;
            }
            private set
            {
                _runs = value;
            }
        }

        [XmlIgnore]
        public bool RunsSpecified
        {
            get
            {
                return Runs.Count != 0;
            }
        }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs_1 { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }



        [XmlAttribute("alias", Form = XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }



        [XmlAttribute("abbr", Form = XmlSchemaForm.Unqualified)]
        public string Abbr { get; set; }



        [XmlAttribute("market", Form = XmlSchemaForm.Unqualified)]
        public string Market { get; set; }



        [XmlAttribute("founded", Form = XmlSchemaForm.Unqualified)]
        public string Founded { get; set; }



        [XmlAttribute("hits", Form = XmlSchemaForm.Unqualified)]
        public string Hits { get; set; }



        [XmlAttribute("errors", Form = XmlSchemaForm.Unqualified)]
        public string Errors { get; set; }



        [XmlAttribute("win", Form = XmlSchemaForm.Unqualified)]
        public string Win { get; set; }



        [XmlAttribute("loss", Form = XmlSchemaForm.Unqualified)]
        public string Loss { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitcherType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitcherType : IBasePlayerAttributes, IPitcherRecordAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("win", Form = XmlSchemaForm.Unqualified)]
        public string Win { get; set; }



        [XmlAttribute("loss", Form = XmlSchemaForm.Unqualified)]
        public string Loss { get; set; }



        [XmlAttribute("save", Form = XmlSchemaForm.Unqualified)]
        public string Save { get; set; }



        [XmlAttribute("hold", Form = XmlSchemaForm.Unqualified)]
        public string Hold { get; set; }



        [XmlAttribute("blown_save", Form = XmlSchemaForm.Unqualified)]
        public string Blown_Save { get; set; }



        [XmlAttribute("era", Form = XmlSchemaForm.Unqualified)]
        public string Era { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPitcherRecordAttributes
    {



        string Win
        {
            get;
            set;
        }



        string Loss
        {
            get;
            set;
        }



        string Save
        {
            get;
            set;
        }



        string Hold
        {
            get;
            set;
        }



        string Blown_Save
        {
            get;
            set;
        }



        string Era
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeRoster", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeRoster
    {

        [XmlIgnore]
        private Collection<PlayerType> _player;



        [XmlElement("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PlayerType> Player
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

        public TeamTypeRoster()
        {
            _player = new Collection<PlayerType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("playerType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayerType : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeLineup", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeLineup
    {

        [XmlIgnore]
        private Collection<PlayerLineupType> _player;



        [XmlElement("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PlayerLineupType> Player
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

        public TeamTypeLineup()
        {
            _player = new Collection<PlayerLineupType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeScoring", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeScoring
    {

        [XmlIgnore]
        private Collection<InningScoreType> _inning;



        [XmlElement("inning", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<InningScoreType> Inning
        {
            get
            {
                return _inning;
            }
            private set
            {
                _inning = value;
            }
        }

        [XmlIgnore]
        public bool InningSpecified
        {
            get
            {
                return Inning.Count != 0;
            }
        }

        public TeamTypeScoring()
        {
            _inning = new Collection<InningScoreType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("inningScoreType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class InningScoreType
    {



        [XmlElement("scoring", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public ScoringType Scoring { get; set; }

        [XmlIgnore]
        private Collection<InningScoreTypeInning_Half> _inning_Half;



        [XmlElement("inning_half", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<InningScoreTypeInning_Half> Inning_Half
        {
            get
            {
                return _inning_Half;
            }
            private set
            {
                _inning_Half = value;
            }
        }

        [XmlIgnore]
        public bool Inning_HalfSpecified
        {
            get
            {
                return Inning_Half.Count != 0;
            }
        }

        public InningScoreType()
        {
            _inning_Half = new Collection<InningScoreTypeInning_Half>();
        }



        [XmlAttribute("number", Form = XmlSchemaForm.Unqualified)]
        public string Number { get; set; }



        [XmlAttribute("runs", Form = XmlSchemaForm.Unqualified)]
        public string Runs { get; set; }



        [XmlAttribute("hits", Form = XmlSchemaForm.Unqualified)]
        public string Hits { get; set; }



        [XmlAttribute("errors", Form = XmlSchemaForm.Unqualified)]
        public string Errors { get; set; }



        [XmlAttribute("sequence", Form = XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("scoringType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringType
    {

        [XmlIgnore]
        private Collection<TeamType> _home;



        [XmlElement("home", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamType> Home
        {
            get
            {
                return _home;
            }
            private set
            {
                _home = value;
            }
        }

        [XmlIgnore]
        public bool HomeSpecified
        {
            get
            {
                return Home.Count != 0;
            }
        }

        public ScoringType()
        {
            _home = new Collection<TeamType>();
            _away = new Collection<TeamType>();
        }

        [XmlIgnore]
        private Collection<TeamType> _away;



        [XmlElement("away", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamType> Away
        {
            get
            {
                return _away;
            }
            private set
            {
                _away = value;
            }
        }

        [XmlIgnore]
        public bool AwaySpecified
        {
            get
            {
                return Away.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("InningScoreTypeInning_Half", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class InningScoreTypeInning_Half
    {

        [XmlIgnore]
        private Collection<LineupType> _lineup;



        [XmlElement("lineup", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<LineupType> Lineup
        {
            get
            {
                return _lineup;
            }
            private set
            {
                _lineup = value;
            }
        }

        [XmlIgnore]
        public bool LineupSpecified
        {
            get
            {
                return Lineup.Count != 0;
            }
        }

        public InningScoreTypeInning_Half()
        {
            _lineup = new Collection<LineupType>();
        }



        [XmlAttribute("type", Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeStatistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeStatistics
    {

        [XmlIgnore]
        private Collection<HittingStatsGroupType> _hitting;



        [XmlElement("hitting", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingStatsGroupType> Hitting
        {
            get
            {
                return _hitting;
            }
            private set
            {
                _hitting = value;
            }
        }

        [XmlIgnore]
        public bool HittingSpecified
        {
            get
            {
                return Hitting.Count != 0;
            }
        }

        public TeamTypeStatistics()
        {
            _hitting = new Collection<HittingStatsGroupType>();
            _pitching = new Collection<PitchingStatsGroupType>();
            _fielding = new Collection<FieldingStatsGroupType>();
        }

        [XmlIgnore]
        private Collection<PitchingStatsGroupType> _pitching;



        [XmlElement("pitching", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsGroupType> Pitching
        {
            get
            {
                return _pitching;
            }
            private set
            {
                _pitching = value;
            }
        }

        [XmlIgnore]
        public bool PitchingSpecified
        {
            get
            {
                return Pitching.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<FieldingStatsGroupType> _fielding;



        [XmlElement("fielding", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldingStatsGroupType> Fielding
        {
            get
            {
                return _fielding;
            }
            private set
            {
                _fielding = value;
            }
        }

        [XmlIgnore]
        public bool FieldingSpecified
        {
            get
            {
                return Fielding.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayers", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayers
    {

        [XmlIgnore]
        private Collection<TeamTypePlayersPlayer> _player;



        [XmlElement("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
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
    [XmlType("TeamTypePlayersPlayer", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayer : IBasePlayerAttributes
    {



        [XmlElement("statistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public TeamTypePlayersPlayerStatistics Statistics { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypePlayersPlayerStatistics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypePlayersPlayerStatistics
    {

        [XmlIgnore]
        private Collection<HittingStatsGroupType> _hitting;



        [XmlElement("hitting", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<HittingStatsGroupType> Hitting
        {
            get
            {
                return _hitting;
            }
            private set
            {
                _hitting = value;
            }
        }

        [XmlIgnore]
        public bool HittingSpecified
        {
            get
            {
                return Hitting.Count != 0;
            }
        }

        public TeamTypePlayersPlayerStatistics()
        {
            _hitting = new Collection<HittingStatsGroupType>();
            _pitching = new Collection<PitchingStatsGroupType>();
            _fielding = new Collection<FieldingStatsGroupType>();
            _pitch_Metrics = new Collection<PitchMetricsStatsGroupType>();
        }

        [XmlIgnore]
        private Collection<PitchingStatsGroupType> _pitching;



        [XmlElement("pitching", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchingStatsGroupType> Pitching
        {
            get
            {
                return _pitching;
            }
            private set
            {
                _pitching = value;
            }
        }

        [XmlIgnore]
        public bool PitchingSpecified
        {
            get
            {
                return Pitching.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<FieldingStatsGroupType> _fielding;



        [XmlElement("fielding", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<FieldingStatsGroupType> Fielding
        {
            get
            {
                return _fielding;
            }
            private set
            {
                _fielding = value;
            }
        }

        [XmlIgnore]
        public bool FieldingSpecified
        {
            get
            {
                return Fielding.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitchMetricsStatsGroupType> _pitch_Metrics;

        [XmlElement("pitch_metrics", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitchMetricsStatsGroupType> Pitch_Metrics
        {
            get
            {
                return _pitch_Metrics;
            }
            private set
            {
                _pitch_Metrics = value;
            }
        }

        [XmlIgnore]
        public bool Pitch_MetricsSpecified
        {
            get
            {
                return Pitch_Metrics.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeRuns", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeRuns
    {

        [XmlIgnore]
        private Collection<TeamTypeRunsRun> _run;



        [XmlElement("run", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<TeamTypeRunsRun> Run
        {
            get
            {
                return _run;
            }
            private set
            {
                _run = value;
            }
        }

        [XmlIgnore]
        public bool RunSpecified
        {
            get
            {
                return Run.Count != 0;
            }
        }

        public TeamTypeRuns()
        {
            _run = new Collection<TeamTypeRunsRun>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("TeamTypeRunsRun", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class TeamTypeRunsRun
    {

        [XmlIgnore]
        private Collection<RunnerType> _runner;



        [XmlElement("runner", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<RunnerType> Runner
        {
            get
            {
                return _runner;
            }
            private set
            {
                _runner = value;
            }
        }

        public TeamTypeRunsRun()
        {
            _runner = new Collection<RunnerType>();
        }



        [XmlAttribute("hitter_id", Form = XmlSchemaForm.Unqualified)]
        public string Hitter_Id { get; set; }



        [XmlAttribute("hitter_outcome", Form = XmlSchemaForm.Unqualified)]
        public string Hitter_Outcome { get; set; }



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("inning", Form = XmlSchemaForm.Unqualified)]
        public string Inning { get; set; }



        [XmlAttribute("inning_half", Form = XmlSchemaForm.Unqualified)]
        public string Inning_Half { get; set; }



        [XmlAttribute("pitcher_id", Form = XmlSchemaForm.Unqualified)]
        public string Pitcher_Id { get; set; }



        [XmlAttribute("type", Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("runnerType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class RunnerType : IBasePlayerAttributes
    {



        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }



        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }



        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }



        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }



        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }



        [XmlAttribute("abbr_name", Form = XmlSchemaForm.Unqualified)]
        public string Abbr_Name { get; set; }



        [XmlAttribute("jersey_number", Form = XmlSchemaForm.Unqualified)]
        public string Jersey_Number { get; set; }



        [XmlAttribute("position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Position { get; set; }

        [XmlIgnore]
        public bool PositionSpecified { get; set; }



        [XmlAttribute("primary_position", Form = XmlSchemaForm.Unqualified)]
        public PositionType Primary_Position { get; set; }

        [XmlIgnore]
        public bool Primary_PositionSpecified { get; set; }



        [XmlAttribute("status", Form = XmlSchemaForm.Unqualified)]
        public string Status { get; set; }



        [XmlAttribute("starting_base", Form = XmlSchemaForm.Unqualified)]
        public string Starting_Base { get; set; }



        [XmlAttribute("ending_base", Form = XmlSchemaForm.Unqualified)]
        public string Ending_Base { get; set; }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseTeamAttributes : IBaseOrganizationAttributes
    {



        string Abbr
        {
            get;
            set;
        }



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
    [XmlType("pitchingType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingType
    {

        [XmlIgnore]
        private Collection<PitcherType> _win;



        [XmlArray("win", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Win
        {
            get
            {
                return _win;
            }
            private set
            {
                _win = value;
            }
        }

        public PitchingType()
        {
            _win = new Collection<PitcherType>();
            _loss = new Collection<PitcherType>();
            _save = new Collection<PitcherType>();
            _hold = new Collection<PitcherType>();
            _blown_Save = new Collection<PitcherType>();
        }

        [XmlIgnore]
        private Collection<PitcherType> _loss;



        [XmlArray("loss", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Loss
        {
            get
            {
                return _loss;
            }
            private set
            {
                _loss = value;
            }
        }

        [XmlIgnore]
        private Collection<PitcherType> _save;



        [XmlArray("save", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Save
        {
            get
            {
                return _save;
            }
            private set
            {
                _save = value;
            }
        }

        [XmlIgnore]
        public bool SaveSpecified
        {
            get
            {
                return Save.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitcherType> _hold;



        [XmlArray("hold", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Hold
        {
            get
            {
                return _hold;
            }
            private set
            {
                _hold = value;
            }
        }

        [XmlIgnore]
        public bool HoldSpecified
        {
            get
            {
                return Hold.Count != 0;
            }
        }

        [XmlIgnore]
        private Collection<PitcherType> _blown_Save;



        [XmlArray("blown_save", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        [XmlArrayItem("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Blown_Save
        {
            get
            {
                return _blown_Save;
            }
            private set
            {
                _blown_Save = value;
            }
        }

        [XmlIgnore]
        public bool Blown_SaveSpecified
        {
            get
            {
                return Blown_Save.Count != 0;
            }
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("pitchingCategoryType", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PitchingCategoryType
    {

        [XmlIgnore]
        private Collection<PitcherType> _player;



        [XmlElement("player", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
        public Collection<PitcherType> Player
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

        public PitchingCategoryType()
        {
            _player = new Collection<PitcherType>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeOfficials", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeOfficials
    {

        [XmlIgnore]
        private Collection<GameTypeOfficialsOfficial> _official;



        [XmlElement("official", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
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
        public bool OfficialSpecified => Official.Count != 0;

        public GameTypeOfficials()
        {
            _official = new Collection<GameTypeOfficialsOfficial>();
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeOfficialsOfficial", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd", AnonymousType = true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeOfficialsOfficial : IBasePersonnelAttributes
    {
        [XmlAttribute("id", Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        [XmlAttribute("first_name", Form = XmlSchemaForm.Unqualified)]
        public string First_Name { get; set; }

        [XmlAttribute("last_name", Form = XmlSchemaForm.Unqualified)]
        public string Last_Name { get; set; }

        [XmlAttribute("full_name", Form = XmlSchemaForm.Unqualified)]
        public string Full_Name { get; set; }

        [XmlAttribute("preferred_name", Form = XmlSchemaForm.Unqualified)]
        public string Preferred_Name { get; set; }

        [XmlAttribute("assignment", Form = XmlSchemaForm.Unqualified)]
        public string Assignment { get; set; }

        [XmlAttribute("experience", Form = XmlSchemaForm.Unqualified)]
        public string Experience { get; set; }
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

        string Attendance
        {
            get;
            set;
        }

        string Duration
        {
            get;
            set;
        }

        string Game_Number
        {
            get;
            set;
        }

        IBaseGameAttributesDay_Night Day_Night
        {
            get;
            set;
        }

        bool Double_Header
        {
            get;
            set;
        }

        bool Split_Squad
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

        bool Tbd
        {
            get;
            set;
        }

        string Ps_Round
        {
            get;
            set;
        }

        string Ps_Game
        {
            get;
            set;
        }
    }



    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesStatus", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum IBaseGameAttributesStatus
    {
        [XmlEnum("scheduled")]
        Scheduled,

        [XmlEnum("inprogress")]
        Inprogress,

        [XmlEnum("complete")]
        Complete,

        [XmlEnum("closed")]
        Closed,

        [XmlEnum("canceled")]
        Canceled,

        [XmlEnum("postponed")]
        Postponed,

        [XmlEnum("suspended")]
        Suspended,

        [XmlEnum("wdelay")]
        Wdelay,

        [XmlEnum("fdelay")]
        Fdelay,

        [XmlEnum("odelay")]
        Odelay,

        [XmlEnum("maintenance")]
        Maintenance,

        [XmlEnum("unnecessary")]
        Unnecessary
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesCoverage", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum IBaseGameAttributesCoverage
    {
        [XmlEnum("full")]
        Full,

        [XmlEnum("boxscore")]
        Boxscore
    }

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesDay_Night", Namespace = "http://feed.elasticstats.com/schema/baseball/v6.5/game.xsd")]
    public enum IBaseGameAttributesDay_Night
    {
        D,
        N
    }
}
