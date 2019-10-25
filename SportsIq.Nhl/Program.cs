using log4net;
using SportsIq.DependencyInjection.Nhl;
using SportsIq.Games.Nhl;
using SportsIq.MainConsoleTemplate.Nhl;
using Topshelf;
using Container = SimpleInjector.Container;

namespace SportsIq.Nhl
{
    public static class Program
    {
        public static Container DependencyInjectionContainer { get; }
        public static readonly ILog Logger;

        static Program()
        {
            // static constructors run before all other code
            Logger = LogManager.GetLogger(typeof(Program));
            DependencyInjectionContainer = DependencyInjectorNhl.Configure();
        }

        public static void Main()
        {
            ConfigureMlbService();
        }

        private static void ConfigureMlbService()
        {
            HostFactory.Run(serviceConfig =>
            {
                serviceConfig.Service<MlbWindowsService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new MlbWindowsService());
                    serviceInstance.WhenStarted(execute => execute.Start());
                    serviceInstance.WhenStopped(execute => execute.Stop());
                });

                serviceConfig.SetServiceName("NhlService");
                serviceConfig.SetDisplayName("NHL Service");
                serviceConfig.SetDescription("Service to calculate NHL game odds");
                serviceConfig.StartAutomatically();
            });
        }
    }

    public class MlbWindowsService
    {
        private readonly NhlMainConsole _nhlMainConsole;

        public MlbWindowsService()
        {
            _nhlMainConsole = Program.DependencyInjectionContainer.GetInstance<NhlMainConsole>();
            _nhlMainConsole.DiContainer = Program.DependencyInjectionContainer;
            _nhlMainConsole.CreateGameFromGameDto = NhlGameConverter.CreateNhlGameFromNhlGameDto;
            _nhlMainConsole.Logger = Program.Logger;
        }

        public bool Start()
        {
            _nhlMainConsole.Start();
            return true;
        }

        public bool Stop()
        {
            _nhlMainConsole.Stop();
            //Program.DependencyInjectionContainer.Dispose();
            return true;
        }
    }
}
