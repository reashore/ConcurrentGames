using Google.Cloud.Datastore.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace SportsIq.NoSqlDataAccess
{
    public interface IDatastore
    {
        Task AddFinishedOdds(Guid gameId, string oddsJson);
        Task AddFinishedProps(Guid gameId, string oddsJson);
        Dictionary<string, double> GetMarketOdds(Guid gameId);
    }

    public class Datastore : DatastoreBase, IDatastore
    {
        private readonly KeyFactory _marketOddsKeyFactory;
        private readonly KeyFactory _finishedOddsKeyFactory;

        public Datastore()
        {
            string datastoreMarketOddsKey = ConfigurationManager.AppSettings["datastoreMarketOddsKey"];
            _marketOddsKeyFactory = DatastoreDb.CreateKeyFactory(datastoreMarketOddsKey);

            string datastoreFinishedtOddsKey = ConfigurationManager.AppSettings["datastoreFinishedOddsKey"];
            _finishedOddsKeyFactory = DatastoreDb.CreateKeyFactory(datastoreFinishedtOddsKey);
        }

        public async Task AddFinishedOdds(Guid gameId, string oddsJson)
        {
            string gameIdString = gameId.ToString();

            await AsyncRetryPolicy.ExecuteAsync(async () =>
            {
                Entity entity = new Entity
                {
                    Key = _finishedOddsKeyFactory.CreateKey(gameIdString),
                    ["odds"] = new Value
                    {
                        StringValue = oddsJson,
                        ExcludeFromIndexes = true
                    },
                    ["comp_id"] = gameIdString
                };

                Key key = await DatastoreDb.UpsertAsync(entity);
            });
        }
        
        public async Task AddFinishedProps(Guid gameId, string oddsJson)
        {
            try
            {
                string gameIdString = gameId.ToString();

                await AsyncRetryPolicy.ExecuteAsync(async () =>
                {
                    Entity entity = new Entity
                    {
                        Key = _finishedOddsKeyFactory.CreateKey(gameIdString),
                        ["props"] = new Value
                        {
                            StringValue = oddsJson,
                            ExcludeFromIndexes = true
                        },
                        ["comp_id"] = gameIdString
                    };

                    Key key = await DatastoreDb.UpsertAsync(entity);
                });
            }
            catch (Exception e)
            {
                Logger.Info(e);
            }
        }
        
        public Dictionary<string, double> GetMarketOdds(Guid gameId)
        {
            string gameIdString = gameId.ToString();
            Key key = _marketOddsKeyFactory.CreateKey(gameIdString);
            string store = ConfigurationManager.AppSettings["datastoreMarketOddsKey"];
            Query query = new Query(store)
            {
                Filter = Filter.Equal("__key__", key)
            };

            DatastoreQueryResults datastoreQueryResults = DatastoreDb.RunQuery(query);

            if (datastoreQueryResults.Entities.Count == 0)
            {
                return new Dictionary<string, double>();
            }

            string odds = datastoreQueryResults.Entities[0].Properties["odds"].StringValue;
            Dictionary<string, double> oddsDictionary = JsonConvert.DeserializeObject<Dictionary<string, double>>(odds);

            return oddsDictionary;
        }
    }
}
