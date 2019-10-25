using System;
using System.Collections.Generic;
using System.Configuration;
using log4net;

namespace SportsIq.SqlDataAccess
{
    // There are two levels of data access and hence two interfaces:
    // 1) MainConsole only requires the ability to call GetGames()
    // 2) Game requires the ability to read detailed information about games.
    
    public interface IDataAccessBase<TGameDto>
    {
        List<TGameDto> GetGames(int numberGameDays, bool loadPlayers);
    }

    public abstract class DataAccessBase : IDisposable
    {
        public static readonly ILog Logger;
        protected readonly string ConnectionString;

        static DataAccessBase()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DataAccessBase));
        }

        protected DataAccessBase()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["PostgresConnectionString"];
            ConnectionString = connectionStringSettings.ConnectionString;
        }

        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                //NpgsqlConnection?.Dispose();
            }

            // Free native resources
            //NpgsqlConnection?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataAccessBase()
        {
            Dispose(false);
        }

        #endregion
    }
}