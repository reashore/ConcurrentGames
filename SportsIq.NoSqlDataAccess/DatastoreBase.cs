using Google.Cloud.Datastore.V1;
using Polly;
using Polly.Retry;
using System;
using System.Configuration;
using log4net;

namespace SportsIq.NoSqlDataAccess
{
    public abstract class DatastoreBase
    {
        protected static readonly ILog Logger;
        protected readonly DatastoreDb DatastoreDb;
        protected readonly AsyncRetryPolicy AsyncRetryPolicy;

        static DatastoreBase()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DatastoreBase));
        }

        protected DatastoreBase()
        {
            string projectId = ConfigurationManager.AppSettings["projectId"];
            // todo this line is very problematic and prevents migration above .net 4.7.1
            DatastoreDb = DatastoreDb.Create(projectId);

            AsyncRetryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(10,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        string message = $"Datastore Retry - Count:{retryCount}, Exception:{exception.Message}";
                        Logger.Error(message);
                    }
                );
        }
    }
}
