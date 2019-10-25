using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;


// todo this class is a copy of NFL and must be recreated
// ReSharper disable All

namespace SportsIq.Models.SportRadar.Ncaafb.GameInfo
{
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("game", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public class NcaafbGameInfo : IBaseGameAttributes, IExtGameAttributes
    {
        [XmlIgnore]
        private Collection<CoinTossType> _coin_Toss;
        
        [XmlElement("coin_toss", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<CoinTossType> Coin_Toss
        {
            get => _coin_Toss;
            private set => _coin_Toss = value;
        }
        
        [XmlIgnore]
        public bool Coin_TossSpecified => Coin_Toss.Count != 0;
        
        public NcaafbGameInfo()
        {
            _coin_Toss = new Collection<CoinTossType>();
            _scoring_Drives = new Collection<ScoringDriveType>();
            _scoring_Plays = new Collection<GamePlayScoresType>();
        }
        
        [XmlElement("summary", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public SummaryType Summary { get; set; }
        
        [XmlElement("situation", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public FieldSituationType Situation { get; set; }
        
        [XmlElement("last_event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GameTypeLast_Event Last_Event { get; set; }
        
        [XmlElement("scoring", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GameTypeScoring Scoring { get; set; }
        
        [XmlIgnore]
        private Collection<ScoringDriveType> _scoring_Drives;
        
        [XmlArray("scoring_drives", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("drive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ScoringDriveType> Scoring_Drives
        {
            get => _scoring_Drives;
            private set => _scoring_Drives = value;
        }
        
        [XmlIgnore]
        public bool Scoring_DrivesSpecified => Scoring_Drives.Count != 0;

        [XmlIgnore]
        private Collection<GamePlayScoresType> _scoring_Plays;
        
        [XmlArray("scoring_plays", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GamePlayScoresType> Scoring_Plays
        {
            get => _scoring_Plays;
            private set => _scoring_Plays = value;
        }
        
        [XmlIgnore]
        public bool Scoring_PlaysSpecified => Scoring_Plays.Count != 0;

        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        [XmlAttribute("utc_offset", Form=XmlSchemaForm.Unqualified)]
        public string Utc_Offset { get; set; }
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        [XmlAttribute("scheduled", Form=XmlSchemaForm.Unqualified, DataType="dateTime")]
        public DateTime Scheduled { get; set; }
        
        [XmlAttribute("status", Form=XmlSchemaForm.Unqualified)]
        public IBaseGameAttributesStatus Status { get; set; }
        
        [XmlAttribute("attendance", Form=XmlSchemaForm.Unqualified)]
        public string Attendance { get; set; }
        
        [XmlAttribute("weather", Form=XmlSchemaForm.Unqualified)]
        public string Weather { get; set; }
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        [XmlAttribute("quarter", Form=XmlSchemaForm.Unqualified)]
        public string Quarter { get; set; }
        
        [XmlAttribute("entry_mode", Form=XmlSchemaForm.Unqualified)]
        public IExtGameAttributesEntry_Mode Entry_Mode { get; set; }
        
        [XmlIgnore]
        public bool Entry_ModeSpecified { get; set; }
    }
    

    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("referenceType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ReferenceType
    {
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("origin", Form=XmlSchemaForm.Unqualified)]
        public string Origin { get; set; }
    }
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("venueType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class VenueType
    {
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
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
        
        
        
        [XmlAttribute("roof_type", Form=XmlSchemaForm.Unqualified)]
        public VenueTypeRoof_Type Roof_Type { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Roof_Type-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Roof_Type property is specified.</para>
        
        [XmlIgnore]
        public bool Roof_TypeSpecified { get; set; }
        
        
        
        [XmlAttribute("surface", Form=XmlSchemaForm.Unqualified)]
        public VenueTypeSurface Surface { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Surface-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Surface property is specified.</para>
        
        [XmlIgnore]
        public bool SurfaceSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("VenueTypeRoof_Type", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum VenueTypeRoof_Type
    {
        
        
        
        [XmlEnum("outdoor")]
        Outdoor,
        
        
        
        [XmlEnum("dome")]
        Dome,
        
        
        
        [XmlEnum("retractable_dome")]
        Retractable_Dome
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("VenueTypeSurface", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum VenueTypeSurface
    {
        
        
        
        [XmlEnum("turf")]
        Turf,
        
        
        
        [XmlEnum("artificial")]
        Artificial
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("teamType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlInclude(typeof(GameTeamType))]
    public class TeamType : IBaseTeamAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("market", Form=XmlSchemaForm.Unqualified)]
        public string Market { get; set; }
        
        
        
        [XmlAttribute("founded", Form=XmlSchemaForm.Unqualified)]
        public string Founded { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
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
        
        
        
        string Reference
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseOrganizationAttributes : IBaseIdentityAttributes
    {
        
        
        
        string Alias
        {
            get;
            set;
        }
        
        
        
        string Name
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseIdentityAttributes
    {
        
        
        
        string Id
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
    [XmlType("references", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("references", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public class References
    {
        
        [XmlIgnore]
        private Collection<ReferenceType> _reference;
        
        
        
        [XmlElement("reference", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ReferenceType> Reference
        {
            get => _reference;
            private set => _reference = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Reference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Reference collection is empty.</para>
        
        [XmlIgnore]
        public bool ReferenceSpecified => Reference.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="References" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="References" /> class.</para>
        
        public References()
        {
            _reference = new Collection<ReferenceType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("franchise", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlRoot("franchise", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public class Franchise : IBaseOrganizationAttributes
    {
        
        [XmlIgnore]
        private Collection<ReferenceType> _references;
        
        
        
        [XmlArray("references", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("reference", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ReferenceType> References
        {
            get => _references;
            private set => _references = value;
        }
        
        
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Franchise" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Franchise" /> class.</para>
        
        public Franchise()
        {
            _references = new Collection<ReferenceType>();
        }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("summaryType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class SummaryType
    {
        
        
        
        [XmlElement("season", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public SummaryTypeSeason Season { get; set; }
        
        
        
        [XmlElement("week", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public SummaryTypeWeek Week { get; set; }
        
        
        
        [XmlElement("venue", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public VenueType Venue { get; set; }
        
        
        
        [XmlElement("home", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GameTeamType Home { get; set; }
        
        
        
        [XmlElement("away", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GameTeamType Away { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("SummaryTypeSeason", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class SummaryTypeSeason : IBaseSeasonAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("year", Form=XmlSchemaForm.Unqualified)]
        public string Year { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public IBaseSeasonAttributesType Type { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseSeasonAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Name
        {
            get;
            set;
        }
        
        
        
        string Year
        {
            get;
            set;
        }
        
        
        
        IBaseSeasonAttributesType Type
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseSeasonAttributesType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IBaseSeasonAttributesType
    {
        
        
        
        PRE,
        
        
        
        REG,
        
        
        
        PST
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("SummaryTypeWeek", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class SummaryTypeWeek : IBaseWeekAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("title", Form=XmlSchemaForm.Unqualified)]
        public string Title { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseWeekAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Sequence
        {
            get;
            set;
        }
        
        
        
        string Title
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameTeamType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTeamType : TeamType
    {
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
        
        
        
        [XmlAttribute("remaining_timeouts", Form=XmlSchemaForm.Unqualified)]
        public string Remaining_Timeouts { get; set; }
        
        
        
        [XmlAttribute("used_timeouts", Form=XmlSchemaForm.Unqualified)]
        public string Used_Timeouts { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("fieldSituationType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldSituationType
    {
        
        
        
        [XmlElement("possession", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public TeamType Possession { get; set; }
        
        
        
        [XmlElement("location", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public FieldLocationType Location { get; set; }
        
        
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        
        
        [XmlAttribute("down", Form=XmlSchemaForm.Unqualified)]
        public string Down { get; set; }
        
        
        
        [XmlAttribute("yfd", Form=XmlSchemaForm.Unqualified)]
        public string Yfd { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("fieldLocationType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class FieldLocationType : IBaseTeamAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("market", Form=XmlSchemaForm.Unqualified)]
        public string Market { get; set; }
        
        
        
        [XmlAttribute("founded", Form=XmlSchemaForm.Unqualified)]
        public string Founded { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("yardline", Form=XmlSchemaForm.Unqualified)]
        public string Yardline { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameEventType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameEventType : IBaseGameEventAttributes
    {
        
        
        
        [XmlElement("description", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public string Description { get; set; }
        
        
        
        [XmlElement("alt-description", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public string Alt_Description { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public decimal Sequence { get; set; }
        
        
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        
        
        [XmlAttribute("wall_clock", Form=XmlSchemaForm.Unqualified, DataType="dateTime")]
        public DateTime Wall_Clock { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Wall_Clock-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Wall_Clock property is specified.</para>
        
        [XmlIgnore]
        public bool Wall_ClockSpecified { get; set; }
        
        
        
        [XmlAttribute("deleted", Form=XmlSchemaForm.Unqualified)]
        public bool Deleted { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Deleted-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Deleted property is specified.</para>
        
        [XmlIgnore]
        public bool DeletedSpecified { get; set; }
        
        
        
        [XmlAttribute("source", Form=XmlSchemaForm.Unqualified)]
        public string Source { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public GameEventTypeType Type { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseGameEventAttributes
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
        
        
        
        decimal Sequence
        {
            get;
            set;
        }
        
        
        
        string Clock
        {
            get;
            set;
        }
        
        
        
        DateTime Wall_Clock
        {
            get;
            set;
        }
        
        
        
        bool Deleted
        {
            get;
            set;
        }
        
        
        
        string Source
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameEventTypeType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum GameEventTypeType
    {
        
        
        
        [XmlEnum("setup")]
        Setup,
        
        
        
        [XmlEnum("timeout")]
        Timeout,
        
        
        
        [XmlEnum("comment")]
        Comment,
        
        
        
        [XmlEnum("period_end")]
        Period_End,
        
        
        
        [XmlEnum("game_over")]
        Game_Over
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("basePlayType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlInclude(typeof(ExtGamePlayType))]
    [XmlInclude(typeof(GamePlayDetailsType))]
    [XmlInclude(typeof(GamePlayScoresType))]
    [XmlInclude(typeof(GamePlayType))]
    public class BasePlayType : IBasePlayAttributes
    {
        
        
        
        [XmlElement("start_situation", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public FieldSituationType Start_Situation { get; set; }
        
        
        
        [XmlElement("end_situation", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public FieldSituationType End_Situation { get; set; }
        
        
        
        [XmlElement("description", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public string Description { get; set; }
        
        
        
        [XmlElement("alt-description", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public string Alt_Description { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public decimal Sequence { get; set; }
        
        
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        
        
        [XmlAttribute("wall_clock", Form=XmlSchemaForm.Unqualified, DataType="dateTime")]
        public DateTime Wall_Clock { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Wall_Clock-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Wall_Clock property is specified.</para>
        
        [XmlIgnore]
        public bool Wall_ClockSpecified { get; set; }
        
        
        
        [XmlAttribute("deleted", Form=XmlSchemaForm.Unqualified)]
        public bool Deleted { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Deleted-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Deleted property is specified.</para>
        
        [XmlIgnore]
        public bool DeletedSpecified { get; set; }
        
        
        
        [XmlAttribute("source", Form=XmlSchemaForm.Unqualified)]
        public string Source { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public IBasePlayAttributesType Type { get; set; }
        
        
        
        [XmlAttribute("away_points", Form=XmlSchemaForm.Unqualified)]
        public string Away_Points { get; set; }
        
        
        
        [XmlAttribute("home_points", Form=XmlSchemaForm.Unqualified)]
        public string Home_Points { get; set; }
        
        
        
        [XmlAttribute("play_clock", Form=XmlSchemaForm.Unqualified)]
        public string Play_Clock { get; set; }
        
        
        
        [XmlAttribute("scoring_play", Form=XmlSchemaForm.Unqualified)]
        public bool Scoring_Play { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Scoring_Play-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Scoring_Play property is specified.</para>
        
        [XmlIgnore]
        public bool Scoring_PlaySpecified { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public bool Goaltogo { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Goaltogo-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Goaltogo property is specified.</para>
        
        [XmlIgnore]
        public bool GoaltogoSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePlayAttributes : IBaseGameEventAttributes
    {
        
        
        
        IBasePlayAttributesType Type
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBasePlayAttributesType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IBasePlayAttributesType
    {
        
        
        
        [XmlEnum("pass")]
        Pass,
        
        
        
        [XmlEnum("rush")]
        Rush,
        
        
        
        [XmlEnum("faircatch_kick")]
        Faircatch_Kick,
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point,
        
        
        
        [XmlEnum("conversion")]
        Conversion,
        
        
        
        [XmlEnum("free_kick")]
        Free_Kick,
        
        
        
        [XmlEnum("kickoff")]
        Kickoff,
        
        
        
        [XmlEnum("punt")]
        Punt,
        
        
        
        [XmlEnum("field_goal")]
        Field_Goal,
        
        
        
        [XmlEnum("penalty")]
        Penalty
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gamePlayType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayType : BasePlayType
    {
        
        
        
        [XmlElement("drive-info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayTypeDrive_Info Drive_Info { get; set; }
        
        
        
        [XmlElement("score", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayScoreType Score { get; set; }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayStatisticsType Statistics { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayTypeDrive_Info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayTypeDrive_Info
    {
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("playScoreType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayScoreType
    {
        
        
        
        [XmlElement("points-after-play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayScoreTypePoints_After_Play Points_After_Play { get; set; }
        
        
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("away_points", Form=XmlSchemaForm.Unqualified)]
        public string Away_Points { get; set; }
        
        
        
        [XmlAttribute("home_points", Form=XmlSchemaForm.Unqualified)]
        public string Home_Points { get; set; }
        
        
        
        [XmlAttribute("points", Form=XmlSchemaForm.Unqualified)]
        public string Points { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayScoreTypePoints_After_Play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayScoreTypePoints_After_Play : IBaseGameEventAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public decimal Sequence { get; set; }
        
        
        
        [XmlAttribute("clock", Form=XmlSchemaForm.Unqualified)]
        public string Clock { get; set; }
        
        
        
        [XmlAttribute("wall_clock", Form=XmlSchemaForm.Unqualified, DataType="dateTime")]
        public DateTime Wall_Clock { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Wall_Clock-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Wall_Clock property is specified.</para>
        
        [XmlIgnore]
        public bool Wall_ClockSpecified { get; set; }
        
        
        
        [XmlAttribute("deleted", Form=XmlSchemaForm.Unqualified)]
        public bool Deleted { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Deleted-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Deleted property is specified.</para>
        
        [XmlIgnore]
        public bool DeletedSpecified { get; set; }
        
        
        
        [XmlAttribute("source", Form=XmlSchemaForm.Unqualified)]
        public string Source { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public PlayScoreTypePoints_After_PlayType Type { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayScoreTypePoints_After_PlayType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayScoreTypePoints_After_PlayType
    {
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point,
        
        
        
        [XmlEnum("conversion")]
        Conversion,
        
        
        
        [XmlEnum("penalty")]
        Penalty
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("playStatisticsType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsType
    {
        
        [XmlIgnore]
        private Collection<PlayStatisticsTypeTimeout> _timeout;
        
        
        
        [XmlElement("timeout", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeTimeout> Timeout
        {
            get => _timeout;
            private set => _timeout = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Timeout-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Timeout collection is empty.</para>
        
        [XmlIgnore]
        public bool TimeoutSpecified => Timeout.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="PlayStatisticsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="PlayStatisticsType" /> class.</para>
        
        public PlayStatisticsType()
        {
            _timeout = new Collection<PlayStatisticsTypeTimeout>();
            _misc = new Collection<PlayStatisticsTypeMisc>();
            _pass = new Collection<PlayStatisticsTypePass>();
            _receive = new Collection<PlayStatisticsTypeReceive>();
            _rush = new Collection<PlayStatisticsTypeRush>();
            _kick = new Collection<PlayStatisticsTypeKick>();
            _punt = new Collection<PlayStatisticsTypePunt>();
            _field_Goal = new Collection<PlayStatisticsTypeField_Goal>();
            _extra_Point = new Collection<PlayStatisticsTypeExtra_Point>();
            _penalty = new Collection<PlayStatisticsTypePenalty>();
            _fumble = new Collection<PlayStatisticsTypeFumble>();
            _return = new Collection<PlayStatisticsTypeReturn>();
            _block = new Collection<PlayStatisticsTypeBlock>();
            _defense = new Collection<PlayStatisticsTypeDefense>();
            _conversion = new Collection<PlayStatisticsTypeConversion>();
            _defense_Conversion = new Collection<PlayStatisticsTypeDefense_Conversion>();
            _down_Conversion = new Collection<PlayStatisticsTypeDown_Conversion>();
            _first_Down = new Collection<PlayStatisticsTypeFirst_Down>();
        }
        
        [XmlIgnore]
        private Collection<PlayStatisticsTypeMisc> _misc;
        
        
        
        [XmlElement("misc", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeMisc> Misc
        {
            get => _misc;
            private set => _misc = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Misc-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Misc collection is empty.</para>
        
        [XmlIgnore]
        public bool MiscSpecified => Misc.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypePass> _pass;
        
        
        
        [XmlElement("pass", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypePass> Pass
        {
            get => _pass;
            private set => _pass = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pass-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Pass collection is empty.</para>
        
        [XmlIgnore]
        public bool PassSpecified => Pass.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeReceive> _receive;
        
        
        
        [XmlElement("receive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeReceive> Receive
        {
            get => _receive;
            private set => _receive = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Receive-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Receive collection is empty.</para>
        
        [XmlIgnore]
        public bool ReceiveSpecified => Receive.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeRush> _rush;
        
        
        
        [XmlElement("rush", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeRush> Rush
        {
            get => _rush;
            private set => _rush = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Rush-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Rush collection is empty.</para>
        
        [XmlIgnore]
        public bool RushSpecified => Rush.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeKick> _kick;
        
        
        
        [XmlElement("kick", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeKick> Kick
        {
            get => _kick;
            private set => _kick = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Kick-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Kick collection is empty.</para>
        
        [XmlIgnore]
        public bool KickSpecified => Kick.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypePunt> _punt;
        
        
        
        [XmlElement("punt", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypePunt> Punt
        {
            get => _punt;
            private set => _punt = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Punt-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Punt collection is empty.</para>
        
        [XmlIgnore]
        public bool PuntSpecified => Punt.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeField_Goal> _field_Goal;
        
        
        
        [XmlElement("field_goal", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeField_Goal> Field_Goal
        {
            get => _field_Goal;
            private set => _field_Goal = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Field_Goal-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Field_Goal collection is empty.</para>
        
        [XmlIgnore]
        public bool Field_GoalSpecified => Field_Goal.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeExtra_Point> _extra_Point;
        
        
        
        [XmlElement("extra_point", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeExtra_Point> Extra_Point
        {
            get => _extra_Point;
            private set => _extra_Point = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Extra_Point-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Extra_Point collection is empty.</para>
        
        [XmlIgnore]
        public bool Extra_PointSpecified => Extra_Point.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypePenalty> _penalty;
        
        
        
        [XmlElement("penalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypePenalty> Penalty
        {
            get => _penalty;
            private set => _penalty = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Penalty-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Penalty collection is empty.</para>
        
        [XmlIgnore]
        public bool PenaltySpecified => Penalty.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeFumble> _fumble;
        
        
        
        [XmlElement("fumble", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeFumble> Fumble
        {
            get => _fumble;
            private set => _fumble = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fumble-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Fumble collection is empty.</para>
        
        [XmlIgnore]
        public bool FumbleSpecified => Fumble.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeReturn> _return;
        
        
        
        [XmlElement("return", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeReturn> Return
        {
            get => _return;
            private set => _return = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Return-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Return collection is empty.</para>
        
        [XmlIgnore]
        public bool ReturnSpecified => Return.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeBlock> _block;
        
        
        
        [XmlElement("block", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeBlock> Block
        {
            get => _block;
            private set => _block = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Block-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Block collection is empty.</para>
        
        [XmlIgnore]
        public bool BlockSpecified => Block.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeDefense> _defense;
        
        
        
        [XmlElement("defense", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeDefense> Defense
        {
            get => _defense;
            private set => _defense = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Defense-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Defense collection is empty.</para>
        
        [XmlIgnore]
        public bool DefenseSpecified => Defense.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeConversion> _conversion;
        
        
        
        [XmlElement("conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeConversion> Conversion
        {
            get => _conversion;
            private set => _conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool ConversionSpecified => Conversion.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeDefense_Conversion> _defense_Conversion;
        
        
        
        [XmlElement("defense_conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeDefense_Conversion> Defense_Conversion
        {
            get => _defense_Conversion;
            private set => _defense_Conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Defense_Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Defense_Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool Defense_ConversionSpecified => Defense_Conversion.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeDown_Conversion> _down_Conversion;
        
        
        
        [XmlElement("down_conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeDown_Conversion> Down_Conversion
        {
            get => _down_Conversion;
            private set => _down_Conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Down_Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Down_Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool Down_ConversionSpecified => Down_Conversion.Count != 0;

        [XmlIgnore]
        private Collection<PlayStatisticsTypeFirst_Down> _first_Down;
        
        
        
        [XmlElement("first_down", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayStatisticsTypeFirst_Down> First_Down
        {
            get => _first_Down;
            private set => _first_Down = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die First_Down-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the First_Down collection is empty.</para>
        
        [XmlIgnore]
        public bool First_DownSpecified => First_Down.Count != 0;
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeTimeout", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeTimeout : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("description", Form=XmlSchemaForm.Unqualified)]
        public string Description { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("basePlayStatisticType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlInclude(typeof(ExtPlayStatisticsTypeBlock))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeConversion))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeDefense))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeDefense_Conversion))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeDown_Conversion))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeExtra_Point))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeField_Goal))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeFirst_Down))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeFumble))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeKick))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeMisc))]
    [XmlInclude(typeof(ExtPlayStatisticsTypePass))]
    [XmlInclude(typeof(ExtPlayStatisticsTypePenalty))]
    [XmlInclude(typeof(ExtPlayStatisticsTypePunt))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeReceive))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeReturn))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeRush))]
    [XmlInclude(typeof(ExtPlayStatisticsTypeTimeout))]
    [XmlInclude(typeof(PlayStatisticsTypeBlock))]
    [XmlInclude(typeof(PlayStatisticsTypeConversion))]
    [XmlInclude(typeof(PlayStatisticsTypeDefense))]
    [XmlInclude(typeof(PlayStatisticsTypeDefense_Conversion))]
    [XmlInclude(typeof(PlayStatisticsTypeDown_Conversion))]
    [XmlInclude(typeof(PlayStatisticsTypeExtra_Point))]
    [XmlInclude(typeof(PlayStatisticsTypeField_Goal))]
    [XmlInclude(typeof(PlayStatisticsTypeFirst_Down))]
    [XmlInclude(typeof(PlayStatisticsTypeFumble))]
    [XmlInclude(typeof(PlayStatisticsTypeKick))]
    [XmlInclude(typeof(PlayStatisticsTypeMisc))]
    [XmlInclude(typeof(PlayStatisticsTypePass))]
    [XmlInclude(typeof(PlayStatisticsTypePenalty))]
    [XmlInclude(typeof(PlayStatisticsTypePunt))]
    [XmlInclude(typeof(PlayStatisticsTypeReceive))]
    [XmlInclude(typeof(PlayStatisticsTypeReturn))]
    [XmlInclude(typeof(PlayStatisticsTypeRush))]
    [XmlInclude(typeof(PlayStatisticsTypeTimeout))]
    public class BasePlayStatisticType
    {
        
        
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public TeamType Team { get; set; }
        
        
        
        [XmlElement("player", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public BasePlayStatisticTypePlayer Player { get; set; }
        
        
        
        [XmlAttribute("nullified", Form=XmlSchemaForm.Unqualified)]
        public bool Nullified { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Nullified-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Nullified property is specified.</para>
        
        [XmlIgnore]
        public bool NullifiedSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("BasePlayStatisticTypePlayer", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class BasePlayStatisticTypePlayer : IBasePlayerAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("jersey", Form=XmlSchemaForm.Unqualified)]
        public string Jersey { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("position", Form=XmlSchemaForm.Unqualified)]
        public IBasePlayerAttributesPosition Position { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Position-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Position property is specified.</para>
        
        [XmlIgnore]
        public bool PositionSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePlayerAttributes : IBaseIdentityAttributes
    {
        
        
        
        string Name
        {
            get;
            set;
        }
        
        
        
        string Jersey
        {
            get;
            set;
        }
        
        
        
        string Reference
        {
            get;
            set;
        }
        
        
        
        IBasePlayerAttributesPosition Position
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBasePlayerAttributesPosition", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IBasePlayerAttributesPosition
    {
        
        
        
        C,
        
        
        
        [XmlEnum("C/G")]
        CSlashG,
        
        
        
        CB,
        
        
        
        [XmlEnum("CB/RS")]
        CBSlashRS,
        
        
        
        [XmlEnum("CB/S")]
        CBSlashS,
        
        
        
        DB,
        
        
        
        DE,
        
        
        
        [XmlEnum("DE/LB")]
        DESlashLB,
        
        
        
        DL,
        
        
        
        DT,
        
        
        
        FB,
        
        
        
        [XmlEnum("FB/RB")]
        FBSlashRB,
        
        
        
        FS,
        
        
        
        G,
        
        
        
        [XmlEnum("G/C")]
        GSlashC,
        
        
        
        [XmlEnum("G/T")]
        GSlashT,
        
        
        
        [XmlEnum("H/B")]
        HSlashB,
        
        
        
        [XmlEnum("H/B/T")]
        HSlashBSlashT,
        
        
        
        HB,
        
        
        
        ILB,
        
        
        
        K,
        
        
        
        KR,
        
        
        
        L,
        
        
        
        LB,
        
        
        
        [XmlEnum("LB/DE")]
        LBSlashDE,
        
        
        
        LS,
        
        
        
        MLB,
        
        
        
        NT,
        
        
        
        OG,
        
        
        
        OL,
        
        
        
        OLB,
        
        
        
        OT,
        
        
        
        P,
        
        
        
        QB,
        
        
        
        [XmlEnum("QB/WR")]
        QBSlashWR,
        
        
        
        RB,
        
        
        
        [XmlEnum("RB/ST")]
        RBSlashST,
        
        
        
        [XmlEnum("RB/WR")]
        RBSlashWR,
        
        
        
        RS,
        
        
        
        S,
        
        
        
        SS,
        
        
        
        T,
        
        
        
        [XmlEnum("T/G")]
        TSlashG,
        
        
        
        TE,
        
        
        
        [XmlEnum("TE/DT")]
        TESlashDT,
        
        
        
        [XmlEnum("TE/FB")]
        TESlashFB,
        
        
        
        [XmlEnum("TE/LS")]
        TESlashLS,
        
        
        
        TEW,
        
        
        
        WR,
        
        
        
        [XmlEnum("WR/CB")]
        WRSlashCB,
        
        
        
        [XmlEnum("WR/KR")]
        WRSlashKR,
        
        
        
        [XmlEnum("WR/PR")]
        WRSlashPR,
        
        
        
        [XmlEnum("WR/RB")]
        WRSlashRB,
        
        
        
        [XmlEnum("WR/RS")]
        WRSlashRS
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeMisc", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeMisc : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypePass", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypePass : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("att_yards", Form=XmlSchemaForm.Unqualified)]
        public string Att_Yards { get; set; }
        
        
        
        [XmlAttribute("interception", Form=XmlSchemaForm.Unqualified)]
        public string Interception { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("sack", Form=XmlSchemaForm.Unqualified)]
        public string Sack { get; set; }
        
        
        
        [XmlAttribute("sack_yards", Form=XmlSchemaForm.Unqualified)]
        public double Sack_Yards { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Sack_Yards-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Sack_Yards property is specified.</para>
        
        [XmlIgnore]
        public bool Sack_YardsSpecified { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeReceive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeReceive : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("target", Form=XmlSchemaForm.Unqualified)]
        public string Target { get; set; }
        
        
        
        [XmlAttribute("reception", Form=XmlSchemaForm.Unqualified)]
        public string Reception { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("yards_after_catch", Form=XmlSchemaForm.Unqualified)]
        public string Yards_After_Catch { get; set; }
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("dropped", Form=XmlSchemaForm.Unqualified)]
        public string Dropped { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeRush", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeRush : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
        
        
        
        [XmlAttribute("lateral", Form=XmlSchemaForm.Unqualified)]
        public string Lateral { get; set; }
        
        
        
        [XmlAttribute("tlost", Form=XmlSchemaForm.Unqualified)]
        public string Tlost { get; set; }
        
        
        
        [XmlAttribute("tlost_yards", Form=XmlSchemaForm.Unqualified)]
        public string Tlost_Yards { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeKick", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeKick : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("gross_yards", Form=XmlSchemaForm.Unqualified)]
        public string Gross_Yards { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("endzone", Form=XmlSchemaForm.Unqualified)]
        public string Endzone { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("own_rec", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec { get; set; }
        
        
        
        [XmlAttribute("own_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Td { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypePunt", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypePunt : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("endzone", Form=XmlSchemaForm.Unqualified)]
        public string Endzone { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeField_Goal", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeField_Goal : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("att_yards", Form=XmlSchemaForm.Unqualified)]
        public string Att_Yards { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("missed", Form=XmlSchemaForm.Unqualified)]
        public string Missed { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("returned", Form=XmlSchemaForm.Unqualified)]
        public string Returned { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeExtra_Point", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeExtra_Point : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("missed", Form=XmlSchemaForm.Unqualified)]
        public string Missed { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("returned", Form=XmlSchemaForm.Unqualified)]
        public string Returned { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("aborted", Form=XmlSchemaForm.Unqualified)]
        public string Aborted { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypePenalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypePenalty : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("penalty", Form=XmlSchemaForm.Unqualified)]
        public string Penalty { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeFumble", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeFumble : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("forced", Form=XmlSchemaForm.Unqualified)]
        public string Forced { get; set; }
        
        
        
        [XmlAttribute("out_of_bounds", Form=XmlSchemaForm.Unqualified)]
        public string Out_Of_Bounds { get; set; }
        
        
        
        [XmlAttribute("own_rec", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec { get; set; }
        
        
        
        [XmlAttribute("opp_rec", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec { get; set; }
        
        
        
        [XmlAttribute("own_rec_yards", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Yards { get; set; }
        
        
        
        [XmlAttribute("opp_rec_yards", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec_Yards { get; set; }
        
        
        
        [XmlAttribute("own_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Td { get; set; }
        
        
        
        [XmlAttribute("opp_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec_Td { get; set; }
        
        
        
        [XmlAttribute("lost", Form=XmlSchemaForm.Unqualified)]
        public string Lost { get; set; }
        
        
        
        [XmlAttribute("play_category", Form=XmlSchemaForm.Unqualified)]
        public string Play_Category { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeReturn", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeReturn : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("return", Form=XmlSchemaForm.Unqualified)]
        public string Return { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("faircatch", Form=XmlSchemaForm.Unqualified)]
        public string Faircatch { get; set; }
        
        
        
        [XmlAttribute("out_of_bounds", Form=XmlSchemaForm.Unqualified)]
        public string Out_Of_Bounds { get; set; }
        
        
        
        [XmlAttribute("downed", Form=XmlSchemaForm.Unqualified)]
        public string Downed { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("lateral", Form=XmlSchemaForm.Unqualified)]
        public string Lateral { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public PlayStatisticsTypeReturnCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
        
        
        
        [XmlAttribute("play_category", Form=XmlSchemaForm.Unqualified)]
        public string Play_Category { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeReturnCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayStatisticsTypeReturnCategory
    {
        
        
        
        [XmlEnum("punt_return")]
        Punt_Return,
        
        
        
        [XmlEnum("kick_return")]
        Kick_Return
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeBlock", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeBlock : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("block", Form=XmlSchemaForm.Unqualified)]
        public string Block { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public PlayStatisticsTypeBlockCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeBlockCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayStatisticsTypeBlockCategory
    {
        
        
        
        [XmlEnum("field_goal")]
        Field_Goal,
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point,
        
        
        
        [XmlEnum("punt")]
        Punt
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeDefense", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeDefense : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("tackle", Form=XmlSchemaForm.Unqualified)]
        public string Tackle { get; set; }
        
        
        
        [XmlAttribute("ast_tackle", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Tackle { get; set; }
        
        
        
        [XmlAttribute("primary", Form=XmlSchemaForm.Unqualified)]
        public string Primary { get; set; }
        
        
        
        [XmlAttribute("sack", Form=XmlSchemaForm.Unqualified)]
        public string Sack { get; set; }
        
        
        
        [XmlAttribute("ast_sack", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Sack { get; set; }
        
        
        
        [XmlAttribute("sack_yards", Form=XmlSchemaForm.Unqualified)]
        public double Sack_Yards { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Sack_Yards-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Sack_Yards property is specified.</para>
        
        [XmlIgnore]
        public bool Sack_YardsSpecified { get; set; }
        
        
        
        [XmlAttribute("block", Form=XmlSchemaForm.Unqualified)]
        public string Block { get; set; }
        
        
        
        [XmlAttribute("pass_defended", Form=XmlSchemaForm.Unqualified)]
        public string Pass_Defended { get; set; }
        
        
        
        [XmlAttribute("qb_hit", Form=XmlSchemaForm.Unqualified)]
        public string Qb_Hit { get; set; }
        
        
        
        [XmlAttribute("interception", Form=XmlSchemaForm.Unqualified)]
        public string Interception { get; set; }
        
        
        
        [XmlAttribute("int_yards", Form=XmlSchemaForm.Unqualified)]
        public string Int_Yards { get; set; }
        
        
        
        [XmlAttribute("int_touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Int_Touchdown { get; set; }
        
        
        
        [XmlAttribute("tlost", Form=XmlSchemaForm.Unqualified)]
        public string Tlost { get; set; }
        
        
        
        [XmlAttribute("ast_tlost", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Tlost { get; set; }
        
        
        
        [XmlAttribute("tlost_yards", Form=XmlSchemaForm.Unqualified)]
        public string Tlost_Yards { get; set; }
        
        
        
        [XmlAttribute("forced_fumble", Form=XmlSchemaForm.Unqualified)]
        public string Forced_Fumble { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public string Category { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeConversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeConversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public PlayStatisticsTypeConversionCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeConversionCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayStatisticsTypeConversionCategory
    {
        
        
        
        [XmlEnum("pass")]
        Pass,
        
        
        
        [XmlEnum("receive")]
        Receive,
        
        
        
        [XmlEnum("rush")]
        Rush,
        
        
        
        [XmlEnum("turnover")]
        Turnover
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeDefense_Conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeDefense_Conversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public PlayStatisticsTypeDefense_ConversionCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeDefense_ConversionCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayStatisticsTypeDefense_ConversionCategory
    {
        
        
        
        [XmlEnum("conversion")]
        Conversion,
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeDown_Conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeDown_Conversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("down", Form=XmlSchemaForm.Unqualified)]
        public string Down { get; set; }
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeFirst_Down", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayStatisticsTypeFirst_Down : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public PlayStatisticsTypeFirst_DownCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayStatisticsTypeFirst_DownCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum PlayStatisticsTypeFirst_DownCategory
    {
        
        
        
        [XmlEnum("pass")]
        Pass,
        
        
        
        [XmlEnum("rush")]
        Rush,
        
        
        
        [XmlEnum("penalty")]
        Penalty
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameDriveType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameDriveType : IBaseDriveAttributes
    {
        
        [XmlIgnore]
        private Collection<GameEventType> _event;
        
        
        
        [XmlElement("event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GameEventType> Event
        {
            get => _event;
            private set => _event = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Event-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Event collection is empty.</para>
        
        [XmlIgnore]
        public bool EventSpecified => Event.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GameDriveType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GameDriveType" /> class.</para>
        
        public GameDriveType()
        {
            _event = new Collection<GameEventType>();
            _play = new Collection<GamePlayType>();
        }
        
        [XmlIgnore]
        private Collection<GamePlayType> _play;
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GamePlayType> Play
        {
            get => _play;
            private set => _play = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Play collection is empty.</para>
        
        [XmlIgnore]
        public bool PlaySpecified => Play.Count != 0;


        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("start_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason Start_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Start_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Start_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool Start_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("end_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason End_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die End_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the End_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool End_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("first_downs", Form=XmlSchemaForm.Unqualified)]
        public string First_Downs { get; set; }
        
        
        
        [XmlAttribute("gain", Form=XmlSchemaForm.Unqualified)]
        public string Gain { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public bool Inside_20 { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Inside_20-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Inside_20 property is specified.</para>
        
        [XmlIgnore]
        public bool Inside_20Specified { get; set; }
        
        
        
        [XmlAttribute("penalty_yards", Form=XmlSchemaForm.Unqualified)]
        public string Penalty_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
        
        
        
        [XmlAttribute("scoring_drive", Form=XmlSchemaForm.Unqualified)]
        public bool Scoring_Drive { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Scoring_Drive-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Scoring_Drive property is specified.</para>
        
        [XmlIgnore]
        public bool Scoring_DriveSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseDriveAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Sequence
        {
            get;
            set;
        }
        
        
        
        DriveReason Start_Reason
        {
            get;
            set;
        }
        
        
        
        DriveReason End_Reason
        {
            get;
            set;
        }
        
        
        
        string Duration
        {
            get;
            set;
        }
        
        
        
        string First_Downs
        {
            get;
            set;
        }
        
        
        
        string Gain
        {
            get;
            set;
        }
        
        
        
        bool Inside_20
        {
            get;
            set;
        }
        
        
        
        string Penalty_Yards
        {
            get;
            set;
        }
        
        
        
        string Play_Count
        {
            get;
            set;
        }
        
        
        
        bool Scoring_Drive
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("driveReason", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum DriveReason
    {
        
        
        
        UNKNOWN,
        
        
        
        Touchdown,
        
        
        
        Safety,
        
        
        
        [XmlEnum("Field Goal")]
        Field_Goal,
        
        
        
        [XmlEnum("Missed FG")]
        Missed_FG,
        
        
        
        [XmlEnum("Blocked FG")]
        Blocked_FG,
        
        
        
        [XmlEnum("Blocked FG, Downs")]
        Blocked_FGComma_Downs,
        
        
        
        [XmlEnum("Blocked FG, Safety")]
        Blocked_FGComma_Safety,
        
        
        
        Punt,
        
        
        
        [XmlEnum("Blocked Punt")]
        Blocked_Punt,
        
        
        
        [XmlEnum("Blocked Punt, Downs")]
        Blocked_PuntComma_Downs,
        
        
        
        [XmlEnum("Blocked Punt, Safety")]
        Blocked_PuntComma_Safety,
        
        
        
        Downs,
        
        
        
        Interception,
        
        
        
        Fumble,
        
        
        
        [XmlEnum("Fumble, Safety")]
        FumbleComma_Safety,
        
        
        
        [XmlEnum("Muffed FG")]
        Muffed_FG,
        
        
        
        [XmlEnum("Muffed Punt")]
        Muffed_Punt,
        
        
        
        [XmlEnum("Muffed Kickoff")]
        Muffed_Kickoff,
        
        
        
        Kickoff,
        
        
        
        [XmlEnum("Own Kickoff")]
        Own_Kickoff,
        
        
        
        [XmlEnum("Onside Kick")]
        Onside_Kick,
        
        
        
        [XmlEnum("Kickoff, No Play")]
        KickoffComma_No_Play,
        
        
        
        [XmlEnum("End of Half")]
        End_Of_Half,
        
        
        
        [XmlEnum("End of Game")]
        End_Of_Game
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("extPlayStatisticsType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsType
    {
        
        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeTimeout> _timeout;
        
        
        
        [XmlElement("timeout", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeTimeout> Timeout
        {
            get => _timeout;
            private set => _timeout = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Timeout-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Timeout collection is empty.</para>
        
        [XmlIgnore]
        public bool TimeoutSpecified => Timeout.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ExtPlayStatisticsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ExtPlayStatisticsType" /> class.</para>
        
        public ExtPlayStatisticsType()
        {
            _timeout = new Collection<ExtPlayStatisticsTypeTimeout>();
            _misc = new Collection<ExtPlayStatisticsTypeMisc>();
            _pass = new Collection<ExtPlayStatisticsTypePass>();
            _receive = new Collection<ExtPlayStatisticsTypeReceive>();
            _rush = new Collection<ExtPlayStatisticsTypeRush>();
            _kick = new Collection<ExtPlayStatisticsTypeKick>();
            _punt = new Collection<ExtPlayStatisticsTypePunt>();
            _field_Goal = new Collection<ExtPlayStatisticsTypeField_Goal>();
            _extra_Point = new Collection<ExtPlayStatisticsTypeExtra_Point>();
            _penalty = new Collection<ExtPlayStatisticsTypePenalty>();
            _fumble = new Collection<ExtPlayStatisticsTypeFumble>();
            _return = new Collection<ExtPlayStatisticsTypeReturn>();
            _block = new Collection<ExtPlayStatisticsTypeBlock>();
            _defense = new Collection<ExtPlayStatisticsTypeDefense>();
            _conversion = new Collection<ExtPlayStatisticsTypeConversion>();
            _defense_Conversion = new Collection<ExtPlayStatisticsTypeDefense_Conversion>();
            _down_Conversion = new Collection<ExtPlayStatisticsTypeDown_Conversion>();
            _first_Down = new Collection<ExtPlayStatisticsTypeFirst_Down>();
        }
        
        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeMisc> _misc;
        
        
        
        [XmlElement("misc", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeMisc> Misc
        {
            get => _misc;
            private set => _misc = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Misc-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Misc collection is empty.</para>
        
        [XmlIgnore]
        public bool MiscSpecified => Misc.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypePass> _pass;
        
        
        
        [XmlElement("pass", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypePass> Pass
        {
            get => _pass;
            private set => _pass = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pass-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Pass collection is empty.</para>
        
        [XmlIgnore]
        public bool PassSpecified => Pass.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeReceive> _receive;
        
        
        
        [XmlElement("receive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeReceive> Receive
        {
            get => _receive;
            private set => _receive = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Receive-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Receive collection is empty.</para>
        
        [XmlIgnore]
        public bool ReceiveSpecified => Receive.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeRush> _rush;
        
        
        
        [XmlElement("rush", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeRush> Rush
        {
            get => _rush;
            private set => _rush = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Rush-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Rush collection is empty.</para>
        
        [XmlIgnore]
        public bool RushSpecified => Rush.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeKick> _kick;
        
        
        
        [XmlElement("kick", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeKick> Kick
        {
            get => _kick;
            private set => _kick = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Kick-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Kick collection is empty.</para>
        
        [XmlIgnore]
        public bool KickSpecified => Kick.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypePunt> _punt;
        
        
        
        [XmlElement("punt", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypePunt> Punt
        {
            get => _punt;
            private set => _punt = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Punt-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Punt collection is empty.</para>
        
        [XmlIgnore]
        public bool PuntSpecified => Punt.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeField_Goal> _field_Goal;
        
        
        
        [XmlElement("field_goal", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeField_Goal> Field_Goal
        {
            get => _field_Goal;
            private set => _field_Goal = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Field_Goal-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Field_Goal collection is empty.</para>
        
        [XmlIgnore]
        public bool Field_GoalSpecified => Field_Goal.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeExtra_Point> _extra_Point;
        
        
        
        [XmlElement("extra_point", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeExtra_Point> Extra_Point
        {
            get => _extra_Point;
            private set => _extra_Point = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Extra_Point-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Extra_Point collection is empty.</para>
        
        [XmlIgnore]
        public bool Extra_PointSpecified => Extra_Point.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypePenalty> _penalty;
        
        
        
        [XmlElement("penalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypePenalty> Penalty
        {
            get => _penalty;
            private set => _penalty = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Penalty-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Penalty collection is empty.</para>
        
        [XmlIgnore]
        public bool PenaltySpecified => Penalty.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeFumble> _fumble;
        
        
        
        [XmlElement("fumble", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeFumble> Fumble
        {
            get => _fumble;
            private set => _fumble = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fumble-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Fumble collection is empty.</para>
        
        [XmlIgnore]
        public bool FumbleSpecified => Fumble.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeReturn> _return;
        
        
        
        [XmlElement("return", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeReturn> Return
        {
            get => _return;
            private set => _return = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Return-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Return collection is empty.</para>
        
        [XmlIgnore]
        public bool ReturnSpecified => Return.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeBlock> _block;
        
        
        
        [XmlElement("block", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeBlock> Block
        {
            get => _block;
            private set => _block = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Block-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Block collection is empty.</para>
        
        [XmlIgnore]
        public bool BlockSpecified => Block.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeDefense> _defense;
        
        
        
        [XmlElement("defense", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeDefense> Defense
        {
            get => _defense;
            private set => _defense = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Defense-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Defense collection is empty.</para>
        
        [XmlIgnore]
        public bool DefenseSpecified => Defense.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeConversion> _conversion;
        
        
        
        [XmlElement("conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeConversion> Conversion
        {
            get => _conversion;
            private set => _conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool ConversionSpecified => Conversion.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeDefense_Conversion> _defense_Conversion;
        
        
        
        [XmlElement("defense_conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeDefense_Conversion> Defense_Conversion
        {
            get => _defense_Conversion;
            private set => _defense_Conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Defense_Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Defense_Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool Defense_ConversionSpecified => Defense_Conversion.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeDown_Conversion> _down_Conversion;
        
        
        
        [XmlElement("down_conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeDown_Conversion> Down_Conversion
        {
            get => _down_Conversion;
            private set => _down_Conversion = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Down_Conversion-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Down_Conversion collection is empty.</para>
        
        [XmlIgnore]
        public bool Down_ConversionSpecified => Down_Conversion.Count != 0;

        [XmlIgnore]
        private Collection<ExtPlayStatisticsTypeFirst_Down> _first_Down;
        
        
        
        [XmlElement("first_down", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtPlayStatisticsTypeFirst_Down> First_Down
        {
            get => _first_Down;
            private set => _first_Down = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die First_Down-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the First_Down collection is empty.</para>
        
        [XmlIgnore]
        public bool First_DownSpecified => First_Down.Count != 0;
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeTimeout", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeTimeout : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("description", Form=XmlSchemaForm.Unqualified)]
        public string Description { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeMisc", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeMisc : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypePass", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypePass : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("att_yards", Form=XmlSchemaForm.Unqualified)]
        public string Att_Yards { get; set; }
        
        
        
        [XmlAttribute("interception", Form=XmlSchemaForm.Unqualified)]
        public string Interception { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("sack", Form=XmlSchemaForm.Unqualified)]
        public string Sack { get; set; }
        
        
        
        [XmlAttribute("sack_yards", Form=XmlSchemaForm.Unqualified)]
        public string Sack_Yards { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("incompletion_type", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypePassIncompletion_Type Incompletion_Type { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Incompletion_Type-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Incompletion_Type property is specified.</para>
        
        [XmlIgnore]
        public bool Incompletion_TypeSpecified { get; set; }
        
        
        
        [XmlAttribute("blitz", Form=XmlSchemaForm.Unqualified)]
        public string Blitz { get; set; }
        
        
        
        [XmlAttribute("hurry", Form=XmlSchemaForm.Unqualified)]
        public string Hurry { get; set; }
        
        
        
        [XmlAttribute("knockdown", Form=XmlSchemaForm.Unqualified)]
        public string Knockdown { get; set; }
        
        
        
        [XmlAttribute("pocket_time", Form=XmlSchemaForm.Unqualified)]
        public double Pocket_Time { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pocket_Time-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Pocket_Time property is specified.</para>
        
        [XmlIgnore]
        public bool Pocket_TimeSpecified { get; set; }
        
        
        
        [XmlAttribute("batted_pass", Form=XmlSchemaForm.Unqualified)]
        public string Batted_Pass { get; set; }
        
        
        
        [XmlAttribute("on_target_throw", Form=XmlSchemaForm.Unqualified)]
        public string On_Target_Throw { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypePassIncompletion_Type", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypePassIncompletion_Type
    {
        
        
        
        [XmlEnum("Thrown Away")]
        Thrown_Away,
        
        
        
        [XmlEnum("Pass Defended")]
        Pass_Defended,
        
        
        
        [XmlEnum("Dropped Pass")]
        Dropped_Pass,
        
        
        
        Spike,
        
        
        
        [XmlEnum("Poorly Thrown")]
        Poorly_Thrown
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeReceive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeReceive : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("target", Form=XmlSchemaForm.Unqualified)]
        public string Target { get; set; }
        
        
        
        [XmlAttribute("reception", Form=XmlSchemaForm.Unqualified)]
        public string Reception { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("yards_after_catch", Form=XmlSchemaForm.Unqualified)]
        public string Yards_After_Catch { get; set; }
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("dropped", Form=XmlSchemaForm.Unqualified)]
        public string Dropped { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
        
        
        
        [XmlAttribute("broken_tackles", Form=XmlSchemaForm.Unqualified)]
        public string Broken_Tackles { get; set; }
        
        
        
        [XmlAttribute("catchable", Form=XmlSchemaForm.Unqualified)]
        public string Catchable { get; set; }
        
        
        
        [XmlAttribute("yards_after_contact", Form=XmlSchemaForm.Unqualified)]
        public string Yards_After_Contact { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeRush", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeRush : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("goaltogo", Form=XmlSchemaForm.Unqualified)]
        public string Goaltogo { get; set; }
        
        
        
        [XmlAttribute("lateral", Form=XmlSchemaForm.Unqualified)]
        public string Lateral { get; set; }
        
        
        
        [XmlAttribute("tlost", Form=XmlSchemaForm.Unqualified)]
        public string Tlost { get; set; }
        
        
        
        [XmlAttribute("tlost_yards", Form=XmlSchemaForm.Unqualified)]
        public string Tlost_Yards { get; set; }
        
        
        
        [XmlAttribute("broken_tackles", Form=XmlSchemaForm.Unqualified)]
        public string Broken_Tackles { get; set; }
        
        
        
        [XmlAttribute("kneel_down", Form=XmlSchemaForm.Unqualified)]
        public string Kneel_Down { get; set; }
        
        
        
        [XmlAttribute("scramble", Form=XmlSchemaForm.Unqualified)]
        public string Scramble { get; set; }
        
        
        
        [XmlAttribute("yards_after_contact", Form=XmlSchemaForm.Unqualified)]
        public string Yards_After_Contact { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeKick", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeKick : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("gross_yards", Form=XmlSchemaForm.Unqualified)]
        public string Gross_Yards { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("endzone", Form=XmlSchemaForm.Unqualified)]
        public string Endzone { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("own_rec", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec { get; set; }
        
        
        
        [XmlAttribute("own_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Td { get; set; }
        
        
        
        [XmlAttribute("onside_attempt", Form=XmlSchemaForm.Unqualified)]
        public string Onside_Attempt { get; set; }
        
        
        
        [XmlAttribute("onside_success", Form=XmlSchemaForm.Unqualified)]
        public string Onside_Success { get; set; }
        
        
        
        [XmlAttribute("squib_kick", Form=XmlSchemaForm.Unqualified)]
        public string Squib_Kick { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypePunt", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypePunt : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public string Inside_20 { get; set; }
        
        
        
        [XmlAttribute("endzone", Form=XmlSchemaForm.Unqualified)]
        public string Endzone { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("hang_time", Form=XmlSchemaForm.Unqualified)]
        public double Hang_Time { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Hang_Time-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Hang_Time property is specified.</para>
        
        [XmlIgnore]
        public bool Hang_TimeSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeField_Goal", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeField_Goal : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("att_yards", Form=XmlSchemaForm.Unqualified)]
        public string Att_Yards { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("missed", Form=XmlSchemaForm.Unqualified)]
        public string Missed { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("returned", Form=XmlSchemaForm.Unqualified)]
        public string Returned { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeExtra_Point", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeExtra_Point : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("missed", Form=XmlSchemaForm.Unqualified)]
        public string Missed { get; set; }
        
        
        
        [XmlAttribute("blocked", Form=XmlSchemaForm.Unqualified)]
        public string Blocked { get; set; }
        
        
        
        [XmlAttribute("returned", Form=XmlSchemaForm.Unqualified)]
        public string Returned { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("aborted", Form=XmlSchemaForm.Unqualified)]
        public string Aborted { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypePenalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypePenalty : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("penalty", Form=XmlSchemaForm.Unqualified)]
        public string Penalty { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeFumble", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeFumble : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("fumble", Form=XmlSchemaForm.Unqualified)]
        public string Fumble { get; set; }
        
        
        
        [XmlAttribute("forced", Form=XmlSchemaForm.Unqualified)]
        public string Forced { get; set; }
        
        
        
        [XmlAttribute("out_of_bounds", Form=XmlSchemaForm.Unqualified)]
        public string Out_Of_Bounds { get; set; }
        
        
        
        [XmlAttribute("own_rec", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec { get; set; }
        
        
        
        [XmlAttribute("opp_rec", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec { get; set; }
        
        
        
        [XmlAttribute("own_rec_yards", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Yards { get; set; }
        
        
        
        [XmlAttribute("opp_rec_yards", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec_Yards { get; set; }
        
        
        
        [XmlAttribute("own_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Own_Rec_Td { get; set; }
        
        
        
        [XmlAttribute("opp_rec_td", Form=XmlSchemaForm.Unqualified)]
        public string Opp_Rec_Td { get; set; }
        
        
        
        [XmlAttribute("lost", Form=XmlSchemaForm.Unqualified)]
        public string Lost { get; set; }
        
        
        
        [XmlAttribute("play_category", Form=XmlSchemaForm.Unqualified)]
        public string Play_Category { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeReturn", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeReturn : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("return", Form=XmlSchemaForm.Unqualified)]
        public string Return { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Touchdown { get; set; }
        
        
        
        [XmlAttribute("firstdown", Form=XmlSchemaForm.Unqualified)]
        public string Firstdown { get; set; }
        
        
        
        [XmlAttribute("faircatch", Form=XmlSchemaForm.Unqualified)]
        public string Faircatch { get; set; }
        
        
        
        [XmlAttribute("out_of_bounds", Form=XmlSchemaForm.Unqualified)]
        public string Out_Of_Bounds { get; set; }
        
        
        
        [XmlAttribute("downed", Form=XmlSchemaForm.Unqualified)]
        public string Downed { get; set; }
        
        
        
        [XmlAttribute("touchback", Form=XmlSchemaForm.Unqualified)]
        public string Touchback { get; set; }
        
        
        
        [XmlAttribute("lateral", Form=XmlSchemaForm.Unqualified)]
        public string Lateral { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypeReturnCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
        
        
        
        [XmlAttribute("play_category", Form=XmlSchemaForm.Unqualified)]
        public string Play_Category { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeReturnCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypeReturnCategory
    {
        
        
        
        [XmlEnum("punt_return")]
        Punt_Return,
        
        
        
        [XmlEnum("kick_return")]
        Kick_Return
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeBlock", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeBlock : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("block", Form=XmlSchemaForm.Unqualified)]
        public string Block { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypeBlockCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeBlockCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypeBlockCategory
    {
        
        
        
        [XmlEnum("field_goal")]
        Field_Goal,
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point,
        
        
        
        [XmlEnum("punt")]
        Punt
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeDefense", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeDefense : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("tackle", Form=XmlSchemaForm.Unqualified)]
        public string Tackle { get; set; }
        
        
        
        [XmlAttribute("ast_tackle", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Tackle { get; set; }
        
        
        
        [XmlAttribute("primary", Form=XmlSchemaForm.Unqualified)]
        public string Primary { get; set; }
        
        
        
        [XmlAttribute("sack", Form=XmlSchemaForm.Unqualified)]
        public string Sack { get; set; }
        
        
        
        [XmlAttribute("ast_sack", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Sack { get; set; }
        
        
        
        [XmlAttribute("sack_yards", Form=XmlSchemaForm.Unqualified)]
        public double Sack_Yards { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Sack_Yards-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Sack_Yards property is specified.</para>
        
        [XmlIgnore]
        public bool Sack_YardsSpecified { get; set; }
        
        
        
        [XmlAttribute("block", Form=XmlSchemaForm.Unqualified)]
        public string Block { get; set; }
        
        
        
        [XmlAttribute("pass_defended", Form=XmlSchemaForm.Unqualified)]
        public string Pass_Defended { get; set; }
        
        
        
        [XmlAttribute("qb_hit", Form=XmlSchemaForm.Unqualified)]
        public string Qb_Hit { get; set; }
        
        
        
        [XmlAttribute("interception", Form=XmlSchemaForm.Unqualified)]
        public string Interception { get; set; }
        
        
        
        [XmlAttribute("int_yards", Form=XmlSchemaForm.Unqualified)]
        public string Int_Yards { get; set; }
        
        
        
        [XmlAttribute("int_touchdown", Form=XmlSchemaForm.Unqualified)]
        public string Int_Touchdown { get; set; }
        
        
        
        [XmlAttribute("tlost", Form=XmlSchemaForm.Unqualified)]
        public string Tlost { get; set; }
        
        
        
        [XmlAttribute("ast_tlost", Form=XmlSchemaForm.Unqualified)]
        public string Ast_Tlost { get; set; }
        
        
        
        [XmlAttribute("tlost_yards", Form=XmlSchemaForm.Unqualified)]
        public string Tlost_Yards { get; set; }
        
        
        
        [XmlAttribute("forced_fumble", Form=XmlSchemaForm.Unqualified)]
        public string Forced_Fumble { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public string Category { get; set; }
        
        
        
        [XmlAttribute("def_target", Form=XmlSchemaForm.Unqualified)]
        public string Def_Target { get; set; }
        
        
        
        [XmlAttribute("def_comp", Form=XmlSchemaForm.Unqualified)]
        public string Def_Comp { get; set; }
        
        
        
        [XmlAttribute("blitz", Form=XmlSchemaForm.Unqualified)]
        public string Blitz { get; set; }
        
        
        
        [XmlAttribute("hurry", Form=XmlSchemaForm.Unqualified)]
        public string Hurry { get; set; }
        
        
        
        [XmlAttribute("knockdown", Form=XmlSchemaForm.Unqualified)]
        public string Knockdown { get; set; }
        
        
        
        [XmlAttribute("missed_tackles", Form=XmlSchemaForm.Unqualified)]
        public string Missed_Tackles { get; set; }
        
        
        
        [XmlAttribute("batted_pass", Form=XmlSchemaForm.Unqualified)]
        public string Batted_Pass { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeConversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeConversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("safety", Form=XmlSchemaForm.Unqualified)]
        public string Safety { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypeConversionCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeConversionCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypeConversionCategory
    {
        
        
        
        [XmlEnum("pass")]
        Pass,
        
        
        
        [XmlEnum("receive")]
        Receive,
        
        
        
        [XmlEnum("rush")]
        Rush,
        
        
        
        [XmlEnum("turnover")]
        Turnover
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeDefense_Conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeDefense_Conversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypeDefense_ConversionCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeDefense_ConversionCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypeDefense_ConversionCategory
    {
        
        
        
        [XmlEnum("conversion")]
        Conversion,
        
        
        
        [XmlEnum("extra_point")]
        Extra_Point
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeDown_Conversion", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeDown_Conversion : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("down", Form=XmlSchemaForm.Unqualified)]
        public string Down { get; set; }
        
        
        
        [XmlAttribute("attempt", Form=XmlSchemaForm.Unqualified)]
        public string Attempt { get; set; }
        
        
        
        [XmlAttribute("complete", Form=XmlSchemaForm.Unqualified)]
        public string Complete { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeFirst_Down", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtPlayStatisticsTypeFirst_Down : BasePlayStatisticType
    {
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public ExtPlayStatisticsTypeFirst_DownCategory Category { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Category-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Category property is specified.</para>
        
        [XmlIgnore]
        public bool CategorySpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtPlayStatisticsTypeFirst_DownCategory", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum ExtPlayStatisticsTypeFirst_DownCategory
    {
        
        
        
        [XmlEnum("pass")]
        Pass,
        
        
        
        [XmlEnum("rush")]
        Rush,
        
        
        
        [XmlEnum("penalty")]
        Penalty
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("extGamePlayType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtGamePlayType : BasePlayType, IExtPlayAttributes
    {
        
        
        
        [XmlElement("drive-info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtGamePlayTypeDrive_Info Drive_Info { get; set; }
        
        
        
        [XmlElement("score", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayScoreType Score { get; set; }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtPlayStatisticsType Statistics { get; set; }
        
        
        
        [XmlAttribute("qb_at_snap", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesQb_At_Snap Qb_At_Snap { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Qb_At_Snap-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Qb_At_Snap property is specified.</para>
        
        [XmlIgnore]
        public bool Qb_At_SnapSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_punt", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Punt { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Punt-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Punt property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_PuntSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_field_goal", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Field_Goal { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Field_Goal-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Field_Goal property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_Field_GoalSpecified { get; set; }
        
        
        
        [XmlAttribute("players_rushed", Form=XmlSchemaForm.Unqualified)]
        public string Players_Rushed { get; set; }
        
        
        
        [XmlAttribute("men_in_box", Form=XmlSchemaForm.Unqualified)]
        public string Men_In_Box { get; set; }
        
        
        
        [XmlAttribute("play_direction", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPlay_Direction Play_Direction { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play_Direction-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Play_Direction property is specified.</para>
        
        [XmlIgnore]
        public bool Play_DirectionSpecified { get; set; }
        
        
        
        [XmlAttribute("left_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Left_Tightends { get; set; }
        
        
        
        [XmlAttribute("right_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Right_Tightends { get; set; }
        
        
        
        [XmlAttribute("hash_mark", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesHash_Mark Hash_Mark { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Hash_Mark-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Hash_Mark property is specified.</para>
        
        [XmlIgnore]
        public bool Hash_MarkSpecified { get; set; }
        
        
        
        [XmlAttribute("screen_pass", Form=XmlSchemaForm.Unqualified)]
        public bool Screen_Pass { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Screen_Pass-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Screen_Pass property is specified.</para>
        
        [XmlIgnore]
        public bool Screen_PassSpecified { get; set; }
        
        
        
        [XmlAttribute("pocket_location", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPocket_Location Pocket_Location { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pocket_Location-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Pocket_Location property is specified.</para>
        
        [XmlIgnore]
        public bool Pocket_LocationSpecified { get; set; }
        
        
        
        [XmlAttribute("blitz", Form=XmlSchemaForm.Unqualified)]
        public bool Blitz { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Blitz-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Blitz property is specified.</para>
        
        [XmlIgnore]
        public bool BlitzSpecified { get; set; }
        
        
        
        [XmlAttribute("huddle", Form=XmlSchemaForm.Unqualified)]
        public string Huddle { get; set; }
        
        
        
        [XmlAttribute("pass_route", Form=XmlSchemaForm.Unqualified)]
        public string Pass_Route { get; set; }
        
        
        
        [XmlAttribute("running_lane", Form=XmlSchemaForm.Unqualified)]
        public string Running_Lane { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ExtGamePlayTypeDrive_Info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtGamePlayTypeDrive_Info
    {
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IExtPlayAttributes
    {
        
        
        
        IExtPlayAttributesQb_At_Snap Qb_At_Snap
        {
            get;
            set;
        }
        
        
        
        bool Fake_Punt
        {
            get;
            set;
        }
        
        
        
        bool Fake_Field_Goal
        {
            get;
            set;
        }
        
        
        
        string Players_Rushed
        {
            get;
            set;
        }
        
        
        
        string Men_In_Box
        {
            get;
            set;
        }
        
        
        
        IExtPlayAttributesPlay_Direction Play_Direction
        {
            get;
            set;
        }
        
        
        
        string Left_Tightends
        {
            get;
            set;
        }
        
        
        
        string Right_Tightends
        {
            get;
            set;
        }
        
        
        
        IExtPlayAttributesHash_Mark Hash_Mark
        {
            get;
            set;
        }
        
        
        
        bool Screen_Pass
        {
            get;
            set;
        }
        
        
        
        IExtPlayAttributesPocket_Location Pocket_Location
        {
            get;
            set;
        }
        
        
        
        bool Blitz
        {
            get;
            set;
        }
        
        
        
        string Huddle
        {
            get;
            set;
        }
        
        
        
        string Pass_Route
        {
            get;
            set;
        }
        
        
        
        string Running_Lane
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IExtPlayAttributesQb_At_Snap", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IExtPlayAttributesQb_At_Snap
    {
        
        
        
        Shotgun,
        
        
        
        [XmlEnum("Under Center")]
        Under_Center,
        
        
        
        Pistol
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IExtPlayAttributesPlay_Direction", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IExtPlayAttributesPlay_Direction
    {
        
        
        
        [XmlEnum("Left Sideline")]
        Left_Sideline,
        
        
        
        Left,
        
        
        
        Middle,
        
        
        
        Right,
        
        
        
        [XmlEnum("Right Sideline")]
        Right_Sideline
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IExtPlayAttributesHash_Mark", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IExtPlayAttributesHash_Mark
    {
        
        
        
        [XmlEnum("Left Hash")]
        Left_Hash,
        
        
        
        [XmlEnum("Right Hash")]
        Right_Hash,
        
        
        
        Middle
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IExtPlayAttributesPocket_Location", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IExtPlayAttributesPocket_Location
    {
        
        
        
        [XmlEnum("Scramble Left")]
        Scramble_Left,
        
        
        
        [XmlEnum("Scramble Right")]
        Scramble_Right,
        
        
        
        [XmlEnum("Boot Left")]
        Boot_Left,
        
        
        
        [XmlEnum("Boot Right")]
        Boot_Right,
        
        
        
        [XmlEnum("Rollout Left")]
        Rollout_Left,
        
        
        
        [XmlEnum("Rollout Right")]
        Rollout_Right,
        
        
        
        Middle
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("extGameDriveType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtGameDriveType : IBaseDriveAttributes
    {
        
        [XmlIgnore]
        private Collection<GameEventType> _event;
        
        
        
        [XmlElement("event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GameEventType> Event
        {
            get => _event;
            private set => _event = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Event-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Event collection is empty.</para>
        
        [XmlIgnore]
        public bool EventSpecified => Event.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ExtGameDriveType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ExtGameDriveType" /> class.</para>
        
        public ExtGameDriveType()
        {
            _event = new Collection<GameEventType>();
            _play = new Collection<ExtGamePlayType>();
        }
        
        [XmlIgnore]
        private Collection<ExtGamePlayType> _play;
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ExtGamePlayType> Play
        {
            get => _play;
            private set => _play = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Play collection is empty.</para>
        
        [XmlIgnore]
        public bool PlaySpecified => Play.Count != 0;


        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("start_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason Start_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Start_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Start_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool Start_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("end_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason End_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die End_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the End_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool End_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("first_downs", Form=XmlSchemaForm.Unqualified)]
        public string First_Downs { get; set; }
        
        
        
        [XmlAttribute("gain", Form=XmlSchemaForm.Unqualified)]
        public string Gain { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public bool Inside_20 { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Inside_20-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Inside_20 property is specified.</para>
        
        [XmlIgnore]
        public bool Inside_20Specified { get; set; }
        
        
        
        [XmlAttribute("penalty_yards", Form=XmlSchemaForm.Unqualified)]
        public string Penalty_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
        
        
        
        [XmlAttribute("scoring_drive", Form=XmlSchemaForm.Unqualified)]
        public bool Scoring_Drive { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Scoring_Drive-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Scoring_Drive property is specified.</para>
        
        [XmlIgnore]
        public bool Scoring_DriveSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("playDetailsType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsType : IPlayDetailsAttributes
    {
        
        
        
        [XmlElement("description", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public string Description { get; set; }
        
        
        
        [XmlAttribute("description", Form=XmlSchemaForm.Unqualified)]
        public string Description_1 { get; set; }
        
        
        
        [XmlElement("start_location", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtFieldLocationType Start_Location { get; set; }
        
        
        
        [XmlElement("end_location", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtFieldLocationType End_Location { get; set; }
        
        
        
        [XmlElement("penalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayDetailsTypePenalty Penalty { get; set; }
        
        
        
        [XmlElement("recovery", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayDetailsTypeRecovery Recovery { get; set; }
        
        
        
        [XmlElement("review", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayDetailsTypeReview Review { get; set; }
        
        [XmlIgnore]
        private Collection<PlayDetailsTypePlayersPlayer> _players;
        
        
        
        [XmlArray("players", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("player", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsTypePlayersPlayer> Players
        {
            get => _players;
            private set => _players = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Players-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Players collection is empty.</para>
        
        [XmlIgnore]
        public bool PlayersSpecified => Players.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="PlayDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="PlayDetailsType" /> class.</para>
        
        public PlayDetailsType()
        {
            _players = new Collection<PlayDetailsTypePlayersPlayer>();
        }
        
        
        
        [XmlAttribute("direction", Form=XmlSchemaForm.Unqualified)]
        public string Direction { get; set; }
        
        
        
        [XmlAttribute("category", Form=XmlSchemaForm.Unqualified)]
        public string Category { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("result", Form=XmlSchemaForm.Unqualified)]
        public string Result { get; set; }
        
        
        
        [XmlAttribute("onside", Form=XmlSchemaForm.Unqualified)]
        public string Onside { get; set; }
        
        
        
        [XmlAttribute("sack_split", Form=XmlSchemaForm.Unqualified)]
        public string Sack_Split { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("extFieldLocationType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ExtFieldLocationType
    {
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("yardline", Form=XmlSchemaForm.Unqualified)]
        public string Yardline { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayDetailsTypePenalty", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsTypePenalty
    {
        
        
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public TeamType Team { get; set; }
        
        
        
        [XmlAttribute("description", Form=XmlSchemaForm.Unqualified)]
        public string Description { get; set; }
        
        
        
        [XmlAttribute("result", Form=XmlSchemaForm.Unqualified)]
        public string Result { get; set; }
        
        
        
        [XmlAttribute("yards", Form=XmlSchemaForm.Unqualified)]
        public string Yards { get; set; }
        
        
        
        [XmlAttribute("no_play", Form=XmlSchemaForm.Unqualified)]
        public string No_Play { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayDetailsTypeRecovery", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsTypeRecovery
    {
        
        
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public TeamType Team { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
        
        
        
        [XmlAttribute("first_touch", Form=XmlSchemaForm.Unqualified)]
        public string First_Touch { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayDetailsTypeReview", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsTypeReview
    {
        
        
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public TeamType Team { get; set; }
        
        
        
        [XmlAttribute("type", Form=XmlSchemaForm.Unqualified)]
        public string Type { get; set; }
        
        
        
        [XmlAttribute("result", Form=XmlSchemaForm.Unqualified)]
        public string Result { get; set; }
        
        
        
        [XmlAttribute("reversed", Form=XmlSchemaForm.Unqualified)]
        public string Reversed { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayDetailsTypePlayers", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsTypePlayers
    {
        
        [XmlIgnore]
        private Collection<PlayDetailsTypePlayersPlayer> _player;
        
        
        
        [XmlElement("player", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsTypePlayersPlayer> Player
        {
            get => _player;
            private set => _player = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Player-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Player collection is empty.</para>
        
        [XmlIgnore]
        public bool PlayerSpecified => Player.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="PlayDetailsTypePlayers" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="PlayDetailsTypePlayers" /> class.</para>
        
        public PlayDetailsTypePlayers()
        {
            _player = new Collection<PlayDetailsTypePlayersPlayer>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("PlayDetailsTypePlayersPlayer", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PlayDetailsTypePlayersPlayer : IBasePlayerAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("jersey", Form=XmlSchemaForm.Unqualified)]
        public string Jersey { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
        
        
        
        [XmlAttribute("position", Form=XmlSchemaForm.Unqualified)]
        public IBasePlayerAttributesPosition Position { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Position-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Position property is specified.</para>
        
        [XmlIgnore]
        public bool PositionSpecified { get; set; }
        
        
        
        [XmlAttribute("role", Form=XmlSchemaForm.Unqualified)]
        public string Role { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IPlayDetailsAttributes
    {
        
        
        
        string Direction
        {
            get;
            set;
        }
        
        
        
        string Category
        {
            get;
            set;
        }
        
        
        
        string Description
        {
            get;
            set;
        }
        
        
        
        string Sequence
        {
            get;
            set;
        }
        
        
        
        string Yards
        {
            get;
            set;
        }
        
        
        
        string Result
        {
            get;
            set;
        }
        
        
        
        string Onside
        {
            get;
            set;
        }
        
        
        
        string Sack_Split
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gamePlayDetailsType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayDetailsType : BasePlayType, IExtPlayAttributes
    {
        
        
        
        [XmlElement("drive-info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayDetailsTypeDrive_Info Drive_Info { get; set; }
        
        
        
        [XmlElement("score", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayScoreType Score { get; set; }
        
        
        
        [XmlElement("quarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayDetailsTypeQuarter Quarter { get; set; }
        
        
        
        [XmlElement("overtime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayDetailsTypeOvertime Overtime { get; set; }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtPlayStatisticsType Statistics { get; set; }
        
        [XmlIgnore]
        private Collection<PlayDetailsType> _details;
        
        
        
        [XmlArray("details", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("detail", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsType> Details
        {
            get => _details;
            private set => _details = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Details-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Details collection is empty.</para>
        
        [XmlIgnore]
        public bool DetailsSpecified => Details.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GamePlayDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GamePlayDetailsType" /> class.</para>
        
        public GamePlayDetailsType()
        {
            _details = new Collection<PlayDetailsType>();
        }
        
        
        
        [XmlAttribute("qb_at_snap", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesQb_At_Snap Qb_At_Snap { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Qb_At_Snap-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Qb_At_Snap property is specified.</para>
        
        [XmlIgnore]
        public bool Qb_At_SnapSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_punt", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Punt { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Punt-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Punt property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_PuntSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_field_goal", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Field_Goal { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Field_Goal-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Field_Goal property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_Field_GoalSpecified { get; set; }
        
        
        
        [XmlAttribute("players_rushed", Form=XmlSchemaForm.Unqualified)]
        public string Players_Rushed { get; set; }
        
        
        
        [XmlAttribute("men_in_box", Form=XmlSchemaForm.Unqualified)]
        public string Men_In_Box { get; set; }
        
        
        
        [XmlAttribute("play_direction", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPlay_Direction Play_Direction { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play_Direction-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Play_Direction property is specified.</para>
        
        [XmlIgnore]
        public bool Play_DirectionSpecified { get; set; }
        
        
        
        [XmlAttribute("left_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Left_Tightends { get; set; }
        
        
        
        [XmlAttribute("right_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Right_Tightends { get; set; }
        
        
        
        [XmlAttribute("hash_mark", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesHash_Mark Hash_Mark { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Hash_Mark-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Hash_Mark property is specified.</para>
        
        [XmlIgnore]
        public bool Hash_MarkSpecified { get; set; }
        
        
        
        [XmlAttribute("screen_pass", Form=XmlSchemaForm.Unqualified)]
        public bool Screen_Pass { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Screen_Pass-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Screen_Pass property is specified.</para>
        
        [XmlIgnore]
        public bool Screen_PassSpecified { get; set; }
        
        
        
        [XmlAttribute("pocket_location", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPocket_Location Pocket_Location { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pocket_Location-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Pocket_Location property is specified.</para>
        
        [XmlIgnore]
        public bool Pocket_LocationSpecified { get; set; }
        
        
        
        [XmlAttribute("blitz", Form=XmlSchemaForm.Unqualified)]
        public bool Blitz { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Blitz-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Blitz property is specified.</para>
        
        [XmlIgnore]
        public bool BlitzSpecified { get; set; }
        
        
        
        [XmlAttribute("huddle", Form=XmlSchemaForm.Unqualified)]
        public string Huddle { get; set; }
        
        
        
        [XmlAttribute("pass_route", Form=XmlSchemaForm.Unqualified)]
        public string Pass_Route { get; set; }
        
        
        
        [XmlAttribute("running_lane", Form=XmlSchemaForm.Unqualified)]
        public string Running_Lane { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayDetailsTypeDrive_Info", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayDetailsTypeDrive_Info
    {
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("net_yards", Form=XmlSchemaForm.Unqualified)]
        public string Net_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayDetailsTypeQuarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayDetailsTypeQuarter : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBasePeriodAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Number
        {
            get;
            set;
        }
        
        
        
        string Sequence
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayDetailsTypeOvertime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayDetailsTypeOvertime : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayDetailsTypeDetails", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayDetailsTypeDetails
    {
        
        [XmlIgnore]
        private Collection<PlayDetailsType> _detail;
        
        
        
        [XmlElement("detail", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsType> Detail
        {
            get => _detail;
            private set => _detail = value;
        }
        
        
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GamePlayDetailsTypeDetails" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GamePlayDetailsTypeDetails" /> class.</para>
        
        public GamePlayDetailsTypeDetails()
        {
            _detail = new Collection<PlayDetailsType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gameDetailsDriveType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameDetailsDriveType : IBaseDriveAttributes
    {
        
        [XmlIgnore]
        private Collection<GameEventType> _event;
        
        
        
        [XmlElement("event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GameEventType> Event
        {
            get => _event;
            private set => _event = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Event-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Event collection is empty.</para>
        
        [XmlIgnore]
        public bool EventSpecified => Event.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GameDetailsDriveType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GameDetailsDriveType" /> class.</para>
        
        public GameDetailsDriveType()
        {
            _event = new Collection<GameEventType>();
            _play = new Collection<GamePlayDetailsType>();
        }
        
        [XmlIgnore]
        private Collection<GamePlayDetailsType> _play;
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GamePlayDetailsType> Play
        {
            get => _play;
            private set => _play = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Play collection is empty.</para>
        
        [XmlIgnore]
        public bool PlaySpecified => Play.Count != 0;


        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("start_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason Start_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Start_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Start_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool Start_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("end_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason End_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die End_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the End_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool End_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("first_downs", Form=XmlSchemaForm.Unqualified)]
        public string First_Downs { get; set; }
        
        
        
        [XmlAttribute("gain", Form=XmlSchemaForm.Unqualified)]
        public string Gain { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public bool Inside_20 { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Inside_20-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Inside_20 property is specified.</para>
        
        [XmlIgnore]
        public bool Inside_20Specified { get; set; }
        
        
        
        [XmlAttribute("penalty_yards", Form=XmlSchemaForm.Unqualified)]
        public string Penalty_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
        
        
        
        [XmlAttribute("scoring_drive", Form=XmlSchemaForm.Unqualified)]
        public bool Scoring_Drive { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Scoring_Drive-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Scoring_Drive property is specified.</para>
        
        [XmlIgnore]
        public bool Scoring_DriveSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("coinTossType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class CoinTossType
    {
        
        
        
        [XmlElement("home", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public CoinTossTypeHome Home { get; set; }
        
        
        
        [XmlElement("away", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public CoinTossTypeAway Away { get; set; }
        
        
        
        [XmlAttribute("quarter", Form=XmlSchemaForm.Unqualified)]
        public string Quarter { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("CoinTossTypeHome", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class CoinTossTypeHome : ICoinTossAttributes
    {
        
        
        
        [XmlAttribute("outcome", Form=XmlSchemaForm.Unqualified)]
        public string Outcome { get; set; }
        
        
        
        [XmlAttribute("decision", Form=XmlSchemaForm.Unqualified)]
        public string Decision { get; set; }
        
        
        
        [XmlAttribute("direction", Form=XmlSchemaForm.Unqualified)]
        public string Direction { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface ICoinTossAttributes
    {
        
        
        
        string Outcome
        {
            get;
            set;
        }
        
        
        
        string Decision
        {
            get;
            set;
        }
        
        
        
        string Direction
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("CoinTossTypeAway", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class CoinTossTypeAway : ICoinTossAttributes
    {
        
        
        
        [XmlAttribute("outcome", Form=XmlSchemaForm.Unqualified)]
        public string Outcome { get; set; }
        
        
        
        [XmlAttribute("decision", Form=XmlSchemaForm.Unqualified)]
        public string Decision { get; set; }
        
        
        
        [XmlAttribute("direction", Form=XmlSchemaForm.Unqualified)]
        public string Direction { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeLast_Event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeLast_Event
    {
        
        
        
        [XmlElement("event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GameEventType Event { get; set; }
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayDetailsType Play { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeScoring", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeScoring
    {
        
        [XmlIgnore]
        private Collection<PeriodType> _quarter;
        
        
        
        [XmlElement("quarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PeriodType> Quarter
        {
            get => _quarter;
            private set => _quarter = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Quarter-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Quarter collection is empty.</para>
        
        [XmlIgnore]
        public bool QuarterSpecified => Quarter.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GameTypeScoring" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GameTypeScoring" /> class.</para>
        
        public GameTypeScoring()
        {
            _quarter = new Collection<PeriodType>();
            _overtime = new Collection<PeriodType>();
        }
        
        [XmlIgnore]
        private Collection<PeriodType> _overtime;
        
        
        
        [XmlElement("overtime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PeriodType> Overtime
        {
            get => _overtime;
            private set => _overtime = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Overtime-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Overtime collection is empty.</para>
        
        [XmlIgnore]
        public bool OvertimeSpecified => Overtime.Count != 0;
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("periodType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class PeriodType : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("away_points", Form=XmlSchemaForm.Unqualified)]
        public string Away_Points { get; set; }
        
        
        
        [XmlAttribute("home_points", Form=XmlSchemaForm.Unqualified)]
        public string Home_Points { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeScoring_Drives", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeScoring_Drives
    {
        
        [XmlIgnore]
        private Collection<ScoringDriveType> _drive;
        
        
        
        [XmlElement("drive", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<ScoringDriveType> Drive
        {
            get => _drive;
            private set => _drive = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Drive-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Drive collection is empty.</para>
        
        [XmlIgnore]
        public bool DriveSpecified => Drive.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GameTypeScoring_Drives" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GameTypeScoring_Drives" /> class.</para>
        
        public GameTypeScoring_Drives()
        {
            _drive = new Collection<ScoringDriveType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("scoringDriveType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringDriveType : IBaseDriveAttributes
    {
        
        
        
        [XmlElement("quarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ScoringDriveTypeQuarter Quarter { get; set; }
        
        
        
        [XmlElement("overtime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ScoringDriveTypeOvertime Overtime { get; set; }
        
        
        
        [XmlElement("team", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ScoringDriveTypeTeam Team { get; set; }
        
        
        
        [XmlElement("plays", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ScoringDriveTypePlays Plays { get; set; }
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
        
        
        
        [XmlAttribute("start_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason Start_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Start_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Start_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool Start_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("end_reason", Form=XmlSchemaForm.Unqualified)]
        public DriveReason End_Reason { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die End_Reason-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the End_Reason property is specified.</para>
        
        [XmlIgnore]
        public bool End_ReasonSpecified { get; set; }
        
        
        
        [XmlAttribute("duration", Form=XmlSchemaForm.Unqualified)]
        public string Duration { get; set; }
        
        
        
        [XmlAttribute("first_downs", Form=XmlSchemaForm.Unqualified)]
        public string First_Downs { get; set; }
        
        
        
        [XmlAttribute("gain", Form=XmlSchemaForm.Unqualified)]
        public string Gain { get; set; }
        
        
        
        [XmlAttribute("inside_20", Form=XmlSchemaForm.Unqualified)]
        public bool Inside_20 { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Inside_20-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Inside_20 property is specified.</para>
        
        [XmlIgnore]
        public bool Inside_20Specified { get; set; }
        
        
        
        [XmlAttribute("penalty_yards", Form=XmlSchemaForm.Unqualified)]
        public string Penalty_Yards { get; set; }
        
        
        
        [XmlAttribute("play_count", Form=XmlSchemaForm.Unqualified)]
        public string Play_Count { get; set; }
        
        
        
        [XmlAttribute("scoring_drive", Form=XmlSchemaForm.Unqualified)]
        public bool Scoring_Drive { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Scoring_Drive-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Scoring_Drive property is specified.</para>
        
        [XmlIgnore]
        public bool Scoring_DriveSpecified { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ScoringDriveTypeQuarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringDriveTypeQuarter : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ScoringDriveTypeOvertime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringDriveTypeOvertime : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ScoringDriveTypeTeam", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringDriveTypeTeam : IBaseTeamAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("sr_id", Form=XmlSchemaForm.Unqualified)]
        public string Sr_Id { get; set; }
        
        
        
        [XmlAttribute("alias", Form=XmlSchemaForm.Unqualified)]
        public string Alias { get; set; }
        
        
        
        [XmlAttribute("name", Form=XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
        
        
        
        [XmlAttribute("market", Form=XmlSchemaForm.Unqualified)]
        public string Market { get; set; }
        
        
        
        [XmlAttribute("founded", Form=XmlSchemaForm.Unqualified)]
        public string Founded { get; set; }
        
        
        
        [XmlAttribute("reference", Form=XmlSchemaForm.Unqualified)]
        public string Reference { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("ScoringDriveTypePlays", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class ScoringDriveTypePlays
    {
        
        [XmlIgnore]
        private Collection<GameEventType> _event;
        
        
        
        [XmlElement("event", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GameEventType> Event
        {
            get => _event;
            private set => _event = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Event-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Event collection is empty.</para>
        
        [XmlIgnore]
        public bool EventSpecified => Event.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ScoringDriveTypePlays" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ScoringDriveTypePlays" /> class.</para>
        
        public ScoringDriveTypePlays()
        {
            _event = new Collection<GameEventType>();
            _play = new Collection<GamePlayDetailsType>();
        }
        
        [XmlIgnore]
        private Collection<GamePlayDetailsType> _play;
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GamePlayDetailsType> Play
        {
            get => _play;
            private set => _play = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Play collection is empty.</para>
        
        [XmlIgnore]
        public bool PlaySpecified => Play.Count != 0;
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GameTypeScoring_Plays", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GameTypeScoring_Plays
    {
        
        [XmlIgnore]
        private Collection<GamePlayScoresType> _play;
        
        
        
        [XmlElement("play", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<GamePlayScoresType> Play
        {
            get => _play;
            private set => _play = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Play collection is empty.</para>
        
        [XmlIgnore]
        public bool PlaySpecified => Play.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GameTypeScoring_Plays" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GameTypeScoring_Plays" /> class.</para>
        
        public GameTypeScoring_Plays()
        {
            _play = new Collection<GamePlayScoresType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("gamePlayScoresType", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayScoresType : BasePlayType, IExtPlayAttributes
    {
        
        
        
        [XmlElement("score", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public PlayScoreType Score { get; set; }
        
        
        
        [XmlElement("quarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayScoresTypeQuarter Quarter { get; set; }
        
        
        
        [XmlElement("overtime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public GamePlayScoresTypeOvertime Overtime { get; set; }
        
        
        
        [XmlElement("statistics", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public ExtPlayStatisticsType Statistics { get; set; }
        
        [XmlIgnore]
        private Collection<PlayDetailsType> _details;
        
        
        
        [XmlArray("details", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        [XmlArrayItem("detail", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsType> Details
        {
            get => _details;
            private set => _details = value;
        }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Details-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Details collection is empty.</para>
        
        [XmlIgnore]
        public bool DetailsSpecified => Details.Count != 0;


        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GamePlayScoresType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GamePlayScoresType" /> class.</para>
        
        public GamePlayScoresType()
        {
            _details = new Collection<PlayDetailsType>();
        }
        
        
        
        [XmlAttribute("qb_at_snap", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesQb_At_Snap Qb_At_Snap { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Qb_At_Snap-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Qb_At_Snap property is specified.</para>
        
        [XmlIgnore]
        public bool Qb_At_SnapSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_punt", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Punt { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Punt-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Punt property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_PuntSpecified { get; set; }
        
        
        
        [XmlAttribute("fake_field_goal", Form=XmlSchemaForm.Unqualified)]
        public bool Fake_Field_Goal { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fake_Field_Goal-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Fake_Field_Goal property is specified.</para>
        
        [XmlIgnore]
        public bool Fake_Field_GoalSpecified { get; set; }
        
        
        
        [XmlAttribute("players_rushed", Form=XmlSchemaForm.Unqualified)]
        public string Players_Rushed { get; set; }
        
        
        
        [XmlAttribute("men_in_box", Form=XmlSchemaForm.Unqualified)]
        public string Men_In_Box { get; set; }
        
        
        
        [XmlAttribute("play_direction", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPlay_Direction Play_Direction { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Play_Direction-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Play_Direction property is specified.</para>
        
        [XmlIgnore]
        public bool Play_DirectionSpecified { get; set; }
        
        
        
        [XmlAttribute("left_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Left_Tightends { get; set; }
        
        
        
        [XmlAttribute("right_tightends", Form=XmlSchemaForm.Unqualified)]
        public string Right_Tightends { get; set; }
        
        
        
        [XmlAttribute("hash_mark", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesHash_Mark Hash_Mark { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Hash_Mark-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Hash_Mark property is specified.</para>
        
        [XmlIgnore]
        public bool Hash_MarkSpecified { get; set; }
        
        
        
        [XmlAttribute("screen_pass", Form=XmlSchemaForm.Unqualified)]
        public bool Screen_Pass { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Screen_Pass-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Screen_Pass property is specified.</para>
        
        [XmlIgnore]
        public bool Screen_PassSpecified { get; set; }
        
        
        
        [XmlAttribute("pocket_location", Form=XmlSchemaForm.Unqualified)]
        public IExtPlayAttributesPocket_Location Pocket_Location { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Pocket_Location-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Pocket_Location property is specified.</para>
        
        [XmlIgnore]
        public bool Pocket_LocationSpecified { get; set; }
        
        
        
        [XmlAttribute("blitz", Form=XmlSchemaForm.Unqualified)]
        public bool Blitz { get; set; }
        
        
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Blitz-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Blitz property is specified.</para>
        
        [XmlIgnore]
        public bool BlitzSpecified { get; set; }
        
        
        
        [XmlAttribute("huddle", Form=XmlSchemaForm.Unqualified)]
        public string Huddle { get; set; }
        
        
        
        [XmlAttribute("pass_route", Form=XmlSchemaForm.Unqualified)]
        public string Pass_Route { get; set; }
        
        
        
        [XmlAttribute("running_lane", Form=XmlSchemaForm.Unqualified)]
        public string Running_Lane { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayScoresTypeQuarter", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayScoresTypeQuarter : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayScoresTypeOvertime", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayScoresTypeOvertime : IBasePeriodAttributes
    {
        
        
        
        [XmlAttribute("id", Form=XmlSchemaForm.Unqualified)]
        public string Id { get; set; }
        
        
        
        [XmlAttribute("number", Form=XmlSchemaForm.Unqualified)]
        public string Number { get; set; }
        
        
        
        [XmlAttribute("sequence", Form=XmlSchemaForm.Unqualified)]
        public string Sequence { get; set; }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("GamePlayScoresTypeDetails", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd", AnonymousType=true)]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class GamePlayScoresTypeDetails
    {
        
        [XmlIgnore]
        private Collection<PlayDetailsType> _detail;
        
        
        
        [XmlElement("detail", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
        public Collection<PlayDetailsType> Detail
        {
            get => _detail;
            private set => _detail = value;
        }
        
        
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="GamePlayScoresTypeDetails" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="GamePlayScoresTypeDetails" /> class.</para>
        
        public GamePlayScoresTypeDetails()
        {
            _detail = new Collection<PlayDetailsType>();
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IBaseGameAttributes
    {
        
        
        
        string Id
        {
            get;
            set;
        }
        
        
        
        string Sr_Id
        {
            get;
            set;
        }
        
        
        
        string Number
        {
            get;
            set;
        }
        
        
        
        string Utc_Offset
        {
            get;
            set;
        }
        
        
        
        string Reference
        {
            get;
            set;
        }
        
        
        
        DateTime Scheduled
        {
            get;
            set;
        }
        
        
        
        IBaseGameAttributesStatus Status
        {
            get;
            set;
        }
        
        
        
        string Attendance
        {
            get;
            set;
        }
        
        
        
        string Weather
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IBaseGameAttributesStatus", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
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
        
        
        
        [XmlEnum("time-tbd")]
        Time_Tbd,
        
        
        
        [XmlEnum("flex-schedule")]
        Flex_Schedule
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    public interface IExtGameAttributes
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
        
        
        
        IExtGameAttributesEntry_Mode Entry_Mode
        {
            get;
            set;
        }
    }
    
    
    
    [GeneratedCode("XmlSchemaClassGenerator", "2.0.0.0")]
    [Serializable]
    [XmlType("IExtGameAttributesEntry_Mode", Namespace="http://feed.elasticstats.com/schema/nfl/premium/boxscore-v5.0.xsd")]
    public enum IExtGameAttributesEntry_Mode
    {
        
        
        
        INGEST,
        
        
        
        LDE
    }
}
