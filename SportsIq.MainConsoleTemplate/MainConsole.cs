using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using SportsIq.Games;
using SportsIq.Models.GamesDto;
using SportsIq.PubSub;
using SportsIq.SportsRadar;
using SportsIq.SqlDataAccess;

namespace SportsIq.MainConsoleTemplate
{
    public interface IMainConsole
    {
        void Start();
        void Stop();
    }

    public class MainConsole<TGame, TGameDto, TRadar, TDataAccess> : IMainConsole
        where TGame : IGame
        where TGameDto : IGameDto
        where TRadar : IRadarBase
        where TDataAccess : IDataAccessBase<TGameDto>
    {
        #region Fields and Constructors

        private TRadar _radar;
        private readonly IPubSubUtil _pubSubUtil;
        private readonly TDataAccess _dataAccess;
        private readonly string _subscriptionId;
        private readonly bool _isTeamMode;
        private readonly bool _isSimulation;
        private readonly int _numberGameDays;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        private CountdownEvent _loadCompleteCountdownEvent;
        private readonly ConcurrentBag<TGame> _gamesConcurrentBag;
        private List<Task> _gamesTaskList;

        public MainConsole(TDataAccess dataAccess, TRadar radar, IPubSubUtil pubSubUtil)
        {
            _subscriptionId = ConfigurationManager.AppSettings["subscriptionId"];
            string isTeamModeString = ConfigurationManager.AppSettings["isTeamMode"];
            _isTeamMode = Convert.ToBoolean(isTeamModeString);
            string isSimulationString = ConfigurationManager.AppSettings["isSimulation"];
            _isSimulation = Convert.ToBoolean(isSimulationString);
            string numberGameDaysString = ConfigurationManager.AppSettings["numberGameDays"];
            _numberGameDays = Convert.ToInt32(numberGameDaysString);
            _dataAccess = dataAccess;
            _radar = radar;
            _pubSubUtil = pubSubUtil;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _gamesConcurrentBag = new ConcurrentBag<TGame>();
        }

        public ILog Logger { get; set; }
        public Container DiContainer { get; set; }
        public Func<TGameDto, bool, Container, TGame> CreateGameFromGameDto { get; set; }

        #endregion

        #region Service Control Methods

        public void Start()
        {
            try
            {
                // Task.Factory.StartNew() supports child tasks, but Task.Run() does not
                Task startTask = Task.Factory.StartNew(StartGame, _cancellationToken);
                Task readExecutePrintLoopTask = Task.Factory.StartNew(ReadExecutePrintLoop, _cancellationToken);
            }
            catch (Exception exception)
            {
                Logger.Error($"Start() exception = {exception}");
            }
        }

        public void Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel();
                UnloadGames(_gamesConcurrentBag);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        #region Private Methods

        private void ReadExecutePrintLoop()
        {
            bool done = false;

            while (!done)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                char character = consoleKeyInfo.KeyChar;
                character = character.ToString().ToLower().First();

                switch (character)
                {
                    case 'q':
                        Console.WriteLine("\nExiting");
                        done = true;
                        // todo cancel tasks
                        break;

                    case 's':
                        Console.WriteLine("\nPrint task status");
                        DisplayTaskStatus();
                        break;

                    default:
                        Console.WriteLine("Command not found");
                        break;
                }
            }
        }

        private void StartGame()
        {
            try
            {
                const bool loadGamesConcurrently = true;
                int numberGames = LoadGames(loadGamesConcurrently);

                if (numberGames == 0)
                {
                    Logger.Info("No games found");
                    return;
                }

                Logger.Info($"Number games = {numberGames}");

                // wait for games to signal LoadComplete before continuing and starting subscriptions
                _loadCompleteCountdownEvent.Wait(_cancellationToken);
                Logger.Info("************************************ Loaded games ************************************");

                Task sportRadarSubscriptionTask = Task.Run(async () => await SubscribeToSportRadarGameEvents(), _cancellationToken);
                SubscribeToPubSubTopic();
            }
            catch (Exception exception)
            {
                Logger.Error($"StartGame() exception = {exception}");
            }
        }

        private int LoadGames(bool loadGamesConcurrently)
        {
            // players are not loaded in team mode
            bool loadPlayers = !_isTeamMode;
            List<TGameDto> gameDtoList = _dataAccess.GetGames(_numberGameDays, loadPlayers);
            // gameDtoList = gameDtoList.Skip(4).Take(1).ToList();
            int numberGames = gameDtoList.Count;
            _loadCompleteCountdownEvent = new CountdownEvent(numberGames);
            _gamesTaskList = new List<Task>();
            int gameNumber = 1;

            foreach (TGameDto gameDto in gameDtoList)
            {
                // debugging is easier if the games are not loaded concurrently
                if (loadGamesConcurrently)
                {
                    // prevents gameNumber from being captured as a closure variable
                    int tempGameNumber = gameNumber;
                    Task gameTask = Task.Factory.StartNew(() => LoadGame(gameDto, loadPlayers, tempGameNumber, numberGames), _cancellationToken, TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
                    _gamesTaskList.Add(gameTask);
                    _cancellationToken.ThrowIfCancellationRequested();
                }
                else
                {
                    LoadGame(gameDto, loadPlayers, gameNumber, numberGames);
                }

                gameNumber++;
            }

            return numberGames;
        }

        private async void LoadGame(TGameDto gameDto, bool loadPlayers, int gameNumber, int numberGames)
        {
            using (ThreadScopedLifestyle.BeginScope(DiContainer))
            {
                TGame game = default(TGame);

                try
                {
                    Logger.Info($"{gameDto.GameId} Loading game");
                    game = CreateGameFromGameDto(gameDto, loadPlayers, DiContainer);
                    Guid gameId = game.GameId;
                    game.LoadCompleteCountdownEvent = _loadCompleteCountdownEvent;
                    game.IsTeamMode = _isTeamMode;
                    _radar.IsSimulation = game.IsSimulation;
                    game.AddRadarGameEventHandler();
                    game.AddPubSubEventHandler();

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    game.LoadModelData();
                    string description = game.Description;
                    Logger.Info($"{gameId} ({description}) Loaded game ({gameNumber,-2} of {numberGames,-2}) in {stopwatch.Elapsed.Milliseconds,4} msec  ");
                    _gamesConcurrentBag.Add(game);

                    bool done = false;
                    TimeSpan radarGameEventTimeout = TimeSpan.FromSeconds(60);

                    while (!done)
                    {
                        _cancellationToken.ThrowIfCancellationRequested();

                        if (game.ModelUpdateRequired)
                        {
                            Logger.Info($"{gameId} ({description}) Updating model");
                            game.RunModel();
                        }

                        // todo exit loop if there are no heartbeats
                        // check if heartbeats are missing, but only after game loading is complete
                        //bool isNoHeartbeat = game.InitialModelDataLoadComplete && game.GetTimeSinceLastGameEventOrHeartbeat() > radarGameEventTimeout;

                        //if (game.GetTimeSinceLastGameEventOrHeartbeat() > radarGameEventTimeout)
                        //{
                        //    const string message = "No radar game event or heartbeat in 60 seconds";
                        //    Logger.Error(message);
                        //    done = true;
                        //}

                        await Task.Delay(game.ModelUpdateInterval, _cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    // every thread must signal LoadComplete in order for the wait to be released
                    if (game != null && !game.InitialModelDataLoadComplete)
                    {
                        _loadCompleteCountdownEvent.Signal();
                    }

                    Logger.Error($"LoadGame() exception = {exception}");
                }
            }
        }

        private async Task SubscribeToSportRadarGameEvents()
        {
            Logger.Info("Subscribing to SportRadar game events");
            await _radar.SubscribeToGameEvents();
        }

        private void SubscribeToPubSubTopic()
        {
            Logger.Info($"Subscribing to pubsub topic {_subscriptionId}");
            _pubSubUtil.SubscribeToTopic(_subscriptionId);
        }

        #endregion

        #region Other Code

        private void DisplayTaskStatus()
        {
            Logger.Info("Task status:");

            foreach (Task task in _gamesTaskList)
            {
                Logger.Info(task.Status);
            }
        }

        private static void UnloadGames(ConcurrentBag<TGame> gamesConcurrentBag)
        {
            foreach (var game in gamesConcurrentBag)
            {
                game.RemoveRadarGameEventHandler();
                game.RemovePubSubEventHandler();
            }

            gamesConcurrentBag = null;
            GC.Collect();
        }

        #endregion
    }
}
